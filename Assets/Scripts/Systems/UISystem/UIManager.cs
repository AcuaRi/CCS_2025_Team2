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
    [Tooltip("Resistant Shield Prefab")]
    [SerializeField] private GameObject resistantShieldPrefab;
    
    [Header("UI Elements")]
    [Tooltip("BodyHpGauge Image")]
    [SerializeField] private Image bodyHpGaugeImage;
    [Tooltip("BodyHpGauge Text")]
    [SerializeField] private TextMeshProUGUI bodyHpGaugeText;
    [Tooltip("Enemy HpGaugeParent Transform")]
    [SerializeField] private Transform EnemyHpGaugeParent;
    [Tooltip("Resistant Shield Parent Transform")]
    [SerializeField] private Transform resistantShieldParent;
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
    
    /// <summary>
    /// Instantiate ResistantShield Prefab at ResistantShieldUIParent and Return the ResistantShield Component
    /// </summary>
    /// <param name="position">initial position (screen position)</param>
    /// <returns>ResistantShield Component</returns>
    public ResistantShield GetResistantShield(Vector3 position)
    {
        if (screenSpaceCanvas == null)
        {
            Debug.LogError("Screen Space Canvas is null");
            return null;
        }
            
        GameObject gaugeObj = Instantiate(resistantShieldPrefab, position, Quaternion.identity, resistantShieldParent);
        ResistantShield resistantShield = gaugeObj.GetComponent<ResistantShield>();
            
        if (resistantShield == null)
        {
            Debug.LogError("No ResistantShield Component in the Prefab");
        }
            
        return resistantShield;
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
    /// Instantiate HPGauge Prefab at HPGaugeUIParent and Return the HPGauge Component
    /// </summary>
    /// <param name="position">initial position (screen position)</param>
    /// <returns>HPGauge Component</returns>
    public HPGauge GetHpGauge(Vector3 position)
    {
        if (screenSpaceCanvas == null)
        {
            Debug.LogError("Screen Space Canvas is null");
            return null;
        }
        
        GameObject gaugeObj = Instantiate(hpGaugePrefab, position, Quaternion.identity, EnemyHpGaugeParent);
        HPGauge hpGauge = gaugeObj.GetComponent<HPGauge>();
        if (hpGauge == null)
        {
            Debug.LogError("No HPGauge Component in the Prefab");
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