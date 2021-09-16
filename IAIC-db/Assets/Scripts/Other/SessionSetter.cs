using TMPro;
using UnityEngine;

namespace Other
{
    public class SessionSetter : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text time;

        public void SetTexts(string sessionName, string sessionTime)
        {
            nameText.text = sessionName;
            time.text = sessionTime;
        }
    }
}
