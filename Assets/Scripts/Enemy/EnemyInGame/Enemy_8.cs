using UnityEngine;

public class Enemy_8 : GeneralEnemy
{
    protected override void RegisterDisease()
    {
        DiseaseManager.Instance.Register(8);
    }

    protected override void UnregisterDisease()
    {
        DiseaseManager.Instance.Unregister(8);
    }

    public override void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        base.GetDamaged(damage, medicineType, force);
    }
}