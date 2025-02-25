using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Enemy_Bacillus,
    Enemy_Rensa,
    Enemy_MSSA,
    Enemy_MRSA,
    Enemy_E_Coli,
    Enemy_Roku,
    Enemy_Influenza,
    Enemy_MaicoPlazma,
    Enemy_Kurosuto,
    Enemy_Pesto,
}

[CreateAssetMenu(fileName = "WaveData", menuName = "Wave/WaveData")]
public class WaveData : ScriptableObject
{
    [SerializeField] protected List<WaveInfo> stageInfos;
    [SerializeField] protected List<WaveInfo> waveInfos;
    [SerializeField] protected float waveStartTime;
    [SerializeField] protected float waveDuration;
    
    public List<WaveInfo> StageInfos => stageInfos;
    public List<WaveInfo> WaveInfos => waveInfos;
    public float WaveStartTime => waveStartTime;
    public float WaveDuration => waveDuration;
}