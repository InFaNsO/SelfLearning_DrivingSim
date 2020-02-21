using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class NeuralNetwork
{

    public int[] layer;
    public Layer[] layers;

    public NeuralNetwork (int [] layer, System.Random rand)
    {
        this.layer = new int[layer.Length];
        for (int i = 0; i < layer.Length; ++i)
        {
            this.layer[i] = layer[i];
        }

        layers = new Layer[layer.Length - 1];

        for (int i = 0; i < layer.Length - 1; ++i)
        {
            layers[i] = new Layer(layer[i], layer[i + 1], rand);          //Initialize all the layers in the neural network
        }
    }

    public float[] FeedForward(float [] inputs)
    {

       // Debug.Log("================= Start forward layer = "  + layer.Length);

        layers[0].FeedForward(inputs);

        for (int i = 1; i < layer.Length - 1; ++i)
        {
        //    Debug.Log("                    i=" + i);

            layers[i].FeedForward(layers[i - 1].outputs);
        }

        //Debug.Log("================= end forward");
        return layers[layers.Length - 1].outputs;
    }

    //Not Gonna use this
    public void BackProp(float [] expected)
    {
        for (int i = layers.Length-1; i >= 0; --i)
        {
            if (i == layers.Length - 1)
            {
                layers[i].BackPropOutput(expected);
            }
            else
            {
                layers[i].BackPropHidden(layers[i + 1].gamma, layers[i + 1].weights);
            }
        }

        for (int i = 0; i < layers.Length; ++i)
        {
            layers[i].UpdateWeights();
        }
    }

    public class Layer
    {
        public int numberOfInput;           //# of neurons in the previous layer
        public int numberOfOutput;          //# of neurons in the current layer

        const float learningRate = 0.0033f;         //At this rate the weights will be updted

        public float[] outputs;
        public float [] inputs;
        public float[,] weights { get; private set; }
        public float [,] weightsDelta;
        public float [] gamma;
        public float [] error;

        System.Random random;// = new System.Random();

        public Layer (int numberOfInput, int numberOfOutputs, System.Random rand)                   //Initialize all the variables in a layer
        {
            this.numberOfInput = numberOfInput;
            this.numberOfOutput = numberOfOutputs;

            outputs = new float [numberOfOutput];
            inputs = new float [numberOfInput];

            weights = new float [numberOfOutput, numberOfInput];
            weightsDelta = new float[numberOfOutput, numberOfInput];

            gamma = new float [numberOfOutput];
            error = new float[numberOfOutput];

            random = rand;

            InitializeWeights();
        }

        public void InitializeWeights()
        {
            for (int i = 0; i < numberOfOutput; ++i)
            {
                for (int j = 0; j < numberOfInput; ++j)
                {
                    float rand = (float)random.NextDouble();
                    if (random.NextDouble() > 0.5)
                    {
                        rand *= -1;
                    }

                    weights[i, j] = rand;
                }
            }
        }

        public float GetWeight(int output, int input)
        {
            return weights[output, input];
        }

        public void SetWeight(float value, int i, int j)
        {
            weights[i, j] = value;
        }

        public float[] FeedForward (float [] input)
        {
            this.inputs = input;

            for (int i = 0; i < numberOfOutput; ++i)
            {
                outputs[i] = 0;
                for (int j = 0; j < numberOfInput; ++j)
                {
                    outputs[i] += inputs[j] * weights[i, j];                     //computs the total value of the output using input and weights
                }

                outputs[i] = (float)Math.Tanh(outputs[i]);                      //sets the value between 0 and 1
            }
            return outputs;
        }

        public float TanHDer(float value)
        {
            return 1 - (value * value);
        }

        public void BackPropOutput(float [] expected)
        {
            for (int i = 0; i < numberOfOutput; ++i)
            {
                error[i] = outputs[i] - expected[i];                        //Error has MSE of output layer
            }

            for (int i = 0; i < numberOfOutput; ++i)
            {
                gamma[i] = error[i] * TanHDer(outputs[i]);                  //Finds the gamma value for the output layer 
            }

            for (int i = 0; i < numberOfOutput; ++i)
            {
                for (int j = 0; j < numberOfInput; ++j)
                {
                    weightsDelta[i, j] = gamma[i] * inputs[j];              //Updates the weights delta value for output layer
                }
            }
        }

        public void BackPropHidden(float [] gammaForward, float [,] weightsForward)
        {
            for (int i = 0; i < numberOfOutput; ++i)
            {
                gamma[i] = 0; 
                for (int j = 0; j < gammaForward.Length; ++j)
                {
                    gamma[i] += gammaForward[j] * weightsForward[j, i];                 //finds the gamma value for a hidden layer
                }
                gamma[i] *= TanHDer(outputs[i]);
            }
            for (int i = 0; i < numberOfOutput; ++i)
            {
                for (int j = 0; j < numberOfInput; ++j)
                {
                    weightsDelta[i, j] = gamma[i] * inputs[j];                          //Updates the weights delta value for hidden layer
                }
            }
        }

        public void UpdateWeights()
        {
            for (int i = 0; i < numberOfOutput; ++i)
            {
                for (int j =0; j < numberOfInput; ++i)
                {
                    weights[i, j] -= weightsDelta[i, j] * learningRate;
                }
            }
        }

    }
}
