using UnityEngine;
using UnityEngine.SceneManagement;

public class menu_equations : MonoBehaviour
{
    public void lancerNiveau1()
    {
        SceneManager.LoadSceneAsync("presen_test");
    }
    public void lancerNiveau2()
    {
        SceneManager.LoadSceneAsync("presen_test_niveau2");
    }
    public void lancerNiveau3()
    {
        SceneManager.LoadSceneAsync("presen_test_niveau3");
    }
    public void lancerNiveau4()
    {
        SceneManager.LoadSceneAsync("presen_test_niveau4");

    }
    public void retourAuMenu()
    {
        SceneManager.LoadSceneAsync("Menu_equations");

    }
}
