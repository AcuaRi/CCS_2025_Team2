using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Pool;
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
    [Tooltip("Damage Numerical Effect Prefab")]
    [SerializeField] private GameObject damageNumericalEffectPrefab;
    
    [Header("UI Elements")]
    [Tooltip("BodyHpGauge Image")]
    [SerializeField] private Image bodyHpGaugeImage;
    [Tooltip("BodyHpGauge Text")]
    [SerializeField] private TextMeshProUGUI bodyHpGaugeText;
    [Tooltip("Enemy HpGaugeParent Transform")]
    [SerializeField] private Transform EnemyHpGaugeParent;
    [Tooltip("Resistant Shield Parent Transform")]
    [SerializeField] private Transform resistantShieldParent;
    [Tooltip("Damage Numerical Effect Parent Transform")]
    [SerializeField] private Transform damageNumericalEffectParent;
    [Tooltip("Countdown text (TextMeshProUGUI)")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [Tooltip("Slot Image")]
    [SerializeField] private Image[] slotImages;
    [Tooltip("Color of the slot highlighted")]
    [SerializeField] private Color highlightedSlotColor;
    [Tooltip("Color of the slot unhighlighted")]
    [SerializeField] private Color unhighlightedSlotColor;
    [Tooltip("GameObject of the pausePanel")]
    [SerializeField] private GameObject pausePanel;
    [Tooltip("GameObject of the warningPanel")]
    [SerializeField] private GameObject warningPanel;
    [Tooltip("GameObject of the warningPanel")]
    [SerializeField] private GameObject UIInfoPanel;
    
    private int selectedSlotIndex = 0;
    
    private ObjectPool<GameObject> damageEffectPool;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        damageEffectPool = new ObjectPool<GameObject>(
            CreateEffect,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyEffect,
            maxSize: 200
        );
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
            TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
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

    public void SetPausePanel(bool isPaused)
    {
        pausePanel.SetActive(isPaused);
    }
    
    public void ShowWarning(string message, float duration)
    {
        warningPanel.SetActive(true);
        var warn = warningPanel.GetComponent<WarningPanel>();
        warn.SetWarningText(message);
        warn.SetDuration(duration);
        StartCoroutine(warn.WarningRoutine(duration));
    }

    public UIInfoPanel GetUIInfoPanel()
    {
        return UIInfoPanel.GetComponent<UIInfoPanel>();
    }
    
    private GameObject CreateEffect()
    {
        var effect = Instantiate(damageNumericalEffectPrefab, damageNumericalEffectParent);
        effect.GetComponent<DamageNumericalEffect>().Initialize(damageEffectPool);
        return effect;
    }

    private void OnTakeFromPool(GameObject effect)
    {
        effect.SetActive(true);
    }

    private void OnReturnToPool(GameObject effect)
    {
        effect.SetActive(false);
    }

    private void OnDestroyEffect(GameObject effect)
    {
        Destroy(effect);
    }

    public void ShowDamageEffect(float damage, int damageType, Transform target)
    {
        var effect = damageEffectPool.Get().GetComponent<DamageNumericalEffect>();
        effect.SetDamageNumericalEffect(damage, damageType, target);
    }
}