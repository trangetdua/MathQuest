using UnityEngine;

public class DialogueTrigger : MonoBehaviour, Interactable
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON for difficulties")]
    [SerializeField] private TextAsset introDialogue;
    [SerializeField] private TextAsset inkElementaire;
    [SerializeField] private TextAsset inkIntermediaire;
    [SerializeField] private TextAsset inkAvance;
    [SerializeField] private TextAsset inkExpert;

    private bool playerInRange;
    private bool introDone = false;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        visualCue.SetActive(playerInRange);

        if (playerInRange && InputManager.GetInstance().GetInteractPressed())
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (playerInRange)
        {
            if (!introDone)
            {
                DialogueManager.GetInstance().EnterDialogueMode(introDialogue, OnIntroFinished);
                introDone = true;
            }
            else
            {
                DialogueManager.GetInstance().EnterDialogueMode(GetInkForDifficulty());
            }
        }
    }

    private void OnIntroFinished()
    {
        DialogueManager.GetInstance().EnterDialogueMode(GetInkForDifficulty());
    }

    void TriggerDialogue()
    {
        TextAsset selectedInk = GetInkForDifficulty();

        if (selectedInk != null)
        {
            DialogueManager.GetInstance().EnterDialogueMode(selectedInk);
        }
        else
        {
            Debug.LogError("Ink file is missing for selected difficulty!");
        }
    }

    private TextAsset GetInkForDifficulty()
    {
        string difficulty = GameDataManager.Instance.selectedDifficulty;

        switch (difficulty)
        {
            case "elementaire":
                return inkElementaire;
            case "intermediaire":
                return inkIntermediaire;
            case "avance":
                return inkAvance;
            case "expert":
                return inkExpert;
            default:
                return inkElementaire; // fallback
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
