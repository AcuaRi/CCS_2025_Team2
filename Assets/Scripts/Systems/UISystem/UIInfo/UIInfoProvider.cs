using UnityEngine;
using UnityEngine.EventSystems;

public class UIInfoProvider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private UIInfo uiInfo;
    private UIInfoPanel infoPanel;

    private void Start()
    {
        //infoPanel = FindObjectOfType<UIInfoPanel>();
        infoPanel = UIManager.Instance.GetUIInfoPanel();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoPanel != null)
        {
            infoPanel.UpdateInfo(uiInfo);
        }
    }
    

    public void OnPointerExit(PointerEventData eventData)
    {
        if (infoPanel != null)
        {
            //infoPanel.ClearInfo();
        }
    }
}