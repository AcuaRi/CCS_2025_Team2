using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine_ClusterChild : Bullet
{
    protected override void Start()
    {
        // startingPoint = playerObj.transform.position; ����𔚗􂵂��n�_�ɂ��邽�߂�Cluster���Ő������ɒl������
        addVector = new Vector3(direction.x, direction.y, 0);
        addVector.Normalize();
        //Debug.Log(addVector);

        SoundManager.Instance.PlaySound("Shoot", transform.position);
    }
}
