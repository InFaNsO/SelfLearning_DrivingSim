using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSensor : MonoBehaviour
{
    public float distanceFront = 5.0f;
    public float distanceLeft = 5.0f;
    public float distanceRight = 5.0f;

    private RaycastHit hitFront;
    private RaycastHit hitLeft;
    private RaycastHit hitRight;

    //[SerializeField] LayerMask layermask;
    [SerializeField]LayerMask layermask = 0;
    public void Start()
    {
        //layermask = 1 << 10;
        layermask = ~layermask;
    }

    public void Update()
    {
        bool hit = true;

        //front
        hit = Physics.Raycast(transform.position, transform.forward, out hitFront);
        distanceFront = hitFront.distance;
        //Left
        hit = Physics.Raycast(transform.position, -transform.right, out hitLeft);
        distanceLeft = hitLeft.distance;
        //Left
        hit = Physics.Raycast(transform.position, transform.right, out hitRight);
        distanceRight = hitRight.distance;
    }

    private void OnDrawGizmos()
    {
        //front
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * distanceFront);
        Gizmos.DrawSphere(transform.position + transform.forward * distanceFront, 0.5f);

        //Right
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * distanceRight);
        Gizmos.DrawSphere(transform.position + transform.right * distanceRight, 0.5f);

        //Left
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + -transform.right * distanceLeft);
        Gizmos.DrawSphere(transform.position + -transform.right * distanceLeft, 0.5f);
    }
}
