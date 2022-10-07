using UnityEngine;
using System.Collections;
using Seb.Managers;

namespace Seb.Other
{
    public class MoveWater : MonoBehaviour
    {
        [SerializeField]
        private Transform _waterTransform;

        [SerializeField]
        private float _roundToMove = 2;

        [SerializeField]
        private float _moveAmount = 1;

        [SerializeField]
        private float _moveSpeed = 1;

        public void DoMoveWater()
        {
            if (_waterTransform != null && _roundToMove <= GameManager.CurrentRound)
            {
                StartCoroutine(Move());
            }
        }

        private IEnumerator Move()
        {
            Vector3 newPosition = _waterTransform.position + new Vector3(0f, _moveAmount, 0f);

            // Move to newPosition
            while (true)
            {
                _waterTransform.position = Vector3.MoveTowards(_waterTransform.position, newPosition, _moveSpeed * Time.deltaTime);
                if (_waterTransform.position == newPosition) break;
                yield return null;
            }
        }
    }
}
