using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayIntro : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "Sign in"; 

    void Start()
    {
        string path = System.IO.Path.Combine(Application.streamingAssetsPath, "intro.mp4");
        videoPlayer.url = path;
        videoPlayer.Play();

        videoPlayer.loopPointReached += OnVideoEnd;
        Debug.Log("Path to video: " + path);

    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            videoPlayer.Stop();
            SceneManager.LoadScene(nextSceneName);
        }
    }

}
