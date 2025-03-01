using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Medicine_LandMine : Bullet
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    public int landmineLimit;
    [SerializeField]
    private float _explosionTime;
    [SerializeField]
    private int _explosionTimes;
    [SerializeField]
    private float _explosionRate;

    /*public enum LandMineType
    {
        Omni,

    }*/

    protected override void Start()
    {
        // SoundManager.Instance.PlaySound("Shoot", transform.position);
    }

    protected override void Update()
    {
        if (_checkTime())
        {
            _explode();
        }
    }

    private bool _checkTime()
    {
        _explosionTime -= Time.deltaTime;
        if (_explosionTime <= 0.0f)
        {
            return true;
        }
        return false;
    }

    public void _explode()
    {
        StartCoroutine("ExplodeCoroutine");
    }

    IEnumerator ExplodeCoroutine()
    {
        while (_explosionTimes > 0)
        {
            _explosionTimes--;
            shootingTypeOmni();
            yield return new WaitForSeconds(_explosionRate);
        }
        Destroy(gameObject);
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

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 21) //Enemy
        {
            var enemy = other.gameObject.GetComponent<IDamageable>();
            if (enemy != null)
            {
                _explode();
            }
        }

        if (other.gameObject.layer == 22) //Enemy
        {
            var enemy = other.gameObject.GetComponent<IDamageable>();
            if (enemy != null)
            {
                _explode();
            }
        }
    }
}
