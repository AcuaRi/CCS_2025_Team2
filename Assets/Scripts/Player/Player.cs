using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private int _playerHP;
    [SerializeField]
    private float _playerSpeed; // è„â∫Ç∆ç∂âEÇ≈ë¨Ç≥ÇïœÇ¶ÇÈÇ©Ç‡
    private Rigidbody2D _rb;
    private Vector2 _input;

    public GameObject bulletPrefab;
    /*[SerializeField]
    private int poolSize = 20;*/
    private Vector3 bulletPoint;

    // private Queue<GameObject> bulletPool = new Queue<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        bulletPoint = transform.Find("BulletPoint").localPosition;

        /*for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
    }

    public void _OnMove(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    }

    public void _OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject b = Instantiate(bulletPrefab, transform.position + bulletPoint, Quaternion.identity);
            b.GetComponent<Bullet>().getVector(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void _Move()
    {
        _rb.velocity = new Vector2(_input.x * _playerSpeed, _input.y * _playerSpeed);
    }

    public int GetPlayerHP()
    {
        return _playerHP;
    }
}
