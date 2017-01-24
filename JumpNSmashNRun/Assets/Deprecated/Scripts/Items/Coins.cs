using UnityEngine;
using System.Collections;

public class Coins : MonoBehaviour {
   	
	[SerializeField]
    public float m_ScoreToAdd = 10.0f;

    //Player have to be tagged "Player"
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
			// try to get score script
			Score score = other.gameObject.GetComponent<Score>();
			if(score){
				score.AddToScore (m_ScoreToAdd);
				Destroy (this.gameObject);
			}
			// only add points, if the other objects can obtain points
        }
    }
}
