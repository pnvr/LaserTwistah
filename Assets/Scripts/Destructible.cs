﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour, IDamageable {

    public float Health = 100f;
    public float MinHealth = 0f;
    public float MaxHealth = 100f;
    public bool Healable;
    bool isTakingDamage;
    public float HealingAmount; // per 1 second
    bool levelCompleted;
    bool audioTakesHitsIsOn;
    GameManager gm;

    private void Start() {
        gm = FindObjectOfType<GameManager>();
    }

    public void DamageIt(float damageAmount) {
        //Debug.Log("You damaged it.");
        // Enemy get's some damage.
        Health -= damageAmount*Time.deltaTime;
        GetComponentInChildren<SpriteRenderer>().material.SetFloat("_FlashAmount", (MaxHealth - Health) / MaxHealth);
        isEnemyDestroyed(Health);
        isTakingDamage = true;
        if(isTakingDamage && !audioTakesHitsIsOn) {
            AudioFW.Play("TargetTakesHits");
            audioTakesHitsIsOn = true;
        }
    }

    void isEnemyDestroyed(float health) {
        // If health goes too low we'll remove enemy.
        if (health <= MinHealth) {
            //audioVanishes.Play();
            AudioFW.Play("TargetExplodes");
            if(!levelCompleted) {
                gm.LevelComplete();
                levelCompleted = true;
            }

            Destroy(gameObject, 0.5f);
        }
        //if (health >= enemyMaxHealth) {
        //    health = enemyMaxHealth;
        //}
    }

    void Update() {
        //Debug.Log("Trying to heal...");
        // We'll try to heal target a bit in every frame.
        if (Healable  && !isTakingDamage) {
            if (Health < MaxHealth) {
                Health += (HealingAmount * Time.deltaTime);
                GetComponentInChildren<SpriteRenderer>().material.SetFloat("_FlashAmount", (MaxHealth - Health) / MaxHealth);
            }
        }

        isTakingDamage = false;
        //audioTakesHits.Stop();
    }
}
