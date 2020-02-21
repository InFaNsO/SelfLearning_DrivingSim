using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {

    public GameObject mNextNode = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(this.transform.position, 0.2f);

        if(mNextNode != null)
        {
            Gizmos.DrawLine(this.transform.position, mNextNode.transform.position);
        }

        Gizmos.DrawSphere(this.transform.position, 1.0f);
    }
}
