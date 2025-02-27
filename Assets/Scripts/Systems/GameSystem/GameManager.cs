using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public float RemainingTime => remainingTime;
    private bool isPaused = false;
    public bool IsPaused => isPaused;
    private bool isWaveRunning = false;
    
    
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
        SoundManager.Instance.PlayBGM("BGM");
        
        remainingTime = playTime;
        
        currentHp = maxHp;
        UIManager.Instance.SetBodyHpGaugeUI(currentHp, maxHp);
        
        //StartCoroutine(SpawnWaves());
        
        StartCoroutine(UpdateGameTime());
    }
    
    private IEnumerator SpawnWaves()
    {
        int waveNumber = 0;
    
        while (remainingTime > 0)
        {
            waveNumber++;
            
            GenerateWaveEnemies(waveNumber);
            
            yield return new WaitForSeconds(30f);
        }
    }

    private void GenerateWaveEnemies(int waveNumber)
    {
        //var random = Random.Range(1, 6);
        switch (waveNumber)
        {
            case 1:
                EnemyGenerator.Instance.GenerateEnemy("Enemy_Bacillus", 5, 5, 3);
                EnemyGenerator.Instance.GenerateEnemy("Enemy_Rensa", 5, 5, 3);
                break;
            case 2:
                EnemyGenerator.Instance.GenerateEnemy("Enemy_MSSA", 5, 5, 3);
                EnemyGenerator.Instance.GenerateEnemy("Enemy_MRSA", 5, 5, 3);
                break;
            case 3:
                EnemyGenerator.Instance.GenerateEnemy("Enemy_Pesto", 5, 5, 3);
                EnemyGenerator.Instance.GenerateEnemy("Enemy_Influenza", 5, 5, 3);
                break;
            case 4:
                EnemyGenerator.Instance.GenerateEnemy("Enemy_Roku", 5, 5, 3);
                EnemyGenerator.Instance.GenerateEnemy("Enemy_E_Coli", 5, 5, 3);
                break;
            case 5:
                EnemyGenerator.Instance.GenerateEnemy("Enemy_Kurosuto", 5, 5, 3);
                EnemyGenerator.Instance.GenerateEnemy("Enemy_MaicoPlazma", 5, 5, 3);
                break;
        }
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            SoundManager.Instance.PlaySound("Open_pause", transform.position);
        }
        else
        {
            SoundManager.Instance.PlaySound("Close_pause", transform.position);
        }
        UIManager.Instance.SetPausePanel(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    private void FixedUpdate()
    {
        remainingTime -= Time.fixedDeltaTime;
        if (remainingTime < 0f)
        {
            remainingTime = 0f;
            // GameOver Logic
            SceneLoader.LoadSceneFast("ResultScene 1");
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

        if (currentHp <= 0)
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlayBGM("BGM_Lose");
            SceneLoader.LoadSceneFast("ResultScene");
        }
    }

    public void RecoveryBody(float heal)
    {
        this.currentHp += heal;
        UIManager.Instance.SetBodyHpGaugeUI(currentHp, maxHp);
    }
    
    private IEnumerator UpdateGameTime()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1);
            
            if (!isWaveRunning)
            {
                isWaveRunning = true;
                yield return StartCoroutine(WaveManager.Instance.NextWave(playTime - remainingTime));
                isWaveRunning = false;
            }
        }
        
        //no remaining time
    }
}