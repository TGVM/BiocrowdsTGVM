
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Biocrowds.Core;

public class VisualAgent : MonoBehaviour 
{
    private Animator anim;
    [SerializeField]
    public Queue<float> moveMem;
    public Queue<Vector3> dirMem;
    public float[] qview;
    private Vector3 currPosition;
    private bool updated;
    private bool initialized;
    private Vector3 currMoveVect;
    private Vector3 prevMoveVect;
    [SerializeField]
    Vector3 avgDirSum = new Vector3();
    [SerializeField]
    Vector3 avgDir;
    [SerializeField]
    public List<Vector3> dirView;


	// Update is called once per frame
	public void Step() 
    {
        prevMoveVect = currMoveVect;
        currMoveVect = currPosition - transform.parent.position;
        currMoveVect.y = 0f;
        //Debug.Log(currMoveVect.x + " " + currMoveVect.z);
        moveMem.Dequeue();
        dirMem.Dequeue();
        //moveMem.Enqueue(currMoveVect);
        moveMem.Enqueue(currMoveVect.magnitude);
        dirMem.Enqueue(currMoveVect.normalized);
        float speedSum = 0;
        //float angleDifSum = 0;

        avgDirSum = new Vector3();
        var prevV = moveMem.Peek();
        foreach(float v in moveMem){
            speedSum += v;
            //angleDifSum += Vector3.SignedAngle(prevV, v,Vector3.back);
            prevV = v;
        }
        foreach (Vector3 d in dirMem)
        {
            avgDirSum += d;
        }
        float presentAvgSpeed = (speedSum  / moveMem.Count) ;
        float estFutureSpeed = currMoveVect.magnitude;
        float AvgSpeed = (presentAvgSpeed + estFutureSpeed) / 2;
        avgDir = avgDirSum / dirMem.Count;
        //float presentAvgAngleDif = angleDifSum / moveMem.Count;
        //float estFutureAngDif = Vector3.SignedAngle(prevV, currMoveVect, Vector3.back);
        //float avgAngleDif = (presentAvgAngleDif + estFutureAngDif) / 2;
        float totalAngleDiff = Vector3.SignedAngle(currMoveVect, prevMoveVect, Vector3.up);
        //Debug.Log(totalAngleDiff);
        float angFact = totalAngleDiff / 90f;
        //anim.SetFloat("AngSpeed", angFact * 0.5f);// Mathf.Clamp(angDif/6f,-1f,1f));


        //transform.Rotate(new Vector3(0, totalAngleDiff * 0.05f, 0), Space.World);
        //transform.rotation = Quaternion.Euler(0, Mathf.Atan2(speed.x,speed.z)*180f,0);
        
        Vector3 targetDirection = -avgDir.normalized;
        //transform.rotation = Quaternion.LookRotation(targetDirection);
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 36f);
        }
        //transform.LookAt(transform.position - currMoveVect, Vector3.up);
        anim.SetFloat("Speed", Mathf.Clamp(presentAvgSpeed*32f, 0f, 0.9f));
        //anim.SetFloat("AngSpeed", presentAvgAngleDif/3f);
        anim.SetFloat("Motion_Time", anim.GetFloat("Motion_Time") + (0.02f * presentAvgSpeed * 32f));
        //transform.position = currPosition;
        currPosition = transform.parent.position;
        qview = moveMem.ToArray();
        dirView = dirMem.ToList();
        updated = false;

    }

    public void Initialize(Vector3 pos, Agent p_agent)
    {
        //transform.Rotate(Vector3.right,-90) ;
        anim = GetComponent<Animator>();
        moveMem = new Queue<float>();
        dirMem = new Queue<Vector3>();
        currPosition = new Vector3(pos.x, pos.y, pos.z);
        transform.position = currPosition;
        transform.LookAt(p_agent.goalsList[0].transform.position);
        updated = false;
        for (int i = 0; i < 15; i++)
        {
            moveMem.Enqueue(0);
        }
        for (int i = 0; i < 10; i++)
        {
            dirMem.Enqueue((pos - p_agent.goalsList[0].transform.position).normalized);
        }
        dirView = dirMem.ToList();
        initialized = true;
    }



}

