using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour , ISaveManager
{
    public static GameManager instance;

    public GameObject player;

    [SerializeField] private CheckPoint[] checkPoints;

    public Vector3 spawnPoint;

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

        spawnPoint = player.transform.position;
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
    }

    public void SaveData(ref GameData data)
    {
        data.checkPoints.Clear();
        
        foreach (var checkPoint in checkPoints) 
        {
            data.checkPoints.Add(checkPoint.checkPointId, checkPoint.activated);
        }
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
    }
}
