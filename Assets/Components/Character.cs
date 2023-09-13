using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    readonly Color LIGHT_ON_HIT_RED = new(1, 0.5f, 0.5f);

    // units per second
    public float speed = 10.0f;
    public WeaponManager weaponManager;
    public GameObject initialWeapon;
    public Transform grabPointTransform;
    public Transform bodyTransform;
    public Transform headTransform;

    public bool shouldRotateWeapon = true;

    bool hitImmune = false;

    // Start is called before the first frame update
    public void Start()
    {
        // grabPoint = GameObject.Find("GrabPoint");
        grabPointTransform = transform.Find("GrabPoint");
        bodyTransform = transform.Find("Body");
        headTransform = transform.Find("Head");

        // weaponManager = new WeaponManager(this, initialWeapon);
        weaponManager = gameObject.AddComponent<WeaponManager>();
        weaponManager.Init(initialWeapon);
    }
    // Update is called once per frame
    void Update()
    {
    }

    public void Move(Vector2 normalizedMovement, float deltaTime)
    {
        var movement = deltaTime * speed * normalizedMovement;
        transform.Translate(movement);
        if (movement == Vector2.zero) //when not moving
        {
            bodyTransform.GetComponent<Animator>().SetBool("running", false);
            Vector3 bodyTilt = new Vector3(0, 0, 0);//reset tilt
            Quaternion bodyRotation = Quaternion.Euler(bodyTilt);
            bodyTransform.localRotation = bodyRotation;
        }
        else //when moving
        {
            bodyTransform.GetComponent<Animator>().SetBool("running", true);
        }
        if (movement.x != 0)
        {
            if (movement.x <= 0) //move left
            {
                Vector3 headPos = new Vector3(-1.2f, 4, 0);
                headTransform.localPosition = headPos;
                Vector3 bodyTilt = new Vector3(0, 0, 5);
                Quaternion bodyRotation = Quaternion.Euler(bodyTilt);
                bodyTransform.localRotation = bodyRotation;
            }
            else //move right
            {
                Vector3 headPos = new Vector3(1.2f, 4, 0);
                headTransform.localPosition = headPos;
                Vector3 bodyTilt = new Vector3(0, 0, -5);
                Quaternion bodyRotation = Quaternion.Euler(bodyTilt);
                bodyTransform.localRotation = bodyRotation;
            }

            bodyTransform.GetComponent<SpriteRenderer>().flipX = movement.x < 0;
        }
    }

    public void RotateHeadAndWeapon(Quaternion rotation)
    {
        grabPointTransform.rotation = rotation;
        var shouldFlip = rotation.eulerAngles.z < 180;
        headTransform.GetComponent<SpriteRenderer>().flipX = shouldFlip;
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
        headTransform.GetComponent<SpriteRenderer>().color = LIGHT_ON_HIT_RED;
        bodyTransform.GetComponent<SpriteRenderer>().color = LIGHT_ON_HIT_RED;
        Invoke(nameof(AfterHitImmune), 0.1f);
        hitImmune = true;
    }

    void AfterHitImmune()
    {
        headTransform.GetComponent<SpriteRenderer>().color = Color.white;
        bodyTransform.GetComponent<SpriteRenderer>().color = Color.white;
        hitImmune = false;
    }
}
