using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public Vector2 grabOffset = Vector2.zero;
    public int damage = 10;
    public float attackCooldownSec = 1f;
    public int range = 0;
    public GameObject? equippedCharacter;
    public GameObject? projectilePrefab;
    public bool isAttacking = false;
    private float lastAttackTimeSec = 0;
    public int thrustSpeed = 40;
    public int slashRotateSpeed = 600;
    public bool shouldFlipSlashRotation = false;
    public int slashMaxAngleDeg = 400;
    float passedSlashAngleDeg;
    System.Action? animateFunction;
    Collider2D? physicsCollider;

    void Start()
    {
        // One collider for physics, one for interacting with mouse
        // Wish Unity got something better, even just naming...
        var colliders = GetComponents<Collider2D>();
        foreach (var collider in colliders)
        {
            if (!collider.isTrigger)
            {
                physicsCollider = collider;
                break;
            }
        }
        if (physicsCollider != null)
        {
            physicsCollider.enabled = false;
        }
    }

    void Update()
    {
        if (animateFunction is not null)
        {
            animateFunction();
        }
        if (!CanShowPickupIndicator())
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    bool CanShowPickupIndicator()
    {
        if (equippedCharacter)
        {
            return false;
        }
        var weaponPickupRange = GlobalControl.player.GetComponent<Character>().weaponPickupRange;
        return (GlobalControl.player.transform.position - transform.position).magnitude < weaponPickupRange;
    }

    void OnMouseOver()
    {
        if (CanShowPickupIndicator())
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }

    void OnMouseExit()
    {
        if (CanShowPickupIndicator())
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void Attack()
    {
        if (isAttacking || (lastAttackTimeSec != 0 && Time.time - lastAttackTimeSec < attackCooldownSec))
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

    void BaseMeleeAttackDone()
    {
        isAttacking = false;
        animateFunction = null;
        physicsCollider!.enabled = false;
        var characterComponent = equippedCharacter!.GetComponent<Character>();
        characterComponent.shouldRotateWeapon = true;
    }

    void ThrustAttack()
    {
        animateFunction = ThrustAttackAnimate;
        isAttacking = true;
        thrustGoOff = true;
        physicsCollider!.enabled = true;
        var characterComponent = equippedCharacter!.GetComponent<Character>();
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
        BaseMeleeAttackDone();
    }

    void SlashAttack()
    {
        animateFunction = SlashAttackAnimate;
        isAttacking = true;
        physicsCollider!.enabled = true;
        var characterComponent = equippedCharacter!.GetComponent<Character>();
        characterComponent.shouldRotateWeapon = false;
        passedSlashAngleDeg = 0;
        shouldFlipSlashRotation = Mathf.Abs(characterComponent.grabPointTransform.rotation.eulerAngles.z) > 180;
    }

    void SlashAttackAnimate()
    {
        var characterComponent = equippedCharacter!.GetComponent<Character>();
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
        BaseMeleeAttackDone();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider != equippedCharacter && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Character>().Hit(damage);
        }
    }

    void RangedAttack()
    {
        var firePoint = transform.Find("FirePoint");
        var projectile = Instantiate(projectilePrefab, firePoint.position, equippedCharacter!.transform.rotation)!;
        var projectileComponent = projectile.GetComponent<Projectile>();
        var characterComponent = equippedCharacter.GetComponent<Character>();
        projectileComponent.damage = damage;
        projectileComponent.fromCharacter = equippedCharacter;
        projectile.transform.rotation = characterComponent.grabPointTransform.rotation;
        projectile.GetComponent<Rigidbody2D>().AddForce(
            characterComponent.grabPointTransform.up * projectileComponent.speed
        );
        // projectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 0));
    }

    public void PickupBy(GameObject? character)
    {
        if (character == null)
        {
            transform.SetParent(null);
            // transform.position = character.transform.position;
            equippedCharacter = null;
        }
        else
        {
            transform.SetParent(character.GetComponent<Character>().grabPointTransform);
            transform.SetLocalPositionAndRotation(grabOffset, Quaternion.identity);
            GetComponent<SpriteRenderer>().color = Color.white;
            equippedCharacter = character;
        }
    }
}
