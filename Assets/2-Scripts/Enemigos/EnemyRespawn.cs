using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class EnemyRespawn : MonoBehaviour
{
    public float timeToRespawn;
    public GameObject enemyToRespawn;
    public bool isRespawning;

    // Start is called before the first frame update
    void Start()
    {
        enemyToRespawn = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator RespawnEnemy()
    {
        enemyToRespawn.SetActive(false);

        yield return new WaitForSeconds(timeToRespawn);
        enemyToRespawn.SetActive(true);

        enemyToRespawn.GetComponent<Enemy>().healthPoints = enemyToRespawn.GetComponent<EnemyHealth>().originalHealth;

        enemyToRespawn.GetComponentInChildren<SpriteRenderer>().material = enemyToRespawn.GetComponent<Blink>().original;

        enemyToRespawn.GetComponent<EnemyHealth>().recibeDano = false;

        //Aqui deberia ir la llamada al IENumerator que lleve la animacion de respawn
    }
}
