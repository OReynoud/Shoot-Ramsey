using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurves : MonoBehaviour
{
    public Transform[] positions = new[] { (Transform)null,(Transform)null,(Transform)null,(Transform)null };

    private Vector2 gizmosPositions;

    private void OnDrawGizmos()
    {
        for (float i = 0; i < 1; i += 0.05f)
        {
            gizmosPositions = Mathf.Pow(1 - i, 3) * positions[0].position + 
                              3 * Mathf.Pow(1-i,2)*i*positions[1].position +
                              3 *(1-i) *i*i*positions[2].position +
                              i*i*i *positions[3].position;
            Gizmos.DrawSphere(gizmosPositions,0.1f);
        }
        Gizmos.DrawLine(positions[0].position,positions[1].position);
        Gizmos.DrawLine(positions[2].position,positions[3].position);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
