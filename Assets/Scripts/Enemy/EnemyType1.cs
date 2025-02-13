using UnityEngine;

public class EnemyType1 : GeneralEnemy
{
    public override void GetDamaged(float damage, MedicineType medicineType)
    {
        if(damage <= 0) return;

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
        
        //Debug.Log(generalMonsterData.goodMedicineTypes);
        //Debug.Log(generalMonsterData.badMedicineTypes);
        //Debug.Log(generalMonsterData.resistantMedicineType);
        //Debug.Log("CaculatedDamage: " + caculatedDamage);
        generalMonsterData.hp -= caculatedDamage;
        generalMonsterData.hp = Mathf.Clamp(generalMonsterData.hp, 0, maxHp);
        
        if (hpGaugeInstance != null && !hpGaugeInstance.gameObject.activeSelf)
        {
            hpGaugeInstance.gameObject.SetActive(true);
        }
        
        UpdateHpGauge();

        
        if ( generalMonsterData.hp <= 0)
        {
            Destroy(hpGaugeInstance.gameObject);
            Destroy(this.gameObject);
        }
    }
}
