using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    public GameObject mapMenu;
    public GameObject fondo;
    public GameObject openMap;
    public GameObject closeMap;
    public Canvas canvas;

    private bool isMapping;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        isMapping = false;
    }

    // Update is called once per frame
    void Update()
    {
        Maping();
    }

    public void Maping()
    {
        if (Input.GetKeyDown(KeyCode.M) && !isMapping && !GameObject.Find("Player").GetComponent<Player>().isSpeaking && !PauseMenu.instance.isPaused)
        {
            isMapping = true;
            openMap.SetActive(true);
            Instantiate(openMap, canvas.transform);
            StartCoroutine(OpenCourutine());
            openMap.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.M) && isMapping && !PauseMenu.instance.isPaused)
        {
            closeMap.SetActive(true);
            Instantiate(closeMap, canvas.transform);
            StartCoroutine(CloseCourutine());
            closeMap.SetActive(false);
            isMapping = false;
        }
    }
    IEnumerator OpenCourutine()
    {
        yield return new WaitForSeconds(0.5f);
        fondo.SetActive(true);
        mapMenu.SetActive(true);
    }

    IEnumerator CloseCourutine()
    {
        fondo.SetActive(false);
        mapMenu.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }
}
