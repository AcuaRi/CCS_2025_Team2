using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadSceneFast(string sceneName)
    {
        SoundManager.Instance.StopBGM();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    public static async UniTask LoadScene(string sceneName)
    {
        SoundManager.Instance.StopBGM();
        await LoadLoadingScene();
        
        LoadingScreen loadingScreen = LoadingScreen.Instance;
        if (loadingScreen != null)
        {
            await loadingScreen.LoadSceneAsync(sceneName);
        }
    }

    private static async UniTask LoadLoadingScene()
    {
        var operation = SceneManager.LoadSceneAsync("LoadingScene");
        await UniTask.WaitUntil(() => operation.isDone);
    }
}