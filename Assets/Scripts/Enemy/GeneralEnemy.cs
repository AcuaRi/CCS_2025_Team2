using UnityEngine;

public class GeneralEnemy : MonoBehaviour, IDamageable
{
    protected GeneralEnemyDataStruct generalMonsterData;
    public GeneralEnemyDataStruct GeneralMonsterData => generalMonsterData;
    
    //FSM State
    protected bool isTransition = false;
    protected FSMState idleState;
    protected FSMState attackState;
    protected FSMState deathState;
    
    protected FSMState currentState;
    protected FSMState nextState;

    protected bool FindTarget = false;
    
    [Header("Ref")]
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected GeneralEnemyData refData;

    protected float maxHp;
    protected HPGauge hpGaugeInstance;
    //private const int PlayerLayer = 1 << 6;
    //private const int WallLayer = 1 << 7;
    
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
            sprite = GetComponent<SpriteRenderer>();
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
        }
        UpdateHpGauge();
        IdleEnter();
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
        deathState = new FSMState(null, null, null);
        
        currentState = idleState;
        nextState = idleState;
    }

    protected virtual bool TransitionCheck()
    {
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
        generalMonsterData.moveDirection = ( Vector2.right * Random.Range(-1f, 1f) + Vector2.down * Random.Range(0.5f, 1f)).normalized;
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
        generalMonsterData.moveDirection = (generalMonsterData.targetTransform.position - transform.position).normalized;
        rb.transform.Translate( generalMonsterData.moveSpeed * Time.deltaTime *  generalMonsterData.moveDirection);
        sprite.flipX = ( generalMonsterData.targetTransform.position.x < transform.position.x);
    }
    /// 
    
    protected void TurnBack()
    {
        generalMonsterData.moveDirection.x = -generalMonsterData.moveDirection.x;
        //sprite.flipX = ( generalMonsterData.moveDirection.x < 0 );
    }

    protected void Move()
    {
        if (DetectObstacle())
        {
            TurnBack();
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
        //
        // if (other.gameObject.layer == 9)
        // {
        //     TurnBack();
        // }
        //
        // if (other.gameObject.layer == 6)
        // {
        //     other.gameObject.GetComponent<PlayerController>().GetDamaged( generalMonsterData.attackDamage, this.gameObject,
        //         (((other.transform.position.x > transform.position.x) ? Vector2.right : Vector2.left) + 0.5f * Vector2.up).normalized *  generalMonsterData.knockBackPower);
        // }
        
        if (other.gameObject.layer == Mathf.Log(generalMonsterData.targetLayer.value, 2))
        {
            //Debug.Log($"{this.gameObject.name}({other.gameObject.name}) is detected");
            //Destroy(other.gameObject);
        }
        else
        {
            TurnBack();
        }
    }

    protected float damageTimer = 0f;
    protected virtual void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == Mathf.Log(generalMonsterData.targetLayer.value, 2))
        {
            damageTimer += Time.deltaTime;

            // 누적된 시간이 1초 이상이면 데미지를 적용
            if (damageTimer >= 1f)
            {
                // 충돌한 대상의 Cell 컴포넌트를 가져와 데미지를 입힘
                var cell = other.gameObject.GetComponent<Cell>();
                if (cell != null)
                {
                    cell.GetDamaged(generalMonsterData.attackDamage);
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
    
    public virtual void GetDamaged(float damage, MedicineType medicineType)
    {
        if(damage <= 0) return;
        //if( currentState == deathState) return;

        float caculatedDamage = damage;
        
        //Case of resistant medicine
        if ((medicineType & generalMonsterData.resistantMedicineType) == medicineType)
        {
            
        }
        
        //Case of good medicine
        else if ((medicineType & generalMonsterData.goodMedicineTypes) == medicineType)
        {
            
        }
        
        //Case of bad medicine
        else if ((medicineType & generalMonsterData.badMedicineTypes) == medicineType)
        {
            
        }
        //default?
        else
        {
            
        }
        
        generalMonsterData.hp -= caculatedDamage;
        generalMonsterData.hp = Mathf.Clamp(generalMonsterData.hp, 0, maxHp);
        
        if (hpGaugeInstance != null && !hpGaugeInstance.gameObject.activeSelf)
        {
            hpGaugeInstance.gameObject.SetActive(true);
        }
        
        UpdateHpGauge();

        if ( generalMonsterData.hp <= 0)
        {
            Destroy(hpGaugeInstance.gameObject);
            Destroy(this.gameObject);
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
}
