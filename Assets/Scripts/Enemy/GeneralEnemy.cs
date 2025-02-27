using System.Collections.Generic;
using UnityEngine;

public class GeneralEnemy : MonoBehaviour, IDamageable
{
    protected GeneralEnemyDataStruct generalMonsterData;
    public GeneralEnemyDataStruct GeneralMonsterData => generalMonsterData;
    
    //FSM State
    protected bool isTransition = false;
    protected FSMState idleState;
    protected FSMState attackState;
    protected FSMState vesselState;
    protected FSMState deathState;
    
    protected FSMState currentState;
    protected FSMState nextState;

    protected bool FindTarget = false;
    
    [Header("Ref")]
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Transform bodyTransform;
    [SerializeField] protected GeneralEnemyData refData;

    protected float maxHp;
    protected HPGauge hpGaugeInstance;
    protected ResistantShield shieldInstance;
    protected bool isInVessel = false;
    
    protected float[] medicineResistantProbablity = new float[10];
    
    
    
    protected void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (animator == null)
        {
            //animator = GetComponent<Animator>();
        }
        
        if (sprite == null)
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
        }

        if (bodyTransform == null)
        {
            bodyTransform = GetComponentInChildren<Transform>();
        }
        
        refData.SyncData();
        generalMonsterData = refData.data;
        
        //null check
        if(rb == null) {Debug.LogError($"{this.gameObject.name}(RigidBody2D) is null");}
        //if(animator == null) {Debug.LogError($"{this.gameObject.name}(Animator) is null");}
        if(sprite == null) {Debug.LogError($"{this.gameObject.name}(Sprite) is null");}
        if(refData == null) {Debug.LogError($"{this.gameObject.name}(refData) is null");}
        //if( generalMonsterData.targetLayer != PlayerLayer) {Debug.LogError($"{this.gameObject.name}(targetLayer is not playerLayer)");}
        
        StateInit();
    }

    protected virtual void Start()
    {
        maxHp = generalMonsterData.hp;
        Vector3 initialScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (UIManager.Instance != null)
        {
            hpGaugeInstance = UIManager.Instance.GetHpGauge(initialScreenPos);
            hpGaugeInstance.gameObject.SetActive(false);
            hpGaugeInstance.SetTarget(transform);
            
            if (generalMonsterData.resistantMedicineType != MedicineType.None && shieldInstance == null)
            {
                shieldInstance = UIManager.Instance.GetResistantShield(initialScreenPos);
                shieldInstance.SetTarget(transform);
            }
        }
        UpdateHpGauge();
        
        IdleEnter();
        
        if(generalMonsterData.divisionCoolTime <= 0) return;
        Invoke("Division", generalMonsterData.divisionCoolTime);
    }

    protected void FixedUpdate()
    {
        if (isTransition && currentState != nextState)
        {
            currentState = nextState;
            currentState.OnEnter?.Invoke();
            isTransition = false;
        }
        
        currentState.OnUpdate?.Invoke();
        isTransition = TransitionCheck();
        
        if(isTransition && currentState != nextState) currentState.OnExit?.Invoke();
        
    }

    /// 
    protected virtual void StateInit()
    {
        idleState = new FSMState( IdleEnter, IdleUpdate, null);
        attackState = new FSMState( AttackEnter, AttackUpdate, null);
        vesselState = new FSMState( VesselEnter, VesselUpdate, VesselExit);
        deathState = new FSMState(DeathEnter, null, null);
        
        currentState = idleState;
        nextState = idleState;
    }

    protected virtual bool TransitionCheck()
    {
        if (nextState == deathState)
        {
            return true;
        }
        
        if (isInVessel && currentState != vesselState)
        {
            return true;
        }
        
        if (currentState == idleState)
        {
            if (FindTarget)
            {
                FindTarget = false;
                return true;
            }
        }

        if (currentState == attackState)
        {
            if (nextState == idleState)
            {
                return true;
            }
        }
        
        return false;
    }

    protected virtual void IdleEnter()
    {
        rb.velocity = Vector2.zero;
        generalMonsterData.moveDirection = ( Vector2.right * Random.Range(-1f, 1f) + Vector2.down * Random.Range(0.5f, 1f)).normalized;
        bodyTransform.up = -generalMonsterData.moveDirection;
        GetSnappedRotation(generalMonsterData.moveDirection);
        Invoke("CheckTarget", 0.25f);
    }
    
    protected virtual void IdleUpdate()
    {
        Move();
    }
    
    protected virtual void AttackEnter()
    {
        //animator.SetTrigger("Attack");
        Attack();
    }
    
    protected virtual void AttackUpdate()
    {
        if (generalMonsterData.targetTransform == null || generalMonsterData.targetTransform.gameObject.activeSelf == false)
        {
            generalMonsterData.targetTransform = null;
            nextState = idleState;
            return;
        }

        if (Vector2.Distance(generalMonsterData.targetTransform.position, transform.position) >
            generalMonsterData.targetLossRadius)
        {
            //Debug.Log("test");
            generalMonsterData.targetTransform = null;
            nextState = idleState;
            return;
        }
        
        generalMonsterData.moveDirection = (generalMonsterData.targetTransform.position - transform.position).normalized;
        GetSnappedRotation(generalMonsterData.moveDirection);
        rb.transform.Translate( generalMonsterData.moveSpeed * Time.deltaTime *  generalMonsterData.moveDirection);
    }

    protected virtual void VesselEnter()
    {
        this.gameObject.layer = 22; //enemy(Passed)
        bodyTransform.gameObject.layer = 22;
        
        generalMonsterData.moveDirection = (3f * Vector2.left + generalMonsterData.moveDirection).normalized;
        GetSnappedRotation(generalMonsterData.moveDirection);
        RegisterDisease();
    }
    
    protected virtual void RegisterDisease()
    {
        
    }

    public virtual void SetVesselState()
    {
        if(currentState == vesselState) return;
        nextState = vesselState;
        isInVessel = true;
    }
    
    protected virtual void VesselUpdate()
    {
        //moveSpeed in Vessel needs to be equal?
        rb.transform.Translate( 10f * Time.deltaTime *  generalMonsterData.moveDirection);
    }

    protected virtual void VesselExit()
    {
        isInVessel = false;
        UnregisterDisease();
    }
    
    protected virtual void UnregisterDisease()
    {
        
    }   

    protected virtual void DeathEnter()
    {
        SlotSelectMock.Instance.IncreaseCurrentPoints(generalMonsterData.points);
        Destroy(hpGaugeInstance.gameObject);
        if (shieldInstance != null)
        {
            Destroy(shieldInstance.gameObject);
        }
        Destroy(this.gameObject);
    }
    
    protected void Move()
    {
        if (DetectObstacle())
        {
            generalMonsterData.moveDirection.x = -generalMonsterData.moveDirection.x;
            GetSnappedRotation(generalMonsterData.moveDirection);
        }
        
        rb.transform.Translate( generalMonsterData.moveSpeed * Time.deltaTime *  generalMonsterData.moveDirection);
    }

    protected virtual void CheckTarget()
    {
        if ( currentState != idleState) return;

        Collider2D target = Physics2D.OverlapCircle(rb.position,  generalMonsterData.recognizeRadius,  generalMonsterData.targetLayer);
        if (target != null && target.gameObject.activeSelf == true)
        {
            generalMonsterData.targetTransform = target.transform;
            nextState = attackState;
            FindTarget = true;
        }
        else if (generalMonsterData.targetTransform == null)
        {
            rb.velocity = Vector2.zero;
            generalMonsterData.moveDirection = ( Vector2.right * Random.Range(-1f, 1f) + Vector2.down * Random.Range(0.5f, 1f)).normalized;
            GetSnappedRotation(generalMonsterData.moveDirection);
        }
        
        Invoke("CheckTarget", 0.25f);
    }
    
    protected bool DetectObstacle()
    {
        // RaycastHit2D hit = Physics2D.Raycast(rb.transform.position,  generalMonsterData.moveDirection, generalMonsterData.obstacleRaycastDistance, GroundLayer);
        // Debug.DrawRay(rb.transform.position, ( generalMonsterData.moveDirection) * generalMonsterData.obstacleRaycastDistance, Color.blue);
        //
        // if (hit.collider != null) return true;
        //
         return false;
    }
    
    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if ( currentState == deathState) return;

        if (currentState == vesselState)
        {
            generalMonsterData.moveDirection.y = -generalMonsterData.moveDirection.y;
        }
        else
        {
            generalMonsterData.moveDirection.x = -generalMonsterData.moveDirection.x;
        }
        
        GetSnappedRotation(generalMonsterData.moveDirection);
        
        if (other.gameObject.layer == 30)
        {
            var target = other.gameObject.GetComponent<IDamageable>();
            if (target != null)
            {
                target.GetDamaged(generalMonsterData.attackDamage, MedicineType.None,  (other.transform.position - this.transform.position).normalized);
            }
        }
    }

    protected float damageTimer = 0f;
    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == Mathf.Log(generalMonsterData.targetLayer.value, 2) || other.gameObject.layer == 30) // 30 : Player
        {
            damageTimer += Time.deltaTime;
            
            if (damageTimer >= 1f)
            {
                var target = other.gameObject.GetComponent<IDamageable>();
                if (target != null)
                {
                    target.GetDamaged(generalMonsterData.attackDamage);
                }

                // 타이머 초기화
                damageTimer = 0f;
            }
        }
    }

    protected virtual void Attack()
    {
        //Debug.Log("Attack!");
    }

    public virtual void GetDamaged(float damage)
    {
        if(damage <= 0) return;
        if( currentState == deathState) return;

        float caculatedDamage = damage;
        
        generalMonsterData.hp -= caculatedDamage;
        generalMonsterData.hp = Mathf.Clamp(generalMonsterData.hp, 0, maxHp);
        
        if (hpGaugeInstance != null && !hpGaugeInstance.gameObject.activeSelf)
        {
            hpGaugeInstance.gameObject.SetActive(true);
        }
        
        UpdateHpGauge();

        if ( generalMonsterData.hp <= 0)
        {
            nextState = deathState;
        }
    }
    
    public virtual void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        if(damage <= 0) return;
        if( currentState == deathState) return;
        
        
        float caculatedDamage = damage;
        int damageType = 4; //good: 1, bad: 2, resistant: 3, dafault: 4
        
        //Case of resistant medicine
        if ((medicineType & generalMonsterData.resistantMedicineType) == medicineType)
        {
            caculatedDamage = damage / 10;
            SoundManager.Instance.PlaySound("GetDamaged_shield", transform.position);
            damageType = 3;
        }
        
        //Case of good medicine
        else if ((medicineType & generalMonsterData.goodMedicineTypes) == medicineType)
        {
            caculatedDamage = damage * 5;
            SoundManager.Instance.PlaySound("GetDamaged_Good", transform.position);
            //SoundManager.Instance.PlaySound("Test", transform.position);
            damageType = 1;
        }
        
        //Case of bad medicine
        else if ((medicineType & generalMonsterData.badMedicineTypes) == medicineType)
        {
            caculatedDamage = damage / 10;
            SoundManager.Instance.PlaySound("GetDamaged_Bad", transform.position);
            damageType = 2;
        }
        //default?
        else
        {
            SoundManager.Instance.PlaySound("GetDamaged_Default", transform.position);
        }
        
        generalMonsterData.hp -= caculatedDamage;
        generalMonsterData.hp = Mathf.Clamp(generalMonsterData.hp, 0, maxHp);
        
        if (hpGaugeInstance != null && !hpGaugeInstance.gameObject.activeSelf)
        {
            hpGaugeInstance.gameObject.SetActive(true);
        }
        
        UIManager.Instance.ShowDamageEffect(caculatedDamage, damageType, this.transform);
        
        UpdateHpGauge();
        if (force.magnitude > 0.1f)
        {
            rb.AddForce(force, (ForceMode2D)ForceMode.Impulse);
        }
        
        generalMonsterData.targetTransform = GameObject.Find("Player").transform;
        nextState = attackState;
        FindTarget = true;
        
        if ( generalMonsterData.hp <= 0)
        {
            nextState = deathState;
        }
        
        //increase resistance of medicine
        if (medicineType != MedicineType.None)
        {
            medicineResistantProbablity[(int)Mathf.Log((int)medicineType, 2)] += 0.1f;
        }
    }
    
    /// <summary>
    /// 현재 체력 비율에 맞게 HP 게이지를 업데이트합니다.
    /// </summary>
    protected void UpdateHpGauge()
    {
        if (hpGaugeInstance != null)
        {
            float percent = generalMonsterData.hp / maxHp;
            hpGaugeInstance.SetHpGauge(percent);
        }
    }

    protected void Division()
    {
        if (generalMonsterData.hp <= 0) return;
        if (isInVessel) return;
        
        var random = Random.Range(0f, 1f);
        if (random < generalMonsterData.divisionProbability)
        {
            var division = Instantiate(this.gameObject, this.transform.position, this.transform.rotation);
            division.GetComponent<GeneralEnemy>().AcquireResistant(RollTheDiceOfResistant());
        }
        
        if(generalMonsterData.divisionCoolTime <= 0) return;
        Invoke("Division", generalMonsterData.divisionCoolTime);
    }

    protected void AcquireResistant(MedicineType acquiredResistant)
    {
        if (acquiredResistant != MedicineType.None && shieldInstance == null)
        {
            shieldInstance = UIManager.Instance.GetResistantShield(Camera.main.WorldToScreenPoint(transform.position));
            shieldInstance.SetTarget(transform);
        }
        
        MedicineType resistantType1 = (MedicineType)(1 << 0 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5);
        MedicineType resistantType2 = (MedicineType)(1 << 6 | 1 << 7 | 1 << 8);
        MedicineType resistantType3 = (MedicineType)(1 << 9);
        
        int resistantCount = 0;

        if ((acquiredResistant & resistantType1) != MedicineType.None)
        {
            acquiredResistant = resistantType1;
            resistantCount++;
            shieldInstance.SetColor(Color.green);
        }
        if ((acquiredResistant & resistantType2) != MedicineType.None)
        {
            acquiredResistant = resistantType2;
            resistantCount++;
            var orange = new Color(255, 90, 0);
            shieldInstance.SetColor(orange);
        }
        if ((acquiredResistant & resistantType3) != MedicineType.None)
        {
            acquiredResistant = resistantType3;
            resistantCount++;
            shieldInstance.SetColor(Color.red);
        }

        if (resistantCount >= 2)
        {
            shieldInstance.SetColor(Color.white);
        }
        
        generalMonsterData.resistantMedicineType = generalMonsterData.resistantMedicineType | acquiredResistant;
    }

    protected MedicineType RollTheDiceOfResistant()
    { 
        MedicineType acquiredResistant = MedicineType.None;
        
        for (int i = 0; i < medicineResistantProbablity.Length; i++)
        {
            var random = Random.Range(0f, 1f);
         
            if (random < medicineResistantProbablity[i])
            {
                acquiredResistant = acquiredResistant | (MedicineType)(1 << i);
            }
        }
        
        return acquiredResistant;
    }
    
    void GetSnappedRotation(Vector2 movementDirection)
    {
        Quaternion desiredRotation = Quaternion.LookRotation(Vector3.forward, -movementDirection);
    
        // 초당 90도 회전 (필요에 따라 rotationSpeed 값을 조절하세요)
        float rotationSpeed = 45;
    
        // 현재 회전에서 목표 회전으로 부드럽게 회전
        bodyTransform.rotation = Quaternion.RotateTowards(bodyTransform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
    
}
