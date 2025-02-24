using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera targetCamera; // 조작할 카메라
    [SerializeField] private float zoomSpeed = 2f; // 줌 속도
    [SerializeField] private float minZoom = 3f; // 최소 줌 (최대로 확대)
    [SerializeField] private float maxZoom = 10f; // 최대 줌 (최대로 축소)

    private void Update()
    {
        if (targetCamera == null) return;

        // 마우스 우클릭이 눌려 있을 때만 줌 조절 가능
        if (Input.GetMouseButton(1))
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel"); // 마우스 휠 입력값
            if (scroll != 0)
            {
                targetCamera.orthographicSize -= scroll * zoomSpeed;
                targetCamera.orthographicSize = Mathf.Clamp(targetCamera.orthographicSize, minZoom, maxZoom);
            }
        }
    }
}