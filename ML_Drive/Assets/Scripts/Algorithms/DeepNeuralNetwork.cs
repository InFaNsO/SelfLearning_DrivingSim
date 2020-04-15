using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepNeuralNetwork : MonoBehaviour
{
    [SerializeField] int mNumberOfInputs = 4;
    [SerializeField] int mNumberOfOutputs = 3;
    [SerializeField] public List<NeuralLayer> mLayers = new List<NeuralLayer>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < mLayers.Count; ++i)
        {
            if (i == 0)
            {
                mLayers[i].SetInputCount(mNumberOfInputs);
                mLayers[i].Initialize();
                continue;
            }

            mLayers[i].SetInputCount(mLayers[i - 1].mOutputCount);

            if (i +1 == mLayers.Count)
            {
                mLayers[i].mOutputCount = mNumberOfOutputs;
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

        for (int i = 1; i < mLayers.Count - 1; ++i)
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

}
