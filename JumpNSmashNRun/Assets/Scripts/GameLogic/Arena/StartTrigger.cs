using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class StartTrigger : MonoBehaviour
{

    [SerializeField] private ArenaManager manager = null;

    void OnTriggerEnter(Collider col)
    {
        // deactivating respawn and character movement
        // deactivating camera movement
        if (col.gameObject.CompareTag("Player"))
        {
            // order important for arena manager method which is called.
            manager.playerInTrigger(col.gameObject);
        }
    }


    public void destroyTrigger()
    {
        this.gameObject.SetActive(false);
        // coole animations and explosions and stuff :D
    }

}
