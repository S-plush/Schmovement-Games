using System.Collections;
using TMPro;
using UnityEngine;

public class MoneyRotateCode : MonoBehaviour
{
    public static int MoneyCount;

    private GameObject UICounter;
    private TMP_Text UIText;
    private MeshRenderer meshRenderer;
    private Collider col;

    private void Start()
    {
        UICounter = GameObject.Find("Canvas").transform.Find("ScrapCounter").gameObject;
        UIText = UICounter.transform.Find("Count").GetComponent<TMP_Text>();
        UIText.text = MoneyCount.ToString();
        UICounter.SetActive(false);

        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MoneyCount++;
            UIText.text = MoneyCount.ToString();

            //hide the scrap so it can't be triggered again
            meshRenderer.enabled = false;
            col.enabled = false;

            //show UI for 3 seconds
            StartCoroutine(moneyUITimer());
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, 5);
    }

    private IEnumerator moneyUITimer()
    {
        UICounter.SetActive(true);
        yield return new WaitForSeconds(5f);
        UICounter.SetActive(false);

        //destroy the scrap after UI hides
        Destroy(gameObject);
    }
}
