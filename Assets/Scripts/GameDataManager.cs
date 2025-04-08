using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public string selectedDifficulty = "elementaire"; // default

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // contain objects through scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
