using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiseaseManager : MonoBehaviour
{
    public static DiseaseManager Instance { get; private set; }

    [SerializeField] private List<int> counts = new List<int>();
    [SerializeField] private List<int> diseasethreshold = new List<int>();
    private List<bool> isDiseased;
    
    [SerializeField] private List<GameObject> diseasedSlots = new List<GameObject>();
    private List<Image> diseasedImages = new List<Image>();
    private List<TextMeshProUGUI> diseasedTexts = new List<TextMeshProUGUI>();
    
    
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
        isDiseased = new List<bool>();
        
        for (int i = 0; i < counts.Count; i++)
        {
            isDiseased.Add(false);
        }

        for (int i = 0; i < diseasedSlots.Count; i++)
        {
            diseasedImages.Add(diseasedSlots[i].GetComponentInChildren<Image>());
            diseasedTexts.Add(diseasedSlots[i].GetComponentInChildren<TextMeshProUGUI>());
            diseasedTexts[i].text = $"{counts[i]} / {diseasethreshold[i]}";
        }
        
    }

    public void Register(int index)
    {
        counts[index]++;
        
        diseasedTexts[index].text = $"{counts[index]} / {diseasethreshold[index]}";
        
        
        if (!isDiseased[index] && counts[index] >= diseasethreshold[index])
        {
            isDiseased[index] = true;
            diseasedTexts[index].color = Color.red;
        }
    }
    
    public void Unregister(int index)
    {
        counts[index] = Mathf.Max(counts[index] - 1, 0);
        diseasedTexts[index].text = $"{counts[index]} / {diseasethreshold[index]}";
        
        if (isDiseased[index] && counts[index] < diseasethreshold[index])
        {
            isDiseased[index] = false;
            diseasedTexts[index].color = Color.white;
        }
        
    }
    
    
}