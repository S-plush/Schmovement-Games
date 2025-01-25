using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSpells : MonoBehaviour
{
    public GameObject explosionSpell;
    public GameObject spell2;

    // Start is called before the first frame update
    void Start()
    {
        explosionSpell.SetActive(true);
        spell2.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
