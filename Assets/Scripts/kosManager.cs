using UnityEngine;

public class KosManager : MonoBehaviour
{
    public GameObject bramboryPrefab;     // Prefab pro brambory
    public GameObject jablkaPrefab;       // Prefab pro jablka

    public Transform spawnPointBrambory;  // Místo pro spawn brambor
    public Transform spawnPointJablka;    // Místo pro spawn jablek

    void Start()
    {
        SpawnujObjekt();
    }

    void SpawnujObjekt()
    {
        if (DataManager.vybranaPolozka == "Brambory" && spawnPointBrambory != null)
        {
            Debug.Log("Spawnuji BRAMBORY na pozici: " + spawnPointBrambory.position);
            Instantiate(bramboryPrefab, spawnPointBrambory.position, Quaternion.identity);
        }
        else if (DataManager.vybranaPolozka == "Jablka" && spawnPointJablka != null)
        {
            Debug.Log("Spawnuji JABLKA na pozici: " + spawnPointJablka.position);
            Instantiate(jablkaPrefab, spawnPointJablka.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Neznámá položka nebo chybí spawn point!");
        }
    }
}
