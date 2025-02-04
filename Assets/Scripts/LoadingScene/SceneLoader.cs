using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadSceneFast(string sceneName)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    public static async UniTask LoadScene(string sceneName)
    {
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