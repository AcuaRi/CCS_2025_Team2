using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DamageNumericalEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private Color goodColor;
    [SerializeField] private Color badColor;
    [SerializeField] private Color resistantColor;
    [SerializeField] private Color defaultColor;
    
    private Vector3 target;
    public Vector3 worldOffset = new Vector3(1f, 1f, 0f);
    private ObjectPool<GameObject> pool;
    
    public void Initialize(ObjectPool<GameObject> objectPool)
    {
        pool = objectPool;
    }
    
    public void SetDamageNumericalEffect(float damage, int damageType, Transform parent)
    {
        target = parent.position;
        damageText.text = $"{damage}";
        
        switch (damageType)
        {
            case 1:     //good damage type
                damageText.color = goodColor;
                damageText.text += "!!";
                break;
            case 2:     //bad damage type
                damageText.color = badColor;
                break;
            case 3:     //resistant damage type
                damageText.color = resistantColor;
                break;
            default:    //default damage type
                damageText.color = defaultColor;
                break;
        }
        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target + worldOffset);
        (transform as RectTransform).position = screenPos;
        
        StartCoroutine(AnimateAndReturn());
    }

    private IEnumerator AnimateAndReturn()
    {
        float duration = .5f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target + worldOffset);
            (transform as RectTransform).position = screenPos;
            
            transform.position += Vector3.Lerp(Vector3.zero, new Vector3(0f, 50f, 0f), t);
            //canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            damageText.color = new Color(
                damageText.color.r, 
                damageText.color.g,
                damageText.color.b,
                Mathf.Lerp(255f, 0f, t));

            yield return null;
        }
        
        pool.Release(this.gameObject);
    }
}
