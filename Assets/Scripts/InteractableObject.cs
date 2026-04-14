using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string objectName = "ObjetSansNom";

    private Renderer rend;
    private Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;
    }

    void OnMouseDown()
    {
        if (rend != null)
            rend.material.color = Color.yellow;

        Debug.Log($"[Clic] Objet touché : {objectName} | Temps : {Time.time:F2}s");

        if (DataManager.Instance != null)
        {
            DataManager.Instance.RegisterInteraction(objectName);
            DataManager.Instance.AddClick();
        }
    }

    void OnMouseUp()
    {
        if (rend != null)
            rend.material.color = originalColor;
    }
}