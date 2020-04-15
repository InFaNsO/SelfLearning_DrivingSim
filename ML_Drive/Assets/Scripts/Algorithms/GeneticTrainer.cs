using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticTrainer : MonoBehaviour
{
    public Transform SpawnPoint;

    public int Generation = 0;
    public float mutationRate = 0.3f;

    float FitnessSum = 0;

    List<DNA> mPopulation = new List<DNA>();
    List<Score> mPopulationScores = new List<Score>();

    int mBest = -1;
    int mSecondBest = -1;

    void Start()
    {
        var dnas = FindObjectsOfType<DNA>();

        for(int i = 0; i < dnas.Length; ++i)
        {
            mPopulation.Add(dnas[i]);
            mPopulationScores.Add(dnas[i].GetComponent<Score>());
        }

        mBest = 0;
        mSecondBest = 0;
    }


    void Update()
    {
        for(int i = 0; i < mPopulationScores.Count; ++i)
        {
            if(mPopulationScores[i].mScore > mPopulationScores[mBest].mScore)
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

    void IncrementGeneration()
    {
        List<DNA> nextGen = new List<DNA>();

        for (int i = 0; i < mPopulation.Count; ++i)
        {
            DNA parent1 = null;
            DNA parent2 = null;
            DNA child = null;

            if (i == 0)
            {
                parent1 = mPopulation[mBest];
                parent2 = mPopulation[mBest];
                child = parent1.CrossOver(parent2);
                //child.SetColor(Color.green);
            }
            else
            {
                if (i < mPopulation.Count * 0.5)
                {
                    parent1 = mPopulation[mBest];
                    parent2 = mPopulation[mSecondBest];
                    child = parent1.CrossOver(parent2);
                    //child.mGene.SetColor(Color.yellow);
                }
                else
                {
                    parent1 = mPopulation[mBest];
                    parent2 = mPopulation[GetRandomParentIndex(mBest)];
                    child = parent1.CrossOver(parent2);
                    //child.mGene.SetColor(Color.red);
                }
            }
            nextGen.Add(child);
        }

        for(int i = 0; i < nextGen.Count; ++i)
        {
            mPopulation[i] = nextGen[i];
        }
    }

    void ResetAllToSpawn()
    {

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

}
