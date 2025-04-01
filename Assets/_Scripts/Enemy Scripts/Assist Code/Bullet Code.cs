using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCode : MonoBehaviour
{
    public int Damage;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Damage = 1;
        this.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 5f;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }

    }
}