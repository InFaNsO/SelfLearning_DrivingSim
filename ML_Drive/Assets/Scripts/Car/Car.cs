using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    enum Action
    {
        Accelerate,
        Decelerate,
        TurnLeft,
        TurnRight
    }

    [SerializeField] float MaxSpeed = 25.0f;
    [SerializeField] float TurnSpeed = 5.0f;

    DeepNeuralNetwork myNeuralNet = null;
    DNA myDna = null;
    Rigidbody myRigidbody = null;
    CarSensor mySensor = null;

    float speed = 10.0f;

    List<float> inputForDNN = new List<float>();
    List<float> output;

    Action move;
    float moveVal = 0.0f;
    Action turn;
    float turnVal = 0.0f;

    void Start()
    {
        myNeuralNet = GetComponent<DeepNeuralNetwork>();
        mySensor = GetComponent<CarSensor>();
        myRigidbody = GetComponent<Rigidbody>();
        myDna = GetComponent<DNA>();

        for(int i = 0; i < 4; ++i)
        {
            inputForDNN.Add(10.0f);
        }
    }

    void Update()
    {
        inputForDNN[0] = myRigidbody.velocity.magnitude;
        inputForDNN[1] = mySensor.distanceFront;
        inputForDNN[2] = mySensor.distanceLeft;
        inputForDNN[3] = mySensor.distanceRight;

        output = myNeuralNet.FeedForward(ref inputForDNN);

        if (myDna.IsAlive)
        {
            move = output[0] > output[1] ? Action.Accelerate : Action.Decelerate;
            moveVal = output[0] > output[1] ? output[0] : -output[1];
            turn = output[2] > output[3] ? Action.TurnLeft : Action.TurnRight;
            turnVal = output[2] > output[3] ? output[2] : -output[3];

            myRigidbody.AddForce(transform.forward * moveVal * MaxSpeed + transform.forward * 5.0f);
            transform.Rotate(Vector3.up, TurnSpeed * turnVal);

            Vector3 vel = new Vector3();

            vel.x = myRigidbody.velocity.x > MaxSpeed ? MaxSpeed : myRigidbody.velocity.x;
            vel.y = myRigidbody.velocity.y != 0.0f ? 0.0f: 0.0f;
            vel.z = myRigidbody.velocity.z > MaxSpeed ? MaxSpeed : myRigidbody.velocity.z;

            myRigidbody.velocity = vel;
        }
    }
}
