using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine_ClusterChild : Bullet
{
    protected override void Start()
    {
        // startingPoint = playerObj.transform.position; ‚±‚ê‚ğ”š—ô‚µ‚½’n“_‚É‚·‚é‚½‚ß‚ÉCluster‘¤‚Å¶¬‚É’l‚ğ“ü‚ê‚é
        addVector = new Vector3(direction.x, direction.y, 0);
        addVector.Normalize();
        //Debug.Log(addVector);

        SoundManager.Instance.PlaySound("Shoot", transform.position);
    }
}
