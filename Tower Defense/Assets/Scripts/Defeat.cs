using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defeat : MonoBehaviour {
    private ParticleSystem particle;

	// Use this for initialization
	void Start () {
        particle = GetComponent<ParticleSystem>();
        Invoke("Delete", particle.main.duration);
	}
	
	public void Delete()
    {
        Destroy(gameObject);
    }
}
