using UnityEngine;

public class Enemy_MaicoPlazma : GeneralEnemy
{
    protected override void RegisterDisease()
    {
        DiseaseManager.Instance.Register(9);
    }

    protected override void UnregisterDisease()
    {
        DiseaseManager.Instance.Unregister(9);
    }

    public override void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        base.GetDamaged(damage, medicineType, force);
    }
}