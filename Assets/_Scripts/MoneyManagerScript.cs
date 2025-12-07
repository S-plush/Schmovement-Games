using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManagerScript : MonoBehaviour
{
    public static int MoneyCount;
    
    [SerializeField] private GameObject UICounter;
    [SerializeField] private TMP_Text UIText;

    public void addMoney(int x) {
        UIText.text = x.ToString();



        //show UI for 3 seconds
        StartCoroutine(moneyUITimer());
    }



    private IEnumerator moneyUITimer() {
        UICounter.SetActive(true);
        yield return new WaitForSeconds(5f);
        UICounter.SetActive(false);

        //destroy the scrap after UI hides
        //Destroy(gameObject);
    }

}
