using System;
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
        if (joystick.Horizontal >= 0.4f)
            x = joystick.Horizontal;
        else if (joystick.Horizontal <= -0.4f)
            x = joystick.Horizontal;
        else
            x = 0;

        return x;
    }

    public float GetInputY()
    {
        float y;
        if (joystick.Vertical >= 0.4f)
            y = joystick.Vertical;
        else if (joystick.Vertical <= -0.4f)
            y = joystick.Vertical;
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
