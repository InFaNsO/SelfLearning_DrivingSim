using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepNeuralNetwork : MonoBehaviour
{
    int mNumberOfInputs = 4;
    int mNumberOfOutputs = 3;

    [SerializeField] UIDNN NNConfig;
    
    [SerializeField] List<int> LayerNumNnurons = new List<int>();
    public List<NeuralLayer> mLayers = new List<NeuralLayer>();
    [SerializeField] float mMutationRate = 0.03f;

    // Start is called before the first frame update
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        mLayers.Clear();
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

    public void Copy(DeepNeuralNetwork other)
    {
        for (int i = 0; i < mLayers.Count; ++i)
        {
            mLayers[i].Copy(other.mLayers[i]);
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

    public void GeneticCrossover(ref GameObject otherParent, out GameObject childGO)
    {
        childGO = Instantiate(otherParent);
        var child = childGO.GetComponent<DeepNeuralNetwork>();
        child.LayerNumNnurons = LayerNumNnurons;
        child.Initialize();

        if (Random.Range(0.0f, 1.0f) > mMutationRate || this == otherParent)
        {
            var otherDNN = otherParent.GetComponent<DeepNeuralNetwork>();

            for (int layerIndex = 0; layerIndex < mLayers.Count; ++layerIndex)
            {
                for (int i = 0; i < mLayers[layerIndex].mWeights.Count; ++i)
                {
                    for (int j = 0; j < mLayers[layerIndex].mWeights[i].Count; ++j)
                    {
                        if (Random.Range(0.0f, 1.0f) < 0.5)
                        {
                            child.mLayers[layerIndex].mWeights[i][j] = mLayers[layerIndex].mWeights[i][j];
                        }
                        else
                        {
                            child.mLayers[layerIndex].mWeights[i][j] = otherDNN.mLayers[layerIndex].mWeights[i][j];
                        }
                    }
                }

            }
        }
        else
        {
            child.Randomize();
        }
    }

    public void OnSetButton()
    {
        LayerNumNnurons.Clear(); 

        for(int i = 0; i < NNConfig.myLayers.Count; ++i)
        {
            LayerNumNnurons.Add(NNConfig.myLayers[i].GetComponent<UIDNNLayer>().myNeurons.Count);
        }

        Initialize();
    }
}
