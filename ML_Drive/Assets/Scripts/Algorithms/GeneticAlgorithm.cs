using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using System.Threading;

public class GeneticAlgorithm
{
    public List<DNA> mPopulation { get; private set; }

    public int MGeneration { get; private set; }
    public float mMutation;

    public float mFitnessSum;
    public DNA MBest { get; private set; }
    public DNA MSecondBest { get; private set; }
    public PathFinding.Car[] MBestGenes { get; private set; }
    public int populationSize;

    private System.Random random;

    public GeneticAlgorithm(int PopulationSize, int DNASize, System.Random rand, Func <PathFinding.Car> GetRandomGene, Func <int, float> FitnessFunc, float mutationRate)
    {
        MGeneration = 1;
        mMutation = mutationRate;
        mPopulation = new List<DNA>();
        populationSize = PopulationSize;
        random = rand;

        for (int i = 0; i < PopulationSize; ++i)
        {
            mPopulation.Add(new DNA(random, GetRandomGene, mMutation, true ));
        }
    }

    public void ResetAllPositions()
    {
        for(int index = 0; index < mPopulation.Count; ++index)
        {

            mPopulation[index].mGene.SendToSpawn();
        }
    }

    public void NewGeneration(Func<PathFinding.Car> GetRandomGene, Func<bool> DeleteAllDeadCars)
    {
        List<DNA> carsCopy = new List<DNA>(mPopulation);
        mPopulation.Clear();

        CalculateFitness(carsCopy);

        for (int i = 0; i < populationSize; ++i)
        {
            DNA Parent1 = null;
            DNA Parent2 = null;
            DNA child = null;
            if (i == 0)
            {
                Parent1 = MBest;
                Parent2 = MBest;
                child = Parent1.CrossOver(Parent2, GetRandomGene);
                child.mGene.SetColor(Color.green);
            }
            else
            {   
                if (i < populationSize * 0.5)
                {
                    Parent1 = MBest;
                    Parent2 = MSecondBest;
                    child = Parent1.CrossOver(Parent2, GetRandomGene);
                    child.mGene.SetColor(Color.yellow);
                }
                else
                {
                    Parent1 = MBest;
                    Parent2 = ChooseParent(Parent1, carsCopy);
                    child = Parent1.CrossOver(Parent2, GetRandomGene);
                    child.mGene.SetColor(Color.red);
                }
            }
            mPopulation.Add(child);
        }
        DeleteAllDeadCars();
        ++MGeneration;
    }

    public void CalculateFitness(List<DNA> cars)
    {
        mFitnessSum = 1;

        DNA best = cars[0];
        DNA secondBest = cars[0];

        for (int i = 0; i < cars.Count; ++i)
        {
            mFitnessSum += cars[i].mGene.Fitness;

            if (cars[i].mGene.Fitness > best.mGene.Fitness && cars[i].mGene.isAlive)
            {
                best = cars[i];
            }
        }
        for (int i = 0; i < cars.Count; ++i)
        {
            mFitnessSum += cars[i].mGene.Fitness;

            if (cars[i].mGene.Fitness > best.mGene.Fitness && cars[i] != best)
            {
                best = cars[i];
            }
        }

        MBest = best;
        MSecondBest = secondBest;
       // MBestGenes = best.mGene;
    }

    public bool BestFitness()
    {
        for (int i = 0; i < mPopulation.Count; ++i)
        {
            if(mPopulation[i].mGene.mLap)
            {
                return true;
            }
        }

        return false;
    }
    public DNA ChooseParent(DNA parent, List<DNA> cars)
    {
        double randomNum = random.NextDouble() * mFitnessSum;

        for (int i = 0; i < cars.Count; ++i)
        {
            if (randomNum < cars[i].CalculateFitness(i))
            {
                return cars[i];
            }
        }

        int value = random.Next(cars.Count);
        while (cars[value] == parent)
        {
            value = random.Next(cars.Count);
        }

        return cars[value]; 
    }
}

