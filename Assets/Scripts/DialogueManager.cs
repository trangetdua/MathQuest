using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image characterImage;

    [SerializeField] private TMP_InputField answerInputField; 
    [SerializeField] private Button submitButton;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private GameObject[] heartIcons;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Lives")]
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    private Story currentStory;
    private bool dialogueIsPlaying;
    private static DialogueManager instance;

    private string currentQuestionKey = "";

    private System.Action onDialogueComplete;

    private bool waitingForCorrectAnswer = false;

    private Dictionary<string, string> questionAnswersElementaire = new Dictionary<string, string>()
{
    { "Il prévoit de fournir 20 litres", "200000" },
    { "l’État a besoin de 2 doses", "30000" },
    { "Un village compte 900 habitants", "850" },
    { "répartir 600 sacs de riz", "200" },
    { "4 tentes de 50 places", "200" }
};

    private Dictionary<string, string> questionAnswersIntermediaire = new Dictionary<string, string>()
{
    { "Au moment de la crue du Nil", "50" },
    { "Le seked représente", "4.3" },
    { "une pyramide à base carrée", "108" },
    { "pour que (2/3)x + 5 = 35", "45" }, 
    { "compare son ombre à celle d’un bâton", "25" }
};

    private Dictionary<string, string> questionAnswersAvance = new Dictionary<string, string>()
{
    { "région au bord du Gange", "18" },
    { "Aire ≈ ((8/9)×D)²", "256" },
    { "Un terrain de 2400", "720" },
    { "l’angle d’inclinaison est de 52°", "128" },
    { "la plus grande solution de cette équation", "5" }
};
    private Dictionary<string, string> questionAnswersExpert = new Dictionary<string, string>()
{
    { "crues majeures du Nil par an", "18.0" },
    { "obélisque penche de 6°", "2" },
    { "hexagone régulier de côté 10", "3897" },
    { "famille A reçoit 2/9", "319" },
    { "1000 sacs de grain subissent", "630" }
};


    private int correctAnswersCount = 0;
    private int totalQuestionsCount => currentQuestionAnswers?.Count ?? 0;

    private bool allQuestionsAnsweredCorrectly = false;
    private Dictionary<string, string> currentQuestionAnswers;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("found more than one dialogue manager in the scene");
        }

        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        currentLives = maxLives;
        UpdateLivesUI();
        gameOverPanel.SetActive(false);

    }

    private void UpdateLivesUI()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].SetActive(i < currentLives);
        }
    }


    public void EnterDialogueMode(TextAsset inkJson, System.Action onComplete = null)
    {
        currentStory = new Story(inkJson.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        this.onDialogueComplete = onComplete;

        string difficulty = GameDataManager.Instance.selectedDifficulty;
        switch (difficulty)
        {
            case "elementaire":
                currentQuestionAnswers = questionAnswersElementaire;
                break;
            case "intermediaire":
                currentQuestionAnswers = questionAnswersIntermediaire;
                break;
            case "avance":
                currentQuestionAnswers = questionAnswersAvance;
                break;
            case "expert":
                currentQuestionAnswers = questionAnswersExpert;
                break;
            default:
                currentQuestionAnswers = questionAnswersElementaire;
                break;
        }

        correctAnswersCount = 0;
        allQuestionsAnsweredCorrectly = false;
        currentLives = maxLives;
        UpdateLivesUI();
        gameOverPanel.SetActive(false);

        ContinueStory();
    }


    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        if (onDialogueComplete != null)
        {
            var callback = onDialogueComplete;
            onDialogueComplete = null;
            callback.Invoke();
        }
    }


    private void Update()
    {
        if (!dialogueIsPlaying) 
        { 
            return;
        }

        //press space for skip to next line
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
        }
    }

    private void ContinueStory()
    {
        if (waitingForCorrectAnswer)
        {
            return; 
        }

        if (currentStory.canContinue)
        {
            string line = currentStory.Continue().Trim();
            Debug.Log(">> LINE: " + line);

            string speaker = "";
            if (line.Contains(":"))
            {
                var parts = line.Split(':');
                speaker = parts[0].Trim();
                line = parts[1].Trim();
                nameText.text = speaker;
            }
            else
            {
                nameText.text = "";
            }

            dialogueText.text = line;

            answerInputField.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
            resultText.gameObject.SetActive(false);

            string portraitName = "";
            if (currentStory.variablesState["portrait"] != null)
            {
                portraitName = currentStory.variablesState["portrait"].ToString();
            }

            Debug.Log($"[DEBUG] Speaker: {speaker}, portrait var: {currentStory.variablesState["portrait"]}, portraitName: {portraitName}");

            Sprite loadedSprite = Resources.Load<Sprite>($"image/Player/{portraitName}");
            Debug.Log($"[DEBUG] Loaded Sprite Name: {loadedSprite?.name}");

            if (loadedSprite != null)
            {
                characterImage.sprite = loadedSprite;
                characterImage.enabled = true;
            }
            else
            {
                characterImage.enabled = false;
            }

            foreach (var questionKey in currentQuestionAnswers.Keys)
            {
                if (line.Contains(questionKey))
                {
                    ShowAnswerInput(questionKey);
                    return;
                }
            }

        }
        else
        {
            if (allQuestionsAnsweredCorrectly)
            {
                nameText.text = "Imhotep";
                dialogueText.text = "Très bien. Retourne à l'endroit auquel tu appartiens.";
                characterImage.sprite = Resources.Load<Sprite>("image/Player/Imhotep");
                characterImage.enabled = true;

                StartCoroutine(DisappearNPC());
            }
            else
            {
                ExitDialogueMode();
            }
        }
    }


    public void OnSubmitAnswer()
    {
        string userAnswer = answerInputField.text.Trim();
        string correctAnswer = currentQuestionAnswers[currentQuestionKey];

        if (userAnswer == correctAnswer)
        {
            resultText.text = "Bonne reponse !";
            correctAnswersCount++;

            if (correctAnswersCount == totalQuestionsCount)
            {
                allQuestionsAnsweredCorrectly = true;
            }

            StartCoroutine(HideAnswerAndContinue());
        }
        else
        {
            currentLives--;
            UpdateLivesUI();

            if (currentLives <= 0)
            {
                GameOver();
            }
            else
            {
                resultText.text = "Mauvaise réponse. Réessaie.";
            }
        }
    }

    private void GameOver()
    {
        dialogueIsPlaying = false;
        answerInputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        resultText.gameObject.SetActive(true);
        resultText.text = "Vous avez perdu.";

        gameOverPanel.SetActive(true);
    }


    IEnumerator HideAnswerAndContinue()
    {
        yield return new WaitForSeconds(2f);
        answerInputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
        waitingForCorrectAnswer = false;
        ContinueStory();
    }
    IEnumerator DisappearNPC()
    {
        yield return new WaitForSeconds(2f); 
        ExitDialogueMode();

        GameObject npc = GameObject.Find("imhotep"); 
        if (npc != null)
        {
            npc.SetActive(false); 
        }
    }


    void ShowAnswerInput(string questionKey)
    {
        currentQuestionKey = questionKey;
        waitingForCorrectAnswer = true;
        answerInputField.text = "";
        answerInputField.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
        resultText.gameObject.SetActive(true);
        resultText.text = ""; // reset
    }

}
