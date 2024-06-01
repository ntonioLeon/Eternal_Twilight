using Fungus;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour , ISaveManager
{
    public static GameManager instance;

    public GameObject player;
    [SerializeField] private CheckPoint[] checkPoints;
    public Vector3 spawnPoint;
    public GameObject porton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }

    private void Start()
    {
        checkPoints = FindObjectsOfType<CheckPoint>();

        spawnPoint = new Vector3(SaveManager.instance.gameData.x, SaveManager.instance.gameData.y, SaveManager.instance.gameData.z);

        SpawnPlayer();

        porton.transform.rotation = Quaternion.Euler(0, 0, SaveManager.instance.gameData.portonRotation);

        
    }

    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(scene.name);
    }

    public void DeactivateCheckPoints()
    {
        for(int i = 0; i < checkPoints.Length; i++)
        {
            checkPoints[i].ResetCkeckPoint();
        }
    }

    public void LoadData(GameData data)
    {
        foreach(KeyValuePair<string, bool> pair in data.checkPoints)
        {
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.checkPointId == pair.Key && pair.Value == true)
                {
                    checkPoint.ActivateCheckPoint();
                }
            }
        }

        spawnPoint = new Vector3(data.x, data.y, data.z);

        porton.transform.rotation = Quaternion.Euler(0, 0, data.portonRotation);
    }

    public void SaveData(ref GameData data)
    {
        data.checkPoints.Clear();
        
        foreach (var checkPoint in checkPoints) 
        {
            data.checkPoints.Add(checkPoint.checkPointId, checkPoint.activated);
        }

        data.x = spawnPoint.x;
        data.y = spawnPoint.y;
        data.z = spawnPoint.z;

        data.portonRotation = porton.transform.eulerAngles.z;
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(2f);
        PauseMenu.instance.SwichOffEndScreen();
        player.GetComponent<Player>().stateMachine.ChangeState(player.GetComponent<Player>().idleState);

        player.transform.position = spawnPoint;
        player.GetComponent<CharacterStats>().currentHealth = player.GetComponent<CharacterStats>().maxHealth.GetValue();

        PlayerManager.instance.GetComponent<CharacterStats>().isDead = false;
        player.GetComponent<Player>().StartMal();
    }

    private void SpawnPlayer()
    {
        player.transform.position = spawnPoint;
    }
}
