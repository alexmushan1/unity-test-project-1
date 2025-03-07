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
        var currentWeapon = GetCurrentWeapon();
        if (currentWeapon == null)
        {
            return false;
        }
        return (transform.position - GameObject.Find("Player").transform.position).magnitude < currentWeapon.range + 20;

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
        lastDirection = GetMovingDirection();
        lastMoveTimeSec = Time.time;
        characterComponent.Move(lastDirection, Time.deltaTime);
    }

    Vector2 GetMovingDirection()
    {
        if (player != null)
        {
            var currentWeapon = GetCurrentWeapon();
            if (currentWeapon != null)
            {
                // Get aways from the player if we're in cooldown
                if (!currentWeapon.isAttacking && currentWeapon.InCooldown() && Random.value < 0.8)
                {
                    return (transform.position - player.transform.position).normalized;
                }
                // Get closer when we're far away from the player
                if (Random.value < (currentWeapon.weaponType == Weapon.WeaponType.Ranged ? 0.2 : 0.7))
                {
                    return (player.transform.position - transform.position).normalized;
                }
            }
        }
        // Move randomly
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    // Weapon.WeaponType? GetCurrentWeaponType()
    // {
    //     var currentWeapon = GetCurrentWeapon();
    //     if (currentWeapon != null)
    //     {
    //         return currentWeapon.weaponType;
    //     }
    //     return null;
    // }

    Weapon? GetCurrentWeapon()
    {
        var currentWeapon = GetComponent<WeaponManager>().currentWeapon;
        if (currentWeapon != null)
        {
            return currentWeapon.GetComponent<Weapon>();
        }
        return null;

    }
}
