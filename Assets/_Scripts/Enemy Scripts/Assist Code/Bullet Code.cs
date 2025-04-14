using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCode : MonoBehaviour
{
    public int damage;

    public float speed;


    
    // Start is called before the first frame update
    void Start()
    {
        if(damage == 0) {
            damage = 1;
        }

        if (speed == 0) {
            speed = 5f;
        }

        StartCoroutine(decay());

        this.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Enemy Attack")
        {
            Destroy(this.gameObject);
        }

    }
    private IEnumerator decay() {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }



}