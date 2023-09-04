using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public int speed = 500;
    public int expirationTimeSec = 5;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, expirationTimeSec);
    }

    // Update is called once per frame
    void Update()
    {
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Enemy"))
    //     {
    //         collision.gameObject.SendMessage("ApplyDamage", 10);
    //     }
    // }
}
