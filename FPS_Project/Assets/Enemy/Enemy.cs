using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyHP = 100;
    public GameObject projectile;
    public Transform projectilePoint;

    public Animator animator;
    public void Shoot()
    {
        Rigidbody rb = Instantiate(projectile, projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 30f, ForceMode.Impulse);
        rb.AddForce(transform.up * 7, ForceMode.Impulse);
    }

    public void TakeDamage(int damageAmount)
    {
        enemyHP -= damageAmount;
        Debug.Log(enemyHP);
        if (enemyHP <= 0)
        {
            // Play death animation or destroy enemy
            //animator.SetTrigger("death");
            GetComponent<CapsuleCollider>().enabled= false;
            Debug.Log("Dead");
            Destroy(gameObject);
            //this
        }
        else
        {
            // play damage animation or show hp
            //animator.SetTrigger("damage");
        }
    }
}
