using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Salad : BezierPathing
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        var randomValue = 0.1f * Random.Range(-10, 11);
        if (Mathf.Abs(randomValue) < 0.2f)
        {
            randomValue = 1;
        }
        anim.SetFloat("Speed", randomValue);
        speed = Gordon.instance.saladSpeed;
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
