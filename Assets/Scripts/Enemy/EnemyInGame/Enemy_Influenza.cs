using UnityEngine;

public class Enemy_Influenza : GeneralEnemy
{
    protected override void RegisterDisease()
    {
        DiseaseManager.Instance.Register(5);
    }

    protected override void UnregisterDisease()
    {
        DiseaseManager.Instance.Unregister(5);
    }

    public override void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        base.GetDamaged(damage, medicineType, force);
    }
}