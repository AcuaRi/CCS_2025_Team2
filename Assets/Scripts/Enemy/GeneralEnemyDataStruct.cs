using UnityEngine;

[System.Serializable]
public struct GeneralEnemyDataStruct
{
    public float hp;
    
    public float moveSpeed;
    public Vector2 moveDirection;
    
    public float targetLossRadius;
    public float recognizeRadius;
    public LayerMask targetLayer;
    public Transform targetTransform;
    
    public float knockBackPower;
    public float attackSpeed;
    public float attackDamage;

    public float divisionCoolTime;
    public float divisionProbability;
    
    
    public MedicineType goodMedicineTypes;
    public MedicineType badMedicineTypes;
    public MedicineType resistantMedicineType;

    public int points;
}