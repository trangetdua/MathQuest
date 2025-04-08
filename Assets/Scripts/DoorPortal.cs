using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorPortal : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "SelectLevel";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
