using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GeneticTrainer : MonoBehaviour
{

    public Transform SpawnPoint;
    [SerializeField] int PopulationSize = 10;
    [SerializeField] GameObject GeneType = null;

    [SerializeField] UIDNN NNConfig;

    [SerializeField] TMP_Text genText;
    string genTextString = "Generation - ";

    public int Generation = 0;
    public float mutationRate = 0.3f;

    float FitnessSum = 0;

    List<DNA> mPopulation = new List<DNA>();
    List<GameObject> myPopulation = null;

    int mBest = -1;
    int mSecondBest = -1;

    public bool IncrementGenerationTrig = false;

    void Start()
    {
        if(myPopulation != null)
        {
            for(int i = 0; i < myPopulation.Count; ++i)
                Destroy(myPopulation[i]);
            myPopulation.Clear();
        }

        myPopulation = new List<GameObject>();
        for (int i = 0; i < PopulationSize; ++i)
        {
            myPopulation.Add(Instantiate(GeneType, SpawnPoint.position, SpawnPoint.rotation));
            SetCar(i);
        }

        mBest = 0;
        mSecondBest = 0;
    }

    private void OnEnable()
    {
    }

    void Update()
    {
        genText.text = genTextString + Generation;

        if(IncrementGenerationTrig)
        {
            IncrementGeneration();
            IncrementGenerationTrig = false;
        }

        for(int i = 0; i < mPopulation.Count; i++)
        {
            if(mPopulation[i].mFitness > mPopulation[mBest].mFitness)
            {
                mSecondBest = mBest;
                mBest = i;
            }
        }

        //Check if all are dead
        for(int i = 0; i < mPopulation.Count; ++i)
        {
            if (mPopulation[i].IsAlive)
                return;
        }

        IncrementGeneration();
    }

    public void UIIncrement()
    {
        if (mBest < mPopulation.Count)
            IncrementGeneration();
        else
            Debug.Log("Data Wrong");
    }

    void IncrementGeneration()
    {
        List<DeepNeuralNetwork> nextGen = new List<DeepNeuralNetwork>();

        for (int i = 0; i < mPopulation.Count; ++i)
        {
            DeepNeuralNetwork parent1 = null;
            DeepNeuralNetwork parent2 = null;
            DeepNeuralNetwork child;

            if (i == 0)
            {
                parent1 = myPopulation[mBest].GetComponent<DeepNeuralNetwork>();
                parent2 = myPopulation[mBest].GetComponent<DeepNeuralNetwork>();
                //child.SetColor(Color.green);
            }
            else
            {
                if (i < mPopulation.Count * 0.5)
                {
                    parent1 = myPopulation[mBest].GetComponent<DeepNeuralNetwork>();
                    parent2 = myPopulation[mSecondBest].GetComponent<DeepNeuralNetwork>();
                    //child.mGene.SetColor(Color.yellow);
                }
                else
                {
                    parent1 = myPopulation[mBest].GetComponent<DeepNeuralNetwork>();
                    parent2 = myPopulation[GetRandomParentIndex(mBest)].GetComponent<DeepNeuralNetwork>();
                    //child.mGene.SetColor(Color.red);
                }
            }
            child = parent1.GeneticCrossover(ref parent2);
            nextGen.Add(child);
        }

        for(int i = 0; i < PopulationSize; ++i)
        {
            Destroy(myPopulation[i]);
        }
        myPopulation.Clear();
        mPopulation.Clear();

        for(int i = 0; i < PopulationSize; ++i)
        {
            myPopulation.Add(Instantiate(GeneType, SpawnPoint.position, SpawnPoint.rotation));
            SetCar(i);
            var dna = myPopulation[i].GetComponent<DeepNeuralNetwork>();
            dna = nextGen[i];
        }

        mBest = 0;
        mSecondBest = 0;

        Generation++;
    }

    void SetCar(int index)
    {
        myPopulation[index].GetComponent<Car>().enabled = true;
        myPopulation[index].GetComponent<CarSensor>().enabled = true;
        myPopulation[index].GetComponent<DNA>().enabled = true;
        mPopulation.Add(myPopulation[index].GetComponent<DNA>());
        myPopulation[index].GetComponent<DeepNeuralNetwork>().Initialize();
        myPopulation[index].SetActive(true);
    }

    int GetRandomParentIndex(int otherInd)
    {
        int index = otherInd;

        while(index == otherInd)
        {
            index = (int)Random.Range(0.0f, mPopulation.Count - 1.0f);
        }

        return index;
    }

    public void OnSetButton()
    {
        PopulationSize = NNConfig.PopulationSize;
    }

}
