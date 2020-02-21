using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] float mScore = 0.0f;

    public void AddScore(float val)
    {
        mScore += val;
    }

    public float GetScore() { return mScore; }
}
