using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class EjemploEvent : MonoBehaviour {

    public static EjemploEvent Instance;
    public delegate void Print(string s);
    public static event Print onSpaceKeyPressed;

    void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onSpaceKeyPressed != null)
        {
            onSpaceKeyPressed("Vaya, vaya");
        }
    }
}
