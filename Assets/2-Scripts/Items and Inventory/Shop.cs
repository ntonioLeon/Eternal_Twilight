using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
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


    [Header("Segundo Item")]
    [SerializeField] private Image segundoImage;
    [SerializeField] private Button segundoAdd;
    [SerializeField] private Button segundoLess;
    [SerializeField] private Text segundoQtty;
    [SerializeField] private Text segundoPrice;
    [SerializeField] private Text segundoName;
    private int segundoPrecio;
    private int segundoCantidad;

    [Header("Tercer Item")]
    [SerializeField] private Image tercerImage;
    [SerializeField] private Button tercerAdd;
    [SerializeField] private Button tercerLess;
    [SerializeField] private Text tercerQtty;
    [SerializeField] private Text tercerPrice;
    [SerializeField] private Text tercerName;
    private int tercerPrecio;
    private int tercerCantidad;

    [Header("Cuarta Item")]
    [SerializeField] private Image cuatroImage;
    [SerializeField] private Button cuatroAdd;
    [SerializeField] private Button cuatroLess;
    [SerializeField] private Text cuatroQtty;
    [SerializeField] private Text cuatroPrice;
    [SerializeField] private Text cuatroName;
    private int cuatroPrecio;
    private int cuatroCantidad;

    [Header("Quinto Item")]
    [SerializeField] private Image quintoImage;
    [SerializeField] private Button quintoAdd;
    [SerializeField] private Button quintoLess;
    [SerializeField] private Text quintoQtty;
    [SerializeField] private Text quintoPrice;
    [SerializeField] private Text quintoName;
    private int quintoPrecio;
    private int quintoCantidad;

    [Header("Total Summit")]
    [SerializeField] private Text totalSummit;
    [SerializeField] private GameObject buyButton;
    private int total;

    void Start()
    {
        PlayerManager.instance.currency = 999;
        SetValues();
        RefreshList();
        ResetQtty();
    }

    void Update()
    {
        TotalSummit();
        CanIBuy();
    }
    public void TotalSummit()
    {
        total = primerCantidad * primerPrecio + segundoCantidad * segundoPrecio + tercerCantidad * tercerPrecio + cuatroCantidad * cuatroPrecio + quintoCantidad * quintoPrecio;
        totalSummit.text = total.ToString();
    }
    #region aux
    private void SetValues()
    {
        reset2Zero();
        if (PlayerPrefs.GetString("Logged").Equals("S"))
        {
            // recibo Diccionario key : value 
            List<string> gomenasai = JSonToList();
           
            /*foreach(KeyValuePair<string, string> pair in gomenasai)
            {
                ///pair.Key = pair.Value;
            }
            */
            primerPrice.text = gomenasai[0];
            primerName.text = gomenasai[1];

            segundoPrice.text = gomenasai[2];
            segundoName.text = gomenasai[3];

            tercerPrice.text = gomenasai[4];
            tercerName.text = gomenasai[5];

            cuatroPrice.text = gomenasai[6];
            cuatroName.text = gomenasai[7];

            quintoPrice.text = gomenasai[8];
            quintoName.text = gomenasai[9];
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
    private List<string> JSonToList()
    {
        return null;
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
        reset2Zero();
        RefreshList();
        ResetQtty();
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
        if(total > PlayerManager.instance.currency)
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
