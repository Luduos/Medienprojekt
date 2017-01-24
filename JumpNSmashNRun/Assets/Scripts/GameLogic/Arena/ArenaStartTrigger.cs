using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ArenaStartTrigger : MonoBehaviour {

    [SerializeField]
    private GameObject manager;

    void OnTriggerEnter(Collider col)
    {
        // deactivating respawn and character movement
        // deactivating camera movement
        if(col.gameObject.CompareTag("Player"))
        {
            //manager.increasePlayerCount();
            //manager.playerInTrigger(col.GetComponent<GameObject>());
        }
    }

    public void destroyTrigger()
    {
        this.gameObject.SetActive(false);
        // coole animations and explosions and stuff :D
    }

}
