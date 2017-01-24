using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameEnd : MonoBehaviour {

    private List<GameObject> m_playerList;
        
    private int m_currMaxPlayer;

    private float m_highscore = 0.0f;

    // saving current best player for message
    GameObject m_currentBestPlayer = null;

    [SerializeField]
    private Text m_winMessage = null;

    [SerializeField]
    private float m_ShowWinMessageTime = 5.0f;

    private bool m_AllPlayersInArea = false;

    void Start()
    {
        m_currMaxPlayer = GameObject.FindGameObjectsWithTag("Player").Length;

        m_playerList = new List<GameObject>();
        this.enabled = false;
    }

	void OnTriggerEnter(Collider col)
    {
        m_currMaxPlayer = GameObject.FindGameObjectsWithTag("Player").Length;

        if (col.gameObject.CompareTag("Player"))
        {
            GameObject player = col.gameObject;
            // deactivate respawn
            player.GetComponent<PlayerLeftScreen>().SetInRunPhase(false);
            // deactivate rigidbody and user control
            player.GetComponent<UserControl>().enabled = false;
            player.GetComponent<Rigidbody>().Sleep();
            // deactivate scoring
            GameObject.Find("RunPhase").GetComponent<PlayerPointUpdate>().enabled = false;
            m_playerList.Add(player);
            CalculateWinner();
        }

    }

    void Update()
    {
        if (m_AllPlayersInArea)
        {
            if (m_ShowWinMessageTime < 0.0f)
            {
                SceneManager.LoadScene("Menu");
            }
            else
            {
                m_winMessage.text = "" + m_currentBestPlayer.name + "\nwins the game!";
                m_ShowWinMessageTime -= Time.deltaTime;
            }
            
        }
    }

    private void CalculateWinner()
    {
        this.enabled = true;
        foreach(GameObject player in m_playerList)
        {
           
            // getting score of player
            float currentScore = player.GetComponent<Score>().GetScore();
            // update score if current player is better, else nothing happens
            if (m_highscore < currentScore)
            {
                m_highscore = currentScore;
                m_currentBestPlayer = player;
            } 
        }

        // if every player is at the finish line, display win message
        if (m_playerList.Count >= m_currMaxPlayer)
        {
            StopCamera();
            m_winMessage.gameObject.SetActive(true);
            m_AllPlayersInArea = true;
            
        }

    }
    
    private void StopCamera()
    {
        CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
        if(null != cameraMovement)
        {
            cameraMovement.setStop(true);
        }
    }

    private void CheckWinCanvas()
    {
        if(null == m_winMessage)
        {
            GameObject hud = GameObject.Find("HUD");
            if (null != hud)
            {
                Transform notificationCanvasTransform = hud.transform.Find("NotificationCanvas");
                if (null != notificationCanvasTransform)
                {
                    m_winMessage = notificationCanvasTransform.gameObject.GetComponent<Text>();
                }
                else
                {
                    Debug.LogError("Couldn't find NotificationCanvas in LevelStart to begin Countdown");
                    this.enabled = false;
                }
            }
        }
    }
}
