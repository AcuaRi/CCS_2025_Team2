using UnityEngine;
using UnityEngine.UI;

public class HPGauge : MonoBehaviour
{
    [SerializeField] private Image fillImage;       // 채워지는 이미지
    
    // 따라다닐 대상과 월드상의 오프셋 (예: 적의 위쪽)
    private Transform target;
    public Vector3 worldOffset = new Vector3(0f, 1f, 0f);

    /// <summary>
    /// 이 게이지가 따라갈 대상을 설정합니다.
    /// </summary>
    /// <param name="targetTransform">대상 Transform</param>
    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    /// <summary>
    /// 체력 비율(0.0 ~ 1.0)에 따라 게이지 UI를 업데이트합니다.
    /// </summary>
    public void SetHpGauge(float percent)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = percent;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // 대상의 월드 위치에 오프셋을 더한 후, 화면 좌표로 변환
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + worldOffset);
            // RectTransform의 위치 업데이트 (Screen Space Overlay에서는 RectTransform.position이 화면 좌표)
            (transform as RectTransform).position = screenPos;
        }
    }
}