using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [Tooltip("Gameplay Time (second)")]
    [SerializeField] private float playTime = 300f;
    [Tooltip("Body MaxHp")]
    [SerializeField] private float maxHp;
    private float currentHp;
    private float remainingTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        remainingTime = playTime;
        
        currentHp = maxHp;
        UIManager.Instance.SetBodyHpGaugeUI(currentHp, maxHp);
        
        EnemyGenerator.Instance.GenerateEnemy("EnemyType1_Good", 2, 5, 5);
        EnemyGenerator.Instance.GenerateEnemy("EnemyType2_Good", 2, 5, 5);
    }

    private void FixedUpdate()
    {
        remainingTime -= Time.fixedDeltaTime;
        if (remainingTime < 0f)
        {
            remainingTime = 0f;
            // GameOver Logic
        }
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCountdownUI(remainingTime);
        }
    }
    
    public void GetDamagedInBody(float damage)
    {
        if(damage <= 0) return;
        
        this.currentHp -= damage;
        UIManager.Instance.SetBodyHpGaugeUI(currentHp, maxHp);
    }
}