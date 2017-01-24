using UnityEngine;
using System.Collections;

public class NotificationPanelSetter : MonoBehaviour {

    [SerializeField]
    private GameObject m_NotificationPanel = null;

    void Start()
    {
        if(null == m_NotificationPanel)
        {
            if(null != this.transform.parent)
            {
                m_NotificationPanel = transform.parent.gameObject;
            }
        }
    }

	void OnEnable()
    {
        if(null != m_NotificationPanel)
        {
            m_NotificationPanel.SetActive(true);
        }
    }

    void OnDisable()
    {
        if(null != m_NotificationPanel)
        {
            m_NotificationPanel.SetActive(false);
        }
    }
}
