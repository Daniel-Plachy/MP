using UnityEngine;
using UnityEngine.AI;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public Transform[] laneRightWaypoints, laneLeftWaypoints;
    public float minInterval = 1f, maxInterval = 3f;
    public int maxCars = 5;

    float timer, nextSpawn;
    int activeCars;

    void Start() => ScheduleNext();

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextSpawn && activeCars < maxCars)
        {
            timer = 0f;
            SpawnCar();
            ScheduleNext();
        }
    }

    void ScheduleNext() => nextSpawn = Random.Range(minInterval, maxInterval);

    void SpawnCar()
    {
        if (carPrefabs.Length == 0) return;
        var prefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

        // urč model a výšku
        int num = 0;
        if (prefab.name.StartsWith("CAR_") && int.TryParse(prefab.name.Substring(4,2), out var n))
            num = n;
        float y = (num>=1 && num<=3) ? 1f : 1.135f;

        // vyber pruh
        bool right = Random.value < .5f;
        var wps = right ? laneRightWaypoints : laneLeftWaypoints;
        if (wps.Length<2) return;

        // XZ z navmeshe
        Vector3 posXZ = wps[0].position;
        if (NavMesh.SamplePosition(posXZ, out var hit, 1f, NavMesh.AllAreas))
            posXZ = hit.position;

        // spawn root
        var car = Instantiate(prefab, posXZ, Quaternion.identity);

        // agent
        var agent = car.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.Warp(posXZ);

        // rotace +90/–90
        car.transform.rotation = Quaternion.Euler(0, right ? 90f : -90f, 0);

        // přidej height-maintainer a nastav mu výšku
        var hm = car.AddComponent<CarHeightMaintainer>();
        hm.desiredY = y;

        // cíl
        Vector3 endXZ = wps[1].position;
        if (NavMesh.SamplePosition(endXZ, out hit, 1f, NavMesh.AllAreas))
            endXZ = hit.position;
        agent.SetDestination(endXZ);

        // despawn
        activeCars++;
        var dsp = car.AddComponent<CarDespawner>();
        dsp.onDespawn = () => activeCars--;
    }
}
