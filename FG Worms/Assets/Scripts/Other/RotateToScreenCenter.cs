using UnityEngine;

namespace Seb.Other
{
    public class RotateToScreenCenter : MonoBehaviour
    {
        void Update()
        {
            transform.LookAt(Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 100f)));
        }
    }
}
