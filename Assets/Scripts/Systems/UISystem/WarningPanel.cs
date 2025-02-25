using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarningPanel : MonoBehaviour
{
    [SerializeField] private Sprite[] warningSprites;
    [SerializeField] private float blinkInterval;
    
    private float duration = 2f;
    
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textMesh;
    
    public IEnumerator WarningRoutine(float duration)
    {
        float elapsedTime = 0f;
        bool isSprite1 = false;

        while (elapsedTime < duration)
        {
            //Debug.Log(elapsedTime);
            image.sprite = isSprite1 ? warningSprites[0] : warningSprites[1];
            isSprite1 = !isSprite1;

            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }
        
        this.gameObject.SetActive(false);
    }

    public void SetWarningText(string text)
    {
        textMesh.text = text;
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }
}
