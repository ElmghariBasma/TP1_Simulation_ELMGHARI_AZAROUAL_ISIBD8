using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private CharacterController controller;
    private float verticalVelocity = 0f;
    private float gravity = -9.81f;

    void Start()
    {
        // Récupère le composant CharacterController attaché au Player
        controller = GetComponent<CharacterController>();

        // Capture la souris pour que la caméra tourne avec
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        // 1. Gestion du déplacement (ZQSD / WASD)
        float horizontal = Input.GetAxis("Horizontal"); // A/D ou Q/D
        float vertical = Input.GetAxis("Vertical");     // W/S

        // Calcul du vecteur de déplacement relatif à la rotation du joueur
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Applique la vitesse
        controller.Move(move * speed * Time.deltaTime);

        // 2. Gestion simple de la gravité (pour rester collé au sol)
        if (controller.isGrounded)
        {
            verticalVelocity = -2f; // Petite force vers le bas pour coller au sol
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Applique la gravité sur l'axe Y uniquement
        controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);

        // 3. Rotation de la caméra avec la souris (Gauche/Droite)
        float mouseX = Input.GetAxis("Mouse X") * 2f; // Sensibilité
        transform.Rotate(0, mouseX, 0);
    }
}