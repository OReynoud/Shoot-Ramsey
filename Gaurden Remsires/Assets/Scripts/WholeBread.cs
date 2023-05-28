using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class WholeBread : MonoBehaviour
{
    private bool canRotate = true;
    private float timer;

    private bool seeking = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(Vector3.one * Gordon.instance.wholeBreadFinalScale, 0.3f);
        timer = Gordon.instance.wholeBreadAimingTime;
        StartCoroutine(StartSeeking());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canRotate)
        {
            transform.rotation *= Quaternion.Euler(0,0,Gordon.instance.wholeBreadSpinningSpeed);
            return;
        }

        if (seeking)
        {
            var targetPos = new Vector3(PlayerMouvement.instance.transform.position.x, transform.position.y, 0);
            transform.position =
                Vector3.MoveTowards(transform.position, targetPos, Gordon.instance.wholeBreadAimingSpeed);
            if (timer > 0)
            {
                timer -= Time.fixedDeltaTime;
                
            }
            else
            {
                seeking = false;
                var centerPos = new Vector3(transform.position.x, 0, 0);
                Gordon.instance.explosionPos = centerPos;
                transform.DOMove(centerPos, Gordon.instance.wholeBreadShootingSpeed).OnComplete((() =>
                {
                    Instantiate(Gordon.instance.breadExplosionPrefab, Gordon.instance.explosionPos, Quaternion.identity);
                    Destroy(gameObject);
                }));
            }
        }
        
        
    }

    IEnumerator StartSeeking()
    {
        
        yield return new WaitForSeconds(Gordon.instance.wholeBreadLoadingTime);
        canRotate = false;
        transform.DORotate(Vector3.zero, 0.2f);
        yield return new WaitForSeconds(0.6f);
        seeking = true;
    }
}
