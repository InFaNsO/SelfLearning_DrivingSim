using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using System.Xml.Serialization;
using System.IO;

public class Main : MonoBehaviour
{
    //The Genetic Algorithm
    //Number of instances
    //The thing size instance has -- 1 (probably)
    //System.Random rand
    //Mutation Rate

    public UnityEngine.SceneManagement.Scene scene1;
    public UnityEngine.SceneManagement.Scene scene2;
    public GameObject carPreFab;
    public GameObject spawnPos;
    int populationSize = 50;
    int DnaSize = 1;
    System.Random random = new System.Random();
    float mutationRate = 0.4f;
    public Text generationText;

   // PathFinding.Car[] activeCars;

    public int[] layer = { 4, 30, 35, 40, 45,50, 55, 60, 55, 50, 45, 40, 35, 30, 4};

    GeneticAlgorithm Sample;
    

    bool isDead = false;
    // Use this for initialization
    void Start ()
    {
        /* activeCars = new PathFinding.Car[populationSize];

         for( int i =0; i < activeCars.Length;i++)
         {
             PathFinding.Car car = Instantiate(carPreFab, spawnPos.transform.position, Quaternion.identity).GetComponent<PathFinding.Car>();
             activeCars[i] = car;
         }
         */
        Application.runInBackground = true;
        Sample = new GeneticAlgorithm(populationSize, DnaSize, random, RandomeGene, FitnessCalculator, mutationRate);



        //Sample.mPopulation[0].mGene[0].t
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Additive);
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Track2", LoadSceneMode.Additive);
        }

        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        int counter = 0;
        foreach (var car in cars)
        {
            if (!car.GetComponent<PathFinding.Car>().isAlive)
            {
                counter++;
            }
        }

        if (counter >= cars.Length)
        {
            Sample.NewGeneration(RandomeGene, DeleteAllDeadCarsInScene);
            DeleteWhiteCars();
        }

        generationText.text = "Generation: " + Sample.MGeneration;

        if(Input.GetKey(KeyCode.K))
        {
            KillAllCars();
        }
    }

    private void KillAllCars()
    {
        foreach(var car in Sample.mPopulation)
        {
            car.mGene.isAlive = false;
        }
    }
    private bool DeleteAllDeadCarsInScene()
    {
       GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
       
       foreach(var car in cars)
       {
            if (car.GetComponent<PathFinding.Car>().isAlive == false)
            {
                Destroy(car);
            }
       }
        return true;
    }

    private void DeleteWhiteCars()
    {
        GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");

        foreach (var car in cars)
        {
            if (car.GetComponentInChildren<MeshRenderer>().material.color == Color.white)
            {
                Destroy(car);
            }
        }
    }

    //Function for getting random gene (NeuralNetwork)
    //Function for fitness Rate
    private PathFinding.Car RandomeGene()
    {
        PathFinding.Car car = Instantiate(carPreFab, spawnPos.transform.position, Quaternion.identity).GetComponent<PathFinding.Car>();
        car.Initialize(layer, random);

        return car;
    }
    private float FitnessCalculator(int index)
    {
       // DNA current = cars[index];
        float score = 0;

        //score = current.mFitness / 260;                                 //score a car will have it it takes 2 laps

        return score;
    }
    
}
