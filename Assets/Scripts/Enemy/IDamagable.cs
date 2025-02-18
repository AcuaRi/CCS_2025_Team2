using UnityEngine;

public interface IDamageable
{
    void GetDamaged(float damage, MedicineType medicineType, Vector2 force);
    void GetDamaged(float damage);
}
