using UnityEngine;

public class KosManager : MonoBehaviour
{
    public GameObject bramboryPrefab; // Prefab pro brambory
    public GameObject jablkaPrefab; // Prefab pro jablka

    public float posunNahoru = .5f; // Nastavitelný posun nahoru
    public float rozptyl = 0.2f; // Nastavitelný rozptyl pro spawn pozice

    public int pocetObjektu = 5; // Počet objektů ke spawnutí

    void Start()
    {
        SpawnujObjekty();
    }

    void SpawnujObjekty()
    {
        for (int i = 0; i < pocetObjektu; i++)
        {
            Vector3 nahodnyPosun = new Vector3(Random.Range(-rozptyl, rozptyl), 0, Random.Range(-rozptyl, rozptyl));
            Vector3 spawnPozice = transform.position + Vector3.up * posunNahoru + nahodnyPosun;

            if (DataManager.vybranaPolozka == "Brambory")
            {
                Instantiate(bramboryPrefab, spawnPozice, Quaternion.identity, transform);
            }
            else if (DataManager.vybranaPolozka == "Jablka")
            {
                Instantiate(jablkaPrefab, spawnPozice, Quaternion.identity, transform);
            }
            else
            {
                Debug.LogWarning("Neznámá vybraná položka: " + DataManager.vybranaPolozka);
            }
        }
    }
}