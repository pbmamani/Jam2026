using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

namespace Pathfinding.BehaviourTrees {
    public interface IStrategy {
        Node.Status Process();

        void Reset() {
            // Noop
        }
    }

    public class ActionStrategy : IStrategy {
        readonly Action doSomething;
        
        public ActionStrategy(Action doSomething) {
            this.doSomething = doSomething;
        }
        
        public Node.Status Process() {
            doSomething();
            return Node.Status.Success;
        }
    }

    public class Condition : IStrategy {
        readonly Func<bool> predicate;
        
        public Condition(Func<bool> predicate) {
            this.predicate = predicate;
        }
        
        public Node.Status Process() => predicate() ? Node.Status.Success : Node.Status.Failure;
    }

    public class PatrolStrategy : IStrategy {
        readonly Transform entity;
        readonly NavMeshAgent agent;
        readonly List<Transform> patrolPoints;
        readonly float patrolSpeed;
        int currentIndex;
        bool isPathCalculated;

        public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f) {
            this.entity = entity;
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.patrolSpeed = patrolSpeed;
        }

        public Node.Status Process() {
            if (currentIndex == patrolPoints.Count) return Node.Status.Success;
            
            var target = patrolPoints[currentIndex];
            agent.SetDestination(target.position);
            //entity.LookAt(target.position.With(y:entity.position.y));
            
            if (isPathCalculated && agent.remainingDistance < 0.1f) {
                currentIndex++;
                isPathCalculated = false;
            }
            
            if (agent.pathPending) {
                isPathCalculated = true;
            }
            
            return Node.Status.Running;
        }
        
        public void Reset() => currentIndex = 0;
    }
    
    public class MoveToTarget : IStrategy {
        readonly Transform entity;
        readonly NavMeshAgent agent;
        readonly Transform target;
        bool isPathCalculated;

        public MoveToTarget(Transform entity, NavMeshAgent agent, Transform target) {
            this.entity = entity;
            this.agent = agent;
            this.target = target;
        }

        public Node.Status Process() {
            if (Vector3.Distance(entity.position, target.position) < 1f) {
                return Node.Status.Success;
            }
            
            agent.SetDestination(target.position);
            //entity.LookAt(target.position.With(y:entity.position.y));

            if (agent.pathPending) {
                isPathCalculated = true;
            }
            return Node.Status.Running;
        }

        public void Reset() => isPathCalculated = false;
    }

    public class DistanceCondition : IStrategy { 
        readonly Transform origin;
        readonly Transform target;
        readonly float maxDistance;
        readonly bool ignoreYAxis;
        public DistanceCondition(Transform origin, Transform target, float maxDistance, bool ignoreYAxis = false)
        {
            this.origin = origin;
            this.target = target;
            this.maxDistance = maxDistance;
            this.ignoreYAxis = ignoreYAxis;
        }

        public Node.Status Process() {
            if (origin == null || target == null)
                return Node.Status.Failure;

            Vector3 posA = origin.position;
            Vector3 posB = target.position;

            if (ignoreYAxis)
            {
                posA.y = 0f;
                posB.y = 0f;
            }

            // Vector3.Distance es equivalente a Vector3.sqrMagnitude,
            // pero comparar magnitudes al cuadrado evita calcular la raíz cuadrada (más eficiente).
            float sqrDistance = (posA - posB).sqrMagnitude;
            float sqrMaxDistance = maxDistance * maxDistance;

            return sqrDistance <= sqrMaxDistance ? Node.Status.Success : Node.Status.Failure;
        }
    }

    public class WaitStrategy : IStrategy
    {
        readonly float duration;
        float timePassed;
        bool isWaiting;

        /// <param name="duration">Tiempo de espera en segundos</param>
        public WaitStrategy(float duration)
        {
            this.duration = duration;
        }

        public Node.Status Process()
        {
            if (!isWaiting)
            {
                // Inicia el temporizador en la primera ejecución
                isWaiting = true;
                timePassed = 0f;
            }

            timePassed += Time.deltaTime;

            if (timePassed >= duration)
            {
                Reset(); // Reinicia el estado interno para futuras ejecuciones
                return Node.Status.Success;
            }

            return Node.Status.Running;
        }

        public void Reset()
        {
            isWaiting = false;
            timePassed = 0f;
        }
    }
}
