using UnityEngine;

public class MovingClouds : MonoBehaviour
{
    public float cloudSpeed = 0.2f; // viteza cu care se misca norii
    private Transform[] cloudTransforms; //array pentru transform ul noriilor
    private float leftBoundary; 
    private float rightBoundary;
    private float spriteWidth;

    void Start()
    {
        //initializam array-ul cu copii norilor.
        cloudTransforms = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            cloudTransforms[i] = transform.GetChild(i);
        }

        // latimea sprite-ului
        if (cloudTransforms.Length > 0)
        {
            spriteWidth = cloudTransforms[0].GetComponent<SpriteRenderer>().bounds.size.x;
        }

        // setam limitele pe baza vizibilitatii camerei si latimii sprite-ului
        Camera mainCamera = Camera.main; //referinta la camera principala
        float screenAspect = (float)Screen.width / Screen.height; //raportul de aspect al programului latimea / inaltime si convertim in float pentru a evita anumite conversii, aveam probleme daca nu converteam.
        float cameraHeight = mainCamera.orthographicSize * 2; //inaltimea vizibila a camerei care este de 2 ori valoarea orthographicSize ului.
        float cameraWidth = cameraHeight * screenAspect; //->raport de aspect al ecranului
        //latimea completa a vizibilului pe ecran.
        leftBoundary = -cameraWidth / 2 - spriteWidth;
        // impartim latimea camerei la 2 pentru a obtine jumatatea latimii si scadem latimea sprite-ului de unde ne rezulta limita din stanga unde norii trebeuie repozitionati

        rightBoundary = cameraWidth / 2 + spriteWidth; //la fel doar pentru dreapta, valoriile fiind pozitive
    }

    void Update()
    {
        foreach (Transform cloud in cloudTransforms)
        {
            // Translatam norii spre stanga pe baza vitezei setate si a timpului scurs intre frame-uri (Time.deltaTime)

            cloud.Translate(Vector3.left * cloudSpeed * Time.deltaTime);

            // repozitionam norii cand acestia merg off-screan
            if (cloud.position.x <= leftBoundary)
            {
                // Gasim norul cel mai din dreapta (rightmostCloud) din toate transformarile norilor
                Transform rightmostCloud = cloudTransforms[0];
                foreach (Transform t in cloudTransforms)
                {
                    // Daca pozitia pe axa X a norului curent este mai mare decat pozitia norului cel mai din dreapta atunci  actualizam rightmostCloud

                    if (t.position.x > rightmostCloud.position.x)
                    {
                        rightmostCloud = t;
                    }
                }

                // repozitionam norul curent (care a trecut de limita din stanga) in dreapta norului cel mai din dreapta
                Vector3 newPosition = new Vector3(rightmostCloud.position.x + spriteWidth, cloud.position.y, cloud.position.z);
                cloud.position = newPosition; //setarea noii pozitii a norului
            }
        }
    }
}
