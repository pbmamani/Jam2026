using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.AI;
using System;
using System.Linq;

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


namespace Pathfinding.BehaviouTree {

    public class HastaFallar : Nodo {
        public HastaFallar(string nombre) : base(nombre) { }

        public override Estado Proceso()
        {
            if (hijos[0].Proceso() == Estado.Fallo) {
                Reiniciar();
                return Estado.Fallo;
            }

            return Estado.Corriendo;
        }
    }

    public class Invertido : Nodo {
        public Invertido(string nombre) : base(nombre) { }
        public override Estado Proceso()
        {
            switch (hijos[0].Proceso()) {
                case Estado.Corriendo:
                    return Estado.Corriendo;
                case Estado.Fallo:
                    return Estado.Exito;
                    default:
                    return Estado.Fallo;
            }
            
        }
    }
    

    public class SeleccionarPrioridad : Selector {
        List<Nodo> prioridadHijos;
        List<Nodo> PrioridadHijos => prioridadHijos ??= OrdenarHijos();

        protected virtual List<Nodo> OrdenarHijos()=>hijos.OrderByDescending(hijo=>hijo.prioridad).ToList();

        public SeleccionarPrioridad(string nombre, int prioridad = 0) : base(nombre,prioridad) { }
        public override void Reiniciar()
        {
            base.Reiniciar();
            prioridadHijos = null;
        }

        public override Estado Proceso()
        {

            foreach (var hijo in PrioridadHijos) {
                switch (hijo.Proceso()) { 
                    case Estado.Corriendo:
                        return Estado.Corriendo;
                    case Estado.Exito:
                        return Estado.Exito;
                    default:
                        continue;
                }
            }
            return Estado.Fallo;
        }
    }

    public class RandomSelector : SeleccionarPrioridad
    {

        protected override List<Nodo> OrdenarHijos() => hijos.Shuffle().ToList();

        public RandomSelector(string nombre, int prioridad =0) : base(nombre, prioridad) { }

    }

    public class Selector : Nodo
    {



        public Selector(string nombre, int prioridad =0) : base(nombre,prioridad) { }

        public override Estado Proceso()
        {

            if (hijo_actual < hijos.Count)
            {
                switch (hijos[hijo_actual].Proceso())
                {
                    case Estado.Corriendo:
                        return Estado.Corriendo;
                    case Estado.Exito:
                        Reiniciar();
                        return Estado.Exito;
                    default:
                        hijo_actual++;
                        return Estado.Corriendo;

                }
            }

            Reiniciar();
            return Estado.Fallo;
        }
    }

    public class  Secuencia:Nodo
    {


        public Secuencia(string nombre, int prioridad=0) : base(nombre,prioridad) { }

        public override Estado Proceso()
        {

            if (hijo_actual < hijos.Count) {
                switch (hijos[hijo_actual].Proceso()) {
                    case Estado.Corriendo:
                        return Estado.Corriendo;
                    case Estado.Fallo:
                        Reiniciar();
                        return Estado.Fallo;
                    default:
                        hijo_actual++;
                        return hijo_actual == hijos.Count ? Estado.Exito : Estado.Corriendo;
                        
                }
            }
            Reiniciar();
            return Estado.Exito;
        }
    }

    public class Condicion : IEstrategia {
        readonly Func<bool> predicate;

        public Condicion(Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        public Nodo.Estado Proceso() => predicate() ? Nodo.Estado.Exito : Nodo.Estado.Fallo;

    }


    public class AccionEstrategia : IEstrategia {
        readonly Action HacerAlgo;

        public AccionEstrategia(Action hacerAlgo)
        {
            HacerAlgo = hacerAlgo;
        }

        public Nodo.Estado Proceso() {
            HacerAlgo();
            return Nodo.Estado.Exito;
        }
    }

    public interface IEstrategia{
        Nodo.Estado Proceso();
        void Reinicio() { 
            //X
        }
    }


    public class ArbolComportamientos : Nodo {
        public ArbolComportamientos(string nombre) : base(nombre) {}
        public override Estado Proceso()
        {
            while (hijo_actual < hijos.Count) { 
                var estado =   hijos[hijo_actual].Proceso();
                if (estado != Estado.Exito) {
                    return estado;
                }
                hijo_actual++;
            }
            return Estado.Exito;
        }
    }


    public class Hoja : Nodo
    {
        readonly IEstrategia estrategia;
        public Hoja( string nombre, IEstrategia estrategia, int prioridad=0):base(nombre,prioridad)
        {
            this.estrategia = estrategia;
        }

        public override Estado Proceso() => estrategia.Proceso();

        public override void Reiniciar() => estrategia.Reinicio();

    }
    //------------------------------------------
    public class Nodo {
      
        
        public enum Estado { Exito, Fallo, Corriendo}
        public readonly string nombre;
        public readonly int prioridad;

        public readonly List<Nodo> hijos = new();

        protected int hijo_actual;


        public Nodo(string nombre="Nodo", int prioridad =0) { 
               this.nombre = nombre;
            this.prioridad = prioridad;
        }

        public void AgregarHijo(Nodo hijo)=>hijos.Add(hijo);

        public virtual Estado Proceso() => hijos[hijo_actual].Proceso();

        public virtual void Reiniciar() {
            hijo_actual = 0;
            foreach (var hijo in hijos) {
                hijo.Reiniciar();
            }
        }

    }

    public class MoverATarget : IEstrategia {
        readonly Transform entidad;
        readonly NavMeshAgent agente;
        readonly Transform punto;
        //readonly float velocidadPatrulla;
       // int indiceActual;
        bool estaCalculando;

        public MoverATarget(Transform entidad, NavMeshAgent agente, Transform punto)
        {
            this.entidad = entidad;
            this.agente = agente;
            this.punto = punto;
           // this.velocidadPatrulla = velocidadPatrulla;
           
        }

        public Nodo.Estado Proceso()
        {
            if (Vector3.Distance(entidad.position, punto.position) < 1f) {
                return Nodo.Estado.Exito;
            }
           
            agente.SetDestination(punto.position);
           // entidad.LookAt(punto);

            /*  if (estaCalculando && agente.remainingDistance < 0.1f)
              {
                  indiceActual++;
                  estaCalculando = false;
              }

              if (agente.pathPending)
              {
                  estaCalculando = true;
              }*/

            if (agente.pathPending)
            {
                estaCalculando = true;
            }

            return Nodo.Estado.Corriendo;
        }

        public void Reinicio() => estaCalculando = false;


    }

    public class PatrullaEstrategia : IEstrategia {
        readonly Transform entidad;
        readonly NavMeshAgent agente;
        readonly List<Transform> puntos;
        readonly float velocidadPatrulla;
        int indiceActual;
        bool estaCalculando;

        public PatrullaEstrategia(Transform entidad, NavMeshAgent agente, List<Transform> puntos, float velocidadPatrulla = 2.0f)
        {
            this.entidad = entidad;
            this.agente = agente;
            this.puntos = puntos;
            this.velocidadPatrulla = velocidadPatrulla;
           
        }

        public Nodo.Estado Proceso() {
            if (indiceActual == puntos.Count) return Nodo.Estado.Exito;

            var target = puntos[indiceActual];
            agente.SetDestination(target.position);
            entidad.LookAt(target);

            if (estaCalculando && agente.remainingDistance < 0.1f) {
                indiceActual++;
                estaCalculando = false;
            }

            if (agente.pathPending) {
                estaCalculando = true;
            }

            return Nodo.Estado.Corriendo;
        }

        public void Reinicio()=>indiceActual = 0;
    }



}
