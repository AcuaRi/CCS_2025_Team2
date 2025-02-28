using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("回転")]
    [SerializeField]
    protected bool _rotationbool = false;
    [SerializeField]
    private int _rotationSpeed = 1000;
    [Header("基本設定")]
    [SerializeField]
    protected float speed = 0.1f;
    [SerializeField]
    protected float deleteDistance = 30 * 30;
    [SerializeField]
    public float firingRate;
    [SerializeField]
    public Player.ShootingMode shootingMode;
    [SerializeField]
    protected float _damageToEnemy = 10.0f;
    [SerializeField]
    protected float _damageToCell = 10.0f;
    //public float lifeTime = 5f;

    protected GameObject playerObj = null;
    
    protected MedicineType _medicineType = MedicineType.None;
    
    protected float distance_P;
    protected Vector3 startingPoint;
    protected Vector3 addVector;
    protected Vector3 direction;
    
    protected virtual void Update()
    {
        transform.position += Time.deltaTime * speed * addVector;
        if (_rotationbool)
        {
            transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
        }
        if (_checkDistance())
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Start()
    {
        playerObj = GameObject.Find("Player"); // ��肭�����Ȃ�������G���[�f���Ăق���
        setStartPoint(playerObj.transform.position);
        //addVector = 100 * new Vector3(direction.x * Time.deltaTime, direction.y * Time.deltaTime, 0);
        addVector = new Vector3(direction.x, direction.y, 0);
        addVector.Normalize();
        //Debug.Log(addVector);
        
        SoundManager.Instance.PlaySound("Shoot", transform.position);
    }

    protected bool _checkDistance()
    {
        distance_P = (startingPoint - transform.position).sqrMagnitude;
        if (distance_P > deleteDistance)
        {
            return true;
        }
        return false;
    }

    public void getVector(Vector3 from, Vector3 to)
    {
        direction = new Vector3(to.x - from.x, to.y - from.y, to.z - from.z);
    }

    public void setStartPoint(Vector3 startPoint)
    {
        this.startingPoint = startPoint;
    }

    public void setMedicineType(MedicineType medicinetype) // 生成時にplayer側から呼び出す
    {
        this._medicineType = medicinetype;
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 21) //Enemy
        {
            var enemy = other.gameObject.GetComponent<IDamageable>();
            if (enemy != null)
            {
                //enemy.GetDamaged(10f, MedicineType.Medicine1, Vector2.zero);
                //enemy.GetDamaged(10f, SlotSelectMock.Instance.selectedMedicineType, 20f * (other.transform.position - transform.position).normalized);
                enemy.GetDamaged(_damageToEnemy, _medicineType, 20f * (other.transform.position - transform.position).normalized);

                //enemy.GetDamaged(10f, _medicineType, 0 * (other.transform.position - transform.position).normalized);
            }
        }
        
        if (other.gameObject.layer == 22) //Enemy
        {
            var enemy = other.gameObject.GetComponent<IDamageable>();
            if (enemy != null)
            {
                //enemy.GetDamaged(10f, SlotSelectMock.Instance.selectedMedicineType, Vector2.zero);
                enemy.GetDamaged(_damageToEnemy, _medicineType, Vector2.zero);
            }
        }

        if (other.gameObject.layer == 11) //Cell
        {
            var cell = other.gameObject.GetComponent<Cell>();
            if (cell != null)
            {
                //cell.GetDamaged(10f, SlotSelectMock.Instance.selectedMedicineType, Vector2.zero);
                cell.GetDamaged(_damageToCell, _medicineType, Vector2.zero);
            }
        }
        
    }
}
