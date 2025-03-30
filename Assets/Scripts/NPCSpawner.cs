using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    // Pole pro různé NPC prefaby, do kterého přetáhneš všechny varianty
    public GameObject[] npcPrefabs;  

    // Spawn pointy pro trasu A–B
    public Transform[] spawnPointsRouteAB;
    // Spawn pointy pro trasu C–D
    public Transform[] spawnPointsRouteCD;
    
    // Počet NPC pro jednotlivé trasy
    public int pocetNPC_AB = 4;
    public int pocetNPC_CD = 2;
    
    // Waypointy pro trasu A–B
    public Transform pointA;
    public Transform pointB;
    
    // Waypointy pro trasu C–D
    public Transform pointC;
    public Transform pointD;
    
    void Start()
    {
        // Spawn NPC pro trasu A–B
        for (int i = 0; i < pocetNPC_AB; i++)
        {
            // Vybere spawn point z pole pro trasu A–B
            Transform spawnPoint = spawnPointsRouteAB[i % spawnPointsRouteAB.Length];
            // Vybere náhodně jeden prefab z pole npcPrefabs
            GameObject selectedPrefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
            GameObject npc = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
            
            // Předá waypointy a volbu trasy do NPCWalker
            NPCWalker walker = npc.GetComponent<NPCWalker>();
            walker.routeOption = NPCWalker.RouteOption.RouteAB;
            walker.pointA = pointA;
            walker.pointB = pointB;
        }
        
        // Spawn NPC pro trasu C–D
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
