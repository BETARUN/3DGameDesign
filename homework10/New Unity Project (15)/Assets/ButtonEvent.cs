using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Vuforia;

public class ButtonEvent : MonoBehaviour, IVirtualButtonEventHandler
{
    public VirtualButtonBehaviour[] vbs;
    public GameObject cube;
    public int index;

    void Start()
    {
        vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
        for (int i = 0; i < vbs.Length; i++)
        {
            vbs[i].RegisterEventHandler(this);
        }
        index = 0;
        cube = GameObject.Find("ImageTarget/Cube");
    }

    void Update()
    {

    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        index++;
        if (index == 4)
            index = 0;
        switch (index)
        {
            case 0:
                cube.GetComponent<Renderer>().material.color = Color.white;
                break;
            case 1:
                cube.GetComponent<Renderer>().material.color = Color.red;
                break;
            case 2:
                cube.GetComponent<Renderer>().material.color = Color.yellow;
                break;
            case 3:
                cube.GetComponent<Renderer>().material.color = Color.green;
                break;
            case 4:
                cube.GetComponent<Renderer>().material.color = Color.blue;
                break;
        }

    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {

    }

}

