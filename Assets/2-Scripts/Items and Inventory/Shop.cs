using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    [SerializeField] private List<ItemData> objetos;

    [Header("Primer Item")]
    [SerializeField] private Image primerImage;
    [SerializeField] private Button primerAdd;
    [SerializeField] private Button primerLess;
    [SerializeField] private Text primerQtty;
    [SerializeField] private Text primerPrice;
    [SerializeField] private Text primerName;
    private int primerPrecio;
    private int primerCantidad;
    private string primerIdSlot;


    [Header("Segundo Item")]
    [SerializeField] private Image segundoImage;
    [SerializeField] private Button segundoAdd;
    [SerializeField] private Button segundoLess;
    [SerializeField] private Text segundoQtty;
    [SerializeField] private Text segundoPrice;
    [SerializeField] private Text segundoName;
    private int segundoPrecio;
    private int segundoCantidad;
    private string segundoIdSlot;

    [Header("Tercer Item")]
    [SerializeField] private Image tercerImage;
    [SerializeField] private Button tercerAdd;
    [SerializeField] private Button tercerLess;
    [SerializeField] private Text tercerQtty;
    [SerializeField] private Text tercerPrice;
    [SerializeField] private Text tercerName;
    private int tercerPrecio;
    private int tercerCantidad;
    private string tercerIdSlot;

    [Header("Cuarta Item")]
    [SerializeField] private Image cuatroImage;
    [SerializeField] private Button cuatroAdd;
    [SerializeField] private Button cuatroLess;
    [SerializeField] private Text cuatroQtty;
    [SerializeField] private Text cuatroPrice;
    [SerializeField] private Text cuatroName;
    private int cuatroPrecio;
    private int cuatroCantidad;
    private string cuartoIdSlot;

    [Header("Quinto Item")]
    [SerializeField] private Image quintoImage;
    [SerializeField] private Button quintoAdd;
    [SerializeField] private Button quintoLess;
    [SerializeField] private Text quintoQtty;
    [SerializeField] private Text quintoPrice;
    [SerializeField] private Text quintoName;
    private int quintoPrecio;
    private int quintoCantidad;
    private string quintoIdSlot;

    [Header("Total Summit")]
    [SerializeField] private Text totalSummit;
    [SerializeField] private Text playerCurrency;
    [SerializeField] private GameObject buyButton;
    private int total;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        RefreshList();
        ResetQtty();
        playerCurrency.text = PlayerManager.instance.currency.ToString();
    }

    void Update()
    {
        playerCurrency.text = PlayerManager.instance.currency.ToString();
        TotalSummit();
        CanIBuy();
    }
    public void TotalSummit()
    {
        total = primerCantidad * primerPrecio + segundoCantidad * segundoPrecio + tercerCantidad * tercerPrecio + cuatroCantidad * cuatroPrecio + quintoCantidad * quintoPrecio;
        totalSummit.text = total.ToString();
    }
    #region aux
    public void SetValues()
    {
        reset2Zero();

        if (PlayerPrefs.GetString("Logged").Equals("S"))
        {
            // recibo Diccionario key : value 
            List<ShopItem> gomenasai = JSonToList();

            primerPrice.text = "" + gomenasai[0].price;
            primerName.text = gomenasai[0].name;
            primerIdSlot = gomenasai[0].itemId;

            segundoPrice.text = "" + gomenasai[1].price;
            segundoName.text = gomenasai[1].name;
            segundoIdSlot = gomenasai[1].itemId;

            tercerPrice.text = "" + gomenasai[2].price;
            tercerName.text = gomenasai[2].name;
            tercerIdSlot = gomenasai[2].itemId;

            cuatroPrice.text = "" + gomenasai[3].price;
            cuatroName.text = gomenasai[3].name;
            cuartoIdSlot = gomenasai[3].itemId;

            quintoPrice.text = "" + gomenasai[4].price;
            quintoName.text = gomenasai[4].name;
            quintoIdSlot = gomenasai[4].itemId;
        }
        else
        {
            primerPrice.text = "5";
            primerName.text = "Item001";

            segundoPrice.text = "7";
            segundoName.text = "Item002";

            tercerPrice.text = "9";
            tercerName.text = "Item003";

            cuatroPrice.text = "15";
            cuatroName.text = "Item004";

            quintoPrice.text = "25";
            quintoName.text = "Item005";
        }
    }

    private List<ShopItem> JSonToList()
    {
               

        string jsonString = PlayerPrefs.GetString("ShopCatalog");

        var itemDict = JsonConvert.DeserializeObject<Dictionary<string, ShopItem>>(jsonString);

        List<ShopItem> itemsToSell = new List<ShopItem>(itemDict.Values);

        return itemsToSell;
    }

    private void ResetQtty()
    {
        primerPrecio = int.Parse(primerPrice.text);
        segundoPrecio = int.Parse(segundoPrice.text);
        tercerPrecio = int.Parse(tercerPrice.text);
        cuatroPrecio = int.Parse(cuatroPrice.text);
        quintoPrecio = int.Parse(quintoPrice.text);
    }
    private void RefreshList()
    {
        primerCantidad = 0;
        segundoCantidad = 0;
        tercerCantidad = 0;
        cuatroCantidad = 0;
        quintoCantidad = 0;
    }
    private void reset2Zero()
    {
        primerQtty.text = "0";
        segundoQtty.text = "0";
        tercerQtty.text = "0";
        cuatroQtty.text = "0";
        quintoQtty.text = "0";
    }
    public void OnPurchase()
    {
        PlayerManager.instance.currency -= total;
        // dar items
        GiveItems();
        reset2Zero();
        RefreshList();
        ResetQtty();
    }

    private void GiveItems()
    {
        Dictionary<string, int> compra = new Dictionary<string, int>();

        if (primerCantidad > 0)
            compra.Add(primerIdSlot, primerCantidad);

        if (segundoCantidad > 0)
            compra.Add(segundoIdSlot, segundoCantidad);

        if (tercerCantidad > 0)
            compra.Add(tercerIdSlot, tercerCantidad);

        if (cuatroCantidad > 0)
            compra.Add(cuartoIdSlot, cuatroCantidad);

        if (quintoCantidad > 0)
            compra.Add(quintoIdSlot, quintoCantidad);

        foreach (KeyValuePair<string, int> pair in compra)
        {
            foreach (var item in Inventory.instance.itemDataBase)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;
                    
                    Inventory.instance.AddItem(itemToLoad.data);
                }
            }
        }
    }

    public void ResetPage()
    {
        reset2Zero();
        RefreshList();
        ResetQtty();
    }
    #endregion

    #region buttons
    public void OnPrimerAdd()
    {
        primerCantidad++;
        primerQtty.text = primerCantidad.ToString();
    }

    public void OnPrimerLess()
    {
        if (primerCantidad > 0)
        {
            primerCantidad--;
            primerQtty.text = primerCantidad.ToString();
        }
    }
    public void OnSegundoAdd()
    {
        segundoCantidad++;
        segundoQtty.text = segundoCantidad.ToString();
    }
    public void OnSegundoLess()
    {
        if (segundoCantidad > 0)
        {
            segundoCantidad--;
            segundoQtty.text = segundoCantidad.ToString();
        }
    }
    public void OnTercerAdd()
    {
        tercerCantidad++;
        tercerQtty.text = tercerCantidad.ToString();
    }
    public void OnTercerLess()
    {
        if (tercerCantidad > 0)
        {
            tercerCantidad--;
            tercerQtty.text = tercerCantidad.ToString();
        }
    }
    public void OnCuatroAdd()
    {
        cuatroCantidad++;
        cuatroQtty.text = cuatroCantidad.ToString();
    }
    public void OnCuatroLess()
    {
        if (cuatroCantidad > 0)
        {
            cuatroCantidad--;
            cuatroQtty.text = cuatroCantidad.ToString();
        }
    }
    public void OnQuintoAdd()
    {
        quintoCantidad++;
        quintoQtty.text = quintoCantidad.ToString();
    }
    public void OnQuintoLess()
    {
        if (quintoCantidad > 0)
        {
            quintoCantidad--;
            quintoQtty.text = quintoCantidad.ToString();
        }
    }
    public void CanIBuy()
    {
        if (total > PlayerManager.instance.currency)
        {
            buyButton.SetActive(false);
        }
        else
        {
            buyButton.SetActive(true);
        }
    }
    #endregion
}
