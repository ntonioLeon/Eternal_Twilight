using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EstadoManager : MonoBehaviour
{
    public static EstadoManager instance;
    [SerializeField] private Image holder;
    [SerializeField] private Sprite poisoned;
    [SerializeField] private Sprite burned;
    [SerializeField] private Sprite shocked;
    [SerializeField] private Sprite healing;
    [SerializeField] private Sprite save;

    private bool damaged = false;

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
        //holder = GetComponent<Image>();

        
    }

    private void CheckDamaged()
    {
        if (!damaged)
        {
            OnSave();
        }
    }

    void Update()
    {
        CheckDamaged();
    }

    public void OnPoisoned()
    {
        holder.sprite = poisoned;
    }

    public void OnBurned()
    {
        holder.sprite = burned;
    }

    public void OnShoked()
    {
        holder.sprite = shocked;
    }

    public void OnHealing()
    {
        holder.sprite = healing;
    }

    public void OnSave()
    {
        holder.sprite = save;
    }
}
