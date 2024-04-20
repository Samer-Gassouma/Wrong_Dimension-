using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 PointerPosition { get; set; }

    public Animator animator;
    public float delay = 0.3f;
    private bool attackBlocked;
    int damage = 1;
    public bool IsAttacking { get; private set; }

    public Transform circleOrigin;
    public float radius;

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }


    public void ActivateDoubleDamage(int powerUpDuration)
    {
         damage += 2;
        StartCoroutine(DeactivateDoubleDamage(powerUpDuration));

    }


    IEnumerator DeactivateDoubleDamage(int powerUpDuration)
    {
        yield return new WaitForSeconds(powerUpDuration);
        damage -= 2;
    }



    private void Update()
    {
        if (IsAttacking)
            return;
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;

        if(transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }else{
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }
    }


    public void ActivateAttackBlocker(int powerUpDuration)
    {
        attackBlocked = true;
        StartCoroutine(DeactivateAttackBlocker(powerUpDuration));

    }

    IEnumerator DeactivateAttackBlocker(int powerUpDuration)
    {
        yield return new WaitForSeconds(powerUpDuration);
        attackBlocked = false;
    }
    public void Attack()
    {
        if (attackBlocked)
            return;
        animator.SetTrigger("Attack");
        IsAttacking = true;
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position,radius))
        {
            if (collider.isTrigger == false)
                continue;
            Debug.Log(collider.name);
            Health health;
            if(health = collider.GetComponent<Health>())
            {
                health.GetHit(damage, transform.parent.gameObject);
            }
        }
    }
}
