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
    private bool isGameOver = false;
    
    private VictoryWaveEffect victoryWaveEffect;
    
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
        victoryWaveEffect = GetComponent<VictoryWaveEffect>();
        
        SoundManager.Instance.PlayBGM("BGM");
        
        remainingTime = playTime;
        
        currentHp = maxHp;
        UIManager.Instance.SetBodyHpGaugeUI(currentHp, maxHp);
        
        StartCoroutine(UpdateGameTime());
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
        if(remainingTime <= 0 || isGameOver) return;
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
            //SceneLoader.LoadSceneFast("ResultScene 1");
            if(!isGameOver) StartCoroutine(GameClearSequence());
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
        if (currentHp <= 0) currentHp = 0;
        UIManager.Instance.SetBodyHpGaugeUI(currentHp, maxHp);

        if (currentHp <= 0)
        {
            //SoundManager.Instance.StopBGM();
            //SoundManager.Instance.PlayBGM("BGM_Lose");
            //SceneLoader.LoadSceneFast("ResultScene");
            
            if(!isGameOver) StartCoroutine(GameOverSequence());
        }
    }

    public void RecoveryBody(float heal)
    {
        this.currentHp += heal;
        if (currentHp > maxHp) currentHp = maxHp;
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
    
    private IEnumerator GameOverSequence()
    {
        isGameOver = true;
        SoundManager.Instance.StopBGM();
        //SoundManager.Instance.PlayBGM("BGM_Lose");

        float duration = 2f;
        float elapsed = 0f;
        float startScale = 1f;
        float endScale = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Time.timeScale의 영향을 받지 않도록
            
            var percent = Mathf.Clamp01(elapsed / duration);
            
            Time.timeScale = Mathf.Lerp(startScale, endScale, percent);

            // 화면 어둡게 만들기 (UIManager 활용)
            UIManager.Instance.SetFadeEffect(percent);

            yield return null;
        }

        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(1f); // 1초 대기 (Time.timeScale=0이므로 Realtime 사용)

        SceneLoader.LoadSceneFast("ResultScene");
    }

    private IEnumerator GameClearSequence()
    {
        isGameOver = true;
        SoundManager.Instance.StopBGM();

        float duration = 1f; // 3초 동안 연출
        float elapsed = 0f;
        float startScale = 1f;
        float endScale = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Time.timeScale의 영향을 받지 않도록
            
            var percent = Mathf.Clamp01(elapsed / duration);
            
            Time.timeScale = Mathf.Lerp(startScale, endScale, percent);

            // 화면 어둡게 만들기 (UIManager 활용)
            //UIManager.Instance.SetFadeEffect(percent);
            yield return null;
        }
        
        Time.timeScale = 0.01f;
        
        victoryWaveEffect.StartVictoryEffect();
        yield return new WaitForSecondsRealtime(3f); 
        
        Time.timeScale = 1f;
        
        yield return new WaitForSecondsRealtime(5f); 
        
        elapsed = 0;
        duration = 1;
        
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Time.timeScale의 영향을 받지 않도록
            
            var percent = Mathf.Clamp01(elapsed / duration);
            
            UIManager.Instance.SetFadeEffect(percent);
            yield return null;
        }
        
        //yield return new WaitForSecondsRealtime(1f); 

        SceneLoader.LoadSceneFast("ResultScene 1");
    }
}