using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    public int damage;

    private Rigidbody rb;

    private bool targetHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
    }

    private void OnTriggerEnter(Collider collision)
    {
        // check if you hit an enemy
        if(collision.gameObject.GetComponent<BasicEnemy>() != null)
        {
            BasicEnemy enemy = collision.gameObject.GetComponent<BasicEnemy>();

            enemy.TakeDamage(damage);

            // destroy projectile
            Destroy(gameObject);
        }
    }
}