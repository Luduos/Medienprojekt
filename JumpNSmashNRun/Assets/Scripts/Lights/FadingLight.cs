using UnityEngine;
using System.Collections;

public class FadingLight : MonoBehaviour {

	
	// Update is called once per frame
	void Update ()
    {
        GetComponent<Light>().range = Mathf.Lerp(GetComponent<Light>().range, 0, Time.deltaTime);
	}
}
