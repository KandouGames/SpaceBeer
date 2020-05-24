using System;
using UnityEngine;


public interface Inputs
{
    float GetInputX();
    float GetInputY();
}

public class PhoneInputs : MonoBehaviour, Inputs
{
    public Joystick joystick;

    public void setJoystick(Joystick j)
    {
        this.joystick = j;
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
}
