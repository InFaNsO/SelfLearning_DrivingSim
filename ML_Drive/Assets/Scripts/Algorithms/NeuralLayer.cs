using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeuralLayer : MonoBehaviour
{

    [SerializeField] public int mOutputCount = 0;       //# of neurons in the current layer
    private int mInputCount = 0;                        //# of neurons in the previous layer
    
    [SerializeField] float mLearningRate = 0.0033f;     //At this rate the weights will be updted

    public List<float> mOutputs = new List<float>();
    public List<float> mInputs = new List<float>();

    public List<List<float>> mWeights;
    public List<List<float>> mWeightsDelta;
    public List<float> mGamma = new List<float>();
    public List<float> mError = new List<float>();

    public void SetInputCount(int count) { mInputCount = count; }
    public int GetInputCount() { return mInputCount; }

    public void Initialize()
    {
        mOutputs.Capacity = mOutputCount;
        mInputs.Capacity = mInputCount;

        mGamma.Capacity = mOutputCount;
        mError.Capacity = mOutputCount;

        InitializeWeights();
    }

    public void InitializeWeights()
    {
        mWeights = new List<List<float>>();
        mWeights.Capacity = mOutputCount;

        mWeightsDelta = new List<List<float>>();
        mWeightsDelta.Capacity = mOutputCount;

        for (int i = 0; i < mOutputCount; ++i)
        {
            mWeights[i] = new List<float>();
            mWeights[i].Capacity = mInputCount;

            mWeightsDelta[i] = new List<float>();
            mWeightsDelta[i].Capacity = mInputCount;

            for (int j = 0; j < mInputCount; ++j)
            {
                float rand = UnityEngine.Random.Range(0.0f, 1.0f);
                if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5)
                {
                    rand *= -1;
                }
                mWeights[i][j] = rand;
            }
        }
    }

    public float GetWeight(int output, int input)
    {
        return mWeights[output][input];
    }

    public void SetWeight(float value, int i, int j)
    {
        mWeights[i][j] = value;
    }


    public ref List<float> FeedForward(ref List<float> input)
    {
        mInputs = input;
        for (int i = 0; i < mOutputCount; ++i)
        {
            mOutputs[i] = 0;
            for (int j = 0; j < mInputCount; ++j)
            {
                mOutputs[i] += mInputs[j] * mWeights[i][j];                     //computs the total value of the output using input and weights
            }
            mOutputs[i] = (float)Math.Tanh(mOutputs[i]);                      //sets the value between 0 and 1
        }
        return ref mOutputs;
    }


    public float TanHDer(float value)
    {
        return 1 - (value * value);
    }

    public void BackPropOutput(ref List<float> expected)
    {
        for (int i = 0; i < mOutputCount; ++i)
        {
            mError[i] = mOutputs[i] - expected[i];                        //Error has MSE of output layer
        }
        for (int i = 0; i < mOutputCount; ++i)
        {
            mGamma[i] = mError[i] * TanHDer(mOutputs[i]);                  //Finds the gamma value for the output layer 
        }
        for (int i = 0; i < mOutputCount; ++i)
        {
            for (int j = 0; j < mInputCount; ++j)
            {
                mWeightsDelta[i][j] = mGamma[i] * mInputs[j];              //Updates the weights delta value for output layer
            }
        }
    }

    public void BackPropHidden(ref List<float> gammaForward, ref List<List<float>> weightsForward)
    {
        for (int i = 0; i < mOutputCount; ++i)
        {
            mGamma[i] = 0;
            for (int j = 0; j < gammaForward.Count; ++j)
            {
                mGamma[i] += gammaForward[j] * weightsForward[j][i];                 //finds the gamma value for a hidden layer
            }
            mGamma[i] *= TanHDer(mOutputs[i]);
        }

        for (int i = 0; i < mOutputCount; ++i)
        {
            for (int j = 0; j < mInputCount; ++j)
            {
                mWeightsDelta[i][j] = mGamma[i] * mInputs[j];                          //Updates the weights delta value for hidden layer
            }
        }
    }


    public void UpdateWeights()
    {
        for (int i = 0; i < mOutputCount; ++i)
        {
            for (int j = 0; j < mInputCount; ++i)
            {
                mWeights[i][j] -= mWeightsDelta[i][j] * mLearningRate;
            }
        }
    }

}
