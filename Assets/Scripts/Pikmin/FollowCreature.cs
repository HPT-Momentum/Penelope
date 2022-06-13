using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowCreature : MonoBehaviour
{
    public Transform player;
    public UnityEngine.AI.NavMeshAgent myAgent;

    public int range;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(this.transform.position, player.position);
        Debug.Log(player.position);
        if(dist < range){
            myAgent.destination = player.position;
        }
        
    }
}
