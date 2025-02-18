using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float _playerHP;
    private float _currentHP;
    [SerializeField]
    private float _playerSpeed; // �㉺�ƍ��E�ő�����ς��邩��
    private Rigidbody2D _rb;
    private Vector2 _input;

    public GameObject bulletPrefab;
    /*[SerializeField]
    private int poolSize = 20;*/
    private Vector3 bulletPoint;
    
    //Stop Player for 10secs when hp < 0
    [SerializeField] private float reviveTime = 10f;
    private bool isAlive = true;

    [SerializeField] private float invincibleTime = 0.5f;
    private bool isInvincible = false;
    private SpriteRenderer _sr;
    private Color _originalColor;
    
    private HPGauge hpGaugeInstance;

    // private Queue<GameObject> bulletPool = new Queue<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        bulletPoint = transform.Find("BulletPoint").localPosition;

        _sr = GetComponentInChildren<SpriteRenderer>();
        _originalColor = _sr.color;
        _currentHP = _playerHP;
        /*for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }*/
    }

    private void Start()
    {
        Vector3 initialScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (UIManager.Instance != null)
        {
            hpGaugeInstance = UIManager.Instance.GetHpGauge(initialScreenPos);
            hpGaugeInstance.gameObject.SetActive(false);
            hpGaugeInstance.SetTarget(transform);
        }
        UpdateHpGauge();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
        _Move();
    }

    public void _OnMove(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    }

    public void _OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(!isAlive) return;
            
            GameObject b = Instantiate(bulletPrefab, transform.position + bulletPoint, Quaternion.identity);
            b.GetComponent<Bullet>().getVector(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void _Move()
    {
        _rb.velocity = new Vector2(_input.x * _playerSpeed, _input.y * _playerSpeed);
    }

    public float GetPlayerHP()
    {
        return _playerHP;
    }

    public void GetDamaged(float damage, MedicineType medicineType, Vector2 force)
    {
        GetDamaged(damage);
    }

    public void GetDamaged(float damage)
    {
        if(!isAlive || isInvincible) return;
        
        _currentHP -= damage;
        
        if (hpGaugeInstance != null && !hpGaugeInstance.gameObject.activeSelf)
        {
            hpGaugeInstance.gameObject.SetActive(true);
        }
        
        UpdateHpGauge();
        
        if (_currentHP <= 0)
        {
            _currentHP = 0;
            isAlive = false;
            StopForRevive();
        }
        else
        {
            StartInvincibility();
        }
    }

    private void StartInvincibility()
    {
        isInvincible = true;
        _sr.color = Color.yellow;
        Invoke("EndInvincibility", invincibleTime);
    }
    
    private void EndInvincibility()
    {
        isInvincible = false;
        _sr.color = _originalColor;
    }
    
    private void StopForRevive()
    {
        _sr.color = Color.black;
        Invoke("Revive", reviveTime);
    }

    private void Revive()
    {
        _currentHP = _playerHP;
        UpdateHpGauge();
        isAlive = true;
        StartInvincibility();
    }
    
    private void UpdateHpGauge()
    {
        if (hpGaugeInstance != null)
        {
            float percent = _currentHP / _playerHP;
            hpGaugeInstance.SetHpGauge(percent);
        }
    }
}
