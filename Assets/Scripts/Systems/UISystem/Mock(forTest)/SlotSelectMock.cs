using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SlotSelectMock : MonoBehaviour
{
    public static SlotSelectMock Instance { get; private set; }
    
    public MedicineType selectedMedicineType;
    public event Action<MedicineType> OnMedicineTypeChanged;

    [Header("UI Settings")]
    [SerializeField] private GameObject[] lockUIPrefab;
    [SerializeField] private float unlockHoldTime = 2f;
    
    [Header("Points Settings")]
    [SerializeField] private int currentPoints = 0;
    [SerializeField] private int[] unlockCosts = new int[10];
    [SerializeField] private TextMeshProUGUI currentPointsText;
    [SerializeField] private int pointsPerSecond = 1;
    
    private Image[] unlockProgressImages;
    private TextMeshProUGUI[] unlockProgressTexts;
    
    private float[] holdTimers = new float[10];
    private bool[] slotUnlocked = new bool[10];
    private bool[] isFlashing = new bool[10];  
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Start()
    {
        int count = lockUIPrefab.Length;
        unlockProgressImages = new Image[count];
        unlockProgressTexts = new TextMeshProUGUI[count];
        
        for (int i = 0; i < count; i++)
        {
            unlockProgressImages[i] = lockUIPrefab[i].GetComponent<Image>();
            unlockProgressTexts[i] = lockUIPrefab[i].GetComponentInChildren<TextMeshProUGUI>();
            holdTimers[i] = 0f;
            slotUnlocked[i] = false;
            isFlashing[i] = false;
            unlockProgressTexts[i].text = unlockCosts[i].ToString();
        }
        
        UnlockSlot(0);
        UnlockSlot(2);

        UIManager.Instance.SelectSlot(0);
        selectedMedicineType = MedicineType.Medicine1;
        OnMedicineTypeChanged?.Invoke(selectedMedicineType);

        InvokeRepeating(nameof(AddPointsPerSecond), 1f, 1f);
    }
    
    void Update()
    {
        for (int i = 0; i <= 9; i++)
        {
            Key key = GetKeyForIndex(i + 1);
            
            if (Keyboard.current[key].isPressed)
            {
                if (slotUnlocked[i])
                {
                    if (Keyboard.current[key].wasPressedThisFrame)
                    {
                        UIManager.Instance.SelectSlot(i);
                        selectedMedicineType = (MedicineType)(1<<i);

                        OnMedicineTypeChanged?.Invoke(selectedMedicineType);
                    }
                }
                else
                {
                    if (currentPoints < unlockCosts[i])
                    {
                        if (!isFlashing[i])
                        {
                            StartCoroutine(FlashInsufficientText(i));
                        }
                        continue;
                    }
                    
                    holdTimers[i] += Time.deltaTime;
                    
                    if (holdTimers[i] >= unlockHoldTime)
                    {
                        UnlockSlot(i);
                        IncreaseUnlockCost(30);
                        UIManager.Instance.SelectSlot(i);
                        selectedMedicineType = (MedicineType)(1<<i);

                        OnMedicineTypeChanged?.Invoke(selectedMedicineType);
                    }
                }
            }
            else
            {
                holdTimers[i] = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < unlockProgressImages.Length; i++)
        {
            if (slotUnlocked[i] == false)
            {
                unlockProgressImages[i].fillAmount = Mathf.Clamp(1 - holdTimers[i] / unlockHoldTime, 0f, 1f);
            }
        }
        
        currentPointsText.text = currentPoints.ToString();
    }
    
    private Key GetKeyForIndex(int index)
    {
        switch (index)
        {
            case 1: return Key.Digit1;
            case 2: return Key.Digit2;
            case 3: return Key.Digit3;
            case 4: return Key.Digit4;
            case 5: return Key.Digit5;
            case 6: return Key.Digit6;
            case 7: return Key.Digit7;
            case 8: return Key.Digit8;
            case 9: return Key.Digit9;
            case 10: return Key.Digit0;
            default: return Key.None;
        }
    }
    
    private void UnlockSlot(int index)
    {
        currentPoints -= unlockCosts[index];
        slotUnlocked[index] = true;
        
        if (unlockProgressImages[index] != null)
        {
            unlockProgressImages[index].fillAmount = 0;
        }
        
        lockUIPrefab[index].SetActive(false);
    }
    
    private void AddPointsPerSecond()
    {
        currentPoints += pointsPerSecond;
    }
    
    // 포인트 부족 시 가격 텍스트가 깜빡이는 코루틴
    private IEnumerator FlashInsufficientText(int index)
    {
        isFlashing[index] = true;
        Color originalColor = unlockProgressTexts[index].color;
        // 깜빡임 효과: 빨간색으로 0.2초, 원래 색으로 0.2초 반복 (총 3회)
        int flashCount = 3;
        for (int i = 0; i < flashCount; i++)
        {
            unlockProgressTexts[index].color = Color.red;
            yield return new WaitForSeconds(0.2f);
            unlockProgressTexts[index].color = originalColor;
            yield return new WaitForSeconds(0.2f);
        }
        isFlashing[index] = false;
    }

    public void IncreaseCurrentPoints(int points)
    {
        currentPoints += points;
    }

    public void IncreaseUnlockCost(int points)
    {
        for(int i = 0; i < unlockCosts.Length; i++)
        {
            unlockCosts[i] += points;
        }

        for(int i = 0; i < unlockProgressTexts.Length; i++)
        {
            unlockProgressTexts[i].text = unlockCosts[i].ToString();
        }
    }
}
