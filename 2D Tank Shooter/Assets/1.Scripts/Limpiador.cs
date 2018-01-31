using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limpiador : MonoBehaviour {

    public string _tag;

	void OnTriggerExit2D (Collider2D col)
    {
        if (col.CompareTag(_tag)) col.gameObject.SetActive(false);
    }
}
