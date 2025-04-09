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
    EquationList donnees; // Liste des équations

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
                //AfficherQuestion(); // Afficher la première question au démarrage
            }
            else
            {
                Debug.LogError("Aucune question chargée.");
            }
        }
        else
        {
            Debug.LogError("Fichier JSON non trouvé : " + chemin);
        }

        // Ajouter des écouteurs pour les boutons
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
            indiceText.text = "";  // Réinitialise l'indice
            panelQuestion.SetActive(true); // Active le Panel
        }
        else
        {
            questionText.text = "Jeu terminé !";
            messageText.text = "";
            indiceText.text = "";
            panelQuestion.SetActive(false); // Masque le Panel à la fin du jeu
        }
    }

    // Vérifier la réponse de l'utilisateur
    public void VerifierReponse()
    {
        if (index >= toutesLesQuestions.Count) return;

        string bonneReponse = toutesLesQuestions[index].solution;
        string reponse = reponseUtilisateur.text.Trim();

        if (reponse == bonneReponse)
        {
            messageText.text = "Bonne réponse !";
        }
        else
        {
            messageText.text = "Mauvaise réponse.\nLa Solution est : " + bonneReponse;
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
