using UnityEngine;

public class GeneralEnemy : MonoBehaviour, IDamageable
{
    protected GeneralEnemyDataStruct generalMonsterData;
    public GeneralEnemyDataStruct GeneralMonsterData => generalMonsterData;
    
    //FSM State
    private bool isTransition = false;
    protected FSMState idleState;
    protected FSMState attackState;
    protected FSMState deathState;
    
    protected FSMState currentState;
    protected FSMState nextState;

    protected bool FindTarget = false;
    
    [Header("Ref")]
    public SpriteRenderer sprite;
    public Animator animator;
    public Rigidbody2D rb;
    [SerializeField] private GeneralEnemyData refData;
    
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
            animator = GetComponent<Animator>();
        }
        
        if (sprite == null)
        {
            sprite = GetComponent<SpriteRenderer>();
        }
        
        refData.SyncData();
        generalMonsterData = refData.data;
        
        //null check
        if(rb == null) {Debug.LogError($"{this.gameObject.name}(RigidBody2D) is null");}
        if(animator == null) {Debug.LogError($"{this.gameObject.name}(Animator) is null");}
        if(sprite == null) {Debug.LogError($"{this.gameObject.name}(Sprite) is null");}
        if(refData == null) {Debug.LogError($"{this.gameObject.name}(refData) is null");}
        //if( generalMonsterData.targetLayer != PlayerLayer) {Debug.LogError($"{this.gameObject.name}(targetLayer is not playerLayer)");}
        
        StateInit();
    }

    protected virtual void Start()
    {
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
        Invoke("CheckTarget", 1f);
    }
    
    protected virtual void IdleUpdate()
    {
        Move();
    }
    
    protected virtual void AttackEnter()
    {
        animator.SetTrigger("Attack");
        Attack();
    }
    
    protected virtual void AttackUpdate()
    {
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
        if (target != null)
        {
            generalMonsterData.targetTransform = target.transform;
            nextState = attackState;
            FindTarget = true;
        }
        
        Invoke("CheckTarget", 1f);
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
        // if ( currentState == deathState) return;
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
    }

    protected virtual void Attack()
    {
        Debug.Log("Attack!");
    }
    
    public void GetDamaged(float damage, MedicineType medicineType)
    {
        if(damage <= 0) return;
        if( currentState == deathState) return;

        generalMonsterData.hp -= damage;
        //UIManager.instance.hitDamageInfo.PrintHitDamage(transform, damage);

        if ( generalMonsterData.hp < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
