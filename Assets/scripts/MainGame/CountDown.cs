using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public int countdownTime;
    public Text countdownDisplay;
    public GameObject Gun;
    
    private void Start()
    {
        Gun.SetActive(false);
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }
        countdownDisplay.text = "START!";

        //GameController.instance.BeginGame();

        yield return new WaitForSeconds(1f);
        countdownDisplay.gameObject.SetActive(false);
        Gun.SetActive(true);
    }
}