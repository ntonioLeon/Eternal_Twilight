using System;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData newItemData)
    {
        data = newItemData;
        AddStack();
    }

    public void AddStack()
    {
        stackSize += 1;
    }

    public void RemoveStack()
    {
        stackSize -= 1;
    }
}
