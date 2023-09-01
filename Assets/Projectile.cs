using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public int speed = 1000;
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
}
