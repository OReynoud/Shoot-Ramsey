using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Gordon : MonoBehaviour
{
    [BoxGroup("Plain Salad")] public int saladAmount;
    [BoxGroup("Plain Salad")] public float saladSpeed;
    [BoxGroup("R A W  Steak")] public float steakSpeed;
    [BoxGroup("R A W  Steak")] public float steakAmount;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            //reduce Gordons hp
            Debug.Log("hit Ramsay");
            Destroy(col.gameObject);
        }
    }
    
}
