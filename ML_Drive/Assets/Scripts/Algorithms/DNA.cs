using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour
{
    public DeepNeuralNetwork myDeepNeuralNetwork = null;
    public float mFitness;

    public bool IsAlive = true;
    
    [SerializeField] float mMutationRate = 0.4f;
    [SerializeField] float CheckpointScore = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        myDeepNeuralNetwork = GetComponent<DeepNeuralNetwork>();
    }

    public DNA CrossOver(DNA otherParent)
    {
        DNA child = new DNA();

        if (Random.Range(0.0f, 1.0f) > mMutationRate)
        {
            for (int layer = 0; layer < myDeepNeuralNetwork.mLayers.Count; ++layer)
            {
                for (int output = 0; output < myDeepNeuralNetwork.mLayers[layer].mOutputCount; ++output)
                {
                    for (int input = 0; input < myDeepNeuralNetwork.mLayers[layer].GetInputCount(); ++input)
                    {
                        if (Random.Range(0.0f, 1.0f) < 0.5)
                        {
                            child.myDeepNeuralNetwork.mLayers[layer].SetWeight(myDeepNeuralNetwork.mLayers[layer].GetWeight(output, input), output, input);
                        }
                        else
                        {
                            child.myDeepNeuralNetwork.mLayers[layer].SetWeight(otherParent.myDeepNeuralNetwork.mLayers[layer].GetWeight(output, input), output, input);
                        }
                    }
                }

            }
        }
        else
        {
            child.Mutate();
        }

        return child;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CheckPoint" && IsAlive)
        {
            mFitness += 10.0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            IsAlive = false;
            return;
        }
    }

    public void AddScore(float points)
    {
        mFitness += points;
    }

    public void Mutate()
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) < mMutationRate)
        {
            myDeepNeuralNetwork.Randomize();
        }
    }
}
