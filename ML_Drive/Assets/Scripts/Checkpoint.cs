using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] float mPoints = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        var score = other.GetComponent<Score>();
        if (score == null)
            return;

        score.AddScore(mPoints);
    }
}
