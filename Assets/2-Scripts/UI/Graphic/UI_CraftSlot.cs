using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    private void OnEnable()
    {
        UpdateSlot(item);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemData_Equipment craftData = item.data as ItemData_Equipment;

        if (Inventory.instance.CanCraft(craftData, craftData.craftingMaterials))
        {
            // Implementar sonido de crafteo exitoso.
        }
        else
        {
            // Implementar sonido de crafteo fallido.
        }
    }
}
