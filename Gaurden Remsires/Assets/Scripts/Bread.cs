using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bread : BezierPathing
{
    // Start is called before the first frame update
    void Start()
    {
        speed = Gordon.instance.breadSpeed;
        allowCoroutine = true;
        transform.DOPunchScale(new Vector3(1, 1.4f, 1), 1.2f,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (allowCoroutine)
        {
            StartCoroutine(FollowPath(currentPath));
        }
    }

    private void OnDestroy()
    {
        Instantiate(Gordon.instance.breadExplosionPrefab, Gordon.instance.breadEndpoint.position, Quaternion.identity);
    }
}
