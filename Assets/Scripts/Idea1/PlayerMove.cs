using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    NavMeshAgent agent;
    public List<Transform> targets;
    Transform currentTarget;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentTarget = targets[0];
    }
    void Start()
    {
        
    }
    public void ChangueDestination(int index) {
        currentTarget=targets[index];
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(currentTarget.position);


    }
}
