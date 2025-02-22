using UnityEngine;
using UnityEngine.UI;

public class ResistantShield : MonoBehaviour
{
    [SerializeField] private Image shieldImage;
    
    private Transform target;

    /// <summary>
    /// Set the transform of target to follow
    /// </summary>
    /// <param name="targetTransform">대상 Transform</param>
    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    public void SetColor(Color color)
    {
        shieldImage.color = color;
    }

    private void Update()
    {
        if (target != null)
        {
            // Translate to screen position
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
            // Update the position of RectTransform(screen position of Screen Space Overlay)
            (transform as RectTransform).position = screenPos;
        }
    }
}