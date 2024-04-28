using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int damage;
    public float maxHealth;
    public string nameChar;

    [SerializeField] private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
    }

}
