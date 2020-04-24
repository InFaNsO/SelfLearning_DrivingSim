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

    float MaxGenTime = 45.0f;
    float nextGenTime = 0;

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
        nextGenTime = Time.time + MaxGenTime;
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
            nextGenTime = Time.time + MaxGenTime;
            IncrementGenerationTrig = false;
        }

        for(int i = 0; i < mPopulation.Count; i++)
        {
            if(!mPopulation[i].IsAlive)
            {
                myPopulation[i].SetActive(false);
            }

            if (mPopulation[i].mFitness > mPopulation[mBest].mFitness)
            {
                mSecondBest = mBest;
                mBest = i;
            }
        }

        if(nextGenTime < Time.time)
        {
            IncrementGeneration();
            nextGenTime = Time.time + MaxGenTime;
            return;
        }

        //Check if all are dead
        for(int i = 0; i < mPopulation.Count; ++i)
        {
            if (mPopulation[i].IsAlive)
                return;
        }

        IncrementGeneration();
        nextGenTime = Time.time + MaxGenTime;

    }

    public void UIIncrement()
    {
        if (mBest < mPopulation.Count)
            IncrementGeneration();
        else
            Debug.Log("Data Wrong");
        nextGenTime = Time.time + MaxGenTime;

    }

    void IncrementGeneration()
    {
        List<GameObject> nextGen = new List<GameObject>();

        for (int i = 0; i < PopulationSize + 1; ++i)
        {
            GameObject parent1 = null;
            GameObject parent2 = null;
            GameObject child = null;
            string name = "No Parent";
            if (i == 0)
            {
                child = myPopulation[mBest];
                child.name = "BestCar";
                parent1 = myPopulation[mBest];
                parent2 = myPopulation[mBest];
                //child.SetColor(Color.green);
            }
            else if(i == 1)
            {
                child = Instantiate(myPopulation[mBest]);
                child.name = "BustRunning";
            }
            else
            {
                if (i < mPopulation.Count * 0.5)
                {
                    parent1 = myPopulation[mBest];
                    parent2 = myPopulation[mSecondBest];
                    name = "Secondbest " + i.ToString();
                    //child.mGene.SetColor(Color.yellow);
                }
                else
                {
                    parent1 = myPopulation[mBest];
                    parent2 = myPopulation[GetRandomParentIndex(mBest)];
                    name = "Random " + i.ToString();
                    //child.mGene.SetColor(Color.red);
                }
            }
            if (i > 1)
            {
                parent1.GetComponent<DeepNeuralNetwork>().GeneticCrossover(ref parent2, out child);
                child.name = name;
                
            }
            nextGen.Add(child);
        }

        float bestScore = mPopulation[mBest].mFitness;

        mPopulation.Clear();
        for (int i = 0; i < myPopulation.Count; ++i)
        {
            if(i != mBest)
                Destroy(myPopulation[i]);
        }
        myPopulation.Clear();

        for(int i = 0; i < nextGen.Count; ++i)
        {
            myPopulation.Add(nextGen[i]);
            SetCar(i, false);
            mPopulation[i].MyReset();
            if (i == 0)
            {
                mPopulation[i].mFitness = bestScore;
                mPopulation[i].IsAlive = false;
            }
            myPopulation[i].transform.position = SpawnPoint.position;
            myPopulation[i].transform.rotation = SpawnPoint.rotation;
        }

        mBest = 0;
        mSecondBest = 0;

        Generation++;
    }

    void SetCar(int index, bool setDNN = true)
    {
        myPopulation[index].GetComponent<Car>().enabled = true;
        myPopulation[index].GetComponent<CarSensor>().enabled = true;
        myPopulation[index].GetComponent<DNA>().enabled = true;
        mPopulation.Add(myPopulation[index].GetComponent<DNA>());
        if(setDNN)
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
