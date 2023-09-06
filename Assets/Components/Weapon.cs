using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Slash,
        Thrust,
        Ranged,
    }

    public WeaponType weaponType;
    public int damage = 10;
    public float attackCooldownSec = 0.2f;
    public int range = 5;
    public GameObject equippedCharacter;
    public GameObject projectilePrefab;
    public bool isAttacking = false;
    private float lastAttackTimeSec = 0;
    public int thrustSpeed = 40;
    public int slashRotateSpeed = 600;
    public bool shouldFlipSlashRotation = false;
    public int slashMaxAngleDeg = 360;
    float passedSlashAngleDeg;
    System.Action animateFunction;

    void Update()
    {
        if (animateFunction is not null)
        {
            animateFunction();
        }
    }

    public void Attack()
    {
        if (isAttacking || Time.time - lastAttackTimeSec < attackCooldownSec)
        {
            return;
        }
        lastAttackTimeSec = Time.time;
        switch (weaponType)
        {
            case WeaponType.Slash:
                SlashAttack();
                break;
            case WeaponType.Thrust:
                ThrustAttack();
                break;
            case WeaponType.Ranged:
                RangedAttack();
                break;
        }
    }

    void ThrustAttack()
    {
        animateFunction = ThrustAttackAnimate;
        isAttacking = true;
        thrustGoOff = true;
        var characterComponent = equippedCharacter.GetComponent<Character>();
        characterComponent.shouldRotateWeapon = false;
    }

    bool thrustGoOff;

    void ThrustAttackAnimate()
    {
        if (thrustGoOff && transform.localPosition.y < range)
        {
            transform.localPosition += new Vector3(0, thrustSpeed, 0) * Time.deltaTime;
        }
        else
        {
            thrustGoOff = false;
            transform.localPosition += new Vector3(0, -thrustSpeed, 0) * Time.deltaTime;
            if (transform.localPosition.y <= 0)
            {
                transform.localPosition = Vector3.zero;
                DoneThrustAttack();
            }
        }
    }

    void DoneThrustAttack()
    {
        isAttacking = false;
        animateFunction = null;
        var characterComponent = equippedCharacter.GetComponent<Character>();
        characterComponent.shouldRotateWeapon = true;
    }

    void SlashAttack()
    {
        animateFunction = SlashAttackAnimate;
        isAttacking = true;
        var characterComponent = equippedCharacter.GetComponent<Character>();
        characterComponent.shouldRotateWeapon = false;
        passedSlashAngleDeg = 0;
        shouldFlipSlashRotation = Mathf.Abs(characterComponent.grabPointTransform.rotation.eulerAngles.z) > 180;
    }

    void SlashAttackAnimate()
    {
        var characterComponent = equippedCharacter.GetComponent<Character>();
        var oldAngle = characterComponent.grabPointTransform.rotation.eulerAngles.z;
        var deltaAngle = (shouldFlipSlashRotation ? -slashRotateSpeed : slashRotateSpeed) * Time.deltaTime;
        var newAngle = oldAngle + deltaAngle;
        passedSlashAngleDeg += Mathf.Abs(deltaAngle);
        // characterComponent.RotateHeadAndWeapon(
        //     Quaternion.AngleAxis(newAngle, Vector3.forward)
        // );
        characterComponent.grabPointTransform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
        if (passedSlashAngleDeg > slashMaxAngleDeg)
        {

            DoneSlashAttack();
        }
    }

    void DoneSlashAttack()
    {
        isAttacking = false;
        animateFunction = null;
        var characterComponent = equippedCharacter.GetComponent<Character>();
        characterComponent.shouldRotateWeapon = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAttacking)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Character>().Hit(damage);
        }
    }

    void RangedAttack()
    {
        var firePoint = transform.Find("FirePoint");
        var projectile = Instantiate(projectilePrefab, firePoint.position, equippedCharacter.transform.rotation);
        var projectileComponent = projectile.GetComponent<Projectile>();
        var characterComponent = equippedCharacter.GetComponent<Character>();
        projectileComponent.damage = damage;
        projectile.transform.rotation = characterComponent.grabPointTransform.rotation;
        projectile.GetComponent<Rigidbody2D>().AddForce(
            characterComponent.grabPointTransform.up * projectileComponent.speed
        );
        // projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 0));
    }
}
