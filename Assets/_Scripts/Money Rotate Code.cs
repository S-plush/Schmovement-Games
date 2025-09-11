using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyRotateCode : MonoBehaviour
{



    private void FixedUpdate() {
        this.gameObject.transform.Rotate(0, 0, 5);
    }
}
