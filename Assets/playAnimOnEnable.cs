using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAnimOnEnable : MonoBehaviour
{
    public string animTriggerName;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.SetTrigger(animTriggerName);
    }

}
