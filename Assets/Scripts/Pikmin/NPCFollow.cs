using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollow : MonoBehaviour
{
    public GameObject ThePlayer;
    public GameObject TheNPC;
    public float TargetDistance;
    public float AllowedDistance = 5;
    public float FollowSpeed;
    public RaycastHit Shot;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookAtCamera();


        
    }
    void Idle()
    {
        LookAtCamera();
        TheNPC.GetComponent<EnemyAi>().CancelGoNextDestination();
        ChangeStateTo(SlimeAnimationState.Idle);
    }
    public void ChangeStateTo(SlimeAnimationState state)
    {
       if (TheNPC == null) return;    
       if (state == TheNPC.GetComponent<EnemyAi>().currentState) return;

       TheNPC.GetComponent<EnemyAi>().currentState = state ;
    }
    void LookAtCamera()
    {
       TheNPC.transform.rotation = Quaternion.Euler(new Vector3(TheNPC.transform.rotation.x, ThePlayer.transform.rotation.y, TheNPC.transform.rotation.z));   
    }
}
