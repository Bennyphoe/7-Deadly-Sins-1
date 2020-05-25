﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterCombat))]
public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    public Stat damage;
    public Stat armor;
    public event System.Action takenDamage;

    CharacterCombat combat;

    protected virtual void Start()
    {
        combat = GetComponent<CharacterCombat>();

    }
    public event System.Action<int, int> OnHealthChanged;
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage (int damage)
    {
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHealth -= damage;
        if (takenDamage != null)
        {
            takenDamage();
        }
        
        Debug.Log (transform.name + " takes " + "damage.");

        if (OnHealthChanged != null)
        {
            OnHealthChanged(maxHealth, currentHealth);
        }

        if (currentHealth <= 0)
        {

            Die();

        }
    }

    public virtual void Die()
    {
        //Die in some way
        //method meant to be overriden
        combat.dead = true;
        Debug.Log(transform.name + " died.");
    }

}
