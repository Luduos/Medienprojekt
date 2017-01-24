using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelStart : MonoBehaviour {

    [SerializeField]
    private float m_GameStartCountdownTime = 3.0f;

    [SerializeField]
    private Text m_NotificationCanvas = null;

    [SerializeField]
    private CameraMovement m_CameraMovement = null;

    private List<GameObject> m_AllPlayersToActivate = new List<GameObject>();

    private PlayerPointUpdate m_RunPhaseUpdate;
	// Use this for initialization
	void Start () {

        SoundManager soundManager = GameObject.FindObjectOfType<SoundManager>();
        AudioSource musicSource = soundManager.GetComponent<AudioSource>();
        musicSource.volume = OptionsMenu.musicVolume;

        Application.targetFrameRate = 144;

        SceneManager.sceneLoaded += OnSceneLoaded;

        FindPlayers();
        ActivateCorrectNumberOfPlayers();

        FindRunPhaseUpdate();
        CheckNotificationCanvs();
        CheckCameraMovement();
        BeginGameStartCountDown();

        SetPlayerInput();
    }

   

    private void FindPlayers()
    {
        GameObject characterGameObject = GameObject.Find("Characters");
        if (null != characterGameObject)
        {
            for (int i = 0; i < LevelSelect.m_NumberOfPlayers; ++i)
            {
                Transform playerTransform = characterGameObject.transform.Find("Player" + (i + 1));
                if (null != playerTransform)
                {
                    m_AllPlayersToActivate.Add(playerTransform.gameObject);
                }
            }

        }
        else
        {
            Debug.LogError("Couldn't Find Characters GameObject, all already active Players will be spawned.");
            GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in foundPlayers)
            {
                m_AllPlayersToActivate.Add(player);
            }
        }
    }

    private void ActivateCorrectNumberOfPlayers()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in allPlayers)
        {
            player.SetActive(false);
        }
        for (int i = 0; i < LevelSelect.m_NumberOfPlayers; ++i)
        {
            foreach (GameObject player in m_AllPlayersToActivate)
            {
                if (player.name.Equals("Player" + (i + 1)))
                {
                    player.SetActive(true);
                }
            }
        }
    }

    private void FindRunPhaseUpdate()
    {
        m_RunPhaseUpdate = GameObject.FindObjectOfType<PlayerPointUpdate>();
        if (null == m_RunPhaseUpdate)
        {
            this.enabled = false;
            Debug.LogError("Didn't find PlayerPointUpdate Script!", this);
        }
    }

    private void CheckNotificationCanvs()
    {
        if(null == m_NotificationCanvas)
        {
            GameObject hud = GameObject.Find("HUD");
            if(null != hud)
            {
                Transform notificationCanvasTransform = hud.transform.Find("NotificationCanvas");
                if(null != notificationCanvasTransform)
                {
                    m_NotificationCanvas = notificationCanvasTransform.gameObject.GetComponent<Text>();
                }
                else
                {
                    Debug.LogError("Couldn't find NotificationCanvas in LevelStart to begin Countdown");
                    this.enabled = false;
                }
            }
        }
    }

    private void CheckCameraMovement()
    {    
        if(null == m_CameraMovement)
        {
            m_CameraMovement = GameObject.FindObjectOfType<CameraMovement>();
            if(null == m_CameraMovement)
            {
                Debug.LogError("Couldn't find camera movement Script in LevelStart", this);
                this.enabled = false;
            }
        }
    }

    void onEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
       
    private void BeginGameStartCountDown()
    {
        this.enabled = true;
        m_NotificationCanvas.gameObject.SetActive(true);
        m_CameraMovement.enabled = false;
    }

    private void EndGameStartCountDown()
    {
        m_RunPhaseUpdate.enabled = true;
        this.enabled = false;
        m_NotificationCanvas.text = "";
        m_NotificationCanvas.gameObject.SetActive(false);
        m_CameraMovement.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_GameStartCountdownTime < 0.01f)
        {
            EndGameStartCountDown();
        }
        else
        {
            m_NotificationCanvas.text = "Prepare to Run in " + (int)(m_GameStartCountdownTime + 1);
            m_RunPhaseUpdate.enabled = false;
            m_GameStartCountdownTime -= Time.deltaTime;  
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       // Debug.LogError("SCene geladen" + "  " + LoadOnClick.players);


    }

    private void SetPlayerInput()
    {
        foreach(GameObject player in m_AllPlayersToActivate)
        {
            UserControl playerControl = player.GetComponent<UserControl>();
            if(null != playerControl)
            {

                switch (player.name)
                {
                    case "Player1":
                        playerControl.m_PlayerPrefix = SelectInput.player1Prefix;
                        break;
                    case "Player2":
                        playerControl.m_PlayerPrefix = SelectInput.player2Prefix;
                        break;
                    case "Player3":
                        playerControl.m_PlayerPrefix = SelectInput.player3Prefix;
                        break;
                    case "Player4":
                        playerControl.m_PlayerPrefix = SelectInput.player4Prefix;
                        break;
                    default:
                        Debug.LogError("Incorrect Player Name found in LevelStart");
                        break;
                }
                
            }
        }
    }

}
