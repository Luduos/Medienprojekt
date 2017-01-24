using UnityEngine;
using System.Collections;

public class FlickerLight : MonoBehaviour {

    public Light myLight;

	// Use this for initialization
	void Start () {
        myLight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Random.value > 0.9)
        {
            if(myLight.enabled == true)
            {
                myLight.enabled = false;
            }
            else
            {
                myLight.enabled = true;
            }
        }
        
	
	}
}
