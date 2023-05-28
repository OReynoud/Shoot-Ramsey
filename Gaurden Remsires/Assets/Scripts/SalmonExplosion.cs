using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SalmonExplosion : MonoBehaviour
{
    public GameObject hitBox;
    // Start is called before the first frame update
    private void Awake()
    {
        hitBox = transform.GetChild(0).gameObject;
    }

    void Start()
    {
        hitBox.transform.localScale = Vector3.one * Gordon.instance.salmonStartExplosionRadius;
        hitBox.transform.DOScale(Vector3.one * Gordon.instance.salmonEndExplosionRadius, Gordon.instance.salmonExplosionTime).OnComplete(
            () =>
            {
                hitBox.GetComponent<SpriteRenderer>().DOColor(Color.clear, Gordon.instance.salmonDecayTime * 0.5f);
                Destroy(gameObject,Gordon.instance.salmonDecayTime);
                Gordon.instance.LETHIMCOOK = false;
            } );
        
    }
}
