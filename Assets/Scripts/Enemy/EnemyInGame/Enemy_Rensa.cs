using UnityEngine;

public class Enemy_Rensa : GeneralEnemy
{
    protected override void RegisterDisease()
    {
        DiseaseManager.Instance.Register(1);
    }

    protected override void UnregisterDisease()
    {
        DiseaseManager.Instance.Unregister(1);
    }

    public override void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        base.GetDamaged(damage, medicineType, force);
    }
}
