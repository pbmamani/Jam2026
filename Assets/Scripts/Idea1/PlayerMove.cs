using Pathfinding.BehaviourTrees;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
//using Pathfinding.BehaviouTree;
//using UnityEditor.ShaderKeywordFilter;

public class PlayerMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    NavMeshAgent agent;
    public List<Transform> targets;
    Transform currentTarget;

    public GameObject Panadero;
    public GameObject Panadero2;
    //ArbolComportamientos Arbol;

    [SerializeField] GameObject LugarSeguro;
    [SerializeField] bool inDanger;

    BehaviourTree tree;

    void Awake()
    {
       // Arbol = new ArbolComportamientos("Jugador");
        agent = GetComponent<NavMeshAgent>();
        // currentTarget = targets[0];
        /* Hoja estaPanadero = new Hoja("Esta El Panadero", new Condicion(() => Panadero.activeSelf));
         Hoja irPanadero = new Hoja("Ir Panadero", new AccionEstrategia(() => agent.SetDestination(Panadero.transform.position)));

         Secuencia IrAlPanadero = new Secuencia("Debemos ir al Panadero",20);
         IrAlPanadero.AgregarHijo(estaPanadero);
         IrAlPanadero.AgregarHijo(irPanadero);

         Secuencia IrAlPanadero2 = new Secuencia("Debemos ir al Panadero 2",30);
         IrAlPanadero2.AgregarHijo(new Hoja("Esta El Panadero 2", new Condicion(() => Panadero2.activeSelf)));
         IrAlPanadero2.AgregarHijo(new Hoja("Ir Panadero 2", new AccionEstrategia(() => agent.SetDestination(Panadero2.transform.position))));


         RandomSelector IrApanaderos = new RandomSelector("Ir a todos los panaderos Aleatorios");
         IrApanaderos.AgregarHijo(IrAlPanadero2);
         IrApanaderos.AgregarHijo(IrAlPanadero);
        */


        //-----------------------
        /*SeleccionarPrioridad Acciones = new SeleccionarPrioridad("Logica de agente");
        Secuencia correrSeguridad = new Secuencia("Ir a la zona segunra", 100);

        bool EsSeguro() {
            if (!inDanger) {
                correrSeguridad.Reiniciar();
                return false;
            }
            return true;
        }

        correrSeguridad.AgregarHijo(new Hoja("Es Seguro?", new Condicion(EsSeguro)));
        correrSeguridad.AgregarHijo(new Hoja("Ir a la zona segura", new MoverATarget(transform, agent, LugarSeguro.transform)));
        Acciones.AgregarHijo(correrSeguridad);

        Selector goToTreasure = new RandomSelector("Ir al Tesoro",50);
        Secuencia ObtenerPrimerTesoro = new Secuencia("Ir primer Tesoro");
        ObtenerPrimerTesoro.AgregarHijo(new Hoja("Ir al tesoro", new Condicion(() => Panadero.activeSelf)));
        ObtenerPrimerTesoro.AgregarHijo(new Hoja("Ir a la zona segura", new MoverATarget(transform, agent, Panadero.transform)));
        ObtenerPrimerTesoro.AgregarHijo(new Hoja("Pick Up", new AccionEstrategia(()=>Panadero.SetActive(false))));
        goToTreasure.AgregarHijo(ObtenerPrimerTesoro);

        Secuencia ObtenerPrimerTesoro2 = new Secuencia("Ir primer Tesoro");
        ObtenerPrimerTesoro2.AgregarHijo(new Hoja("Ir al tesoro", new Condicion(() => Panadero2.activeSelf)));
        ObtenerPrimerTesoro2.AgregarHijo(new Hoja("Ir a la zona segura", new MoverATarget(transform, agent, Panadero2.transform)));
        ObtenerPrimerTesoro2.AgregarHijo(new Hoja("Pick Up", new AccionEstrategia(() => Panadero2.SetActive(false))));
        goToTreasure.AgregarHijo(ObtenerPrimerTesoro2);

        Acciones.AgregarHijo(goToTreasure);

        Hoja patrulla = new Hoja("Patrulla", new PatrullaEstrategia(transform, agent, targets));
        Acciones.AgregarHijo(patrulla);

        Arbol.AgregarHijo(Acciones);*/



    }

    private void Start()
    {
        tree = new BehaviourTree("Hero");

        PrioritySelector actions = new PrioritySelector("Agent Logic");

        Sequence runToSafetySeq = new Sequence("RunToSafety", 100);
        bool IsSafe()
        {
           
                if (!inDanger)
                {
                    runToSafetySeq.Reset();
                    return true;
                }
            

            return false;
        }
        runToSafetySeq.AddChild(new Leaf("isSafe?", new Condition(IsSafe)));
        runToSafetySeq.AddChild(new Leaf("Go To Safety", new MoveToTarget(transform, agent, LugarSeguro.transform)));
        actions.AddChild(runToSafetySeq);

        Selector goToTreasure = new RandomSelector("GoToTreasure", 50);
        Sequence getTreasure1 = new Sequence("GetTreasure1");
        getTreasure1.AddChild(new Leaf("isTreasure1?", new Condition(() => Panadero.activeSelf)));
        getTreasure1.AddChild(new Leaf("GoToTreasure1", new MoveToTarget(transform, agent, Panadero.transform)));
        getTreasure1.AddChild(new Leaf("PickUpTreasure1", new ActionStrategy(() => Panadero.SetActive(false))));
        goToTreasure.AddChild(getTreasure1);

        Sequence getTreasure2 = new Sequence("GetTreasure2");
        getTreasure2.AddChild(new Leaf("isTreasure2?", new Condition(() => Panadero2.activeSelf)));
        getTreasure2.AddChild(new Leaf("GoToTreasure2", new MoveToTarget(transform, agent, Panadero2.transform)));
        getTreasure2.AddChild(new Leaf("PickUpTreasure2", new ActionStrategy(() => Panadero2.SetActive(false))));
        goToTreasure.AddChild(getTreasure2);

        actions.AddChild(goToTreasure);

        Leaf patrol = new Leaf("Patrol", new PatrolStrategy(transform, agent, targets));
        actions.AddChild(patrol);

        tree.AddChild(actions);
    }

    public void ChangueDestination(int index) {
        currentTarget=targets[index];
    }

    // Update is called once per frame
    void Update()
    {
        tree.Process();
        //agent.SetDestination(currentTarget.position);
        //Arbol.Proceso();

    }
}
