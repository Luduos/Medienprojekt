using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private bool paused;
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType < GameManager > ();
            }
            return GameManager.instance;
        }

    }

    public bool Paused
    {
        get
        {
            return paused;
        }

    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
	}

    public void PauseGame()
    {
        paused = !paused;
    }
}
