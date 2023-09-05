using System.Collections;
using System.Collections.Generic;
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
        float dx = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        float dy = Input.GetAxisRaw("Vertical") * Time.deltaTime;
        characterComponent.Move(dx, dy);
    }

    void WeaponRotationControl()
    {
        var positionDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        characterComponent.RotateWeapon(Quaternion.LookRotation(Vector3.forward, positionDelta));
    }
}
