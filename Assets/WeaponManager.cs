using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class WeaponManager : MonoBehaviour
{
    public GameObject currentWeapon;
    public Character character;

    // public WeaponManager(Character character, GameObject initialWeapon)
    // {
    //     this.character = character;
    //     ChangeWeapon(initialWeapon);
    // }
    public void Init(Character character, GameObject initialWeapon)
    {
        this.character = character;
        ChangeWeapon(initialWeapon);
    }

    public void ChangeWeapon(GameObject weapon)
    {
        if (currentWeapon is not null)
        {
            Destroy(currentWeapon);
        }
        currentWeapon = Instantiate(weapon);
        currentWeapon.transform.SetParent(character.grabPoint.transform);
    }
}
