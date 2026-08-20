using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using TMPro;

public class MasterManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static MasterManager Instance;
    [SerializeField] TMP_Text txt;
    [SerializeField] TMP_Text txt2;

    [SerializeField] List<PlayableDirector> playableDirectors = new List<PlayableDirector>();


    string nombre_destino;
    string nombre_encuentro;

    private void Awake()
    {
        Instance=this;
    }

    public void EjecutarEncuentro(string nombre) {
       
            nombre_encuentro = nombre;
       
        
    }

    public void MostrarDataEncuentro() {
        if (nombre_encuentro != "")
        {
            txt2.text = nombre_encuentro;
        }
        else {
            txt2.text = "Sin Encuentros";
        }
            
    }

    public void EjecutarFinal(string nombre) {
        Debug.Log("Cambiando Camara");
        Camera.main.transform.position = new Vector3(0f,8.5f,-1.8f);
        Camera.main.transform.rotation = Quaternion.identity;
        nombre_destino = nombre;
        playableDirectors[0].Play();
    }

    public void MostrarDataEnd() {
        Debug.Log("Llegue al Destino:" + nombre_destino);
        txt.text = nombre_destino;
    }
}
