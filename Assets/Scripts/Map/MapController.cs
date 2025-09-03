using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    PlayerMovement movement;
    public GameObject currentChunk;

    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxObjectDistance;
    float objectDistance;
    float optimizerCooldown;
    public float optimizerCooldownDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = FindFirstObjectByType<PlayerMovement>();
    }

    // Update is called once per frame
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
        if(movement.moveDirection.x > 0 && movement.moveDirection.y == 0) //right
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Right").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Right").position;
                SpawnChunk();
            }
        }
        else if (movement.moveDirection.x < 0 && movement.moveDirection.y == 0) //left
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Left").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Left").position;
                SpawnChunk();
            }
        }
        else if (movement.moveDirection.x == 0 && movement.moveDirection.y > 0) //up
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Up").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Up").position;
                SpawnChunk();
            }
        }
        else if (movement.moveDirection.x == 0 && movement.moveDirection.y < 0) //dwon
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("Down").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("Down").position;
                SpawnChunk();
            }
        }
        else if (movement.moveDirection.x > 0 && movement.moveDirection.y > 0) //up right
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("UpRight").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("UpRight").position;
                SpawnChunk();
            }
        }
        else if (movement.moveDirection.x < 0 && movement.moveDirection.y > 0) //up left
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("UpLeft").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("UpLeft").position;
                SpawnChunk();
            }
        }
        else if (movement.moveDirection.x > 0 && movement.moveDirection.y < 0) //down right
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("DownRight").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("DownRight").position;
                SpawnChunk();
            }
        }
        else if (movement.moveDirection.x < 0 && movement.moveDirection.y < 0) //down left
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("DownLeft").position, checkerRadius, terrainMask))
            {
                noTerrainPosition = currentChunk.transform.Find("DownLeft").position;
                SpawnChunk();
            }
        }
    }

    void SpawnChunk() 
    {
        int rand = UnityEngine.Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand],noTerrainPosition,Quaternion.identity);
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
