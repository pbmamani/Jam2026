using Pathfinding.BehaviourTrees;
using UnityEngine;
using UnityEngine.AI;

public class PlayerBehaviour : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Target;
    public GameObject Entidad;

    NavMeshAgent agent;

    BehaviourTree tree;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        tree = new BehaviourTree("Player");
        PrioritySelector actions = new PrioritySelector("Logica Player");

        Sequence IrEntidad = new Sequence("BuscarEntidad", 100);

        bool IsActive()
        {

            return Entidad.activeSelf;
        }

        IrEntidad.AddChild(new Leaf("Esta Activo?", new Condition(IsActive)));
        IrEntidad.AddChild(new Leaf("Esta Cerca", new DistanceCondition(transform, Entidad.transform, 5.0f)));
        IrEntidad.AddChild(new Leaf("Go To Safety", new MoveToTarget(transform, agent, Entidad.transform)));
        IrEntidad.AddChild(new Leaf("WaitForSeconds", new WaitStrategy(2.0f)));
        IrEntidad.AddChild(new Leaf("Desactivando la entidad", new ActionStrategy(() => Entidad.SetActive(false))));
        actions.AddChild(IrEntidad);

        Sequence goToExit = new Sequence("Buscando Salida 1");
        goToExit.AddChild(new Leaf("Puedo ir a la Salida?", new Condition(() => Target.activeSelf)));
        goToExit.AddChild(new Leaf("Ir a la Salida", new MoveToTarget(transform, agent, Target.transform)));
        goToExit.AddChild(new Leaf("Desactivando la Salida", new ActionStrategy(() => Target.SetActive(false))));

        actions.AddChild(goToExit);

        tree.AddChild(actions);



    }

    // Update is called once per frame
    void Update()
    {
        tree.Process();
    }
}
