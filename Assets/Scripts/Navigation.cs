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
    private CharacterController controller;

    public bool MoveAcrossNavMeshesStarted { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = goal.position;
        MoveAcrossNavMeshesStarted = false;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // if(agent.isOnOffMeshLink && !MoveAcrossNavMeshesStarted)
        // {
        //     StartCoroutine(MoveAcrossNavMeshLink());
        //     MoveAcrossNavMeshesStarted=true;
        // }
    }



    // private IEnumerator MoveAcrossNavMeshLink()
    // {
    //     // OffMeshLinkData data = agent.currentOffMeshLinkData;
    //     // var link = (agent.navMeshOwner as NavMeshLink);
    //     // // agent.updateRotation = false;
    //     // agent.updatePosition = false;

    //     // controller.Move()
    //     // yield return null;
        
    //     // Vector3 startPos = agent.transform.position;
    //     // Vector3 endPos = link.endPoint + Vector3.up * agent.baseOffset;
    //     // float duration = (endPos-startPos).magnitude/agent.velocity.magnitude;
    //     // float t = 0.0f;
    //     // float tStep = 1.0f/duration;

    //     // while(t<1.0f)
    //     // {
    //     //     transform.position = Vector3.Lerp(startPos,endPos,t);
    //     //     agent.destination = transform.position;
    //     //     t+=tStep*Time.deltaTime;
    //     //     yield return null;
    //     // }

    //     // transform.position = endPos;
    //     // agent.updateRotation = true;
    //     // agent.CompleteOffMeshLink();
    //     // MoveAcrossNavMeshesStarted= false;
    // }
}
