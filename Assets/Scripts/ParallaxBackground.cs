using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float backgroundSpeed = 0.8f; //viteza deplasare fundal
    public float tileSizeX;  //dimensiunea pe axa X a tile-ului de fundal

    private Vector3 startPosition; //pozitia initiala

    void Start()
    {
        startPosition = transform.position; //salvam poz init.
        tileSizeX = GetComponent<SpriteRenderer>().bounds.size.x; //setam dimensiunea pe axa folosind marimea sprite ului
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * backgroundSpeed, tileSizeX); //calculam noua pozitie folosind functia Mathf.Repeat
        transform.position = startPosition + Vector3.left * newPosition; //// Actualizam pozitia fundalului pentru efectul de parallax
    }
}
