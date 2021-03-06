using System.Collections;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine;

namespace AuthorizationController
{
    public class LogIn : MonoBehaviour
    {
        [SerializeField] private GameObject logInUi;

        [SerializeField] private TMP_InputField userEmail;
        [SerializeField] private TMP_InputField userPassword;
        [SerializeField] private TMP_Text userErrorMessage;

        public static bool IsLoggedIn { get; set; }
        private string _errorMessage;


        private void Awake()
        {
            LoadLoginData();
        }

        public void OnLoginButtonPressed() //login in app
        {
            StartCoroutine(UserLogIn());
        }

        /// <summary>
        /// Coroutine that work with firebase and check login data.
        /// </summary>
        private IEnumerator UserLogIn()
        {
            yield return new YieldTask(FirebaseApp.CheckAndFixDependenciesAsync());
            yield return new YieldTask(FirebaseAuth.DefaultInstance
                .SignInWithEmailAndPasswordAsync(userEmail.text, userPassword.text)
                .ContinueWith(task =>
                {
                    if (task.IsCanceled || task.IsFaulted)
                    {
                        if (task.Exception?.Flatten().InnerExceptions[0] is FirebaseException exception)
                            _errorMessage = ((AuthError) exception.ErrorCode).ToString();

                        return;
                    }

                    if (task.IsCompleted)
                    {
                        Debug.Log("Logged IN");
                        IsLoggedIn = true;
                    }
                }));

            //TODO: relocate this in another method
            if (IsLoggedIn)
            {
                logInUi.SetActive(false);
                SaveLoginData(userEmail.text, userPassword.text);
            }
            else
            {
                userErrorMessage.text = _errorMessage;
            }
        }

        /// <summary>
        /// If not empty. Loading user data.
        /// </summary>
        private void LoadLoginData()
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString("userLoggedName")) ||
                string.IsNullOrEmpty(PlayerPrefs.GetString("userLoggedPassword"))) return;

            userEmail.text = PlayerPrefs.GetString("userLoggedName");
            userPassword.text = PlayerPrefs.GetString("userLoggedPassword");
            StartCoroutine(UserLogIn());
        }

        /// <summary>
        /// Saving user data. For remember him.
        /// </summary>
        /// <param name="userLoggedName">Name of user thar logged in app.</param>
        /// <param name="userLoggedPassword">Password of user thar logged in app.</param>
        private void SaveLoginData(string userLoggedName, string userLoggedPassword)
        {
            PlayerPrefs.SetString("userLoggedName", userLoggedName);
            PlayerPrefs.SetString("userLoggedPassword", userLoggedPassword);
            PlayerPrefs.Save();
        }
    }
}