using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Canvas References")]
    [Tooltip("Screen Space Overlay Canvas")]
    [SerializeField] private Canvas screenSpaceCanvas;

    [Header("Prefabs")]
    [Tooltip("HPGauge Prefab")]
    [SerializeField] private GameObject hpGaugePrefab;
    
    [Header("UI Elements")]
    [Tooltip("BodyHpGauge Image")]
    [SerializeField] private Image bodyHpGaugeImage;
    [Tooltip("BodyHpGauge Text")]
    [SerializeField] private TextMeshProUGUI bodyHpGaugeText;
    [Tooltip("Countdown text (TextMeshProUGUI)")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [Tooltip("Slot Image")]
    [SerializeField] private Image[] slotImages;
    [Tooltip("Color of the slot highlighted")]
    [SerializeField] private Color highlightedSlotColor;
    [Tooltip("Color of the slot unhighlighted")]
    [SerializeField] private Color unhighlightedSlotColor;
    
    private int selectedSlotIndex = 0;
    
    private void Awake()
    {
        // 싱글턴 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // 씬 전환 시 유지하려면 아래 주석 해제
        // DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SelectSlot(selectedSlotIndex);
    }

    public void SetBodyHpGaugeUI(float currentHp, float maxHp)
    {
        if (bodyHpGaugeImage != null)
        {
            bodyHpGaugeImage.fillAmount = currentHp / maxHp;
        }

        if (bodyHpGaugeText != null)
        {
            bodyHpGaugeText.text = $"{Mathf.CeilToInt(currentHp)}/{Mathf.CeilToInt(maxHp)}";
        }
    }
    
    /// <summary>
    /// Screen Space Overlay Canvas 하위에 HP 게이지 프리팹을 인스턴스화하여 반환합니다.
    /// </summary>
    /// <param name="position">초기 위치 (화면 좌표)</param>
    /// <param name="parent">부모 Transform (기본은 screenSpaceCanvas.transform)</param>
    /// <returns>생성된 HPGauge 컴포넌트</returns>
    public HPGauge GetHpGauge(Vector3 position, Transform parent = null)
    {
        if (screenSpaceCanvas == null)
        {
            Debug.LogError("Screen Space Canvas가 할당되지 않았습니다.");
            return null;
        }
        if (parent == null)
        {
            parent = screenSpaceCanvas.transform;
        }
        GameObject gaugeObj = Instantiate(hpGaugePrefab, position, Quaternion.identity, parent);
        HPGauge hpGauge = gaugeObj.GetComponent<HPGauge>();
        if (hpGauge == null)
        {
            Debug.LogError("생성된 프리팹에 HPGauge 컴포넌트가 없습니다.");
        }
        return hpGauge;
    }
    
    /// <summary>
    /// 남은 플레이 시간을 받아 "분:초:밀리초" 형식(예: 03:25:123)으로 Countdown 텍스트 UI를 업데이트합니다.
    /// </summary>
    /// <param name="remainingTime">남은 시간(초)</param>
    public void UpdateCountdownUI(float remainingTime)
    {
        if (countdownText != null)
        {
            // 남은 시간을 TimeSpan으로 변환 (TimeSpan은 전체 시간을 "분", "초", "밀리초" 등으로 분리해줍니다)
            TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
            // "mm:ss:fff" 형식으로 변환하여 텍스트에 할당 (mm: 분, ss: 초, fff: 밀리초)
            countdownText.text = timeSpan.ToString(@"mm\:ss\:fff");
        }
        else
        {
            Debug.LogWarning("CountdownText가 할당되지 않았습니다.");
        }
    }
    
    // Slot select logic in UI
    public void SelectSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slotImages.Length) return;
        
        slotImages[selectedSlotIndex].color = unhighlightedSlotColor;
        
        selectedSlotIndex = slotIndex;
        slotImages[slotIndex].color = highlightedSlotColor;
    }
    
}