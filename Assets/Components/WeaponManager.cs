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
            Destroy(currentWeapon);
        }
        currentWeapon = Instantiate(weapon);
        currentWeapon.transform.position = GetComponent<Character>().grabPointTransform.position;
        currentWeapon.transform.SetParent(GetComponent<Character>().grabPointTransform);
        currentWeapon.GetComponent<Weapon>().equippedCharacter = gameObject;
    }
}
