using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCode : MonoBehaviour
{
    public int Damage;
    public GameObject player;

    //private Alpha alpha;

    // Start is called before the first frame update
    void Start()
    {
        Damage = 1;
        this.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 5f;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //player.GetComponent<Alpha>().TakeDamage(Damage);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}