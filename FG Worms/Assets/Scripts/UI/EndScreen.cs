using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Seb.UI
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField]
        private GameObject _endScreenCanvas;

        [SerializeField]
        private TextMeshProUGUI _endScreenText;

        private void Awake()
        {
            if (!_endScreenCanvas || !_endScreenText)
            {
                print("Error! No text or canvas. Destroying this");
                Destroy(this);
            }
        }

        public void ShowEndScreen(string endScreenText)
        {
            _endScreenCanvas.SetActive(true);
            _endScreenText.text = endScreenText;
        }
    }
}
