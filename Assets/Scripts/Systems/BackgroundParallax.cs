using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public Transform player; 
    public float parallaxFactor = 0.2f; 
    public float smoothSpeed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (player == null) return;
        
        float targetY = startPosition.y - (player.position.y * parallaxFactor);
        
        //transform.position = Vector3.Lerp(transform.position, new Vector3(startPosition.x, targetY, startPosition.z), Time.deltaTime * smoothSpeed);
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
}