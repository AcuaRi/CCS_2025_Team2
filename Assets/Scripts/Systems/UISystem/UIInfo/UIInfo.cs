using UnityEngine;

[CreateAssetMenu(fileName = "NewUIInfo", menuName = "UI/UIInfo")]
public class UIInfo : ScriptableObject
{
    public Sprite infoSprite;          
    public string nameJP;              
    public string nameEN;              
    public string simpleInfoText;          
    [TextArea(3, 30)] public string detailInfoText; 
}