using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using Cinemachine;

public class BossActivation : MonoBehaviour
{
    public Enemy_NightBorn boss;
    public GameObject bossDialog;

    private void Start()
    {
        boss.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UI_Boss.instance.BossActivation();

            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn()
    {
        PlayerManager.instance.player.bossSpawning = true;
        PlayerManager.instance.player.stateMachine.ChangeState(PlayerManager.instance.player.idleState);
        boss.gameObject.SetActive(true);
        ScreenShake.instance.ShakeCamera(10f, 3f);

        yield return new WaitForSeconds(3f);

        bossDialog.SetActive(true);
        Destroy(gameObject);
    }
}
