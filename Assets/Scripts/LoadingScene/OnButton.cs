using UnityEditor;
using UnityEngine;

public class OnButton : MonoBehaviour
{
    public void OnClick_InGame()
    {
        Time.timeScale = 1;
        SceneLoader.LoadScene("GameMapPrototype1");
    }
    
    public void OnClick_Title()
    {
        Time.timeScale = 1;
        SceneLoader.LoadSceneFast("TitleScene");
    }
    
    public void OnClick_Exit()
    {
        Time.timeScale = 1;
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
