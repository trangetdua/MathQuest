using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void LoadSelectLevelScene()
    {
        SceneManager.LoadScene("SelectLevel");
    }
}
