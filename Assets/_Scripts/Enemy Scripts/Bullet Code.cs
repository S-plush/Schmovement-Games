using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCode : MonoBehaviour
{
    public int Damage;
    public Transform player;

    private Alpha alpha;

    // Start is called before the first frame update
    void Start()
    {
        alpha = player.GetComponent<Alpha>();
        this.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            alpha.TakeDamage(Damage);
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
