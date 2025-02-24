using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Bacillus,
    Rensa,
    MSSA,
    MRSA,
    E_Coli,
    Roku,
    Influenza,
    MaicoPlazma,
    Kurosuto,
    Pesto
}

[System.Serializable]
public class EnemySpawnInfo
{
    public EnemyType enemyType; 
    public int count;       
    public float interval; 
}

[CreateAssetMenu(fileName = "WaveData", menuName = "Game/WaveData")]
public class WaveData : ScriptableObject
{
    public List<EnemySpawnInfo> enemies;
    public float waveStartTime;
}