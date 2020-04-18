using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepNeuralNetwork : MonoBehaviour
{
    [SerializeField] int mNumberOfInputs = 4;
    [SerializeField] int mNumberOfOutputs = 3;
    [SerializeField] List<int> LayerNumNnurons = new List<int>();
    [HideInInspector] public List<NeuralLayer> mLayers = new List<NeuralLayer>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < LayerNumNnurons.Count; ++i)
        {
            mLayers.Add(new NeuralLayer());

            mLayers[i].SetOutputCount(LayerNumNnurons[i]);
            if (i == 0)
            {
                mLayers[i].SetInputCount(0);
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

}
