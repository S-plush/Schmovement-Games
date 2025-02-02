using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 mousePosition;

    public Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        Vector3 rotation = mousePosition - transform.position;
        float rotateZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotateZ);
    }

    public Vector3 AimDirection()
    {
        mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        return (mousePosition - transform.position).normalized;
    }
}
