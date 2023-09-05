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
        HandleRotateWeapon();
        HandleMove();
    }

    void HandleRotateWeapon()
    {
        GetComponent<Character>().RotateHeadAndWeapon(Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position));
    }

    Vector2 lastMovement;
    float lastMoveTimeSec = 0;
    void HandleMove()
    {
        if (lastMoveTimeSec != 0 && Time.time - lastMoveTimeSec < 1)
        {
            GetComponent<Character>().Move(lastMovement, Time.deltaTime);
            return;
        }
        if (Random.value < 0.5)
        {
            lastMovement = (player.transform.position - transform.position).normalized;
        }
        else
        {
            lastMovement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
        lastMoveTimeSec = Time.time;
        GetComponent<Character>().Move(lastMovement, Time.deltaTime);
    }
}
