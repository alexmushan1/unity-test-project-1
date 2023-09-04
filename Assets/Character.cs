using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed = 10.0f;
    public WeaponManager weaponManager;
    public GameObject initialWeapon;
    public GameObject grabPoint;
    // Start is called before the first frame update
    public void Start()
    {
        // grabPoint = GameObject.Find("GrabPoint");
        grabPoint = transform.Find("GrabPoint").gameObject;
        // weaponManager = new WeaponManager(this, initialWeapon);
        weaponManager = gameObject.AddComponent<WeaponManager>();
        weaponManager.Init(initialWeapon);
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void RotateWeapon(Quaternion rotation)
    {
        grabPoint.transform.rotation = rotation;
        var shouldFlip = rotation.eulerAngles.z < 180;
        GetComponent<SpriteRenderer>().flipX = shouldFlip;
        weaponManager.currentWeapon.GetComponent<SpriteRenderer>().flipX = shouldFlip;
    }

    public void PickupWeapon(GameObject weapon)
    {
        weaponManager.ChangeWeapon(weapon);
    }
}
