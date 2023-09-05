using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public int damage = 10;
    public float attackCooldownSec = 0.2f;
    public int range = 5;
    public GameObject equippedCharacter;
    public GameObject projectilePrefab;
    private float lastAttackTimeSec = 0;

    public void Attack()
    {
        if (Time.time - lastAttackTimeSec < attackCooldownSec)
        {
            return;
        }
        lastAttackTimeSec = Time.time;
        if (projectilePrefab != null)
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
}
