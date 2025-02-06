using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour
{
    // 細胞の初期HP（希望する値に設定）
    public float hp = 100f;

    // 揺れの強さ（お好みの値に調整）
    public float shakeAmount = 0.25f;

    // 被撃効果重複実行防止用フラッグ
    private bool isHitEffectPlaying = false;

    /// <summary>
    /// ダメージを受けてHPを減少させ、HPが0以下であればオブジェクトを破壊
    /// ダメージを受けるたびに攻撃効果（赤シフト、揺れ）を実行
    /// </summary>
    /// <param name="damage">入力ダメージ値</param>
    public void GetDamaged(float damage)
    {
        hp -= damage;
        Debug.Log("ダメージを受けました。 現在HP: " + hp);

        if (!isHitEffectPlaying)
        {
            StartCoroutine(HitEffect());
        }

        if (hp <= 0f)
        {
            DestroyCell();
        }
    }

    /// <summary>
    /// 細胞オブジェクトを破壊する関数
    /// </summary>
    private void DestroyCell()
    {
        //Debug.Log("세포가 파괴되었습니다!");
        Destroy(gameObject);
    }

    /// <summary>
    /// 被撃時に赤になって揺れ、0.25秒後に元の状態に戻る効果のコルチン
    /// </summary>
    private IEnumerator HitEffect()
    {
        isHitEffectPlaying = true;
        
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            yield break;
        }
        
        Color originalColor = sr.color;
        Vector3 originalPosition = transform.position;
        
        sr.color = Color.red;

        float effectDuration = 0.25f;
        float elapsed = 0f;

        while (elapsed < effectDuration)
        {
            Vector2 randomOffset = Random.insideUnitCircle * shakeAmount;
            transform.position = originalPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = originalPosition;
        sr.color = originalColor;

        isHitEffectPlaying = false;
    }
}
