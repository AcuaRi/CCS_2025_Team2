using UnityEngine;

public class Enemy_Roku : GeneralEnemy
{
    protected override void RegisterDisease()
    {
        DiseaseManager.Instance.Register(6);
    }

    protected override void UnregisterDisease()
    {
        DiseaseManager.Instance.Unregister(6);
    }

    public override void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        base.GetDamaged(damage, medicineType, force);
    }
}