using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [Tooltip("게임 플레이 시간 (초)")]
    [SerializeField] private float playTime = 300f;
    
    private float remainingTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        remainingTime = playTime;
        
        //EnemyGenerator.Instance.GenerateEnemy("EnemyType1", 10, 5, 5);
        EnemyGenerator.Instance.GenerateEnemy("EnemyType1_Default", 2, 5, 5);
        EnemyGenerator.Instance.GenerateEnemy("EnemyType1_Resistant", 2, 5, 5);
        EnemyGenerator.Instance.GenerateEnemy("EnemyType1_Good", 2, 5, 5);
        EnemyGenerator.Instance.GenerateEnemy("EnemyType1_Bad", 2, 5, 5);
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
}