using UnityEngine;

public class Enemy_MRSA : GeneralEnemy
{
    protected override void RegisterDisease()
    {
        DiseaseManager.Instance.Register(3);
    }

    protected override void UnregisterDisease()
    {
        DiseaseManager.Instance.Unregister(3);
    }

    public override void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        base.GetDamaged(damage, medicineType, force);
    }
}