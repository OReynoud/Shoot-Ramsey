using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gordon : MonoBehaviour
{
    public static Gordon instance;
    public Transform gun;
    private List<Vector3> positions = new List<Vector3>();
    [BoxGroup("Gordon Movement")] public float moveSpeed;
    [BoxGroup("Gordon Movement")] public Vector3 top1;
    [BoxGroup("Gordon Movement")] public Vector3 top2;
    [BoxGroup("Gordon Movement")] public Vector3 top3;
    [BoxGroup("Gordon Movement")] public Vector3 middle1;
    [BoxGroup("Gordon Movement")] public Vector3 middle2;
    [BoxGroup("Gordon Movement")] public Vector3 middle3;
    [BoxGroup("Gordon Movement")] public Vector3 bot1;
    [BoxGroup("Gordon Movement")] public Vector3 bot2;
    [BoxGroup("Gordon Movement")] public Vector3 bot3;
    [BoxGroup("Plain Salad")] public GameObject saladPrefab;
    [BoxGroup("Plain Salad")] public int saladAmount;
    [BoxGroup("Plain Salad")] public float saladSpeed;
    [BoxGroup("Plain Salad")] public float saladArc;
    [BoxGroup("Plain Salad")] public float saladWaves;
    [BoxGroup("Plain Salad")] public float delayBetweenSaladWaves;
    [BoxGroup("R A W  Steak")] public GameObject steakPrefab;
    [BoxGroup("R A W  Steak")] public float steakSpeed;
    [BoxGroup("R A W  Steak")] public int steakAmount;
    [BoxGroup("R A W  Steak")] public float steakArc;
    [BoxGroup("R A W  Steak")] public float delayBetweenSteaks;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
        }
        instance = this;
    }

    void Start()
    {
        positions.Add(top1);
        positions.Add(top2);
        positions.Add(top3);
        positions.Add(middle1);
        positions.Add(middle2);
        positions.Add(middle3);
        positions.Add(bot1);
        positions.Add(bot2);
        positions.Add(bot3);
        ThrowSalad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(top1,0.1f);
        Gizmos.DrawSphere(top2,0.1f);
        Gizmos.DrawSphere(top3,0.1f);
        Gizmos.DrawSphere(middle1,0.1f);
        Gizmos.DrawSphere(middle2,0.1f);
        Gizmos.DrawSphere(middle3,0.1f);
        Gizmos.DrawSphere(bot1,0.1f);
        Gizmos.DrawSphere(bot2,0.1f);
        Gizmos.DrawSphere(bot3,0.1f);
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

    void MoveToPos(Vector3 posToMove)
    {
        transform.DOMove(posToMove, moveSpeed);
    }

    #region Steaks

    void ThrowSteaks()
    {
        MoveToPos(positions[Random.Range(0,positions.Count)]);
        transform.DOScale(transform.localScale, moveSpeed).OnComplete(() =>
        {
            var maxAngle = steakArc * 0.5f;
            var minAngle = -maxAngle;
            for (int i = 0; i < steakAmount; i++)
            {
                var angleSpacing = steakArc / steakAmount;
                var currentAngle = maxAngle - (angleSpacing * i);
                StartCoroutine(SpawnSteaks(delayBetweenSteaks * i, currentAngle));
            }
        });
    }

    IEnumerator SpawnSteaks(float spawnDelay, float angle)
    {
        yield return new WaitForSeconds(spawnDelay);
        gun.rotation = Quaternion.Euler(0,0,angle);
        Instantiate(steakPrefab, gun.position, gun.rotation);
    }

    #endregion

    void ThrowSalad()
    {
        MoveToPos(positions[Random.Range(0,positions.Count)]);
        transform.DOScale(transform.localScale, moveSpeed).OnComplete(() =>
        {
            for (int i = 0; i < saladWaves; i++)
            {
                
                StartCoroutine(SpawnSalad(delayBetweenSaladWaves* i));
            }
        });
    }
    
    IEnumerator SpawnSalad(float spawnDelay)
    {
        yield return new WaitForSeconds(spawnDelay);
        
        var maxAngle = saladArc * 0.5f;
        for (int i = 0; i < saladAmount; i++)
        {
            var angleSpacing = saladArc / saladAmount;
            var currentAngle = maxAngle - (angleSpacing * i);
            var relativePos = PlayerMouvement.instance.transform.position - gun.position;
            gun.rotation = Quaternion.LookRotation(relativePos, Vector3.forward);
            gun.rotation = Quaternion.Euler(0,0,currentAngle + gun.rotation.eulerAngles.z);
            Instantiate(saladPrefab, gun.position, gun.rotation);
        }
    }
}
