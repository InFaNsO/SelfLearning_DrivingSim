﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(8,1,2));
        
    }
}