using Firebase.Database;
using TMPro;
using UnityEngine;

namespace DatabaseController
{
    public class PrefabEditing : MonoBehaviour
    {
        [SerializeField] private TMP_Text sessionName;

        private DatabaseReference _databaseReference;

        private void Start()
        {
            _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void OnDeleteButtonPressed()
        {
            _databaseReference.Child("Session " + sessionName.text).RemoveValueAsync();
            Destroy(gameObject);
        }
    }
}