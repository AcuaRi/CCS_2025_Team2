using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIInfoPanel : MonoBehaviour
{
    [SerializeField] private Image infoImage;  
    [SerializeField] private TextMeshProUGUI name_JP;
    [SerializeField] private TextMeshProUGUI name_EN;
    [SerializeField] private TextMeshProUGUI simpleInfoText;
    [SerializeField] private TMP_InputField detailInfoText; 

    public void UpdateInfo(UIInfo newInfo)
    {
        infoImage.sprite = newInfo.infoSprite;
        name_JP.text = newInfo.nameJP;
        name_EN.text = newInfo.nameEN;
        simpleInfoText.text = newInfo.simpleInfoText;
        detailInfoText.text = newInfo.detailInfoText;
    }

    public void ClearInfo()
    {
        infoImage.sprite = null;
        name_JP.text = "";
        name_EN.text = "";
        simpleInfoText.text = "";
        detailInfoText.text = "";
    }
}