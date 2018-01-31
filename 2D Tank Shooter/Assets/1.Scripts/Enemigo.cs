using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemigo {
    public int vida;

    public void Damage(int dmg) {
        vida -= dmg;
    }

    public void Disparar()
    {

    }


	
}
