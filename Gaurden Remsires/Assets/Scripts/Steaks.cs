using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steaks : BezierPathing
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim.SetFloat("Speed",0.1f * Random.Range(-10,11) + 0.5f);
        speed = Gordon.instance.steakSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (allowCoroutine)
        {
            StartCoroutine(FollowPath(currentPath));
        }
    }
}
