using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoJugador : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag(GameManager.Instance.tagEnemigos))
        {
            GameManager.Instance.ZombiDamage((int.Parse(col.gameObject.name))); //El id del zombi está en el nombre.
            gameObject.SetActive(false);
        }
        
    }
}
