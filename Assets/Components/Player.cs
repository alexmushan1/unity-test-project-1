using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Character characterComponent;

    void Start()
    {
        characterComponent = GetComponent<Character>();
    }

    void Update()
    {
        if (!GlobalControl.isPaused)
        {
            HandleUserInput();
        }
    }

    void HandleUserInput()
    {
        MoveControl();
        WeaponRotationControl();
        AttackConctrol();
        HandlePickUpWeapon();
        if (Input.GetButtonDown("space"))
        {
            DashControl();
        }
    }

    void AttackConctrol()
    {
        if (Input.GetMouseButton(0))
        {
            GetComponent<WeaponManager>().Attack();
        }
    }

    void MoveControl()
    {
        var movement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );
        characterComponent.Move(movement.normalized, Time.deltaTime);
    }

    //attempt to implement dashing
    float currentDashTimeSec = Time.time;
    void DashControl()
    {
        if (currentDashTimeSec != 0 && Time.time - currentDashTimeSec <= 0.5)
        {
            characterComponent.speed *= 4;
        }
        else
        {
            characterComponent.speed /= 4;
        }
        currentDashTimeSec = Time.time;
    }

    void WeaponRotationControl()
    {
        if (characterComponent.shouldRotateWeapon)
        {
            var positionDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            characterComponent.RotateHeadAndWeapon(Quaternion.LookRotation(Vector3.forward, positionDelta));
        }
    }

    void HandlePickUpWeapon()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);
            if (
                hit.collider != null
                && hit.collider.GetComponent<Weapon>()
                && !hit.collider.GetComponent<Weapon>().equippedCharacter
            )
            {
                var distance = (transform.position - hit.collider.transform.position).magnitude;
                if (distance < characterComponent.weaponPickupRange)
                {
                    characterComponent.PickupWeapon(hit.collider.gameObject);
                }
            }
        }
    }
}
