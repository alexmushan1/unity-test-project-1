using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // units per second
    public float speed = 10.0f;
    public WeaponManager weaponManager;
    public GameObject initialWeapon;
    public GameObject grabPoint;

    bool hitImmune = false;
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

    public void Move(Vector2 movement)
    {
        transform.Translate(movement * speed);
        if (movement.x == 0 && movement.y == 0)
        {
            GetComponent<Animator>().SetBool("running", false);
        }
        else
        {
            GetComponent<Animator>().SetBool("running", true);
        }
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

    public void Hit(int damage)
    {
        if (hitImmune)
        {
            return;
        }
        var healthComponent = GetComponent<Health>();
        if (damage >= healthComponent.currentHealth)
        {
            Destroy(gameObject);
            return;
        }
        healthComponent.currentHealth -= damage;
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke(nameof(AfterHitImmune), 1);
        hitImmune = true;
    }

    void AfterHitImmune()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        hitImmune = false;
    }
}
