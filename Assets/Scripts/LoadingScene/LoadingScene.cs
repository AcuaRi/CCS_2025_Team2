using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance { get; private set; }
    public Slider loadingBar;
    public float minimumLoadingTime = 3f; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (loadingBar == null)
        {
            loadingBar = FindObjectOfType<Slider>();
        }
    }
    
    public async UniTask LoadSceneAsync(string sceneName)
    {
        float startTime = Time.time;
        
        var loadOperation = SceneManager.LoadSceneAsync(sceneName);
        
        if (loadOperation == null)
        {
            Debug.LogError("LoadingScene : loadOperation is null");
        }
        loadOperation.allowSceneActivation = false;
        
        
        while (!loadOperation.isDone && loadOperation.progress <= 0.9f)
        {
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            float fakeProcess = Mathf.Clamp01((Time.time - startTime) / minimumLoadingTime);
            
            loadingBar.value = Mathf.Min(progress, fakeProcess);
            
            
            if (loadOperation.progress >= 0.9f && Time.time - startTime >= minimumLoadingTime)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                loadingBar.value = 1f;
                loadOperation.allowSceneActivation = true;
            }

            await UniTask.Yield();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}