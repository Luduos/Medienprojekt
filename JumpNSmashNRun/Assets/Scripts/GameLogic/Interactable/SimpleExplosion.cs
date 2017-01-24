using UnityEngine;
using System.Collections;

public class SimpleExplosion : MonoBehaviour 
{
    [SerializeField]
    private float force;
    [SerializeField]
    private float radius;
    [SerializeField]
    private GameObject box;
    [SerializeField]
    private GameObject explosion;

    private GameObject[] players;

    [SerializeField] private float m_DisabledControlDuration = 2.0f;

    void Start()
    {
        this.enabled = false;
    }

    void OnCollisionEnter(Collision col)
    {
        if (box)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                players = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < players.Length; i++)
                {
                    players[i].GetComponent<Rigidbody>().AddExplosionForce(force, box.transform.position, radius, 0.5f, ForceMode.Impulse);
                }
                Instantiate(this.explosion, this.box.transform.position, Quaternion.identity);

                SoundManager soundManager = GameObject.FindObjectOfType<SoundManager>();
                if (null != soundManager)
                {
                    soundManager.PlayMineExplosionSound();
                }

                Destroy(this.box);
                
                Destroy(this.gameObject);

            }
        }
		

    }

}
