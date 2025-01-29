using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//we can rename this script to an appropriate spell name later
public class ShootingSpell : MonoBehaviour
{
    public Transform rotationAimingPoint;
    public Alpha alpha;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Aiming()
    {
        if (rotationAimingPoint.rotation.z < .2f && rotationAimingPoint.rotation.z > -.19f)
        {
            //spell goes right
        }
        else if (rotationAimingPoint.rotation.z < -.2f && rotationAimingPoint.rotation.z > -.49f)
        {
            //spell goes down right
        }
        else if (rotationAimingPoint.rotation.z < -.5f && rotationAimingPoint.rotation.z > -.79f)
        {
            //spell goes down
        }
        else if (rotationAimingPoint.rotation.z < -.8f && rotationAimingPoint.rotation.z > -.94f)
        {
            //spell goes down left
        }
        else if (rotationAimingPoint.rotation.z < -.95f || rotationAimingPoint.rotation.z > .95f)
        {
            //spell goes left
        }
        else if (rotationAimingPoint.rotation.z < .94f && rotationAimingPoint.rotation.z > .81f)
        {
            //spell goes up left
        }
        else if (rotationAimingPoint.rotation.z < .8f && rotationAimingPoint.rotation.z > .5f)
        {
            //spell goes up
        }
        else if (rotationAimingPoint.rotation.z < .49f && rotationAimingPoint.rotation.z > .21f)
        {
            //spell goes up right
        }
    }
}
