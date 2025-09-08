using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency;
    public float magnitute;
    public Vector3 direction;
    Vector3 initialPosition;

    void Start() 
    {
        initialPosition = transform.position;
    }

    void Update() 
    {
        transform.position = initialPosition + direction * Mathf.Sin(Time.time * frequency) * magnitute;
    }
}
