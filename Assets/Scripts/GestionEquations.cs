using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using TMPro;

[System.Serializable]
public class Equations : MonoBehaviour
{
    public TextAsset jsonFile;
    string contenuJson;
    EquationList donnees; // Liste des �quations

    // Variables pour l'UI
    public TMP_Text questionText;
    public TMP_InputField reponseUtilisateur;
    public TMP_Text messageText;
    public TMP_Text indiceText;
    public Button validerButton;
    public Button continuerButton;
    public Button aideButton;
    public GameObject panelQuestion;

    private List<EquationsAttribut> toutesLesQuestions;
    private int index = 0;

    void Start()
    {
        panelQuestion.SetActive(false);
          string chemin = Application.dataPath + "/data/" + jsonFile.name + ".json"; 
        if (File.Exists(chemin))
        {
            contenuJson = File.ReadAllText(chemin);
            donnees = JsonUtility.FromJson<EquationList>("{\"equations\":" + contenuJson + "}");
            toutesLesQuestions = donnees.equations;
            foreach (var eq in toutesLesQuestions)
            {
                Debug.Log(eq.equation + " -> " + eq.solution + " | Indice: " + eq.indice);
            }

            if (toutesLesQuestions != null && toutesLesQuestions.Count > 0)
            {
                //AfficherQuestion(); // Afficher la premi�re question au d�marrage
            }
            else
            {
                Debug.LogError("Aucune question charg�e.");
            }
        }
        else
        {
            Debug.LogError("Fichier JSON non trouv� : " + chemin);
        }

        // Ajouter des �couteurs pour les boutons
        validerButton.onClick.AddListener(VerifierReponse);
        continuerButton.onClick.AddListener(ContinuerJeu);
        aideButton.onClick.AddListener(AfficherIndice);
    }

    public void AfficherQuestion()
    {
        if (index < toutesLesQuestions.Count)
        {
            questionText.text = toutesLesQuestions[index].equation;
            reponseUtilisateur.text = "";
            messageText.text = "";
            indiceText.text = "";  // R�initialise l'indice
            panelQuestion.SetActive(true); // Active le Panel
        }
        else
        {
            questionText.text = "Jeu termin� !";
            messageText.text = "";
            indiceText.text = "";
            panelQuestion.SetActive(false); // Masque le Panel � la fin du jeu
        }
    }

    // V�rifier la r�ponse de l'utilisateur
    public void VerifierReponse()
    {
        if (index >= toutesLesQuestions.Count) return;

        string bonneReponse = toutesLesQuestions[index].solution;
        string reponse = reponseUtilisateur.text.Trim();

        if (reponse == bonneReponse)
        {
            messageText.text = "Bonne r�ponse !";
        }
        else
        {
            messageText.text = "Mauvaise r�ponse.\nLa Solution est : " + bonneReponse;
        }

        index++;
    }

    // Continuer le jeu en fermant le panel
    public void ContinuerJeu()
    {
        panelQuestion.SetActive(false); // Masque le panel pour continuer le jeu
    }

    // Afficher l'indice
    public void AfficherIndice()
    {
        if (index < toutesLesQuestions.Count)
        {
            indiceText.text = "Indice : " + toutesLesQuestions[index].indice;
        }
    }
}

[System.Serializable]
public class EquationsAttribut
{
    public string equation;
    public string solution;
    public string indice;
}

[System.Serializable]
public class EquationList
{
    public List<EquationsAttribut> equations;
}
