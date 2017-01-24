using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LoadOnClick : MonoBehaviour {


    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }


    public void Quit()
    {
        Application.Quit();
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("jeromeLevelTest");
    }
 
}

