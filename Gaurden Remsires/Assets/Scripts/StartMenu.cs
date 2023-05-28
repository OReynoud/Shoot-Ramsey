using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Image startScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTheGame()
    {
        startScreen.DOColor(Color.clear, 1);
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2);
        PlayerMouvement.instance.enabled = true;
        Gordon.instance.enabled = true;
        gameObject.SetActive(false);
    }
}
