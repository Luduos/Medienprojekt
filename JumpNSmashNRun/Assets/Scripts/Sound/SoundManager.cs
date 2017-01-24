using UnityEngine;
using System.Collections;
using System;

public class SoundManager : MonoBehaviour {

    [SerializeField]
    private AudioSource m_ExplosionSound;
    [SerializeField]
    private AudioSource m_FreezeSound;
    [SerializeField]
    private AudioSource m_MortarSound;
    [SerializeField]
    private AudioSource m_MineExplosionSound;
    [SerializeField]
    private AudioSource m_GetHitSound;
    [SerializeField]
    private AudioSource m_GetKickSound;
    [SerializeField]
    private AudioSource m_FootstepsSound;
    [SerializeField]
    private AudioSource m_JumpMale1;
    [SerializeField]
    private AudioSource m_JumpMale2;
    [SerializeField]
    private AudioSource m_JumpMale3;
    [SerializeField]
    private AudioSource m_JumpMale4;

    // Use this for initialization
    void Start () {
       
	
	}

    public void JumpMale1()
    {
        if (null != m_JumpMale1)
        {
            if (m_JumpMale1.isPlaying == false)
            {
                m_JumpMale1.Play();
                
            }
        }
    }
    public void JumpMale2()
    {
        if (null != m_JumpMale2)
        {
            if (m_JumpMale2.isPlaying == false)
            {
                m_JumpMale2.Play();
            }
        }
    }
    public void JumpMale3()
    {
        if (null != m_JumpMale3)
        {
            if (m_JumpMale3.isPlaying == false)
            {
                m_JumpMale3.Play();
            }
        }
    }

    public void JumpMale4()
    {
        if (null != m_JumpMale4)
        {
            if (m_JumpMale4.isPlaying == false)
            {
                m_JumpMale4.Play();
            }
        }
    }


    public void StopJumpSound()
    {
        if (null != m_JumpMale1)
        {
            m_JumpMale1.Stop();
            m_JumpMale2.Stop();
        }
    }

    public void FootstepsSound()
    {
        if (null != m_FootstepsSound)
        {
            m_FootstepsSound.Play();
        }
    }

    public void PlayGetHitSound()
    {
        if(null != m_GetHitSound)
        {
            m_GetHitSound.Play();
        }
    }
    public void GetKickSound()
    {
        if (null != m_GetKickSound)
        {
            m_GetKickSound.Play();
        }
    }
    

    public void StopFootstepsSound()
    {
        if (null != m_FootstepsSound)
        {
            m_FootstepsSound.Stop();
        }
    }

    public void StopPlayGetHitSound()
    {
        if (null != m_GetHitSound)
        {
            m_GetHitSound.Stop();
        }
    }


    public void PlayExplosionSound()
    {
        if(null != m_ExplosionSound)
        {
            m_ExplosionSound.Play();
        }
    }
    public void PlayFreezeSound()
    {
        if(null != m_FreezeSound)
        {
            m_FreezeSound.Play();
        }
    }

    public void StopFreezeSound()
    {
        if (null != m_FreezeSound)
        {
            m_FreezeSound.Stop();
        }
    }

    public void PlayMortarSound()
    {
        if(null != m_MortarSound)
        {
            m_MortarSound.Play();
        }
    }

    public void PlayMineExplosionSound()
    {
        if (null != m_MineExplosionSound)
        {
            m_MineExplosionSound.Play();
        }
    }


}
