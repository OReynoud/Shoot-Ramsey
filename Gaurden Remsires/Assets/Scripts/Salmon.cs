using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Salmon : MonoBehaviour
{
    private float cookingTimer;
    private bool cooking;

    public float currentSpinningSpeed = 0;

    public GameObject SalmonExplosion;
    // Start is called before the first frame update
    void Start()
    {
        cookingTimer = Gordon.instance.salmonCookingTime;
        StartCoroutine(AutoDestroy());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Gordon.instance.LETHIMCOOK && !cooking)
        {
            cooking = true;
            StartCoroutine(Cooking());

        }
    }

    IEnumerator Cooking()
    {
        while (cookingTimer > 0)
        {
            cookingTimer -= Time.fixedDeltaTime;
            currentSpinningSpeed += Gordon.instance.salmonSpinningRateOverTime;
            if (currentSpinningSpeed > Gordon.instance.salmonMaxSpinningRate)
            {
                currentSpinningSpeed = Gordon.instance.salmonMaxSpinningRate;
            }
            transform.rotation *= Quaternion.Euler(0,0,currentSpinningSpeed);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(Gordon.instance.salmonCookingTime + Gordon.instance.salmonMovingTime);
        Instantiate(SalmonExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
