using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class Health : MonoBehaviour
{
    [SerializeField]
    private int currentHealth, maxHealth;
       
    int damage = 1;
   
    int score = 0;
        
    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }
   






    public void GetHit(int amount, GameObject sender)
    {
        if (isDead)
            return;
        if (sender.layer == gameObject.layer)
            return;

        Debug.Log("Health: " + currentHealth);
        Debug.Log("sender layer: " + sender.layer);
        Debug.Log("sender: " + sender);
        Debug.Log("Hit: " + amount);

        if(sender.ToString().Contains("Player"))
        {
            score += amount;
         }
        else
        {
            score -= amount;
        }

        GameManager.UpdateScore(score);

        currentHealth -= amount;

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            isDead = true;
            Destroy(gameObject);
        }
    }
}
