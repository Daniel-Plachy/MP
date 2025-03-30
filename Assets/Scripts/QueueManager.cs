using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class QueueManager : MonoBehaviour
{
    [Header("Queue Points")]
    public Transform storepointA;
    public Transform storepointB;
    public Transform storepointWait;


    private NPCWalker occupantA;
    private NPCWalker occupantB;


    public void JoinQueue(NPCWalker npc)
    {

        if (occupantA == null)
        {
            occupantA = npc;
            npc.SetDestination(storepointA.position);
            Debug.Log(npc.name + " jde na slot A.");
        }

        else if (occupantB == null)
        {
            occupantB = npc;
            npc.SetDestination(storepointB.position);
            Debug.Log(npc.name + " jde na slot B.");
        }
        else
        {

            Debug.LogWarning("Fronta je plná! " + npc.name + " se nemůže zařadit.");
        }
    }


    public void ServeCustomer()
    {
        if (occupantA != null)
        {

            occupantA.SetDestination(storepointWait.position);
            Debug.Log(occupantA.name + " byl obsloužen a jde do WAIT zóny.");
            occupantA = null;
        }


        if (occupantB != null)
        {
            occupantA = occupantB;
            occupantB = null;

            occupantA.SetDestination(storepointA.position);
            Debug.Log(occupantA.name + " se posouvá z B na A.");
        }
    }
}
