using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Gordon : MonoBehaviour
{
    public static Gordon instance;
    public Transform gun;
    private List<Vector3> positions = new List<Vector3>();
    public Image healthBar;
    [Foldout("Gordon Movement")] public float moveSpeed;
    [Foldout("Gordon Movement")] public Vector3 top1;
    [Foldout("Gordon Movement")] public Vector3 top2;
    [Foldout("Gordon Movement")] public Vector3 top3;
    [Foldout("Gordon Movement")] public Vector3 middle1;
    [Foldout("Gordon Movement")] public Vector3 middle2;
    [Foldout("Gordon Movement")] public Vector3 middle3;
    [Foldout("Gordon Movement")] public Vector3 bot1;
    [Foldout("Gordon Movement")] public Vector3 bot2;
    [Foldout("Gordon Movement")] public Vector3 bot3;
    [Foldout("Plain Salad")] public GameObject saladPrefab;
    [Foldout("Plain Salad")] public int saladAmount;
    [Foldout("Plain Salad")] public float saladSpeed;
    [Foldout("Plain Salad")] public float saladArc;
    [Foldout("Plain Salad")] public float saladWaves;
    [Foldout("Plain Salad")] public float delayBetweenSaladWaves;
    [Foldout("R A W  Steak")] public GameObject steakPrefab;
    [Foldout("R A W  Steak")] public float steakSpeed;
    [Foldout("R A W  Steak")] public int steakAmount;
    [Foldout("R A W  Steak")] public float steakArc;
    [Foldout("R A W  Steak")] public float delayBetweenSteaks;
    [Foldout("Plain Bread")] public GameObject breadPrefab;
    [Foldout("Plain Bread")] public BezierCurves topBread;
    [Foldout("Plain Bread")] public BezierCurves bottomBread;
    [Foldout("Plain Bread")] public float breadSpeed;
    [Foldout("Plain Bread")] public float breadSpawnTimer;
    [Foldout("Plain Bread")] public Transform breadEndpoint;
    [Foldout("Plain Bread")] public GameObject breadExplosionPrefab;
    [Foldout("Plain Bread")] public float explosionStartRadius;
    [Foldout("Plain Bread")] public float explosionEndRadius;
    [Foldout("Plain Bread")] public float explosionTime;
    [Foldout("Plain Bread")] public float explosionStayDuration;
    [Foldout("Whole Bread")] public GameObject wholeBreadPrefab;
    [Foldout("Whole Bread")] public float wholeBreadLoadingTime;
    [Foldout("Whole Bread")] public float wholeBreadAimingSpeed;
    [Foldout("Whole Bread")] public float wholeBreadAimingTime;
    [Foldout("Whole Bread")] public float wholeBreadSpinningSpeed;
    [Foldout("Whole Bread")] public float wholeBreadFinalScale;
    [Foldout("Whole Bread")] public float wholeBreadShootingSpeed;
    private GameObject currentWholeBread;
    [HideInInspector] public Vector3 explosionPos;
    [Foldout("rAaAw Salmon")] public GameObject salmonPrefab;
    [Foldout("rAaAw Salmon")] public GameObject salmonExplosionPrefab;
    [Foldout("rAaAw Salmon")] public Transform[] salmonPositions;
    [Foldout("rAaAw Salmon")] public int salmonAmount;
    [Foldout("rAaAw Salmon")] public float salmonMovingTime;
    [Foldout("rAaAw Salmon")] public float salmonCookingTime;
    [Foldout("rAaAw Salmon")] public float salmonSpinningRateOverTime;
    [Foldout("rAaAw Salmon")] public float salmonMaxSpinningRate;
    [Foldout("rAaAw Salmon")] public bool LETHIMCOOK;
    [Foldout("rAaAw Salmon")] public float salmonStartExplosionRadius;
    [Foldout("rAaAw Salmon")] public float salmonEndExplosionRadius;
    [Foldout("rAaAw Salmon")] public float salmonExplosionTime;
    [Foldout("rAaAw Salmon")] public float salmonDecayTime;
    [Foldout("Sound")] public AudioSource source;
    [Foldout("Sound")] public AudioClip[] steakWarning;
    [Foldout("Sound")] public AudioClip[] saladWarning;
    [Foldout("Sound")] public AudioClip[] breadWarning;
    [Foldout("Sound")] public AudioClip[] salmonWarning;
    [Foldout("Sound")] public AudioClip deathSound;
    [Foldout("Sound")] public float delayForSound;
    public int maxHealth;
    public int currentHealth;
    public bool aboveHalfHealth;
    public float atkSpeed1;
    public float atkSpeed2;
    private float atkTimer;
    public bool canAttack;
    private Collider2D coll;
    public bool isDying;
    public float timeBetweenExplosions;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
        }
        instance = this;
        coll = GetComponent<Collider2D>();
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
        currentHealth = maxHealth;
        ThrowSalmon();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            return;
        }
        healthBar.fillAmount = (float)currentHealth / maxHealth;
        if (currentHealth < maxHealth * 0.5f && aboveHalfHealth)
        {
            aboveHalfHealth = false;
        }
        if (atkTimer > 0 && canAttack)
        {
            atkTimer -= Time.fixedDeltaTime;
        }
        else if(canAttack)
        {
            canAttack = false;
            var chosenAttack = Random.Range(1, 6);
            switch (chosenAttack)
            {
                case 1:
                    ThrowSteaks();
                    break;
                case 2:
                    ThrowSalad();
                    break;
                case 3:
                    ThrowPlainBread();
                    break;
                case 4:
                    StartCoroutine(ThrowWholeBread());
                    break;
                case 5:
                    ThrowSalmon();
                    break;
            }
            atkTimer = aboveHalfHealth ? atkSpeed1 : atkSpeed2;
        }
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
            currentHealth--;
            Debug.Log("hit Ramsay");
            Destroy(col.gameObject);
        }

        if (currentHealth <= 0)
        {
            Win();
        }
    }

    void MoveToPos(Vector3 posToMove)
    {
        transform.DOMove(posToMove, moveSpeed);
    }
    IEnumerator EnableAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        canAttack = true;
    }
    void Win()
    {
        coll.enabled = false;
        PlayerMouvement.instance.coll.enabled = false;
        isDying = true;
        StartCoroutine(RequestExplosions());
        source.Stop();
        source.clip = deathSound;
        source.Play();
        transform.DOShakeScale(4, Vector3.right,100).OnComplete((() =>
        {
            isDying = false;
            transform.DOMoveX(transform.position.x + 10, 1);
            PlayerMouvement.instance.winScreen.DOColor(Color.white, 0.5f);
            PlayerMouvement.instance.musicChannel.Stop();
            PlayerMouvement.instance.musicChannel.clip = PlayerMouvement.instance.winMusic;
            PlayerMouvement.instance.musicChannel.volume = 0.3f;
            PlayerMouvement.instance.musicChannel.Play();
        }));
    }

    IEnumerator RequestExplosions()
    {
        while (isDying)
        {
            yield return new WaitForSeconds(timeBetweenExplosions);
            Instantiate(salmonExplosionPrefab, (Vector3)Random.insideUnitCircle + transform.position, Quaternion.identity);
        }
    }
    #region Steaks

    void ThrowSteaks()
    {
        MoveToPos(positions[Random.Range(0,positions.Count)]);
        transform.DOScale(transform.localScale, moveSpeed).OnComplete(() =>
        {
            var maxAngle = steakArc * 0.5f;
            for (int i = 0; i < steakAmount; i++)
            {
                var angleSpacing = steakArc / steakAmount;
                var currentAngle = maxAngle - (angleSpacing * i);
                StartCoroutine(SpawnSteaks(delayBetweenSteaks * i, currentAngle));
            }
        });
        StartCoroutine(EnableAttack(steakAmount * delayBetweenSteaks));
        source.Stop();
        source.clip = steakWarning[Random.Range(0, steakWarning.Length)];
        source.PlayDelayed(delayForSound);
    }

    IEnumerator SpawnSteaks(float spawnDelay, float angle)
    {
        yield return new WaitForSeconds(spawnDelay);
        gun.rotation = Quaternion.Euler(0,0,angle);
        Instantiate(steakPrefab, gun.position, gun.rotation);
    }

    #endregion

    #region Salad

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
        StartCoroutine(EnableAttack(saladWaves * delayBetweenSaladWaves));
        source.Stop();
        source.clip = saladWarning[Random.Range(0, saladWarning.Length)];
        source.PlayDelayed(delayForSound);
    }
    
    IEnumerator SpawnSalad(float spawnDelay)
    {
        yield return new WaitForSeconds(spawnDelay);
        
        var maxAngle = saladArc * 0.5f;
        var noise = Random.Range(-10, 11);
        for (int i = 0; i < saladAmount; i++)
        {
            var angleSpacing = saladArc / saladAmount;
            var spacing = maxAngle - (angleSpacing * i);
            var relativePos = gun.position - PlayerMouvement.instance.transform.position;
            var normedRelative = relativePos.normalized;
            var relativeAngle = Mathf.Atan2(normedRelative.y, normedRelative.x) * Mathf.Rad2Deg;
            gun.rotation = Quaternion.AngleAxis(relativeAngle + spacing + noise,Vector3.forward);
            Instantiate(saladPrefab, gun.position, gun.rotation);
        }
    }

    #endregion

    #region PlainBread

    void ThrowPlainBread()
    {
        MoveToPos(positions[Random.Range(0,positions.Count)]);
        transform.DOScale(transform.localScale, moveSpeed).OnComplete(() =>
        {
            StartCoroutine(SpawnBread());
        });
        
    }

    IEnumerator SpawnBread()
    {
        source.Stop();
        source.clip = breadWarning[Random.Range(0, breadWarning.Length)];
        source.PlayDelayed(delayForSound);
        StartCoroutine(EnableAttack(0));
        yield return new WaitForSeconds(breadSpawnTimer);
        var currentBread = Instantiate(breadPrefab, gun.position, Quaternion.identity);
        topBread = currentBread.transform.GetChild(0).gameObject.GetComponent<BezierCurves>();
        bottomBread = currentBread.transform.GetChild(1).gameObject.GetComponent<BezierCurves>();
        topBread.positions[3].position = breadEndpoint.position;
        bottomBread.positions[3].position = breadEndpoint.position;
            
        topBread.positions[1].position = new Vector3(
            topBread.positions[0].position.x,
            topBread.positions[0].position.y + 6, 
            0);
            
        topBread.positions[2].position = new Vector3(
            topBread.positions[3].position.x + 5,
            topBread.positions[3].position.y + 4, 
            0);
            
        bottomBread.positions[1].position = new Vector3(
            bottomBread.positions[0].position.x,
            bottomBread.positions[0].position.y - 6, 
            0);
            
        bottomBread.positions[2].position = new Vector3(
            bottomBread.positions[3].position.x + 5,
            bottomBread.positions[3].position.y - 4, 
            0);
        yield return new WaitUntil(() => !topBread);
        Debug.Log("No more bread");
        Instantiate(breadExplosionPrefab, breadEndpoint.position, Quaternion.identity);
    }

    #endregion

    #region WholeBread

    IEnumerator ThrowWholeBread()
    {
        MoveToPos(positions[Random.Range(0,positions.Count)]);
        transform.DOScale(transform.localScale, moveSpeed);
        source.Stop();
        source.clip = breadWarning[Random.Range(0, breadWarning.Length)];
        source.PlayDelayed(delayForSound);
        yield return new WaitForSeconds(moveSpeed);
        StartCoroutine(EnableAttack(wholeBreadLoadingTime));
        currentWholeBread = Instantiate(wholeBreadPrefab, gun.position + Vector3.up, Quaternion.identity);
        currentWholeBread.transform.DOMove(new Vector3(currentWholeBread.transform.position.x, 4, 0),wholeBreadLoadingTime);
        
        var bottom = Instantiate(wholeBreadPrefab, gun.position - Vector3.up, Quaternion.identity);
        bottom.transform.DOMove(new Vector3(bottom.transform.position.x,  - 4, 0),wholeBreadLoadingTime);
        
    }

    #endregion

    #region Salmon

    void ThrowSalmon()
    {
        MoveToPos(positions[Random.Range(0,positions.Count)]);
        LETHIMCOOK = false;
        List<Transform> availablePositions = new List<Transform>();
        availablePositions.AddRange(salmonPositions);
        for (int i = 0; i < salmonAmount; i++)
        {
            var chosenIndex = Random.Range(0, availablePositions.Count);
            StartCoroutine(SpawnSalmon(availablePositions[chosenIndex]));
            availablePositions.RemoveAt(chosenIndex);
        }
    }

    IEnumerator SpawnSalmon(Transform location)
    {
        source.Stop();
        source.clip = salmonWarning[Random.Range(0, salmonWarning.Length)];
        source.PlayDelayed(delayForSound);
        yield return new WaitForSeconds(moveSpeed);
        StartCoroutine(EnableAttack(salmonMovingTime));
        var currentSalmon = Instantiate(salmonPrefab, gun.position, Quaternion.identity);
        currentSalmon.transform.DOMove(location.position, salmonMovingTime);
        yield return new WaitForSeconds(salmonMovingTime);
        LETHIMCOOK = true;

    }

    #endregion
    
    
}
