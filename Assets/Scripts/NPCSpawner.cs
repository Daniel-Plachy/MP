using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcPrefabs;  
    public Transform[] spawnPointsRouteAB;
    public Transform[] spawnPointsRouteCD;
    public int pocetNPC_AB = 2;
    public int pocetNPC_CD = 2;
    
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public Transform pointD;
    void Start()
    {
        for (int i = 0; i < pocetNPC_AB; i++)
        {
            Transform spawnPoint = spawnPointsRouteAB[i % spawnPointsRouteAB.Length];
            GameObject selectedPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
            GameObject npc = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);           
            NPCWalker walker = npc.GetComponent<NPCWalker>();
            walker.routeOption = NPCWalker.RouteOption.RouteAB;
            walker.pointA = pointA;
            walker.pointB = pointB;
        }
        for (int i = 0; i < pocetNPC_CD; i++)
        {
            Transform spawnPoint = spawnPointsRouteCD[i % spawnPointsRouteCD.Length];
            GameObject selectedPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
            GameObject npc = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);          
            NPCWalker walker = npc.GetComponent<NPCWalker>();
            walker.routeOption = NPCWalker.RouteOption.RouteCD;
            walker.pointC = pointC;
            walker.pointD = pointD;
        }
    }
}
