using UnityEngine;
using System.Collections;

public class FireLightBehaviour : MonoBehaviour {
    private Light m_thisLight;
    private Color m_originalColor;
    private float m_timePassed;
    private float m_changeValue;

	// Use this for initialization
	void Start () {
        m_thisLight = this.GetComponent<Light>();
        if (m_thisLight != null)
            m_originalColor = m_thisLight.color;
        else
        {
            enabled = false;
            return;
        }
        m_changeValue = 0;
        m_timePassed = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
        m_timePassed = Time.time;
        m_timePassed = m_timePassed - Mathf.Floor(m_timePassed);
        m_thisLight.color = m_originalColor * CalculateChange();
	
	}

    private float CalculateChange()
    {
        m_changeValue = -Mathf.Sin(m_timePassed * 12 * Mathf.PI) * 0.05f + 0.95f;
        return m_changeValue;
    }
}
