using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public bool cinematica;
    public int currency; //Almas, los puntos que sea que vayamos a gastar.
    //float[] transformPosition;
    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public List<string> equipmentId;
    public SerializableDictionary<string, bool> checkPoints;

    public GameData()
    {
        cinematica = false;
        currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        checkPoints = new SerializableDictionary<string, bool>{};
    }
}
