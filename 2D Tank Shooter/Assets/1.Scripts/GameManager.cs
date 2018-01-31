using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    enum nivelRespawn { abajo, centro, arriba }
    public static GameManager Instance{ get; set; }
    public List<Zombi> zombiList = new List<Zombi>();
    GameObject player;
    GameObject Horda;
    public int vidaZombi = 3;
    public int fuerzaJugador = 1;
    public string tagEnemigos;
    public float timeRespawn;
    Transform[] respawn = null;
    int n = 0;
    Dictionary<int, Zombi> dicZombis = new Dictionary<int, Zombi>();
    public List<int> zombi_desactivado_ID = new List<int>(0);
    //Sprites Enemigos
    public Sprite zombie_sprite1;
    public bool game_is_active;
    void Awake()
    {
        Instance = this;
        player = player ?? transform.Find("Player").gameObject;
    }

    public void StartGame()
    {
        game_is_active = true;
        if (respawn == null)
        {
            InicializarArrays();
        }else
        {
            StartCoroutine(rutinaSpawn());
        }
        
        SetPlayerActive(true);
    }

    public void SetPlayerActive(bool estado)
    {
        player.SetActive(estado);
    }

    private void InicializarArrays()
    {
        //ARRAY RESPAWNS
        Transform respawnPadre = transform.Find("[Respawns]");
        int nRespawns = respawnPadre.childCount;
        respawn = new Transform[nRespawns];
        for (int x = 0; x < nRespawns; x++)
        {
            respawn[x] = respawnPadre.GetChild(x);
        }
        //Rutina Zombis
        Horda = new GameObject("Horda");
        StartCoroutine(rutinaSpawn());
    }
    
    public void StopGame()
    {
        game_is_active = false;
        Boom();
        SetPlayerActive(false);
    }

    IEnumerator rutinaSpawn()
    {
        while (game_is_active)
        {
            if(zombi_desactivado_ID.Count == 0)
            {
                GameObject newZombi = CrearZombi(n);
                n++;
            }else
            {
                ReciclarZombi(zombi_desactivado_ID[0]);
                zombi_desactivado_ID.RemoveAt(0);
            }
            yield return new WaitForSeconds(timeRespawn);
        }
    }

    GameObject CrearZombi(int id)
    {
        Zombi newInstance = new Zombi();
        dicZombis.Add(id, newInstance);
        zombiList.Add(newInstance);
        newInstance.vida = vidaZombi;
        newInstance.ID = id;
        newInstance.velocidadMovimiento = Random.Range(1.5f, 2.5f);
        GameObject newZombi = new GameObject(id.ToString());
        newInstance.myTransform = newZombi.transform;
        newZombi.transform.position = SpawnAleatorio();
        newZombi.transform.SetParent(Horda.transform);
        newZombi.transform.tag = "Enemigo";
        BoxCollider2D collider = newZombi.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        SpriteRenderer renderer = newZombi.AddComponent<SpriteRenderer>();
        newInstance.zombi_sprite = renderer.sprite = zombie_sprite1;
        return newZombi;
    }
    void ReciclarZombi(int id)
    {
        Zombi zombiT = dicZombis[id];
        zombiT.vida = vidaZombi;
        zombiT.myTransform.gameObject.transform.position = SpawnAleatorio();
        zombiT.myTransform.gameObject.SetActive(true);
        zombiList.Add(zombiT);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Boom();
        }
        foreach(Zombi zombie in zombiList)
        {
            zombie.OnUpdate();
        }
    }

    Vector3 SpawnAleatorio()
    {
        Vector3 posiFinal = Vector3.zero;
        switch (Random.Range(0, 3))
        {
            case 0: posiFinal = SelectRespawn(nivelRespawn.abajo); break;
            case 1: posiFinal = SelectRespawn(nivelRespawn.centro); break;
            case 2: posiFinal = SelectRespawn(nivelRespawn.arriba); break;
        }
        return posiFinal + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0);
    }
    Vector3 SelectRespawn(nivelRespawn posY) //Selecciona respawn aleatorio entre izquierda y derecha
    {
        if (posY == nivelRespawn.abajo) if (Random.Range(0, 2) == 0) return respawn[0].position; else return respawn[1].position;
        if (posY == nivelRespawn.centro) if (Random.Range(0, 2) == 0) return respawn[2].position; else return respawn[3].position;
        if (posY == nivelRespawn.arriba) if (Random.Range(0, 2) == 0) return respawn[4].position; else return respawn[5].position;
        return Vector3.zero;
    }

    public void ZombiDamage(int nZombi)
    {
        Zombi zombiBuscado = dicZombis[nZombi];
        if(zombiBuscado.vida - fuerzaJugador > 0)
        {
            zombiBuscado.Damage(fuerzaJugador);
        }else
        {
            DisableZombi(zombiBuscado);
        }
    }

    public void DisableZombi(Zombi z)
    {
        z.myTransform.gameObject.SetActive(false);
        zombiList.Remove(dicZombis[z.ID]);
        zombi_desactivado_ID.Add(z.ID);
    }

    public void DisableZombi(int nZombi)
    {
        Zombi z = dicZombis[nZombi];
        z.myTransform.gameObject.SetActive(false);
        zombiList.Remove(dicZombis[z.ID]);
        zombi_desactivado_ID.Add(z.ID);
    }

    public void Boom()
    {
        //Limpia todos los zombis activos
        List<Zombi> zombisGolpeados = new List<Zombi>();
        foreach(Zombi zombie in zombiList)
        {
            zombisGolpeados.Add(zombie);
            zombie.myTransform.gameObject.SetActive(false);
        }
        foreach(Zombi zombie in zombisGolpeados)
        {
            zombiList.Remove(zombie);
        }
        print("Zombis limpiados: " + zombisGolpeados.Count);
    }

}
