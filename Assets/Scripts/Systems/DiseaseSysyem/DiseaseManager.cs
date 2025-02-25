using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiseaseManager : MonoBehaviour
{
    public static DiseaseManager Instance { get; private set; }

    [SerializeField] private float damageInterval = 5f;
    [SerializeField] private List<int> counts = new List<int>();
    [SerializeField] private List<int> diseasethreshold = new List<int>();
    [SerializeField] private List<float> diseaseDamage = new List<float>();
    [SerializeField] private Image bloodScreenImage;
    private List<bool> isDiseased;
    private float sumDamage;
    
    [SerializeField] private List<GameObject> diseasedSlots = new List<GameObject>();
    private List<Image> diseasedImages = new List<Image>();
    private List<TextMeshProUGUI> diseasedTexts = new List<TextMeshProUGUI>();
    private Coroutine fadeCoroutine;
    
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
        sumDamage = 0;
        
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
        
        StartCoroutine(DamageLoop());
    }

    public void Register(int index)
    {
        counts[index]++;
        
        diseasedTexts[index].text = $"{counts[index]} / {diseasethreshold[index]}";
        
        
        if (!isDiseased[index] && counts[index] >= diseasethreshold[index])
        {
            isDiseased[index] = true;
            diseasedTexts[index].color = Color.red;
            sumDamage += diseaseDamage[index];
            //UpdateBloodScreenImage();
            SoundManager.Instance.PlaySound("Get_sick", transform.position);
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
            sumDamage -= diseaseDamage[index];
            //UpdateBloodScreenImage();
        }
        
    }

    private void UpdateBloodScreenImage()
    {
        float bloodScreenAlpha = (sumDamage >= 10f) ? 1f : sumDamage / 10f;
        bloodScreenImage.color = new Color(1f, 1f, 1f, bloodScreenAlpha);
    }

    IEnumerator DamageLoop()
    {
        while (true)
        {
            if (sumDamage > 0)
            {
                GameManager.Instance.GetDamagedInBody(sumDamage);
                
                UpdateBloodScreenImage();
                FadeOutBloodScreenImage();
            }
            
            yield return new WaitForSeconds(damageInterval);
        }
    }
    
    public void FadeOutBloodScreenImage()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeOutCoroutine());
    }
    
    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;
        Color startColor = bloodScreenImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsedTime < damageInterval)
        {
            elapsedTime += Time.deltaTime;
            bloodScreenImage.color = Color.Lerp(startColor, targetColor, elapsedTime / damageInterval);
            yield return null;
        }

        bloodScreenImage.color = targetColor;
        fadeCoroutine = null;
    }
    
    
}