using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class canvasManager : MonoBehaviour {

    Canvas canvas;
    Scrollbar sensibility_bar;
    Text sensibility_txt;
    float playerSensibility;
    Transform menu_opciones;
    Transform menu_playing;

    void OnEnable()
    {
        canvas = canvas ?? SetCanvasItems();
    }

    Canvas SetCanvasItems()
    {
        menu_opciones = transform.Find("Opciones");
        menu_playing = transform.Find("OpcionesPlaying");
        sensibility_bar = menu_opciones.transform.Find("SensibilityBAR").GetComponent<Scrollbar>();
        sensibility_txt = sensibility_bar.transform.Find("SensibilityValueTxt").GetComponent<Text>();
        if (PlayerPrefs.HasKey("Sensibility")) 
        {
            float sensibilidadGuardada = PlayerPrefs.GetFloat("Sensibility"); //Recuperar sensibilidad guardada
            sensibility_bar.value = sensibilidadGuardada;
            sensibility_txt.text = sensibilidadGuardada.ToString();
        }else
        {
            sensibility_bar.value = 0.2f;
            sensibility_txt.text = sensibility_bar.value.ToString();
            PlayerPrefs.SetFloat("Sensibility", 0.2f);
        }
        sensibility_bar.onValueChanged.AddListener(delegate { UpdateSensibilityValues(); });
        return GetComponent<Canvas>();
    }

    void UpdateSensibilityValues()
    {
        playerSensibility = sensibility_bar.value;
        playerSensibility = Mathf.RoundToInt(playerSensibility * 10) / 10f; //Solo tomar un valor decimal
        sensibility_txt.text = playerSensibility.ToString();
        PlayerPrefs.SetFloat("Sensibility", playerSensibility);
    }

    public void Play()
    {
        menu_opciones.gameObject.SetActive(false);
        menu_playing.gameObject.SetActive(true);
        GameManager.Instance.StartGame();
    }

    public void VolverMenu()
    {
        menu_playing.gameObject.SetActive(false);
        menu_opciones.gameObject.SetActive(true);
        GameManager.Instance.StopGame();
    }


}
