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
        TryFire();
    }

    void TryFire()
    {
        if ((transform.position - GameObject.Find("Player").transform.position).magnitude < GetComponent<WeaponManager>().currentWeapon.GetComponent<Weapon>().range + 20)
        {
            GetComponent<WeaponManager>().Attack();
        }
    }

    void HandleRotateWeapon()
    {
        if (GetComponent<Character>().shouldRotateWeapon)
        {

            GetComponent<Character>().RotateHeadAndWeapon(Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position));
        }
    }

    Vector2 lastMovement;
    float lastMoveTimeSec = 0;
    void HandleMove()
    {
        if (lastMoveTimeSec != 0 && Time.time - lastMoveTimeSec < 0.5)
        {
            GetComponent<Character>().Move(lastMovement, Time.deltaTime);
            return;
        }
        if (Random.value < (GetCurrentWeaponType() == Weapon.WeaponType.Ranged ? 0.3 : 0.7))
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

    Weapon.WeaponType GetCurrentWeaponType()
    {
        return GetComponent<WeaponManager>().currentWeapon.GetComponent<Weapon>().weaponType;
    }
}
