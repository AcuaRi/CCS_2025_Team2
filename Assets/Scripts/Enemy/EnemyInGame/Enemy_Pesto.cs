using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pesto : GeneralEnemy
{
    protected override void RegisterDisease()
    {
        DiseaseManager.Instance.Register(4);
    }

    protected override void UnregisterDisease()
    {
        DiseaseManager.Instance.Unregister(4);
    }

    public override void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        base.GetDamaged(damage, medicineType, force);
    }
}
