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
        // weaponManager = new WeaponManager(this, initialWeapon);
        weaponManager = gameObject.AddComponent<WeaponManager>();
        weaponManager.Init(this, initialWeapon);
    }
    // Update is called once per frame
    void Update()
    {
    }
}
