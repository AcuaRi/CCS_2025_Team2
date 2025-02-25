using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Medicine_Cluster : Bullet
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private ClusterType clusterType;

    public enum ClusterType
    {
        Triple,       // 3����
        Omni          // 8����
    }


    protected override void _checkDistance()
    {
        distance_P = (playerObj.transform.position - transform.position).sqrMagnitude;
        if (distance_P > deleteDistance)
        {
            // ここで弾生成
            if (clusterType == ClusterType.Triple)
            {
                shootingTypeTriple();
            }
            if (clusterType == ClusterType.Omni)
            {
                shootingTypeOmni();
            }
            Destroy(gameObject);
        }
    }

    private void shootingTypeTriple()
    {
            Vector3 shootDirection = addVector;

            // ���ˊp�x�����߂�i�v���C���[���猩���}�E�X�̕����j
            float baseAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

            // 3�����ɔ��ˁi-30�x, 0�x, +30�x�j
            float[] angles = { -30f, 0f, 30f };

            foreach (float angleOffset in angles)
            {
                float finalAngle = baseAngle + angleOffset;
                Vector3 bulletDirection = new Vector3(Mathf.Cos(finalAngle * Mathf.Deg2Rad), Mathf.Sin(finalAngle * Mathf.Deg2Rad), 0f);

                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                b.GetComponent<Bullet>().getVector(transform.position, transform.position + bulletDirection);
                b.GetComponent<Bullet>().setStartPoint(transform.position);
                b.GetComponent<Bullet>().setMedicineType(_medicineType);
        }
    }

    private void shootingTypeOmni()
    {
            float[] angles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };

            foreach (float angle in angles)
            {
                // �p�x���x�N�g���ɕϊ�
                Vector3 bulletDirection = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

                // �e�𐶐����A������ݒ�
                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                b.GetComponent<Bullet>().getVector(transform.position, transform.position + bulletDirection);
                b.GetComponent<Bullet>().setStartPoint(transform.position);
                b.GetComponent<Bullet>().setMedicineType(_medicineType);
        }
    }
}
