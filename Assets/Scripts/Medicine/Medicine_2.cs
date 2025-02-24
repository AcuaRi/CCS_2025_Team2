using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine_2 : Bullet
{
    protected override void Start()
    {
        // startingPoint = playerObj.transform.position; ‚±‚ê‚ð”š—ô‚µ‚½’n“_‚É‚·‚é
        thisTransform = transform;
        //addVector = 100 * new Vector3(direction.x * Time.deltaTime, direction.y * Time.deltaTime, 0);
        addVector = new Vector3(direction.x, direction.y, 0);
        addVector.Normalize();
        //Debug.Log(addVector);

        SoundManager.Instance.PlaySound("Shoot", transform.position);
    }
}
