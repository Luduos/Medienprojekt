using UnityEngine;
using System.Collections.Generic;

public class PunchPhysics : MonoBehaviour {

    private List<GameObject> m_target; 
    //private Rigidbody m_target = null;
    private bool m_playerHitFlag = false;
    private float m_timer;
    [SerializeField] private float m_disableTimer;

    [SerializeField]
    private float m_ExplosionForceMultiplier = 10.0f;

    [SerializeField]
    private float m_YOffset = 0.5f;
    
    private void Start()
    {
        m_target = new List<GameObject>();
        m_timer = m_disableTimer;
    }

    void Update()
    {
        if (m_playerHitFlag)
        {
            m_timer -= Time.deltaTime;

            //A Timer. During this time, the hit player will be incapable of moving, after the timer is over, the controlls will be reaktivated
            //Also resetting the timer, clearing the hit player list.
            if (m_timer <= 0)
            {
                for(int i=0; i< m_target.Count; i++)
                {
                    m_target[i].GetComponent<UserControl>().enabled = true;
                }
                m_target.Clear();
                m_timer = m_disableTimer;
                m_playerHitFlag = false;
            }
        }

    }

    //public void Punch(float time, float distance, Vector3 direction)
    public void Punch(float distance, Vector3 direction, float damage)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + new Vector3(0.0f, m_YOffset, 0.0f), direction);


        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.tag == "Player")
            {
                m_playerHitFlag = true;
                m_target.Add(hit.collider.gameObject);
                //Deactivate control of hit player
                hit.collider.GetComponent<UserControl>().enabled = false;
               
                direction.Normalize();
                //Add Force to the hit player
                if (m_target.Count > 0)
                {
                    for(int i=0; i<m_target.Count; i++)
                    {
                        //Adding a Force to the hit colliders
                        m_target[i].GetComponent<Rigidbody>().AddExplosionForce(m_ExplosionForceMultiplier * damage, this.transform.position, 0.0f, 0.0f, ForceMode.Force);
                        PlaySoundEffect();
                        //If arna mode is on, add Damage
                        if (m_target[i].GetComponent<PlayerHealthComponent>().GetIsArenaPhase())
                        {
                            m_target[i].GetComponent<PlayerHealthComponent>().SubtractHealth(damage);
                        }
                    }
                }
            }
        }
    }
    private void PlaySoundEffect()
    {
        SoundManager soundManager = GameObject.FindObjectOfType<SoundManager>();
        if (null != soundManager)
        {
            soundManager.PlayGetHitSound();
        }
    }

}
