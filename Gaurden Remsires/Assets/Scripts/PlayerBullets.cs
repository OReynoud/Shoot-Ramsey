using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullets : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.right * PlayerMouvement.instance.bulletSpeed;
    }
}
