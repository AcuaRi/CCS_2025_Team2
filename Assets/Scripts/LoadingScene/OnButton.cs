using UnityEditor;
using UnityEngine;

public class OnButton : MonoBehaviour
{
    public void OnClick_InGame()
    {
        SceneLoader.LoadScene("GameMapPrototype1");
    }
    
    public void OnClick_Title()
    {
        SceneLoader.LoadSceneFast("TitleScene");
    }
    
    public void OnClick_Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
