using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

// https://docs.unity3d.com/Manual/nav-CouplingAnimationAndNavigation.html read this 
// Basically gonna have to manually traverse NavMeshLinks.

public class Navigation : MonoBehaviour
{

    public Transform goal;
    private NavMeshAgent agent;

    public bool MoveAcrossNavMeshesStarted { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
        MoveAcrossNavMeshesStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = goal.position;
    }
}
