using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class NewArenaManager : MonoBehaviour {


    private PlayerPointUpdate m_PlayerPointUpdate = null;
    private GameObject[] m_Players = null;

    private NewArenaStart m_CurrentArenaStart = null;

    [SerializeField]
    private Vector3 m_PerPlayerSpawnOffset = new Vector3(0.5f, 0.0f, 0.0f);

    [SerializeField]
    private Text m_ArenaCountdownCanvas = null;

    [SerializeField]
    private Text m_ArenaTimerCanvas = null;

    [SerializeField]
    private Text m_NotificationCanvas = null;

    [SerializeField]
    private float m_ShowEnteringArenaPhaseTextTime = 4.0f;

    [SerializeField]
    private float m_ShowStartingArenaPhaseTextTime = 4.0f;

    [SerializeField]
    private float m_MaxArenaTime = 30.0f;

    [SerializeField]
    private float m_ShowEndingArenaPhaseTextTime = 4.0f;

    [SerializeField]
    private float m_NumberOfPointsForWinner = 1000.0f;

    private float m_PassedTime = 0.0f;

    List<GameObject> m_PlayersAliveList = new List<GameObject>();

    private string m_EndingArenaWhoWonText;

    private enum ArenaPhaseState
    {
        ENTERING,
        STARTING,
        INPROGRESS,
        ENDING,
        INVALID_STATE
    };

    private ArenaPhaseState m_CurrentArenaPhaseState = ArenaPhaseState.INVALID_STATE;

    void Start()
    {
        this.enabled = false;
        CheckCanvas();

    }
    // Use this for initialization
    void LateStart () {
        
        CheckCanvas();
    }

    void Update()
    {
        m_PassedTime += Time.deltaTime;

        switch (m_CurrentArenaPhaseState)
        {
            case ArenaPhaseState.ENTERING:
                UpdateEnteringArenaPhaseState();
                break;
            case ArenaPhaseState.STARTING:
                UpdateStartingArenaPhaseState();
                break;
            case ArenaPhaseState.INPROGRESS:
                UpdateInProgressArenaPhaseState();
                break;
            case ArenaPhaseState.ENDING:
                UpdateEndingArenaPhaseState();
                break;
            default:
                Debug.LogError("No valid current ArenaPhaseState in NewArenaManager", this);
                break;
        }
    }

    private void FindPlayers()
    {
        m_Players = GameObject.FindGameObjectsWithTag("Player");
        if (m_Players.Length < 1)
        {
            Debug.LogError("Couldn't find players in NewArenaManagerScript", this);
        }
    }

    private void FindPlayerPointUpdate()
    {
        m_PlayerPointUpdate = UnityEngine.Object.FindObjectOfType<PlayerPointUpdate>();
        if (null == m_PlayerPointUpdate)
        {
            Debug.LogError("Couldn't find PlayerPointUpdate in NewArenaManager", this);
        }
    }

    private void CheckCanvas()
    {
        if(null == m_ArenaCountdownCanvas)
        {
            Debug.LogError("Couldn't find Arena Countdown Canvas, please supply a GUIText in NewArenaManager script", this);
        }
        if(null == m_ArenaTimerCanvas)
        {
            Debug.LogError("Couldn't find Arena Timer Canvas, please supply a GUIText in NewArenaManager script", this);
        }
        if(null == m_NotificationCanvas)
        {
            Debug.LogError("Couldn't find Notification Canvas, please supply a GUIText in NewArenaManager script", this);
        }
    }

    public void StartArenaPhase(NewArenaStart arenaStart)
    {
        FindPlayers();
        FindPlayerPointUpdate();

        m_CurrentArenaStart = arenaStart;
        this.enabled = true;

        SetPlayerControl(false);
        SetArenaPhase(true);
        SetRigidBodiesEnabled(false);

        EnterEnteringArenaPhaseState();
    }

    private void EnterEnteringArenaPhaseState()
    {
        m_CurrentArenaPhaseState = ArenaPhaseState.ENTERING;
        m_NotificationCanvas.gameObject.SetActive(true);
        StopCamera();

    }

    private void UpdateEnteringArenaPhaseState()
    {
        if(m_PassedTime > m_ShowEnteringArenaPhaseTextTime)
        {
            LeaveEnteringArenaPhaseState();
        }
        else
        {
            m_NotificationCanvas.text = "Entering Arenaphase in \n" + ((int)(m_ShowEnteringArenaPhaseTextTime - m_PassedTime) + 1);
            SetRigidBodiesEnabled(false);
        }
    }

    private void LeaveEnteringArenaPhaseState()
    {
        m_PassedTime = 0.0f;
        m_NotificationCanvas.text = "";
        EnterStartingArenaPhaseState();
    }

    private void EnterStartingArenaPhaseState()
    {
        m_CurrentArenaPhaseState = ArenaPhaseState.STARTING;

        SetCameraPosition(m_CurrentArenaStart.m_FirstArenaPathNode.transform);
        
        SpawnPlayersAtTransform(m_CurrentArenaStart.m_ArenaSpawnPoint);
        SetPlayerHealthUIStatus(true);
    }

    private void UpdateStartingArenaPhaseState()
    {
        if(m_PassedTime > m_ShowStartingArenaPhaseTextTime)
        {
            LeaveStartingArenaPhaseState();
        }
        if(m_PassedTime > m_ShowStartingArenaPhaseTextTime - 1.0f)
        {
            m_NotificationCanvas.text = "FIGHT!";
        }
        else
        {
            m_NotificationCanvas.text = "Prepare to Fight in \n" + ((int)(m_ShowEnteringArenaPhaseTextTime - m_PassedTime) + 1);
        }
    }

    private void LeaveStartingArenaPhaseState()
    {
        m_NotificationCanvas.gameObject.SetActive(false);
        EnterInProgressArenaPhaseState();
        m_PassedTime = 0.0f;
    }

    private void EnterInProgressArenaPhaseState()
    {
        m_CurrentArenaPhaseState = ArenaPhaseState.INPROGRESS;
        m_ArenaTimerCanvas.gameObject.SetActive(true);
        SetPlayerControl(true);
        SwitchToCameraPath(m_CurrentArenaStart.m_FirstArenaPathNode);
    }

    private void UpdateInProgressArenaPhaseState()
    {
        UpdatePlayerAliveList();

        if(m_PlayersAliveList.Count == 1)
        {
            GameObject winner = m_PlayersAliveList[0];
            m_EndingArenaWhoWonText = winner.name + " won!";

            Score winnerScore = winner.GetComponent<Score>();
            if(null != winnerScore)
            {
                m_EndingArenaWhoWonText += " (+" + m_NumberOfPointsForWinner + " Points)";
                winnerScore.AddToScore(m_NumberOfPointsForWinner);
            }

            LeaveInProgressArenaPhaseState();
        }
        else if (m_PassedTime > m_MaxArenaTime)
        {
            m_EndingArenaWhoWonText = "Time's up - Nobody won!";
            LeaveInProgressArenaPhaseState();
        }
        else
        {
            float timeLeft = m_MaxArenaTime - m_PassedTime;
            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(timeLeft);
            m_ArenaTimerCanvas.text = string.Format("{0:00} : {1:00} : {2:000}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);


        }
    }

    private void LeaveInProgressArenaPhaseState()
    {
        m_PlayersAliveList.Clear();
        m_PassedTime = 0.0f;
        m_ArenaTimerCanvas.text = "";
        m_ArenaTimerCanvas.gameObject.SetActive(false);
        EnterEndingArenaPhaseState();
    }

    private void EnterEndingArenaPhaseState()
    {
        SetCameraPosition(m_CurrentArenaStart.m_FirstAfterArenaPathNode.transform);
        StopCamera();
        m_CurrentArenaPhaseState = ArenaPhaseState.ENDING;
        SpawnPlayersAtTransform(m_CurrentArenaStart.m_AfterArenaSpawnPoint);
        SetRigidBodiesEnabled(false);
        SetPlayerControl(false);

        m_NotificationCanvas.gameObject.SetActive(true);
    }

    private void UpdateEndingArenaPhaseState()
    {
       
        if (m_PassedTime > m_ShowEndingArenaPhaseTextTime)
        {
            LeaveEndingArenaPhaseState();
        }
        else
        {
            SetRigidBodiesEnabled(false);
            SetPlayerControl(false);
            m_NotificationCanvas.text = m_EndingArenaWhoWonText + "\n \n Returning to Runphase in \n" + ((int)(m_ShowEndingArenaPhaseTextTime - m_PassedTime) + 1);
        }
    }

    private void LeaveEndingArenaPhaseState()
    {
        m_EndingArenaWhoWonText = "";
        m_NotificationCanvas.gameObject.SetActive(false);
        SwitchToCameraPath(m_CurrentArenaStart.m_FirstAfterArenaPathNode);
        m_CurrentArenaPhaseState = ArenaPhaseState.INVALID_STATE;
        EndArenaPhase();
        m_PassedTime = 0.0f;
    }


    public void EndArenaPhase()
    {
        SetPlayerControl(true);
        SetArenaPhase(false);
        SetPlayerHealthUIStatus(false);

        m_CurrentArenaStart = null;
        this.enabled = false;
        SetRigidBodiesEnabled(true);

    }

    private void SetPlayerControl(bool enabled)
    {
        foreach(GameObject player in m_Players)
        {
            UserControl playerControl = player.GetComponent<UserControl>();
            if (playerControl)
            {
                playerControl.enabled = enabled;
            }
        }
    }

    private void SetArenaPhase(bool inArena)
    {
        m_PlayerPointUpdate.SetArenaPhase(inArena);

        SetPlayerLeftScreenInRunPhase(!inArena);

        SetPlayerHealthScriptInArenaPhase(inArena);
    }

    private void SwitchToCameraPath(CameraNode nextNode)
    {
        Camera mainCamera = Camera.main;
        if (null != mainCamera)
        {
            CameraMovement cameraMovement = mainCamera.GetComponent<CameraMovement>();
            if(null != cameraMovement)
            {
                cameraMovement.JumpToThisNode(nextNode);
            }
        }
    }

    private void SetCameraPosition(Transform newCameraPosition)
    {
        Camera mainCamera = Camera.main;
        if (null != mainCamera)
        {
            CameraMovement cameraMovement = mainCamera.GetComponent<CameraMovement>();
            if (null != cameraMovement)
            {
                cameraMovement.SetXYPosition(newCameraPosition.position.x, newCameraPosition.position.y);
            }
        }
    }

    private void SpawnPlayersAtTransform(Transform spawnPoint)
    {
        for (int i = 0; i < m_Players.Length; ++i)
        {
            GameObject player = m_Players[i];
            player.transform.position = spawnPoint.position + i * m_PerPlayerSpawnOffset;
        }
    }

    private void SetPlayerLeftScreenInRunPhase(bool isInRunPhase)
    {
        foreach (GameObject player in m_Players)
        {
            PlayerLeftScreen playerLeftScreen = player.GetComponent<PlayerLeftScreen>();
            playerLeftScreen.SetInRunPhase(isInRunPhase);
        }
    }

    private void SetRigidBodiesEnabled(bool enabled)
    {
        foreach (GameObject player in m_Players)
        {
            Rigidbody rigidbody = player.GetComponent<Rigidbody>();
            if (enabled)
            {
                rigidbody.WakeUp();
            }
            else
            {
                rigidbody.Sleep();
            }
        }
    }

    private void SetPlayerHealthScriptInArenaPhase(bool inArena)
    {
        // set Player Fighting / Health Component
        foreach (GameObject player in m_Players)
        {
            PlayerHealthComponent playerHealth = player.GetComponent<PlayerHealthComponent>();
            if(null != playerHealth)
            {
                playerHealth.SetIsArenaPhase(inArena);
            }
        }
    }

    private void StopCamera()
    {
        Camera mainCamera = Camera.main;
        if (null != mainCamera)
        {
            CameraMovement cameraMovement = mainCamera.GetComponent<CameraMovement>();
            if (null != cameraMovement)
            {
                cameraMovement.setMoveToNode(null);
            }
        }
    }

    private void SetPlayerHealthUIStatus(bool active)
    {
        foreach(GameObject player in m_Players)
        {
            HudWithAutoConnect playerHud = player.GetComponent<HudWithAutoConnect>();
            if(null != playerHud)
            {
                playerHud.SetHealthUIStatus(active);
            }
        }
    }
   
    private void UpdatePlayerAliveList()
    {
        m_PlayersAliveList.Clear();
        foreach(GameObject player in m_Players)
        {
            PlayerHealthComponent playerHealth = player.GetComponent<PlayerHealthComponent>();
            if(null != playerHealth)
            {
                bool playerIsDead = playerHealth.GetPlayerIsDead();
                if(!playerIsDead && !m_PlayersAliveList.Contains(player))
                {
                    m_PlayersAliveList.Add(player);
                }
            }
        }
    }
}
