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

    public GameObject bullet;
    private Vector3 bulletPoint;
    
    //Stop Player for 10secs when hp < 0
    [SerializeField] private float reviveTime = 10f;
    private bool isAlive = true;

    [SerializeField] private float invincibleTime = 0.5f;
    private bool isInvincible = false;
    private SpriteRenderer _sr;
    private Color _originalColor;
    
    private HPGauge hpGaugeInstance;
    private bool _isShooting;
    private Coroutine _shootingCoroutine;

    [SerializeField]
    private MedicineType _medicineType = MedicineType.Medicine1; // �����l
    private int _medicineNum = 1;
    // �������牺�͒e���ꂼ��̏��A�f�[�^�V�X�e���I�Ȃ̂ɑg�ݍ��ނ���
    [SerializeField]
    private GameObject[] bulletPrefabs = new GameObject[10];
    [SerializeField]
    private float[] _firingrate = new float[10];
    [SerializeField]
    private ShootingMode[] shootingModes = new ShootingMode[10];

    private ShootingMode _currentMode = ShootingMode.Single; // �����l��Single�ɐݒ�

    // �ˌ����[�h�̗񋓌^�A������������Ȃ���������
    public enum ShootingMode
    {
        Single,       // 1���� (type0)
        Triple,       // 3���� (type1)
        Omni         // 8���� (type2)
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        bulletPoint = transform.Find("BulletPoint").localPosition;

        _sr = GetComponentInChildren<SpriteRenderer>();
        _originalColor = _sr.color;
        _currentHP = _playerHP;
        _isShooting = false;
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
        _medicineChoice();
        // Debug.Log(_isCoroutine);

        if (_isShooting && _shootingCoroutine == null)
        {
            switch (_currentMode)
            {
                case ShootingMode.Single:
                    _shootingCoroutine = StartCoroutine(shootingType0());
                    break;
                case ShootingMode.Triple:
                    _shootingCoroutine = StartCoroutine(shootingType1());
                    break;
                case ShootingMode.Omni:
                    _shootingCoroutine = StartCoroutine(shootingType2());
                    break;
            }
        }
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
            _isShooting = true;
            /*GameObject b = Instantiate(bullet, transform.position + bulletPoint, Quaternion.identity);
            b.GetComponent<Bullet>().getVector(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));*/
        }
    }

    public void _OnRelease(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _isShooting= false;
            if (_shootingCoroutine != null) // �R���[�`���������Ă�����~�߂�
            {
                StopCoroutine(_shootingCoroutine);
                _shootingCoroutine = null;
            }
        }
    }

    public void _OnBoth(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // �����ǂ�����?
        }
    }

    private void _Move()
    {
        _rb.velocity = new Vector2(_input.x * _playerSpeed, _input.y * _playerSpeed);
    }

    private void _medicineChoice() // Player�N���X���ł��Ȃ��Ă����������A�e��type�������Ă��Ȃ��ƃ_���[�W���荢��̂Œe�ɂ��^�C�v����
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            _medicineType = MedicineType.Medicine1;
            _medicineNum = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            _medicineType = MedicineType.Medicine2;
            _medicineNum = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            _medicineType = MedicineType.Medicine3;
            _medicineNum = 3;
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            _medicineType = MedicineType.Medicine4;
            _medicineNum = 4;
        }
        bullet = bulletPrefabs[_medicineNum - 1];
        _currentMode = shootingModes[_medicineNum - 1];
    }

    IEnumerator shootingType0()
    {
        while (_isShooting)
        {
            GameObject b = Instantiate(bullet, transform.position + bulletPoint, Quaternion.identity);
            b.GetComponent<Bullet>().getVector(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            yield return new WaitForSeconds(_firingrate[_medicineNum - 1]);
        }
        _shootingCoroutine = null;
    }

    IEnumerator shootingType1()
    {
        while (_isShooting)
        {
            // �v���C���[����}�E�X�ւ̕������擾�i���K���j
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f; // 2D�Q�[���̏ꍇ�AZ���͖���
            Vector3 shootDirection = (mousePos - transform.position).normalized;

            // ���ˊp�x�����߂�i�v���C���[���猩���}�E�X�̕����j
            float baseAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

            // 3�����ɔ��ˁi-30�x, 0�x, +30�x�j
            float[] angles = { -30f, 0f, 30f };

            foreach (float angleOffset in angles)
            {
                float finalAngle = baseAngle + angleOffset;
                Vector3 bulletDirection = new Vector3(Mathf.Cos(finalAngle * Mathf.Deg2Rad), Mathf.Sin(finalAngle * Mathf.Deg2Rad), 0f);

                GameObject b = Instantiate(bullet, transform.position + bulletPoint, Quaternion.identity);
                b.GetComponent<Bullet>().getVector(transform.position, transform.position + bulletDirection);
            }

            yield return new WaitForSeconds(_firingrate[_medicineNum - 1]);
        }
        _shootingCoroutine = null;
    }

    IEnumerator shootingType2()
    {
        while (_isShooting)
        {
            // 8�����̔��ˊp�x�i0�x, 45�x, 90�x, ..., 315�x�j
            float[] angles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };

            foreach (float angle in angles)
            {
                // �p�x���x�N�g���ɕϊ�
                Vector3 bulletDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

                // �e�𐶐����A������ݒ�
                GameObject b = Instantiate(bullet, transform.position + bulletPoint, Quaternion.identity);
                b.GetComponent<Bullet>().getVector(transform.position, transform.position + bulletDirection);
            }

            yield return new WaitForSeconds(_firingrate[_medicineNum - 1]);
        }
        _shootingCoroutine = null;
    }

    public int GetPlayerHP()
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
