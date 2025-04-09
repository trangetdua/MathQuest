using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public TextMeshProUGUI popUpText;

    public void PopUp(string text)
    {

        if (popUpBox == null)
        {
            Debug.LogError("popUpBox is not assigned!");
        }
        else
        {
            Debug.Log("popUpBox is assigned: " + popUpBox.name);
        }

        popUpBox.SetActive(true);
        popUpText.text = text;
        animator.SetTrigger("pop");
    }

}
