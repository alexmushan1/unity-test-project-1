using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public int speed = 3000;
    public int expirationTimeSec = 3;
    public GameObject fromCharacter;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, expirationTimeSec);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != fromCharacter && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Character>().Hit(damage);
            Destroy(gameObject);
        }
    }
}
