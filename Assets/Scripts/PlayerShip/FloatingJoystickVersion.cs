using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystickVersion : Joystick
{
    private Dictionary<int, float> touches;
    private float shootingDeltaTime = 0.1f;

    private int idJoystickTouch;

    [HideInInspector]
    public PlayerShipHandler playerShipHandler;

    protected override void Start()
    {
        touches = new Dictionary<int, float>();

        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        // Saving time each touch to know how long is pressed each one
        int nTouches = Input.touchCount;
        for (int i = 0; i < nTouches; ++i)
        {
            int idTouch = Input.GetTouch(i).fingerId;
            if (!touches.ContainsKey(idTouch))
            {
                touches.Add(idTouch, Time.time);
            }
            
        }

        // First touch always is joystick
        if(nTouches == 1)
        {
            idJoystickTouch = Input.GetTouch(0).fingerId;
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
        }
        
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        // Detecting how long touch has been pressed 
        int nTouches = Input.touchCount;
        for (int i = 0; i < nTouches; ++i)
        {
            int idTouch = Input.GetTouch(i).fingerId;
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                // Releasing joystick
                if(idTouch == idJoystickTouch)
                {
                    background.gameObject.SetActive(false);
                    base.OnPointerUp(eventData);
                }

                // Shooting
                float deltaTimeTouch = Time.time - touches[idTouch];
                if (deltaTimeTouch < shootingDeltaTime)
                    playerShipHandler.Shoot();

                touches.Remove(idTouch);
            }
                
        }
    }
}
