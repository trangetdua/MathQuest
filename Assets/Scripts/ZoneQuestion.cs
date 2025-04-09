using UnityEngine;

public class ZoneQuestion : MonoBehaviour
{
    public Equations equationsScript; // � assigner dans l'inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            equationsScript.AfficherQuestion(); // <--- Affiche la question d�s la collision
        }
    }
}
