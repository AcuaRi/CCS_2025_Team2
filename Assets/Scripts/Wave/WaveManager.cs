using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [SerializeField] private List<WaveData> waveDataList;
    [SerializeField] private float alertTime;
    private int currentWaveIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    

    public IEnumerator NextWave(float currentTime)
    {
        if (currentWaveIndex > waveDataList.Count - 1)
        {
            yield break;
        }
        
        var nextWave = waveDataList[currentWaveIndex];
        var waitTime = (nextWave.WaveStartTime <= currentTime) ? 0 : nextWave.WaveStartTime - currentTime;
        
        //대기 시간 동안 지정한 범위의 몬스터를 랜덤하게 소환하는 로직
        StartCoroutine(StageBeforeWave(nextWave, waitTime));
        
        if (waitTime >= alertTime)
        {
            waitTime = waitTime - alertTime;
        }
        else
        {
            alertTime = waitTime;
            waitTime = 0;
        }
        
        yield return new WaitForSeconds(waitTime);
        
        // add alert logic at here
        UIManager.Instance.ShowWarning($"== Wave {currentWaveIndex + 1}/{waveDataList.Count} ==", alertTime);
        SoundManager.Instance.PlaySound($"Alert_before_wave{currentWaveIndex+1}", this.transform.position);
        
        yield return new WaitForSeconds(alertTime);
        
        StartWave(nextWave);
        
        currentWaveIndex++;
    }

    private void StartWave(WaveData waveData)
    {
        List<WaveInfo> alwaysSpawnList = new List<WaveInfo>();
        List<WaveInfo> randomSpawnList = new List<WaveInfo>();

        foreach (var waveInfo in waveData.WaveInfos)
        {
            if (waveInfo.IsRandom == true)
            {
                randomSpawnList.Add(waveInfo);
            }
            else
            {
                alwaysSpawnList.Add(waveInfo);
            }
        }
        
        foreach (var waveInfo in alwaysSpawnList)
        {
            int calculatedIterations = (int)Math.Floor(waveData.WaveDuration / waveInfo.Interval);
            EnemyGenerator.Instance.GenerateEnemy(waveInfo.EnemyType.ToString(), waveInfo.NumOnce, waveInfo.Interval, calculatedIterations);
        }
        
        if (randomSpawnList.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, randomSpawnList.Count);
            var selectedWaveInfo = randomSpawnList[randomIndex];

            int calculatedIterations = (int)Math.Floor(waveData.WaveDuration / selectedWaveInfo.Interval);
            EnemyGenerator.Instance.GenerateEnemy(selectedWaveInfo.EnemyType.ToString(), selectedWaveInfo.NumOnce, selectedWaveInfo.Interval, calculatedIterations);
        }
    }

    private IEnumerator StageBeforeWave(WaveData waveData, float duration)
    {
        List<WaveInfo> alwaysSpawnList = new List<WaveInfo>();
        List<WaveInfo> randomSpawnList = new List<WaveInfo>();

        foreach (var stageInfo in waveData.StageInfos)
        {
            if (stageInfo.IsRandom == true)
            {
                randomSpawnList.Add(stageInfo);
            }
            else
            {
                alwaysSpawnList.Add(stageInfo);
            }
        }
        
        foreach (var stageInfo in alwaysSpawnList)
        {
            int calculatedIterations = (int)Math.Floor(duration / stageInfo.Interval);
            EnemyGenerator.Instance.GenerateEnemy(stageInfo.EnemyType.ToString(), stageInfo.NumOnce, stageInfo.Interval, calculatedIterations);
        }
        
        if (randomSpawnList.Count > 0)
        {
            float elapsedTime = 0f;
            float spawnInterval = 1f;

            while (elapsedTime < duration)
            {
                int randomIndex = UnityEngine.Random.Range(0, randomSpawnList.Count);
                var selectedWaveInfo = randomSpawnList[randomIndex];

                // ✅ 적 소환 실행
                EnemyGenerator.Instance.GenerateEnemy(selectedWaveInfo.EnemyType.ToString(), 1, 0, 1);

                yield return new WaitForSeconds(spawnInterval);
                elapsedTime += spawnInterval;
            }
        }
        
        
    }
}