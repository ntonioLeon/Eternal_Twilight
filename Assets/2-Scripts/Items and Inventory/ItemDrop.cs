using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (UnityEngine.Random.Range(0, 100) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }


        for (int i = 0; i < possibleItemDrop; i++)
        {
            try
            {
                ItemData randomItem = dropList[UnityEngine.Random.Range(0, dropList.Count - 1)];

                dropList.Remove(randomItem);
                DropItem(randomItem);
            }
            catch (Exception e)
            {
                Debug.Log("El drop es inerior a dos" + e);
                //Si pasa por aqui el drop es 1 o 0.
            }
        }
    }

    protected void DropItem(ItemData itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(itemData, randomVelocity);
    }
}
