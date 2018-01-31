using UnityEngine;
using System;
[Serializable]
public class Zombi : Enemigo{
    public int ID;
    public Sprite zombi_sprite;
    public Transform myTransform;
    public float velocidadMovimiento;
    Vector3 posiJugador = new Vector3(-1, -9, 0);
    Vector3 vectorDirector;
    public void OnUpdate()
    {
        vectorDirector = posiJugador - myTransform.position;
        vectorDirector.Normalize();
        if (zombi_sprite != null) {
            myTransform.position += vectorDirector * Time.deltaTime * velocidadMovimiento;
        } 
    }
}
