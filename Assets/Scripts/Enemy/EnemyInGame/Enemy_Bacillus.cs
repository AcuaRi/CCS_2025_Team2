using UnityEngine;

public class Enemy_Bacillus : GeneralEnemy
{
    protected override void RegisterDisease()
    {
        DiseaseManager.Instance.Register(0);
    }

    protected override void UnregisterDisease()
    {
        DiseaseManager.Instance.Unregister(0);
    }

    public override void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        base.GetDamaged(damage, medicineType, force);
    }
}
