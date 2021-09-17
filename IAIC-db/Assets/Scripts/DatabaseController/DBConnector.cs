using System.Collections;
using System.Linq;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using Other;
using TMPro;
using UnityEngine.UI;


namespace DatabaseController
{
    public class DBConnector : MonoBehaviour
    {
        [SerializeField] private GameObject addSessionUi;
        [SerializeField] private GameObject sessionItem;
        [SerializeField] private GameObject sessionContent;
        [SerializeField] private TMP_InputField sessionName;
        [SerializeField] private TMP_InputField sessionTime;

        private DatabaseReference _databaseReference;


        private void Start()
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            LoadData();
        }

        public void SaveData()
        {
            if (sessionName.text.Equals("") && sessionTime.text.Equals(""))
            {
                Debug.Log("Session Empty");
                return;
            }

            SessionData sessionData = new SessionData(sessionName.text, sessionTime.text);

            string jsonData = JsonUtility.ToJson(sessionData);

            _databaseReference.Child("Session " + sessionName.text).SetRawJsonValueAsync(jsonData);

            sessionName.text = "";
            sessionTime.text = "";
            foreach (Transform child in sessionContent.transform)
            {
                Destroy(child.gameObject);
            }
            LoadData();
            addSessionUi.SetActive(false);
        }

        public void OnAddSessionPressed()
        {
            addSessionUi.SetActive(true);
        }

        private void LoadData()
        {
            FirebaseDatabase.DefaultInstance
                .GetReference("")
                .GetValueAsync().ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("ERROR");
                    }
                    else if (task.IsCompleted)
                    {
                        DataSnapshot snapshot = task.Result;
                        foreach (DataSnapshot sessions in snapshot.Children)
                        {
                            GameObject a = Instantiate(sessionItem, sessionContent.transform);
                            IDictionary dictionary = (IDictionary) sessions.Value;
                            Debug.Log(dictionary["name"] + " " + dictionary["time"]);
                            a.GetComponent<SessionSetter>().SetTexts(dictionary["name"].ToString(),
                                dictionary["time"].ToString());
                        }
                    }
                });
        }
    }
}