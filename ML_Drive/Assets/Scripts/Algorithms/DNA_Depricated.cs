using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DNADepricated
{
    public PathFinding.Car mGene { get; private set; }
    public float mFitness { get; private set; }

    private System.Random random;
    //Func<PathFinding.Car> GetRandomGene;

    public float mMutationRate;

    public DNA(System.Random rand, Func<PathFinding.Car> GetRandomGene,  float mutationRate, bool shouldInitialize = true)
    {
        this.random = rand;

        //this.GetRandomGene = GetRandomGene;
        
        mMutationRate = mutationRate;

        mGene = GetRandomGene();
    }

    public float CalculateFitness(int index)
    {
        float score;
        mFitness = mGene.Fitness;

        score = mFitness / 260;
       // float fitness = FitnessFunc(index);
        return score;
    }

    public DNA CrossOver(DNA otherParent, Func<PathFinding.Car> GetRandomGene)
    {
        if (random.NextDouble() > mMutationRate)
        {
            DNA child = new DNA(random, GetRandomGene, mMutationRate, true);

            //for (int i = 0; i < mgene.length; ++i)
            //{
            //    child.mgene[i] = random.nextdouble() < 0.5 ? mgene[i] : otherparent.mgene[i];
            //}



            //if (typeof(T) == typeof(PathFinding.Car))
            //{

            //}

            //child.mGene = MutateGene(mGene, GetRandomGene);
            child.mGene.isAlive = true;
            return child;
        }
        else
        {
            DNA child = new DNA(random, GetRandomGene, mMutationRate, true);
            //child.mGene = MutateGene(mGene, GetRandomGene);
            child.mGene.isAlive = true; 
            return child;
        }
    }

    public PathFinding.Car MutateGene(PathFinding.Car car, Func<PathFinding.Car> GetRandomGene)
    {
     if (random.NextDouble() < mMutationRate)
     {
            PathFinding.Car returnCar = GetRandomGene();
            returnCar.GetComponentInChildren<MeshRenderer>().material.color = Color.cyan;
            return returnCar;
     }
     else
        {
            PathFinding.Car carCopy = GetRandomGene();
            carCopy.network = car.network;
            carCopy.currentSpeed = car.currentSpeed;
            return carCopy; 
        }
    }
}




