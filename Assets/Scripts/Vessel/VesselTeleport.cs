using UnityEngine;

public class VesselTeleport : MonoBehaviour
{
    [SerializeField] private Transform teleportTo;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 22) //Enemy(Passed)
        {
            other.transform.position = new Vector3(teleportTo.position.x, other.transform.position.y, other.transform.position.z);
            other.GetComponent<IDamageable>().GetDamaged(30f, MedicineType.Medicine7);
            
            //Remove This
            GameManager.Instance.GetDamagedInBody(1f);
        }
    }
}
