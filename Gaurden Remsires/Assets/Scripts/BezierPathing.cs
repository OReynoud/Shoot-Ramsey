using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPathing : MonoBehaviour
{
    public Transform[] pathsToFollow;

    public int currentPath;

    private Vector2 nextPos;
    private float timer;

    public float speed;

    public bool allowCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected IEnumerator FollowPath(int number)
    {
        allowCoroutine = false;
        Vector2 p0 = pathsToFollow[number].GetChild(0).position;
        Vector2 p1 = pathsToFollow[number].GetChild(1).position;
        Vector2 p2 = pathsToFollow[number].GetChild(2).position;
        Vector2 p3 = pathsToFollow[number].GetChild(3).position;

        while (timer < 1)
        {
            timer += Time.fixedDeltaTime * speed;
            
            nextPos = Mathf.Pow(1 - timer, 3) * p0 + 
                              3 * Mathf.Pow(1-timer,2)*timer * p1 +
                              3 *(1-timer) *timer * timer * p2 +
                              timer*timer*timer * p3;
            transform.position = nextPos;
            yield return new WaitForEndOfFrame();
        }

        timer = 0;
        currentPath++;
        if (currentPath > pathsToFollow.Length - 1)
        {
            Destroy(transform.parent.gameObject);
        }

        allowCoroutine = true;
    }
}
