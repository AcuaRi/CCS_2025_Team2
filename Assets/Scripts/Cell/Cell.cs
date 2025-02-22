using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Cell : MonoBehaviour, IDamageable
{
    // 細胞の初期HP（希望する値に設定）
    [SerializeField] private float hp = 50f;
    private float currentHp;
    
    // 揺れの強さ（お好みの値に調整）
    [SerializeField] private float shakeAmount = 0.25f;

    // 被撃効果重複実行防止用フラッグ
    private bool _isHitEffectPlaying = false;

    [SerializeField] private int recoveryTime = 30;
    private int _recoveryTimer = 0;
    public int RecoveryTimer => _recoveryTimer;

    private Color _originalColor;
    private Vector3 _originalPosition;
    private SpriteRenderer _sr;
    
    private void Start()
    {
        currentHp = hp;
        _originalPosition = transform.position;
        _originalColor = GetComponent<SpriteRenderer>().color;
        _sr = GetComponent<SpriteRenderer>();
    }

    public void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        MedicineType resistantType1 = (MedicineType)(1 << 0 | 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5);
        
        if ((medicineType & resistantType1) != MedicineType.None)
        {
            return;
        }
        else
        {
            GetDamaged(damage);
        }
    }

    /// <summary>
    /// ダメージを受けてHPを減少させ、HPが0以下であればオブジェクトを破壊
    /// ダメージを受けるたびに攻撃効果（赤シフト、揺れ）を実行
    /// </summary>
    /// <param name="damage">入力ダメージ値</param>
    public void GetDamaged(float damage)
    {
        if(damage <= 0) return;
        
        
        currentHp -= damage;
        //Debug.Log("ダメージを受けました。 現在HP: " + hp);

        if (currentHp <= 0f)
        {
            DestroyCell();
        }
        
        if (!_isHitEffectPlaying && this.gameObject.activeSelf)
        {
            StartCoroutine(HitEffect());
        }
    }

    /// <summary>
    /// 細胞オブジェクトを破壊する関数
    /// </summary>
    private void DestroyCell()
    {
        transform.position = _originalPosition;
        _sr.color = _originalColor;
        _isHitEffectPlaying = false;
        ResetRecoveryTimer();
        this.gameObject.SetActive(false);
    }

    public void ResetRecoveryTimer()
    {
        _recoveryTimer = recoveryTime;
    }
    
    public void DecreaseRecoveryTimer(int seconds)
    {
        _recoveryTimer -= seconds;
        _recoveryTimer = Mathf.Clamp(_recoveryTimer, 0, int.MaxValue);
    }
    
    public void RecoveryCell()
    {
        currentHp = hp;
        
        this.gameObject.SetActive(true);
        _recoveryTimer = 0;
    }

    /// <summary>
    /// 被撃時に赤になって揺れ、0.25秒後に元の状態に戻る効果のコルチン
    /// </summary>
    private IEnumerator HitEffect()
    {
        _isHitEffectPlaying = true;
        
        if (_sr == null)
        {
            yield break;
        }
        
        _sr.color = Color.red;

        float effectDuration = 0.25f;
        float elapsed = 0f;

        while (elapsed < effectDuration)
        {
            yield return new WaitUntil(() => Time.timeScale > 0);
            
            Vector2 randomOffset = Random.insideUnitCircle * shakeAmount;
            transform.position = _originalPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = _originalPosition;
        _sr.color = _originalColor;

        _isHitEffectPlaying = false;
    }
}
