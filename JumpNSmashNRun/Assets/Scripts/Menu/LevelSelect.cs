using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

    public bool m_IsPlayerInZone;
    public static int m_NumberOfPlayers = 2;
    public Button m_CurrentlyPressedButton;

	// Use this for initialization
	void Start () {
        m_NumberOfPlayers = 2;
        m_IsPlayerInZone = false;
	}
	
    public void NumberOfPlayers(int numberOfPlayers)
    {
        m_NumberOfPlayers = numberOfPlayers;
    }

    public void NumberOfPlayersWasSelected(Button selectedWith)
    {
        if(null != m_CurrentlyPressedButton)
        {
            m_CurrentlyPressedButton.interactable = true;
        }
        m_CurrentlyPressedButton = selectedWith;
        m_CurrentlyPressedButton.interactable = false;
    }
}
