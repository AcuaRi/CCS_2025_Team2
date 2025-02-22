using UnityEngine;

public class Enemy_MSMA : GeneralEnemy
{
    protected override void RegisterDisease()
    {
        DiseaseManager.Instance.Register(2);
    }

    protected override void UnregisterDisease()
    {
        DiseaseManager.Instance.Unregister(2);
    }

    public override void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        base.GetDamaged(damage, medicineType, force);
    }
}
