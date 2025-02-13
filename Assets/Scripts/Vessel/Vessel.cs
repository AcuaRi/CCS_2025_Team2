using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vessel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 21) //Enemy
        {
            var enemy = other.gameObject.GetComponent<GeneralEnemy>();
            if (enemy != null)
            {
                enemy.SetVesselState();
            }
        }
    }
}
