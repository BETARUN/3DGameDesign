using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Vuforia;

public class ButtonEvent : MonoBehaviour, IVirtualButtonEventHandler
{
    public int count;
    public GameObject objecta;
    public VirtualButtonBehaviour[] vbutton;

    void Update()
    {
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
    }

    void Start()
    {
        count = 0;
        vbutton = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbutton.Length; i++)
        {
            vbutton[i].RegisterEventHandler(this);
        }
        objecta = GameObject.Find("ImageTarget/Cube");
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        count++;
        switch (count)
        {
            case 0:
                objecta.GetComponent<Renderer>().material.color = Color.white;
                break;
            case 1:
                objecta.GetComponent<Renderer>().material.color = Color.red;
                break;
            case 2:
                objecta.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case 3:
                objecta.GetComponent<Renderer>().material.color = Color.green;
                break;
        }

    }

}

