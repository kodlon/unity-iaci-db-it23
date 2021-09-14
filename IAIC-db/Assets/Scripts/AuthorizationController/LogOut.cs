using TMPro;
using UnityEngine;

namespace AuthorizationController
{
    public class LogOut : MonoBehaviour
    {
        [SerializeField] private GameObject logInUi;

        [SerializeField] private TMP_InputField userEmail;
        [SerializeField] private TMP_InputField userPassword;


        public void UserLogOut()
        {
            userEmail.text = "";
            userPassword.text = "";
            logInUi.SetActive(true);
            LogIn.IsLoggedIn = false;

            PlayerPrefs.SetString("userLoggedName", "");
            PlayerPrefs.SetString("userLoggedPassword", "");
            PlayerPrefs.Save();
        }
    }
}