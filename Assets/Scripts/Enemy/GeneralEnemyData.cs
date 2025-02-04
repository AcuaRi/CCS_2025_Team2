using UnityEngine;

[CreateAssetMenu(fileName = "GeneralEnemyData", menuName = "EnemyData/GeneralEnemyData")]
public class GeneralEnemyData : ScriptableObject
{
    public GeneralEnemyDataStruct data;
    
    [Header("Hp Setting")] 
    public float hp;
    
    [Header("Move Setting")] 
    public float moveSpeed;
    public Vector2 moveDirection = Vector2.down + Vector2.right;
    
    [Header("Recognize Setting")]
    public float obstacleRaycastDistance;
    public float recognizeRadius;
    public LayerMask targetLayer;
    public Transform targetTransform;
    
    [Header("Attack Setting")]
    public float knockBackPower;
    public float attackSpeed;
    public float attackDamage;

    [Header("Division Setting")] 
    public float divisionCoolTime;
    public float divisionProbability;
    
    [Header("Medicine Setting")] [EnumFlags]  
    public MedicineType medicineType;
    
    //Sync SO class variable to struct data (for deep copy)
    public void SyncData()
    {
        data.hp = hp;

        data.moveSpeed = moveSpeed;
        data.moveDirection = moveDirection;
        
        data.obstacleRaycastDistance = obstacleRaycastDistance;
        data.recognizeRadius = recognizeRadius;
        data.targetLayer = targetLayer;
        data.targetTransform = null;
        
        data.knockBackPower = knockBackPower;
        data.attackSpeed = attackSpeed;
        data.attackDamage = attackDamage;
        
        data.divisionCoolTime = divisionCoolTime;
        data.divisionProbability = divisionProbability;
        
        data.medicineType = medicineType;
    }
}