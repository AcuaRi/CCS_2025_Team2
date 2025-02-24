using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected float speed = 0.1f;
    [SerializeField]
    protected float deleteDistance = 30 * 30;
    //public float lifeTime = 5f;

    protected GameObject playerObj = null;

    protected float distance_P;
    protected Vector3 startingPoint;
    protected Vector3 addVector;
    protected Vector3 direction;
    protected Transform thisTransform;

    private void Update()
    {
        //Vector3 newPosition;
        //newPosition = thisTransform.position + addVector * speed;
        //transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
        transform.position += Time.deltaTime * speed * addVector;
        _checkDistance();
    }

    protected virtual void Start()
    {
        playerObj = GameObject.Find("Player"); // ��肭�����Ȃ�������G���[�f���Ăق���
        setStartPoint(playerObj.transform.position);
        thisTransform = transform;
        //addVector = 100 * new Vector3(direction.x * Time.deltaTime, direction.y * Time.deltaTime, 0);
        addVector = new Vector3(direction.x, direction.y, 0);
        addVector.Normalize();
        //Debug.Log(addVector);
        
        SoundManager.Instance.PlaySound("Shoot", transform.position);
    }

    protected virtual void _checkDistance()
    {
        distance_P = (startingPoint - transform.position).sqrMagnitude;
        if (distance_P > deleteDistance)
        {
            Destroy(gameObject);
        }
    }

    public void getVector(Vector3 from, Vector3 to)
    {
        direction = new Vector3(to.x - from.x, to.y - from.y, to.z - from.z);
    }

    public void setStartPoint(Vector3 startPoint)
    {
        this.startingPoint = startPoint;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 21) //Enemy
        {
            var enemy = other.gameObject.GetComponent<IDamageable>();
            if (enemy != null)
            {
                //enemy.GetDamaged(10f, MedicineType.Medicine1, Vector2.zero);
                enemy.GetDamaged(10f, SlotSelectMock.Instance.selectedMedicineType, 20f * (other.transform.position - transform.position).normalized);
            }
        }
        
        if (other.gameObject.layer == 22) //Enemy
        {
            var enemy = other.gameObject.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.GetDamaged(10f, SlotSelectMock.Instance.selectedMedicineType, Vector2.zero);
            }
        }

        if (other.gameObject.layer == 11) //Cell
        {
            var cell = other.gameObject.GetComponent<Cell>();
            if (cell != null)
            {
                cell.GetDamaged(10f, SlotSelectMock.Instance.selectedMedicineType, Vector2.zero);
            }
        }
        
    }
}
