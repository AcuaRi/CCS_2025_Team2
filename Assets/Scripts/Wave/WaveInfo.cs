using UnityEngine;

[CreateAssetMenu(fileName = "WaveInfo", menuName = "Wave/WaveInfo")]
public class WaveInfo : ScriptableObject
{
    public EnemyType EnemyType;
    public int NumOnce;
    public float Interval = 1;
    public bool IsRandom = false;
}