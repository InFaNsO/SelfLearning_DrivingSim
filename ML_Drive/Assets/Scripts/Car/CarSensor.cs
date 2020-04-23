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
    [SerializeField] public LayerMask layermask = 0;
    int mask = 0;
    public void Start()
    {
        //layermask = 1 << 10;
        //layermask = ~layermask;
        mask = layermask.value;
    }

    public void Update()
    {
        bool hit = true;

        //front
        hit = Physics.Raycast(transform.position, transform.forward, out hitFront, Mathf.Infinity, mask, QueryTriggerInteraction.Ignore);
        distanceFront = hitFront.distance;
        if (hit)
            Debug.Log("Had collision in front");
        
        //Left
        hit = Physics.Raycast(transform.position, -transform.right, out hitLeft, Mathf.Infinity, mask, QueryTriggerInteraction.Ignore);
        distanceLeft = hitLeft.distance;
        //Left
        hit = Physics.Raycast(transform.position, transform.right, out hitRight, Mathf.Infinity, mask, QueryTriggerInteraction.Ignore);
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
