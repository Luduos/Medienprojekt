using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectInput : MonoBehaviour {

    public static string player1Prefix = "P1_";
    public static string player2Prefix = "P2_";
    public static string player3Prefix = "P3_";
    public static string player4Prefix = "P4_";

    [SerializeField]
    private Dropdown player1DD;

    [SerializeField]
    private Dropdown player2DD;

    [SerializeField]
    private Dropdown player3DD;

    [SerializeField]
    private Dropdown player4DD;

    void Start()
    {
        player1DD.onValueChanged.AddListener(
            delegate
            {
                UpdatePlayerPrefixes();
            });
        player1DD.value = 0;

        player2DD.onValueChanged.AddListener(
           delegate
           {
               UpdatePlayerPrefixes(); 
           });
        player2DD.value = 1;

        player3DD.onValueChanged.AddListener(
           delegate
           {
               UpdatePlayerPrefixes();
           });
        player3DD.value = 2;

        player4DD.onValueChanged.AddListener(
           delegate
           {
               UpdatePlayerPrefixes();
           });
        player4DD.value = 3;


        UpdatePlayerPrefixes();
    }


    private void UpdatePlayerPrefixes()
    {
        player1Prefix = "P" + (player1DD.value + 1) + "_";
        player2Prefix = "P" + (player2DD.value + 1) + "_";
        player3Prefix = "P" + (player3DD.value + 1) + "_";
        player4Prefix = "P" + (player4DD.value + 1) + "_";
    }
}
