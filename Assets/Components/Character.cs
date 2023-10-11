using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    readonly Color LIGHT_ON_HIT_RED = new(1, 0.5f, 0.5f);

    // units per second
    public float speed = 10.0f;
    public float dashSpeedMultiplier = 3.0f;
    public float dashDurationSec = 0.2f;
    public float dashCooldownSec = 2.0f;
    public float weaponPickupRange = 20;
    public WeaponManager weaponManager;
    public GameObject initialWeapon;
    public Transform grabPointTransform;
    public Transform bodyTransform;
    public Transform headTransform;
    Vector2 initialHeadPosition;

    public bool shouldRotateWeapon = true;
    public bool canMove = true;
    enum Facing
    {
        Left,
        Right,
    }
    Facing facing = Facing.Right;

    bool hitImmune = false;
    Vector2 lastNormalizedMovement = Vector2.right;
    float lastDashTime = 0;
    Action? dashUpdateFn;

    // Start is called before the first frame update
    public void Start()
    {
        // grabPoint = GameObject.Find("GrabPoint");
        grabPointTransform = transform.Find("GrabPoint");
        bodyTransform = transform.Find("Body");
        headTransform = transform.Find("Head");
        initialHeadPosition = headTransform.localPosition;

        // weaponManager = new WeaponManager(this, initialWeapon);
        weaponManager = gameObject.AddComponent<WeaponManager>();
        weaponManager.Init(initialWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        dashUpdateFn?.Invoke();
    }

    void ApplyTiltAndFacing(float movementX, bool isDashing = false)
    {
        // Reset body tilt when stop moving
        if (movementX == 0)
        {
            // headTransform.localPosition = Vector2.zero;
            var position = initialHeadPosition;
            if (facing == Facing.Left)
            {
                position.x = -position.x;
            }
            headTransform.localPosition = position;
            bodyTransform.localRotation = Quaternion.identity;
            return;
        }

        Vector2 headPos;
        float bodyTiltAngleDeg;
        if (isDashing)
        {
            headPos = new Vector2(2.5f, 3.5f);
            bodyTiltAngleDeg = -20;
        }
        else
        {
            headPos = new Vector2(1.2f, 4);
            bodyTiltAngleDeg = -5;
        }
        var isMovingLeft = movementX < 0;
        if (isMovingLeft)
        {
            headPos.x = -headPos.x;
            bodyTiltAngleDeg = -bodyTiltAngleDeg;
        }
        headTransform.localPosition = headPos;
        bodyTransform.localRotation = Quaternion.AngleAxis(bodyTiltAngleDeg, Vector3.forward);
        bodyTransform.GetComponent<SpriteRenderer>().flipX = isMovingLeft;
        facing = isMovingLeft ? Facing.Left : Facing.Right;
    }

    public void Move(Vector2 normalizedMovement, float deltaTime)
    {
        var movement = deltaTime * speed * normalizedMovement;
        transform.Translate(movement);

        if (movement == Vector2.zero) //when not moving
        {
            bodyTransform.GetComponent<Animator>().SetBool("running", false);
        }
        else //when moving
        {
            lastNormalizedMovement = normalizedMovement;
            bodyTransform.GetComponent<Animator>().SetBool("running", true);
        }
        ApplyTiltAndFacing(movement.x);
    }

    public void Dash()
    {
        if (dashUpdateFn != null)
        {
            return;
        }
        if (lastDashTime != 0 && Time.time - lastDashTime < dashCooldownSec)
        {
            return;
        }
        lastDashTime = Time.time;
        var startTime = Time.time;
        canMove = false;
        dashUpdateFn = () =>
        {
            if (Time.time - startTime >= dashDurationSec)
            {
                canMove = true;
                dashUpdateFn = null;
                ApplyTiltAndFacing(0, true);
                return;
            }
            var movement = Time.deltaTime * speed * dashSpeedMultiplier * lastNormalizedMovement;
            transform.Translate(movement);
            ApplyTiltAndFacing(movement.x, true);
        };
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
