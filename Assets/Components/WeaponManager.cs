using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject currentWeapon;

    // public WeaponManager(Character character, GameObject initialWeapon)
    // {
    //     this.character = character;
    //     ChangeWeapon(initialWeapon);
    // }
    public void Init(GameObject initialWeapon)
    {
        ChangeWeapon(initialWeapon);
    }

    public void ChangeWeapon(GameObject weapon)
    {
        if (currentWeapon != null)
        {
            // Destroy(currentWeapon);
            currentWeapon.GetComponent<Weapon>().PickupBy(null);
        }
        currentWeapon = Instantiate(weapon);
        currentWeapon.GetComponent<Weapon>().PickupBy(gameObject);
    }

    public void Attack()
    {
        currentWeapon.GetComponent<Weapon>().Attack();
    }
}
