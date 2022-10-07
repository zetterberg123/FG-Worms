using TMPro;
using UnityEngine;

namespace Seb
{
    public class GetVersion : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _versionText;
        [SerializeField] private bool _justVersion = false;

        void Start()
        {
            if (_versionText == null) TryGetComponent(out _versionText);
            if (!_justVersion) _versionText.text = "DEMO: v" + Application.version;
            else _versionText.text = "v" + Application.version;
        }
    }
}
