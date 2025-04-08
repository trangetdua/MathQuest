using UnityEngine;

public class AutoDialogueStarter : MonoBehaviour
{
    [SerializeField] private TextAsset introInkJSON;

    private bool dialogueStarted = false;

    private void Start()
    {
        Invoke("StartIntroDialogue", 0.1f);
    }

    private void StartIntroDialogue()
    {
        if (!dialogueStarted && introInkJSON != null)
        {
            DialogueManager.GetInstance().EnterDialogueMode(introInkJSON);
            dialogueStarted = true;
        }
    }
}
