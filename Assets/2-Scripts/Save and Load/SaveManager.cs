using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    private GameData gameData;
    public List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;
    [SerializeField] PlayFabManager playFabManager;
    [SerializeField] Inventory inventory;

    [ContextMenu("Delete save file")]

    public void DeleteSavedData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();
    }

    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
        {
            return true;
        }
        return false;
    }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }


    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        saveManagers = FindAllSaveManagers();
        
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.GetString("Inventario").Equals(""))
        {
            gameData = JsonUtility.FromJson<GameData>(PlayerPrefs.GetString("Inventario"));

            foreach (ISaveManager saveManager in saveManagers)
            {
                saveManager.LoadData(gameData);
                PlayerManager.instance.currency = gameData.currency;
                return;
            }
        }
        else
        {
            gameData = dataHandler.Load();

            if (this.gameData == null)
            {
                Debug.Log("No saved data found!");
                NewGame();
            }

            foreach (ISaveManager saveManager in saveManagers)
            {
                saveManager.LoadData(gameData);
                PlayerManager.instance.currency = gameData.currency;
                return;
            }
        }

        
    }

    public void SaveGame()
    {

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
        if (PlayerPrefs.GetString("Logged").Equals("S"))
        {
            playFabManager.UploadInventory(gameData);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("Logged", "N");
        //SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
    }
}
