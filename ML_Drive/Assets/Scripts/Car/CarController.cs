using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PathFinding
{
    public enum Action
    {
        Default = -1,
        TurnRight,
        TurnLeft,
        Accelerate,
        decelerate
    };
    public class CarController
    {

        public CarController()
        {

        }
        public Action Update()
        {
            if(Input.GetKey(KeyCode.A))
            {
                return Action.TurnLeft;
            }
            else if(Input.GetKey(KeyCode.D))
            {
                return Action.TurnRight;
            }
            else if(Input.GetKey(KeyCode.W))
            {
                return Action.Accelerate;
            }
            else if(Input.GetKey(KeyCode.S))
            {
                return Action.decelerate;
            }
            else
            {
                return Action.Default;
            }
        }
    }
}
