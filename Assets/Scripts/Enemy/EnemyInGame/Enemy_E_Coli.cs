using UnityEngine;

public class Enemy_E_Coli : GeneralEnemy
{
    protected override void RegisterDisease()
    {
        DiseaseManager.Instance.Register(7);
    }

    protected override void UnregisterDisease()
    {
        DiseaseManager.Instance.Unregister(7);
    }

    public override void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        base.GetDamaged(damage, medicineType, force);
    }
}