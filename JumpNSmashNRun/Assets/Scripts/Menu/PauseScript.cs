using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour {

    GameObject m_PauseMenuCanvas;
    bool paused;


	// Use this for initialization
	void Start () {
        paused = false;
        Transform pauseMenuTransform = transform.Find("Canvas/PauseMenuCanvas");
        if(null != pauseMenuTransform)
        {
            m_PauseMenuCanvas = pauseMenuTransform.gameObject;
        }
        else
        {
            this.enabled = false;
            Debug.LogError("Couldn't find PauseMenuCanvas GameObject in PauseScript", this);
        }
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }

        if (paused)
        {
            m_PauseMenuCanvas.SetActive(true);
            Time.timeScale = 0;
            SoundManager sm = GameObject.FindObjectOfType<SoundManager>();
            AudioSource musicSource = sm.GetComponent<AudioSource>();
           musicSource.Pause();
        }
        else if(!paused)
        {
            m_PauseMenuCanvas.SetActive(false);
            Time.timeScale = 1;
            SoundManager sm = GameObject.FindObjectOfType<SoundManager>();
            AudioSource musicSource = sm.GetComponent<AudioSource>();
            musicSource.UnPause();
        }

	}

    public void Resume()
    {
        paused = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
