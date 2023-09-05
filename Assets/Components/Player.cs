using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Character characterComponent;
    // Start is called before the first frame update
    void Start()
    {
        characterComponent = GetComponent<Character>();
    }
    // Update is called once per frame
    void Update()
    {
        MoveControl();
        WeaponRotationControl();
        AttackConctrol();
    }
    void AttackConctrol()
    {
        if (Input.GetMouseButton(0))
        {
            GetComponent<WeaponManager>().currentWeapon.GetComponent<Weapon>().Attack();
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

    void WeaponRotationControl()
    {
        var positionDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        characterComponent.RotateHeadAndWeapon(Quaternion.LookRotation(Vector3.forward, positionDelta));
    }
}
