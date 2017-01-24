using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


/*
 *TODO FOR THIS CLASS: 
 * DEATH ANIMATION
 * WIN ANIMATION
 * PLAYER RESPAWN FOR RUN PHASE 
 */

public class ArenaManager : MonoBehaviour
{

    //Just the trigger
    [SerializeField]
    private GameObject m_startTrigger = null;

    //Everything needed for the Arena Timer
    [SerializeField]
    private Text m_timer = null;
    [SerializeField]
    private float m_time = 5.0f;
    private bool m_timerOn = false;

    //Everything needed for the Countdown ("Pre-Timer")
    [SerializeField]
    private Text m_countdown = null;
    [SerializeField]
    private float m_countdownTime = 4.0f;
    private bool m_countdownOn = false;
    private float m_displayMessageDuration;

    //Scripts for the camera movement
    [SerializeField]
    private CameraNode m_ArenaNode = null;
    [SerializeField]
    private CameraNode m_afterArenaNode = null;
    private CameraMovement m_camScript;
    private bool m_TimeLeft = false;
    private bool m_TimeOver = false;
    [SerializeField]
    private int m_timeSpanForAnimation = -2;
    private bool m_oncePerFrameShouldReallyBeEnoughToAddRandomStuffMaybe = false;


    // List for Checking which player enters the arena Trigger, also needed for enabling/disabling other scripts
    private List<GameObject> m_playerList = null;
    //Script for activating the health component
    private PlayerHealthComponent m_playerHealthScript;  //WHY THE HECK IS THE GREEN LINE THERE?!
    //Script for enabling/disabling the despawn of each player
    private PlayerLeftScreen m_despawnScript;
    // current players in the game
    // TODO: constant from Game Manager -> inheritance to run phase and arena manager
    private int m_currMaxPlayer = 0;

    [SerializeField]
    private float m_winPoints;

    [SerializeField]
    private GameObject m_healthUI;

    private GameObject m_runPhaseManager;

    // #arenamode
    void Start()
    {
        //Initialising the member variables
        m_runPhaseManager = GameObject.Find("RunPhase");
        m_currMaxPlayer = GameObject.FindGameObjectsWithTag("Player").Length;
        m_camScript = Camera.main.GetComponent<CameraMovement>();
        m_playerList = new List<GameObject>();
        resetPlayerList();
        m_displayMessageDuration = m_time - 2.0f;
		m_afterArenaNode.StopBeforeMovingToThisNode = m_time + m_countdownTime + 2.0f;
        //Debug.Log("Start Method \n Player List Count: " + m_playerList.Count + " MaxPlayer: " + m_currMaxPlayer);
    }

    void Update()
    {
        //------ Countdown ------------------------------------------------------------------------
        if (m_countdownOn)
        {
            m_countdownTime -= Time.deltaTime;
            // countdown counting down, formatting
            if (m_countdownTime > 1)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(m_time);
                m_countdown.text = ((int)m_countdownTime).ToString();
            }
            // switch to arenaTimer and fight message
            // enable health component
            else
            {
                m_countdown.text = "FIGHT!";
                m_timer.gameObject.SetActive(true);
                m_timerOn = true;
                setHealthComponent(true);
            }

            // display fight message for a second before deactivating the text display
            if (m_countdownTime < -1)
                m_countdown.gameObject.SetActive(false);
        }
        //----------------------------------------------------------------------------------------------


        //----------- Timer---------------------------------------------------------------------------
        if (m_timerOn)
        {
            // If there is still time on the clock, format it and display it on the screen
            if (!m_TimeOver)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(m_time);
                m_timer.text = string.Format("{0:00} : {1:00} : {2:000}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
                // m_timer.text = string.Format("{0:00} : {1:00}", timeSpan.Minutes, timeSpan.Seconds);
            }

            // Checks if time is over. Ends the arena (plays win animation)
            if (m_time <= 0.0f && !(m_oncePerFrameShouldReallyBeEnoughToAddRandomStuffMaybe))
            {
                //Debug.Log("time < 0");
                EndArenaPhase();
            }
            //Checks if Last Man is standing. Regularly this happens when still time is left, therefore sets time = 0. Ends the arena (plays win animation)
            else if (everyoneIsDead() && !(m_oncePerFrameShouldReallyBeEnoughToAddRandomStuffMaybe))
            {
                m_time = 0.0f;
                EndArenaPhase();
            }


            //------------------------------------------------------------------------------------------------

            m_time -= Time.deltaTime;

            // wait 2 seconds for winning animation
            // reset to run phase
            if (m_time <= m_timeSpanForAnimation)
                switchToRunPhase();
        }

    }

    public void playerInTrigger(GameObject player)
    {
        // Debug.Log("Start Trigger \n Player List Count: " + m_playerList.Count + " MaxPlayer: " + m_currMaxPlayer);
        // deactivating respawn and character movement
        // deactivating camera movement
        m_despawnScript = player.GetComponent<PlayerLeftScreen>();
        m_despawnScript.SetInRunPhase(false);
        player.GetComponent<UserControl>().enabled = false;
        player.GetComponent<Rigidbody>().Sleep();
        m_playerList.Add(player);
        // Debug.Log("Trigger done \n Player List Count: " + m_playerList.Count + " MaxPlayer: " + m_currMaxPlayer);

        if (m_playerList.Count.Equals(m_currMaxPlayer))
        {

            //Activate the CountDown Text
            this.m_countdown.gameObject.SetActive(true);

            // activate camera movement
            m_countdownOn = true;
            m_camScript.setJumpToNode(m_ArenaNode);

            // activate player movement
            for (int i = 0; i < m_playerList.Count; i++)
            {
                m_playerList[i].GetComponent<UserControl>().enabled = true;
                m_playerList[i].GetComponent<Rigidbody>().WakeUp();
            }

            //enables the health Bars
            toggleHealthBars(true);

            // disable run phase manager for arena mode
            m_runPhaseManager.GetComponent<PlayerPointUpdate>().SetArenaPhase(true);

            // destroy trigger
            m_startTrigger.GetComponent<StartTrigger>().destroyTrigger();
        }
    }

    private void resetPlayerList()
    {
        m_playerList.Clear();
        //      Debug.Log(m_playerList.Count);
    }

    private void setHealthComponent(bool isActive)
    {
        // set Player Fighting / Health Component
        for (int i = 0; i < m_playerList.Count; ++i)
        {
             m_playerHealthScript = m_playerList[i].GetComponent<PlayerHealthComponent>();
             m_playerHealthScript.SetIsArenaPhase(isActive);
             m_playerHealthScript = null;
        }
    }

    private void switchToRunPhase()
    {
        //Reactivate the Spawn Component, restore max Health of Players, Reactivating movement, TODO: Respawn all players for the run phase
        for (int i = 0; i < m_playerList.Count; i++)
        {
            m_playerList[i].GetComponent<PlayerLeftScreen>().SetInRunPhase(true);
            m_playerList[i].GetComponent<PlayerHealthComponent>().SetHealthToMax();
            m_playerList[i].GetComponent<UserControl>().enabled = true;
            m_playerList[i].GetComponent<Rigidbody>().WakeUp();

        }

        //Deactivate all Health Components
        setHealthComponent(false);

        //TODO: put first node after arena inside method call
        m_camScript.setJumpToNode(m_afterArenaNode);

        //enable run phase script
        m_runPhaseManager.GetComponent<PlayerPointUpdate>().SetArenaPhase(false);

        //disables the Health bars
        toggleHealthBars(false);

        //reset the Player List for next Arena Phase
        resetPlayerList();

        //Deactivate this arena script
        enabled = false;
    }

    private bool everyoneIsDead()
    {
        bool allDead = false;
        int numberOfDeadPlayers = 0;
        for (int i = 0; i < m_playerList.Count; ++i)
        {
            if (m_playerList[i].GetComponent<PlayerHealthComponent>().GetPlayerIsDead())
                numberOfDeadPlayers++;
        }
        if (numberOfDeadPlayers == m_playerList.Count - 1)
        {
            allDead = true;
        }
        return allDead;
    }

    private void EndArenaPhase()
    {
        //Sets countdown Timer to false, ensures that this method is just called once and deactivates the countdown Dispaly.
        m_countdownOn = false;
        m_oncePerFrameShouldReallyBeEnoughToAddRandomStuffMaybe = true;
        m_TimeOver = true;
        m_countdown.gameObject.SetActive(false);

        //local variables needed to ensure that the right palyer is rewarded
        float health = 0;
        int playerNumber = -1;


        //a temporary List which stores Players who are still alive.
        List<GameObject> playersAlive = new List<GameObject>();

        //Deactivate the timer TextField
        m_timer.gameObject.SetActive(false);

        for (int i = 0; i < m_playerList.Count; ++i)
        {
            //Disables the user controll input, so the players wont move during the winning animation.
            m_playerList[i].GetComponent<UserControl>().enabled = false;
            m_playerList[i].GetComponent<Rigidbody>().Sleep();
        //Check if the player is alive, if yes, add to the alive list.
            if (!(m_playerList[i].GetComponent<PlayerHealthComponent>().GetPlayerIsDead()))
            {
                playersAlive.Add(m_playerList[i]);

                if (health < m_playerList[i].GetComponent<PlayerHealthComponent>().GetCurrentHealth())
                {
                    health = m_playerList[i].GetComponent<PlayerHealthComponent>().GetCurrentHealth();
                    playerNumber = i;
                }

            }
        }

        //Play animation for each person which is alive + adds points to the winner
        for (int i = 0; i < playersAlive.Count; ++i)
        {
            if (playerNumber > -1)
            {
                m_playerList[playerNumber].GetComponent<Score>().AddToScore(m_winPoints);
            }
            //TODO: Play win animation for each person which is alive
        }
    }


    public void PlayerDied(GameObject player)
    {
        //TODO: play Dead animation

        player.GetComponent<UserControl>().enabled = false;
        // maybe let rigibody active?
        player.GetComponent<Rigidbody>().Sleep();
    }

    private void toggleHealthBars(bool status)
    {
        for (int i = 0; i < m_playerList.Count; ++i)
        {
            m_healthUI.gameObject.transform.GetChild(i).gameObject.SetActive(status);
        }
    }

}
