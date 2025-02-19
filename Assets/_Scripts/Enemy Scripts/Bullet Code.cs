using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCode : MonoBehaviour
{

    public int Damage;
    // Start is called before the first frame update
    void Start()
    {

        this.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 5f;
    }

    private void OnCollisionEnter(Collision collision) {

        Debug.Log("destroyed");
        Destroy(this.gameObject);
    }
}
