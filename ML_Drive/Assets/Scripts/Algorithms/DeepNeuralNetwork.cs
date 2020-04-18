using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepNeuralNetwork : MonoBehaviour
{
    [SerializeField] int mNumberOfInputs = 4;
    [SerializeField] int mNumberOfOutputs = 3;
    [SerializeField] List<int> LayerNumNnurons = new List<int>();
    [HideInInspector] public List<NeuralLayer> mLayers = new List<NeuralLayer>();
    [SerializeField] float mMutationRate = 0.03f;

    // Start is called before the first frame update
    private void Start()
    {
        Initialize();
    }
    void Initialize()
    {
        for(int i = 0; i < LayerNumNnurons.Count; ++i)
        {
            mLayers.Add(new NeuralLayer());

            mLayers[i].SetOutputCount(LayerNumNnurons[i]);
            if (i == 0)
            {
                mLayers[i].SetInputCount(LayerNumNnurons[i]);
                mLayers[i].SetOutputCount(LayerNumNnurons[0]);
            }
            else
            {
                mLayers[i].SetInputCount(LayerNumNnurons[i-1]);
                mLayers[i].SetOutputCount(LayerNumNnurons[i]);
            }

            mLayers[i].Initialize();
        }

    }

    public void Randomize()
    {
        for(int i = 0; i < mLayers.Count; ++i)
        {
            mLayers[i].InitializeWeights();
        }
    }
    
    public List<float> FeedForward(ref List<float> inputs)
    {
        mLayers[0].FeedForward(ref inputs);

        for (int i = 1; i < mLayers.Count; ++i)
        {
            mLayers[i].FeedForward(ref mLayers[i - 1].mOutputs);
        }

        return mLayers[mLayers.Count - 1].mOutputs;
    }

    public void BackPropogation(ref List<float> expectedOutput)
    {
        mLayers[mLayers.Count - 1].BackPropOutput(ref expectedOutput);

        for(int i = mLayers.Count - 2; i >= 0; --i)
        {
            mLayers[i].BackPropHidden(ref mLayers[i + 1].mGamma, ref mLayers[i + 1].mWeights);
        }

        for(int i = 0; i < mLayers.Count; ++i)
        {
            mLayers[i].UpdateWeights();
        }
    }

    public DeepNeuralNetwork GeneticCrossover(ref DeepNeuralNetwork otherParent)
    {
        DeepNeuralNetwork child = new DeepNeuralNetwork();
        child.LayerNumNnurons = LayerNumNnurons;
        child.Initialize();

        if (Random.Range(0.0f, 1.0f) > mMutationRate)
        {
            for (int layer = 0; layer < mLayers.Count; ++layer)
            {
                for (int output = 0; output < mLayers[layer].mOutputCount; ++output)
                {
                    for (int input = 0; input < mLayers[layer].GetInputCount(); ++input)
                    {
                        if (Random.Range(0.0f, 1.0f) < 0.5)
                        {
                            child.mLayers[layer].SetWeight(mLayers[layer].GetWeight(output, input), output, input);
                        }
                        else
                        {
                            child.mLayers[layer].SetWeight(otherParent.mLayers[layer].GetWeight(output, input), output, input);
                        }
                    }
                }

            }
        }
        else
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) < mMutationRate)
            {
                child.Randomize();
            }
        }

        return child;
    }
}
