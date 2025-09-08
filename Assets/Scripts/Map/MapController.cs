using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public LayerMask terrainMask;
   // PlayerMovement movement;
    public GameObject currentChunk;
    Vector3 playerLastPosition;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxObjectDistance;
    float objectDistance;
    float optimizerCooldown;
    public float optimizerCooldownDuration;

    
    void Start()
    {
        //movement = FindFirstObjectByType<PlayerMovement>();
        playerLastPosition = player.transform.position;
    }

    
    void Update()
    {
        ChunkChecker();
        ChunkOptimizer();
    }

    void ChunkChecker() 
    {
        if (!currentChunk) 
        {
            return;
        }

        Vector3 moveDirection = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;
        string directionName = GetDirectionName(moveDirection);

        CheckAndSpawnChunk(directionName);

        if (directionName.Count() > 5)
        { HandleDiagonalChunkSpawn(directionName); }
        else { HandleStraightChunkSpawn(directionName); }
    }

    void CheckAndSpawnChunk(string directionName)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(directionName).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(directionName).position);
        }
    }

    void HandleStraightChunkSpawn(string directionName) 
    {
        if (directionName.Contains("Up"))
        {
            CheckAndSpawnChunk("Left");
            CheckAndSpawnChunk("Right");
        }
        else if (directionName.Contains("Down"))
        {
            CheckAndSpawnChunk("Left");
            CheckAndSpawnChunk("Right");
        }
        else if (directionName.Contains("Right"))
        {
            CheckAndSpawnChunk("Down");
            CheckAndSpawnChunk("Up");
        }
        else if (directionName.Contains("Left"))
        {
            CheckAndSpawnChunk("Down");
            CheckAndSpawnChunk("Up");
        }
    }

    void HandleDiagonalChunkSpawn(string directionName) 
    {
        if (directionName.Contains("Up"))
        {
            CheckAndSpawnChunk("Up"); 
        }
        if (directionName.Contains("Down"))
        {
            CheckAndSpawnChunk("Down");
        }
        if (directionName.Contains("Left"))
        {
            CheckAndSpawnChunk("Left");
        }
        if (directionName.Contains("Right"))
        {
            CheckAndSpawnChunk("Right");
        }
    }

    string GetDirectionName(Vector3 direction) 
    {
        direction = direction.normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.y > 0.5f)
            {
                return direction.x > 0 ? "UpRight" : "UpLeft";
            }
            else if (direction.y < -0.5f)
            {
                return direction.x > 0 ? "DownRight" : "DownLeft";
            }
            else 
            {
                return direction.x > 0 ? "Right" : "Left";
            }
        }
        else 
        {
            if (direction.x > 0.5f)
            {
                return direction.y > 0 ? "UpRight" : "DownRight";
            }
            else if (direction.x < -0.5f)
            {
                return direction.y > 0 ? "UpLeft" : "DownLeft";
            }
            else
            {
                return direction.y > 0 ? "Up" : "Down";
            }
        }
    }

    void SpawnChunk(Vector3 spawnPosition) 
    {

        int rand = UnityEngine.Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand],spawnPosition,Quaternion.identity);
        spawnedChunks.Add(latestChunk);

    }

    void ChunkOptimizer() 
    {
        optimizerCooldown -= Time.deltaTime;
        if (optimizerCooldown <= 0f) 
        {
            optimizerCooldown = optimizerCooldownDuration;
        }
        else 
        {
            return;
        }

            foreach (GameObject chunk in spawnedChunks)
            {
                objectDistance = Vector3.Distance(player.transform.position, chunk.transform.position);
                if (objectDistance > maxObjectDistance)
                {
                    chunk.SetActive(false);
                }
                else
                {
                    chunk.SetActive(true);
                }
            }
    }
}
