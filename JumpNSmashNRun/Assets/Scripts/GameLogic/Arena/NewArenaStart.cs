using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class NewArenaStart : MonoBehaviour {

    [SerializeField]
    public CameraNode m_FirstArenaPathNode = null;

    [SerializeField]
    public Transform m_ArenaSpawnPoint = null;

    [SerializeField]
    public CameraNode m_FirstAfterArenaPathNode = null;

    [SerializeField]
    public Transform m_AfterArenaSpawnPoint = null;

    [SerializeField]
    private NewArenaManager m_ArenaManager = null;

	void Start () {
	    if(null == m_ArenaManager)
        {
            FindArenaManager();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_ArenaManager.StartArenaPhase(this);
            this.gameObject.SetActive(false);
        }
    }
	
	

    private void FindArenaManager()
    {
        m_ArenaManager = Object.FindObjectOfType<NewArenaManager>();
        if (null == m_ArenaManager)
        {
            Debug.LogError("Could not find the NewArenaManager in NewArenaStart-Script of object " + this.gameObject.name, this);
        }
    }
}
