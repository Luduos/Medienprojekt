using UnityEngine;
using System.Collections;

//
public abstract class Useable : MonoBehaviour {
	
	[SerializeField] protected Sprite m_UseableSprite = null;

	[SerializeField] protected EffectOnPlayer m_GamelogicEffect = null;

	[SerializeField] protected VisualEffect m_VisualEffect = null;

	void Start(){
		Initialize ();
	}

	void OnDestroy(){
		RemoveItemSpriteInHud ();
	}

	protected abstract void Initialize ();
	public abstract void Use();

	public Sprite UseableSprite{
		get{
			return m_UseableSprite;
		}
		set{
			m_UseableSprite = value;
		}
	}
		
	public EffectOnPlayer UseableGamelogicEffect{
		get{
			return m_GamelogicEffect;
		}	
		set{
			m_GamelogicEffect = value;
		}
	}

	public VisualEffect UseableVisualEffect{
		get{
			return m_VisualEffect;
		}
		set{
			m_VisualEffect = value;
		}
	}

	public void OnPickup (){
		SetItemSpriteInHud ();
	}
		

	protected void SetItemSpriteInHud(){
		HudWithAutoConnect playerHud = gameObject.GetComponent<HudWithAutoConnect> ();
		if(null != playerHud){
			playerHud.ItemSprite = m_UseableSprite;
		}else{
			Debug.Log ("Couldn't find HudWithAutoConnect on " + gameObject.name + ", ItemSprite will not be shown.", this);
		}
	}

	protected void RemoveItemSpriteInHud(){
		HudWithAutoConnect playerHud = gameObject.GetComponent<HudWithAutoConnect> ();
		if(null != playerHud){
			playerHud.ItemSprite = null;
		}
	}

	protected void ShowMissingGamelogicEffectError(){
		DebugText debugText = gameObject.AddComponent<DebugText> ();
		debugText.SetDebugText ("Missing Useable effect.");
		Debug.LogError ("Missing EffectOnPlayer in Useable, destroying Useable", this);

		Destroy (debugText, 5.0f);
	}

	protected EffectOnPlayer ApplyGamelogicEffectToPlayer(GameObject player){
		EffectOnPlayer gamelogicEffect = (EffectOnPlayer)Utils.CopyComponentFromTo (m_GamelogicEffect, player);
		gamelogicEffect.OnStartEffect ();
		return gamelogicEffect;
	}

	protected VisualEffect ApplyVisualEffectTo(GameObject toVisualize){
		VisualEffect visualEffect = (VisualEffect)Utils.CopyComponentFromTo (m_VisualEffect, toVisualize);
		visualEffect.OnStartEffect ();
		return visualEffect;
	}
}
