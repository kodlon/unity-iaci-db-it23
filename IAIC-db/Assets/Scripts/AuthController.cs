using System.Collections;
using Firebase;
using UnityEngine;
using TMPro;
using Firebase.Auth;

public class AuthController : MonoBehaviour
{
    [SerializeField] private TMP_InputField userEmail;
    [SerializeField] private TMP_InputField userPassword;
    [SerializeField] private TMP_Text userErrorMessage;
    [SerializeField] private GameObject logInUi;

    private string _errorMessage;
    private bool _isLoggedIn;


    public void OnLoginButtonPressed() //login in app
    {
        StartCoroutine(LogInCoroutine());
    }

    private IEnumerator LogInCoroutine()
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
                    _isLoggedIn = true;
                }
            }));

        if (_isLoggedIn)
        {
            logInUi.SetActive(false);
        }
        else
        {
            userErrorMessage.text = _errorMessage;
        }
    }
}