using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class BossActivation : MonoBehaviour
{
    public Enemy_NightBorn boss;

    private void Start()
    {
        boss.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UI_Boss.instance.BossActivation();

            StartCoroutine(spawn());
        }
    }

    IEnumerator spawn()
    {
        PlayerManager.instance.player.bossSpawning = true;
        PlayerManager.instance.player.stateMachine.ChangeState(PlayerManager.instance.player.idleState);
        boss.gameObject.SetActive(true);
        ScreenShake.instance.ShakeCamera(10f, 5f);
        yield return new WaitForSeconds(5f);

        PlayerManager.instance.player.bossSpawning = false;
        boss.bossFightBegun = true;
        Destroy(gameObject);
    }
}
