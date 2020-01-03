using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doAnimAndSleep : MonoBehaviour {

    public string animTrigger;
    public float delay;
    public Animator anim;
	// Use this for initialization

    public void doAnim()
    {
        anim.SetTrigger(animTrigger);
       // Invoke("sleepAfterDelay", delay);
    }
	
    public void sleepAfterDelay()
    {
        gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
