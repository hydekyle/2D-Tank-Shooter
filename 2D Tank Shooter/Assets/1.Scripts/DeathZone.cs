using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    public string _tag;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(_tag))
        {
            GameManager.Instance.DisableZombi(int.Parse(col.name));
        }
    }
}
