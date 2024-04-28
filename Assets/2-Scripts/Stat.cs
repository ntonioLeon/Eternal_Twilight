using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat 
{
    [SerializeField]private float baseValue;

    public List<float> modifiers;
    public float GetValue()
    {
        float finalValue = baseValue;
        foreach(float modi in modifiers)
        {
            finalValue += modi;
        } 

        return finalValue;
    }

    public void AddModifier(float modi)
    {
        modifiers.Add(modi);
    }
    public void RemoveModifier(float modi)
    {
        modifiers.Remove(modi);
    }
}