﻿using System;
using UnityEngine;


public interface Inputs
{
    float GetInputX();
    float GetInputY();
    void SetJoystick(FloatingJoystickVersion joystick, PlayerShipHandler playerShipHandler);
}

public class PhoneInputs : MonoBehaviour, Inputs
{
    public FloatingJoystickVersion joystick;

    public void SetJoystick(FloatingJoystickVersion j, PlayerShipHandler playerShipHandler)
    {
        this.joystick = j;
        this.joystick.playerShipHandler = playerShipHandler;
    }

    public float GetInputX()
    {
        float x;
        if (joystick.Horizontal >= 0.3f)
            x = 1;
        else if (joystick.Horizontal <= -0.3f)
            x = -1;
        else
            x = 0;

        print(Input.acceleration.x);

        return x;
    }

    public float GetInputY()
    {
        float y;
        if (joystick.Vertical >= 0.3f)
            y = 1;
        else if (joystick.Vertical <= -0.3f)
            y = -1;
        else
            y = 0;

        return y;
    }
}

public class AccelerometerInput : MonoBehaviour, Inputs
{

    public void SetJoystick(FloatingJoystickVersion j, PlayerShipHandler playerShipHandler)
    {

    }

    public float GetInputX()
    {
        float x;
        if (Input.acceleration.x >= 0.15f)
            x = 1;
        else if (Input.acceleration.x <= -0.15f)
            x = -1;
        else
            x = 0;

        return x;
    }

    public float GetInputY()
    {
        float y;
        if (Input.acceleration.z >= -0.35f)
            y = 1;
        else if (Input.acceleration.z <= -0.75f)
            y = -1;
        else
            y = 0;

        return y;
    }
}

public class StandaloneInput : MonoBehaviour, Inputs
{
    public float GetInputX()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public float GetInputY()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public void SetJoystick(FloatingJoystickVersion joystick, PlayerShipHandler playerShipHandler)
    {
        
    }
}
