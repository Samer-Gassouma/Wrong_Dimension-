using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Item : MonoBehaviour
{

    [Header("Power-Up Settings")]
    public string powerUpEffect; // What the power-up does (e.g., "speedBoost", "doubleDamage" , "healthBoost")
    public int  powerUpDuration = 5; 

     



    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private BoxCollider2D itemCollider;

    [SerializeField]
    int health = 3;
    [SerializeField]
    bool nonDestructible;

    [SerializeField]
    private GameObject hitFeedback, destoyFeedback;

    bool isPressed = false;

   



    enum PowerUp
    {
        speedBoost,
        doubleDamage,
        SpeedSlower,
        AttackBlocker
    }

    private void Awake()
    {

        RandomPower();
    }

    void RandomPower()
    {

        PowerUp power = (PowerUp)UnityEngine.Random.Range(0, 4);
        switch (power)
        {
            case PowerUp.speedBoost:
                powerUpEffect = "speedBoost";
                break;
            case PowerUp.doubleDamage:
                powerUpEffect = "doubleDamage";
                break;
            case PowerUp.SpeedSlower:
                powerUpEffect = "SpeedSlower";
                break;
            case PowerUp.AttackBlocker:
                powerUpEffect = "AttackBlocker";
                break;
        }


    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            Debug.Log("Press E to pick up " + powerUpEffect);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Exit Trigger");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isPressed)
        {
            isPressed = true;
            ApplyPowerUp();
        }
    }


    private void ApplyPowerUp()
    {
        // Find the player object (assuming you have a way to reference it)
        GameObject player = GameObject.FindWithTag("Player");
         
        Debug.Log("Power up effect: " + powerUpEffect);
        // Apply power-up effect based on 'powerUpEffect'
        if (player != null)
        {
            switch (powerUpEffect)
            {
                case "speedBoost":
                    player.GetComponent<PlayerInput>().ActivateSpeedBoost(powerUpDuration); // Assumes you have a PlayerController script
                    break;
                case "doubleDamage":
                    player.GetComponent<WeaponParent>().ActivateDoubleDamage(powerUpDuration); // Assumes you have a Health script
                    break;
                case "SpeedSlower":
                    player.GetComponent<PlayerInput>().ActivateSpeedSlower(powerUpDuration); // Assumes you have a PlayerController script
                    break;
                case "AttackBlocker":
                    player.GetComponent<WeaponParent>().ActivateAttackBlocker(powerUpDuration); // Assumes you have a Health script
                    break;
                default:
                    Debug.LogWarning("Unknown power-up effect: " + powerUpEffect);
                    break;
            }
        }
        StartCoroutine(DestroyItem());
        StartCoroutine(RestoreIsPress());
        // Destroy the item
       // spriteRenderer.transform.DOComplete();
    }

    IEnumerator DestroyItem()
    {
        yield return new WaitForSeconds(0.5f);
        //Destroy(gameObject);
        Debug.Log("Destroy Item" + gameObject.name);
    }

    IEnumerator RestoreIsPress()
    {
        yield return new WaitForSeconds(powerUpDuration);
        isPressed = false;
    }
    public UnityEvent OnGetHit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Initialize(ItemData itemData)
    {
        //set sprite
        spriteRenderer.sprite = itemData.sprite;
        //set sprite offset
        spriteRenderer.transform.localPosition = new Vector2(0.5f * itemData.size.x, 0.5f * itemData.size.y);
        itemCollider.size = itemData.size;
        itemCollider.offset = spriteRenderer.transform.localPosition;

        if (itemData.nonDestructible)
            nonDestructible = true;

        this.health = itemData.health;

    }



    public void GetHit(int damage, GameObject damageDealer)
    {
        if (nonDestructible)
            return;
        if(health>1)
            Instantiate(hitFeedback, spriteRenderer.transform.position, Quaternion.identity);
        else
            Instantiate(destoyFeedback, spriteRenderer.transform.position, Quaternion.identity);
        spriteRenderer.transform.DOShakePosition(0.2f, 0.3f, 75, 1, false, true).OnComplete(ReduceHealth);
    }

    private void ReduceHealth()
    {
        health--;
        if (health <= 0)
        {
            spriteRenderer.transform.DOComplete();
            Destroy(gameObject);
        }
            
    }
}

