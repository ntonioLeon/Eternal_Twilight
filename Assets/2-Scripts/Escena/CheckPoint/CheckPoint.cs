using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string checkPointId;
    public bool activated;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Generate checkpoint Id")]
    private void GenerateId()
    {
        checkPointId = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            GameManager.instance.SetSpawnPoint(transform.position);
            GameManager.instance.DeactivateCheckPoints();
            ActivateCheckPoint();
            SaveManager.instance.SaveGame();
        }
    }

    public void ActivateCheckPoint()
    {
        activated = true;
        anim.SetBool("Active", true);
    }

    public void ResetCkeckPoint()
    {
        activated = false;
        anim.SetBool("Active", false);
    }
}
