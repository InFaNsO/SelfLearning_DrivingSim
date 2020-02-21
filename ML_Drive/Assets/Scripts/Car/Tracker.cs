using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace PathFinding
{

    public enum Direction
    {
        Forward,
        Right,
        Left
    }

    public class Tracker : MonoBehaviour {

       // public Car car;
        public Vector3 hitForwardDirection;
        public Vector3 hitRightDirection;
        public Vector3 hitLeftDirection;

        public Vector3 detectedHitForwardPosition;
        public Vector3 detectedHitRightPosition;
        public Vector3 detectedHitLeftPosition;
        RaycastHit rayHitForward;
        RaycastHit rayHitRight;
        RaycastHit rayHitLeft;
        int count = 1;


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Vector3 yOffset = new Vector3(0, 0.2f, 0);
            //Gizmos.DrawLine(transform.position+ yOffset, transform.position + yOffset + transform.TransformDirection(hitForwardDirection * 5.0f) );


            Gizmos.DrawLine(transform.position, detectedHitForwardPosition);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, detectedHitRightPosition);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, detectedHitLeftPosition);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(detectedHitForwardPosition, 1.0f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(detectedHitRightPosition, 1.0f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(detectedHitLeftPosition, 1.0f);

            //Gizmos.color = Color.yellow;
            //Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(hitRightDirection * 5.0f));
            //Gizmos.color = Color.blue;
            //Gizmos.DrawLine(transform.position, transform.position + transform.TransformDirection(hitLeftDirection * 5.0f));



        }
        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

            int layer_mask = 1 << 8;
            
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(hitForwardDirection), out rayHitForward, 100, layer_mask))
            {
                detectedHitForwardPosition = rayHitForward.point;
            }
            if (Physics.Raycast(transform.position, transform.TransformDirection(hitRightDirection), out rayHitRight, 100, layer_mask))
            {
                detectedHitRightPosition = rayHitRight.point;
            }
            if (Physics.Raycast(transform.position, transform.TransformDirection(hitLeftDirection), out rayHitLeft, 100, layer_mask))
            {
                detectedHitLeftPosition = rayHitLeft.point;
            }

            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                string data = count + ":     Left: ";
                data += GetDistance(Direction.Left);
                data += "     Forwards: " + GetDistance(Direction.Forward);
                data += "     Right: " + GetDistance(Direction.Right);
              //  data += "     Speed: " + car.currentSpeed;
                File.AppendAllText("D:\\Bhavilg\\Inputs.txt", data);
                count++;
            }

            
        }

        public float GetDistance(Direction dir)
        {
            if(dir == Direction.Forward)
            {
                return rayHitForward.distance;
            }
            else if(dir == Direction.Right)
            {
                return rayHitRight.distance;
            }
            else
            {
                return rayHitLeft.distance;
            }
        }
    }
}