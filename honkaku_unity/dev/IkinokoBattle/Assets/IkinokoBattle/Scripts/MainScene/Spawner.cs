using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Start Coroutine
        StartCoroutine(SpawnLoop());
    }

    /// <summary>
    /// Coroutine of enemy generation
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Distance x = 10
            var distanceVector = new Vector3(10,0);
            // Position of enemy generated based on Player.
            var spawnPositionFromPlayer = Quaternion.Euler(0, Random.Range(0, 360f), 0) * distanceVector;
            // Enemy position
            var spawnPosition = playerStatus.transform.position + spawnPositionFromPlayer;
            // Search for coordinate of NavMesh which is the nearest from designated one
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(spawnPosition, out navMeshHit, 10, NavMesh.AllAreas))
            {
                // Copy enemyPrefab. NavMeshAgent is placed on NavMesh for sure.
                Instantiate(enemyPrefab, navMeshHit.position, Quaternion.identity);
            }

            // Wait for 10 seconds
            yield return new WaitForSeconds(10);
            // Break if Player is down.
            if (playerStatus.Life <= 0) break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
