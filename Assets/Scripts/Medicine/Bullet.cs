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
        newPosition = thisTransform.position + addVector * speed;
        transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
        distance_P = (playerObj.transform.position - transform.position).sqrMagnitude;
        if (distance_P > deleteDistance)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerObj = GameObject.Find("Player"); // è„éËÇ≠ìÆÇ©Ç»Ç©Ç¡ÇΩÇÁÉGÉâÅ[ìfÇ¢ÇƒÇŸÇµÇ¢
        thisTransform = transform;
        addVector = 100 * new Vector3(direction.x * Time.deltaTime, direction.y * Time.deltaTime, 0);
        addVector.Normalize();
        Debug.Log(addVector);
    }

    public void getVector(Vector3 from, Vector3 to)
    {
        direction = new Vector3(to.x - from.x, to.y - from.y, to.z - from.z);
    }
}
