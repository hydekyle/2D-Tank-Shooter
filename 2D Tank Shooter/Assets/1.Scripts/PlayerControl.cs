using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class PlayerControl : MonoBehaviour {
    
    public static PlayerControl Instance { get; set; }

    float x;
    float factorSensibility;
    public float delta;
    public GameObject disparoJugador;
    public int fuerzaDisparo;
    public float velocidadDisparo;
    public float delayDisparo;
    public float inclinacionMAX = 60.0f;

    EZObjectPool disparosPool;
    Transform[] puntosDisparo = null;
    int cantidadPuntosDisparo;
    float timeLastShot = 0.0f;
    
    void Awake()
    {
        Instance = this;
        Inicializar();
    }

    void OnEnable()
    {
        factorSensibility = 0.1f + PlayerPrefs.GetFloat("Sensibility");

    }

    void FixedUpdate()
    {
        InputControl();
    }

    void Inicializar()
    {
        //Object Poll de los disparos iniciales
        disparosPool = EZObjectPool.CreateObjectPool(disparoJugador, "Disparos Jugador", 10, true, true, false);
        //Inicializar puntos de disparo
        Transform transformPadre = transform.Find("PuntosDisparo").transform;
        cantidadPuntosDisparo = transformPadre.childCount;
        puntosDisparo = new Transform[cantidadPuntosDisparo];
        for (var x = 0; x < cantidadPuntosDisparo; x++)
        {
            puntosDisparo[x] = transformPadre.GetChild(x);
        }
    }

    void InputControl()
    {
        if (Input.GetKey(KeyCode.Mouse0)) {
            ApuntarMouse();
            Disparar();
        }
    }

    void Disparar()
    {
        if(Time.time > timeLastShot)
        {
            GameObject disparoTEMP;
            disparosPool.TryGetNextObject(ObtenerPuntoDisparo(), transform.rotation, out disparoTEMP);
            disparoTEMP.GetComponent<Rigidbody2D>().velocity = transform.up * velocidadDisparo;
            timeLastShot = Time.time + delayDisparo;
        }
    }

    Vector3 ObtenerPuntoDisparo()
    {
        return puntosDisparo[Random.Range(0, cantidadPuntosDisparo)].transform.position;
    }

    void ApuntarMouse()
    {
        Vector3 touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetMover = new Vector3(touch.x,transform.position.y,transform.position.z);
        float distance = Vector3.Distance(targetMover, transform.position) * 2;
        Mathf.Clamp(distance, 2.0f, 8.0f);
        transform.position = Vector3.MoveTowards(transform.position, targetMover, Time.deltaTime * distance);
    }


    void Update()
    {
        delta = Input.acceleration.x;
        x = (Mathf.Round(delta * 100) / 100f) * (1000 * factorSensibility);
        x = Mathf.Clamp(x, -inclinacionMAX, inclinacionMAX);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, x), Time.deltaTime * 5);
    }
}
