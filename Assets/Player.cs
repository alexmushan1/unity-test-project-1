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
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float dx = Input.GetAxisRaw("Horizontal") * characterComponent.speed;
        float dy = Input.GetAxisRaw("Vertical") * characterComponent.speed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        dx *= Time.deltaTime;
        dy *= Time.deltaTime;

        transform.Translate(dx, dy, 0);
    }

    void WeaponRotationControl()
    {
        var positionDelta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        characterComponent.grabPoint.transform.rotation = Quaternion.LookRotation(Vector3.forward, positionDelta);

    }

    void PickupWeapon(GameObject weapon)
    {
        characterComponent.weaponManager.ChangeWeapon(weapon);
    }
}
