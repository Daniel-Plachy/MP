using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class QueueManager : MonoBehaviour
{
    [Header("Queue Points")]
    public Transform storepointA;    // První slot ve frontě
    public Transform storepointB;    // Druhý slot ve frontě
    public Transform storepointWait; // Kam se přesune obsloužený zákazník

    // Odkazy na NPC, které momentálně zabírají slot A a B
    private NPCWalker occupantA;
    private NPCWalker occupantB;

    /// <summary>
    /// Zavolá se, když se nějaký NPC chce zařadit do fronty.
    /// </summary>
    public void JoinQueue(NPCWalker npc)
    {
        // Pokud je volné A, pošleme ho tam
        if (occupantA == null)
        {
            occupantA = npc;
            npc.SetDestination(storepointA.position);
            Debug.Log(npc.name + " jde na slot A.");
        }
        // Jinak pokud je volné B, půjde tam
        else if (occupantB == null)
        {
            occupantB = npc;
            npc.SetDestination(storepointB.position);
            Debug.Log(npc.name + " jde na slot B.");
        }
        else
        {
            // Fronta je plná (2 sloty), můžeš vymyslet další logiku
            Debug.LogWarning("Fronta je plná! " + npc.name + " se nemůže zařadit.");
        }
    }

    /// <summary>
    /// Simuluje obsloužení zákazníka na slotu A.
    /// - Posune ho na storepointWait.
    /// - Přesune zákazníka z B do A, pokud tam někdo je.
    /// - Uvolní slot B.
    /// </summary>
    public void ServeCustomer()
    {
        if (occupantA != null)
        {
            // 1) Pošleme zákazníka z A na storepointWait
            occupantA.SetDestination(storepointWait.position);
            Debug.Log(occupantA.name + " byl obsloužen a jde do WAIT zóny.");
            occupantA = null;
        }

        // 2) Pokud někdo čeká v B, přesune se do A
        if (occupantB != null)
        {
            occupantA = occupantB;
            occupantB = null;

            occupantA.SetDestination(storepointA.position);
            Debug.Log(occupantA.name + " se posouvá z B na A.");
        }
    }
}
