using UnityEngine;
using System.Collections;

public class FreezeGun : Useable {

	[SerializeField] private LayerMask m_FreezeGunCanHitTheseLayers = -1;
	[SerializeField] private Vector3 m_FreezeGunStartOffset = new Vector3(0f, 0.5f, 0f);
	[SerializeField] private float m_MaxHitDistance = 50.0f;
   
	private RaycastHit m_HitInformation;
	private Ray m_Ray;

    private float m_EffectDuration;

    private SoundManager m_SoundManager;

	override protected void Initialize(){
        m_EffectDuration = m_GamelogicEffect.EffectDuration;
        this.enabled = false;

        m_SoundManager = GameObject.FindObjectOfType<SoundManager>();
    }

	override public void Use(){
        this.enabled = true;
        CheckHit();
        ApplyFreezeGunVisualEffect ();

        PlaySoundEffect();
    }

    void Update()
    {
        if(m_EffectDuration < 0.0f)
        {
            StopSoundEffect();
            Destroy(this);
        }
        else
        {
            m_EffectDuration -= Time.deltaTime;

            CheckHit();
        }
    }

    private void CheckHit()
    {
        bool hasHitSomething;

        Shoot(out hasHitSomething);

        if (hasHitSomething)
        {
            ReactToHit();
        }
    }

	private void Shoot(out bool hasHitSomething){
		Vector3 rayOrigin = transform.position + m_FreezeGunStartOffset;
		Vector3 rayDirection = new Vector3(transform.forward.x, 0f, 0f);

		m_Ray = new Ray (rayOrigin, rayDirection);
		hasHitSomething = Physics.Raycast (
			m_Ray,
			out m_HitInformation, 
			m_MaxHitDistance, 
			m_FreezeGunCanHitTheseLayers);
	}

	private void ReactToHit(){
		GameObject hitGameObject = m_HitInformation.collider.gameObject;
        FreezeGunEffect freezeGunEffect = hitGameObject.GetComponent<FreezeGunEffect>();
		if(hitGameObject.CompareTag("Player") && null == freezeGunEffect){
			ApplyGamelogicEffectToPlayer (hitGameObject);
		}
	}

	private void ApplyFreezeGunVisualEffect(){
		VisualEffect visualEffect = ApplyVisualEffectTo (this.gameObject);

		FreezeGunVisualEffect freezeGunVisualEffect = visualEffect as FreezeGunVisualEffect;
		if(null != freezeGunVisualEffect){
			SetFreezeGunEndPosition ( freezeGunVisualEffect);
		}
	}

	private void SetFreezeGunEndPosition(FreezeGunVisualEffect freezeGunVisualEffect){
		//Transform freezeGunTargetTransform;
		Vector3 maxRayHitPosition;

        maxRayHitPosition = GetMaxRayHitPosition();
        freezeGunVisualEffect.InitializeEffect(maxRayHitPosition);

        // This code was used to attach to the first player hit, before every player in the guns wake could be hit
        /*if(null != m_HitInformation.collider){
			freezeGunTargetTransform = GetHitTransform ();
			freezeGunVisualEffect.InitializeEffect (freezeGunTargetTransform);
		}else{
			maxRayHitPosition = GetMaxRayHitPosition ();
			freezeGunVisualEffect.InitializeEffect (maxRayHitPosition);
		}*/
    }

	private Transform GetHitTransform(){
		return m_HitInformation.collider.gameObject.transform;
	}

	private Vector3 GetMaxRayHitPosition(){
		return m_Ray.origin + m_Ray.direction * m_MaxHitDistance;
	}

    private void PlaySoundEffect()
    {   
        if (null != m_SoundManager)
        {
            m_SoundManager.PlayFreezeSound();
        }
    }

    private void StopSoundEffect()
    {    
        if (null != m_SoundManager)
        {
            m_SoundManager.StopFreezeSound();
        }
    }
}
