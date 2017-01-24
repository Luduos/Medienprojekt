using UnityEngine;
using System.Collections;

public class PlayerHitboxEnterFront: MonoBehaviour {

    private Rigidbody m_colliderInBox;
    private bool m_front;

    private void Start()
    {
        m_front = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            m_colliderInBox = other.GetComponent<Rigidbody>();
            m_front = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        m_colliderInBox = null;
        m_front = false;
    }

    public bool getInBox()
    {
        return m_front;
    }

    public Rigidbody getColliderInBox()
    {
        return m_colliderInBox;
    }
}
