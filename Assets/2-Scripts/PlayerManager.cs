using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;

    public Player player;

    public int currency;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this; //64 Importante
        }
        
    }
    public void LoadData(GameData data)
    {
        this.currency = data.currency;
    }

    public void SaveData(ref GameData data)
    {
        data.currency = this.currency;
    }
}
