using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BreadExplosion : MonoBehaviour
{
    public GameObject hitBox;
    // Start is called before the first frame update
    private void Awake()
    {
        hitBox = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        hitBox.transform.localScale = Vector3.one * Gordon.instance.explosionStartRadius;
        hitBox.transform.DOScale(Vector3.one * Gordon.instance.explosionEndRadius, Gordon.instance.explosionTime).OnComplete(
            () =>
            {
                hitBox.GetComponent<SpriteRenderer>().DOColor(Color.clear, Gordon.instance.explosionStayDuration * 0.5f);
                Destroy(gameObject,Gordon.instance.explosionStayDuration);
            } );
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
