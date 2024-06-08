using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EstadoManager : MonoBehaviour
{
    public static EstadoManager instance;
    public Image holder;
    public Sprite poisoned;
    public Sprite burned;
    public Sprite shocked;
    public Sprite freeze;
    public Sprite healing;
    public Sprite save;

    public bool damaged = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CheckDamaged();
    }

    void Update()
    {
       
    }
    private void CheckDamaged()
    {
        if (!damaged)
        {
            OnGucci();
        }
    }

    public void OnPoisoned()
    {
        holder.sprite = poisoned;
    }

    public void OnBurned()
    {
        //Debug.Log("Pum");
        holder.sprite = burned;
    }

    public void OnShoked()
    {
        holder.sprite = shocked;
    }
    public void OnFreeze()
    {
        holder.sprite = freeze;
    }

    public void OnHealing()
    {
        holder.sprite = healing;
    }

    public void OnGucci()
    {
        holder.sprite = save;
    }
}
