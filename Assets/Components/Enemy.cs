using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Character>().RotateWeapon(Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position));
        GetComponent<Character>().Move((player.transform.position - transform.position).normalized, Time.deltaTime);
    }
}
