using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryWaveEffect : MonoBehaviour
{
    [SerializeField] private GameObject wavePrefab; // 파장 이펙트 프리팹
    [SerializeField] private float waveRadius = 1f; // 초기 크기
    [SerializeField] private float maxWaveSize = 5f; // 최대 크기
    [SerializeField] private float waveDuration = 0.9f; // 파장이 커지는 시간
    [SerializeField] private float waveDelay = 1f; // 파장 간격
    [SerializeField] private int waveDamage = 10; // 데미지
    [SerializeField] private LayerMask enemyLayer; // 적 탐색 레이어

    public void StartVictoryEffect()
    {
        StartCoroutine(WaveEffectSequence());
    }

    private IEnumerator WaveEffectSequence()
    {
        for (int i = 0; i < 3; i++) // 3개의 파장 실행
        {
            if (i == 0)
            {
                wavePrefab.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0.5f);
            }
            else if (i == 1)
            {
                
                wavePrefab.GetComponent<SpriteRenderer>().color = new Color(1, 0.92f, 0.016f, 0.5f);
            }
            else
            {
                wavePrefab.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.5f);
            }
            StartCoroutine(CreateWave());
            
            yield return new WaitForSecondsRealtime(waveDelay);
        }
    }

    private IEnumerator CreateWave()
    {
        GameObject wave = Instantiate(wavePrefab, GameObject.Find("Player").transform.position, Quaternion.identity);
        float elapsedTime = 0f;
        HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>(); // 각 파장마다 새로운 hitEnemies 리스트

        while (elapsedTime < waveDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; 
            float progress = elapsedTime / waveDuration;
            float currentRadius = Mathf.Lerp(waveRadius, maxWaveSize, progress);
            wave.transform.localScale = new Vector3(currentRadius, currentRadius, 1f);

            ApplyDamageToEnemies(currentRadius, hitEnemies);
            yield return null;
        }

        Destroy(wave);
    }

    private void ApplyDamageToEnemies(float radius, HashSet<Collider2D> hitEnemies)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            
            if (!hitEnemies.Contains(enemy))
            {
                hitEnemies.Add(enemy);
                enemy.gameObject.GetComponentInParent<IDamageable>()?.GetDamaged(waveDamage, MedicineType.None, Vector2.zero);
            }
        }
    }
}
