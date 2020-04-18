using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*    public class CarDep : MonoBehaviour
    {
        public PathFinding.Tracker tracker;
        const float maxSpeed = 50.0f;
        const float accelerateSpeed = 0.3f;
        const float turnSpeed = 100.0f;

        public float mTimer;
        public bool mLap;
        public int[] layer;                                              // = { 4, 3, 3, 4 };

        PathFinding.CarController carController;
        public DeepNeuralNetwork network;
        public float currentSpeed = 10.0f;
        public float lastSpeed = 10.0f;
        const float minValue = 0.89f;                                   // minimum value that the output should have in order to be accepted
        //int highest;
        public Vector3 spawnPos = new Vector3(98.26f, -0.07999992f, -57.11f);
        //incase any value doesent reach the minimum requirement
        public float Fitness;           // { get; private set; }

        public bool isAlive;

        public void Initialize(int[] layer, System.Random rand)
        {
            this.layer = layer;
            network = new DeepNeuralNetwork();
            //highest = -1;
            currentSpeed = 10.0f;
            carController = new PathFinding.CarController();
            Fitness = 0;
            isAlive = true;

            mTimer = 0;
            mLap = false;
        }

        // Use this for initialization
        void Start()
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.tag == "tracker")
                {
                    tracker = GetComponent<PathFinding.Tracker>();
                }
            }
            
            //highest = -1;
            currentSpeed = 25.0f;
            carController = new PathFinding.CarController();
            Fitness = 0;
            isAlive = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (isAlive)
            {
                if (!mLap)
                {
                    mTimer += Time.deltaTime;
                }
            //Action action;
            //action = carController.Update();

            //if (tracker.GetDistance(Direction.Left) <=0.0f) {
            //    return;
            //}

            //return;
            //Get Values From the Tracker
            List<float> input = new List<float>();
            input.Add(tracker.GetDistance(PathFinding.Direction.Left));
            input.Add(tracker.GetDistance(PathFinding.Direction.Right));
            input.Add(tracker.GetDistance(PathFinding.Direction.Forward));
            input.Add(currentSpeed); //left, right, front, speed
                var output = network.FeedForward(ref input);

                /* output meaning
                output[0] turn right
                output[1] turn left
                output[2] Aselleration
                output[3] Deseleration
                 

                PathFinding.Action turn = PathFinding.Action.Default;
                PathFinding.Action speed = PathFinding.Action.Default;

                if (output[0] > output[1])
                {
                    turn = PathFinding.Action.TurnRight;
                }
                else
                {
                    turn = PathFinding.Action.TurnLeft;
                }
                if (output[2] > output[3])
                {
                    speed = PathFinding.Action.Accelerate;
                }
                else
                {
                    speed = PathFinding.Action.decelerate;
                }

                //Inputs by AI
                if (turn == PathFinding.Action.TurnLeft)
                {
                    transform.Rotate(Vector3.down, turnSpeed * Time.deltaTime);
                }
                else if (turn == PathFinding.Action.TurnRight)
                {
                    transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
                }
                if (speed == PathFinding.Action.Accelerate && currentSpeed < maxSpeed)
                {
                    currentSpeed += accelerateSpeed;
                }
                else if (speed == PathFinding.Action.decelerate && currentSpeed > 25.0f)
                {
                    currentSpeed -= accelerateSpeed;
                }

                transform.position += transform.forward * currentSpeed * Time.deltaTime;

                //Inputs by Human
                //if (action == Action.TurnLeft)
                //{
                //    transform.Rotate(Vector3.down, turnSpeed * Time.deltaTime);
                //}
                //else if (action == Action.TurnRight)
                //{
                //    transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
                //}
                //if (action == Action.Accelerate && currentSpeed < maxSpeed)
                //{
                //    currentSpeed += accelerateSpeed;
                //}
                //else if (action == Action.decelerate && currentSpeed > -maxSpeed)
                //{
                //    currentSpeed -= accelerateSpeed;
                //}
            }
            

        }

        public void SetColor(Color color)
        {
            GetComponentInChildren<MeshRenderer>().material.color = color;
        }

        public void SendToSpawn()
        {
            transform.position = new Vector3(98.26f, -0.07999992f, -57.11f);
        }

        public void Destroy()
        {
            
        }

        public float GetTime()
        {
            return mTimer;
        }


        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Wall")
            {
                isAlive = false;
            }
            if (other.tag == "Score")
            {
                int score = 10;
                Fitness += score;
            }
            if (other.tag == "Finish")
            {
                if(lastSpeed == currentSpeed)
                {
                    isAlive = false;
                }
                else
                {
                    Fitness += 100;
                    lastSpeed = currentSpeed;
                }
            }
        }
    }
*/