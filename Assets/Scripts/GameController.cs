using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    void Update()
    {
        playerController.HandleUpdate();
    }
}
