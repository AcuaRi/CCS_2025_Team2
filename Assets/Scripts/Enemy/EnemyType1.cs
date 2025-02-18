using UnityEngine;

public class EnemyType1 : GeneralEnemy
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
        if(damage <= 0) return;
        if( currentState == deathState) return;
        
        
        float caculatedDamage = damage;
        
        //Case of resistant medicine
        if ((medicineType & generalMonsterData.resistantMedicineType) == medicineType)
        {
            caculatedDamage = damage / 10;
        }
        
        //Case of good medicine
        else if ((medicineType & generalMonsterData.goodMedicineTypes) == medicineType)
        {
            caculatedDamage = damage * 2;
        }
        
        //Case of bad medicine
        else if ((medicineType & generalMonsterData.badMedicineTypes) == medicineType)
        {
            caculatedDamage = damage / 2;
        }
        //default?
        else
        {
            
        }
        
        generalMonsterData.hp -= caculatedDamage;
        generalMonsterData.hp = Mathf.Clamp(generalMonsterData.hp, 0, maxHp);
        
        if (hpGaugeInstance != null && !hpGaugeInstance.gameObject.activeSelf)
        {
            hpGaugeInstance.gameObject.SetActive(true);
        }
        
        UpdateHpGauge();
        if (force.magnitude > 0.1f)
        {
            rb.AddForce(force, (ForceMode2D)ForceMode.Impulse);
        }
        
        if ( generalMonsterData.hp <= 0)
        {
            nextState = deathState;
        }
        
        //increase resistance of medicine
        if (medicineType != MedicineType.None)
        {
            medicineResistantProbablity[(int)Mathf.Log((int)medicineType, 2)] += 0.1f;
        }
    }
}
