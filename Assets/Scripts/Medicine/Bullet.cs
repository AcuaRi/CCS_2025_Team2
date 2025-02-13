using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.1f;
    [SerializeField]
    float deleteDistance = 30 * 30;
    //public float lifeTime = 5f;

    private GameObject playerObj = null;

    private float distance_P;
    private Vector3 addVector;
    private Vector3 direction;
    private Transform thisTransform;

    private void Update()
    {
        Vector3 newPosition;
        //newPosition = thisTransform.position + addVector * speed;
        //transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
        transform.position += Time.deltaTime * speed * addVector;
        distance_P = (playerObj.transform.position - transform.position).sqrMagnitude;
        if (distance_P > deleteDistance)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerObj = GameObject.Find("Player"); // ��肭�����Ȃ�������G���[�f���Ăق���
        thisTransform = transform;
        //addVector = 100 * new Vector3(direction.x * Time.deltaTime, direction.y * Time.deltaTime, 0);
        addVector = new Vector3(direction.x, direction.y, 0);
        addVector.Normalize();
        //Debug.Log(addVector);
    }

    public void getVector(Vector3 from, Vector3 to)
    {
        direction = new Vector3(to.x - from.x, to.y - from.y, to.z - from.z);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 21) //Enemy
        {
            var enemy = other.gameObject.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.GetDamaged(10f, MedicineType.Medicine1);
            }
        }

        if (other.gameObject.layer == 11) //Cell
        {
            var cell = other.gameObject.GetComponent<Cell>();
            if (cell != null)
            {
                cell.GetDamaged(20f);
            }
        }
        
    }
}
