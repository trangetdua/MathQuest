using UnityEngine;
using UnityEngine.SceneManagement;

public class NiveauSelect : MonoBehaviour
{
    public void SelectElementaire()
    {
        GameDataManager.Instance.selectedDifficulty = "elementaire";
        SceneManager.LoadScene("Egypt1");
    }

    public void SelectIntermediaire()
    {
        GameDataManager.Instance.selectedDifficulty = "intermediaire";
        SceneManager.LoadScene("Egypt1");
    }

    public void SelectAvance()
    {
        GameDataManager.Instance.selectedDifficulty = "avance";
        SceneManager.LoadScene("Egypt1");
    }

    public void SelectExpert()
    {
        GameDataManager.Instance.selectedDifficulty = "expert";
        SceneManager.LoadScene("Egypt1");
    }
}
