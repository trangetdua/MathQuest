using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.SceneManagement;

public class LoginGestion : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField usernameInputField;
    [SerializeField]
    private TMP_InputField passwordInputField;
    [SerializeField]
    private TextMeshProUGUI errorText;

    //Called when the submit button is pressed, it will grab usernam and pwd to check if they are correct
    public void onSubmitLogin()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        //Checkin
        Debug.Log("USERNAME:" + username);
        Debug.Log("PASSWORD:" +  password);

        string loginCheckMessage = CheckLoginInfo(username, password);

        if (string.IsNullOrEmpty(loginCheckMessage)) {
            Debug.Log("LOGIN");
            SceneManager.LoadScene("SelectLevel");
        } else
        {
            Debug.LogError("ERROR" +loginCheckMessage);
            errorText.text = "ERROR" + loginCheckMessage;
        }

    }

    private string CheckLoginInfo(string username, string password)
    {
        string returnString = "";

        if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password)) {
            returnString = "Pas encore saisi";
        }

        if (string.IsNullOrEmpty(username)) {
            returnString = "Username null";
        }

        if (string.IsNullOrEmpty(password))
        {
            returnString = "Username null";
        }

        Debug.Log(returnString);
        return returnString;
    }

    public void RemoveErrorText()
    {
        errorText.text = "";
    }

}
