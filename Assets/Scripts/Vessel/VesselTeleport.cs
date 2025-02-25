using UnityEngine;

public class VesselTeleport : MonoBehaviour
{
    [SerializeField] private Transform teleportTo;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 22) //Enemy(Passed)
        {
            var parent = other.transform.parent;
            parent.position = new Vector3(teleportTo.position.x, other.transform.position.y, other.transform.position.z);
            parent.GetComponent<IDamageable>().GetDamaged(5f);
        }
    }
}
