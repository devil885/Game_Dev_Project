//using System.ComponentModel.Design;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    MapController mapControl;
    public GameObject targetChunk;
    
    void Start()
    {
        mapControl = FindFirstObjectByType<MapController>();
    }

    private void OnTriggerStay2D(Collider2D collider) 
    {
        if (collider.CompareTag("Player")) 
        {
            mapControl.currentChunk = targetChunk;
        }
    } 

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) 
        {
            if (mapControl.currentChunk == targetChunk) 
            {
                mapControl.currentChunk = null;
            }
        }

    }
}
