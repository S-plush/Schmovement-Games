using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private MovingPlatformPath path;
    [SerializeField] private float speed;

    private int targetPointIndex;
    private Transform previousPoint;
    private Transform nextPoint;
    private float pathTime;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        TargetNextPoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;
        float elapsedPercentage = elapsedTime / pathTime;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(previousPoint.position, nextPoint.position, elapsedPercentage);

        if(elapsedPercentage >= 1)
        {
            TargetNextPoint();
        }
    }

    private void TargetNextPoint()
    {
        previousPoint = path.GetPoint(targetPointIndex);
        targetPointIndex = path.GetNextPoint(targetPointIndex);
        nextPoint = path.GetPoint(targetPointIndex);
        elapsedTime = 0;
        float distanceToPoint = Vector3.Distance(previousPoint.position, nextPoint.position);
        pathTime = distanceToPoint * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
