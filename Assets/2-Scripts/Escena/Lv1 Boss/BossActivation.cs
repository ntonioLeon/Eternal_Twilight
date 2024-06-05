using System.Collections;
using UnityEngine;

public class BossActivation : MonoBehaviour
{
    public Enemy_NightBorn boss;
    public GameObject bossDialogEs;
    public GameObject bossDialogEn;

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
        PlayerManager.instance.player.GetComponent<Player>().Speak();
        PlayerManager.instance.player.bossSpawning = true;
        PlayerManager.instance.player.stateMachine.ChangeState(PlayerManager.instance.player.idleState);        
        ScreenShake.instance.ShakeCamera(10f, 3f);
        boss.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);

        if (CambioIdioma.instance.indiceIdioma == 1)
            bossDialogEs.SetActive(true);
        else
            bossDialogEn.SetActive(true);

        Destroy(gameObject);
    }
}
