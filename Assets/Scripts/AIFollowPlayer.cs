using UnityEngine;

public class AIFollowPlayer : MonoBehaviour
{
    [Header("Cible à suivre")]
    public Transform target; // Le joueur à suivre

    [Header("Paramètres de déplacement")]
    public float speed = 3.5f; // Vitesse de poursuite
    public float stopDistance = 1.5f; // Distance d'arrêt (pour ne pas coller le joueur)
    public float rotationSpeed = 5f; // Vitesse de rotation

    void Update()
    {
        // Vérifie que la cible existe
        if (target == null)
        {
            Debug.LogWarning("[AIFollower] Aucune cible définie !");
            return;
        }

        // Calcule la distance entre l'IA et le joueur
        float distance = Vector3.Distance(transform.position, target.position);

        // Si on est plus loin que la distance d'arrêt, on avance
        if (distance > stopDistance)
        {
            // Calcule la direction vers le joueur
            Vector3 direction = (target.position - transform.position).normalized;

            // Déplace l'objet vers le joueur
            transform.position += direction * speed * Time.deltaTime;

            // Fait tourner l'objet pour qu'il regarde le joueur
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Dessine une ligne verte dans la Scene View pour visualiser la poursuite
    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}