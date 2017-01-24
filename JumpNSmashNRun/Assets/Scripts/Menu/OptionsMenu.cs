using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

    public static float musicVolume = 0.5f;
    public static float soundVolum = 0.5f;
   
    
    Slider musicSlider;
    Slider soundSlider;
    // Use this for initialization
	void Start () {

        GameObject musicTemp = GameObject.Find("SliderMusic");
        GameObject soundTemp = GameObject.Find("SliderSound");
        if (musicTemp != null)
        {
            musicSlider = musicTemp.GetComponent<Slider>();
            if (musicSlider != null)
            {
                musicSlider.value = musicVolume;
            }
        }
     
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onValueChanged()
    {
        musicVolume = musicSlider.value;
    }
}
