using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency;
    public float magnitute;
    public Vector3 direction;
    Vector3 initialPosition;
    PickUp pickup;

    void Start() 
    {
        pickup = GetComponent<PickUp>();
        initialPosition = transform.position;
    }

    void Update()
    {
        if (pickup && !pickup.hasBeenCollected)
        {
            transform.position = initialPosition + direction * Mathf.Sin(Time.time * frequency) * magnitute;
        }
    }
}
