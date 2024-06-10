using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject? player;
    Character characterComponent;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        characterComponent = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotateWeapon();
        HandleMove();
        if (ShouldFire())
        {
            GetComponent<WeaponManager>().Attack();
        }
    }

    bool ShouldFire()
    {
        if (player == null)
        {
            return false;
        }
        return (transform.position - GameObject.Find("Player").transform.position).magnitude < GetComponent<WeaponManager>().currentWeapon.GetComponent<Weapon>().range + 20;

    }

    void HandleRotateWeapon()
    {
        if (characterComponent.shouldRotateWeapon)
        {
            if (player != null)
            {
                characterComponent.RotateHeadAndWeapon(Quaternion.LookRotation(Vector3.forward, player.transform.position - transform.position));
            }
        }
    }

    Vector2 lastDirection;
    float lastMoveTimeSec = 0;
    void HandleMove()
    {
        if (!characterComponent.canMove)
        {
            return;
        }
        if (lastMoveTimeSec != 0 && Time.time - lastMoveTimeSec < 0.5)
        {
            characterComponent.Move(lastDirection, Time.deltaTime);
            return;
        }
        if (player != null && Random.value < (GetCurrentWeaponType() == Weapon.WeaponType.Ranged ? 0.2 : 0.7))
        {
            lastDirection = (player.transform.position - transform.position).normalized;
        }
        else
        {
            lastDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
        lastMoveTimeSec = Time.time;
        characterComponent.Move(lastDirection, Time.deltaTime);
    }

    Weapon.WeaponType GetCurrentWeaponType()
    {
        return GetComponent<WeaponManager>().currentWeapon.GetComponent<Weapon>().weaponType;
    }
}
