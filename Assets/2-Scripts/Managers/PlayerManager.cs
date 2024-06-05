using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;

    public Player player;
    public bool CinematicaVista;
    public int currency;
    public Text total;
    public bool bossKilled = false;

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

    private void Start()
    {
        CinematicaVista = SaveManager.instance.gameData.cinematica;
    }

    private void Update()
    {
        total.text = currency.ToString();
    }

    public int GetCurrency() => currency;

    public void LoadData(GameData data)
    {
        this.currency = data.currency;
        this.CinematicaVista = data.cinematica;
    }

    public void SaveData(ref GameData data)
    {
        data.currency = this.currency;
        data.cinematica = this.CinematicaVista;
    }

    public void SetCinematicaToTrue()
    {
        CinematicaVista = true;
    }
}
