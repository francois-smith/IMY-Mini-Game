using System.Drawing;
using Runtime.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Objects
{
    public sealed class MovingObject : MonoBehaviour
    {
        [Header("Movement")]
        public float speed;
        public bool independentSpeed;

        private Vector3 _movement;
        private float _initialSpeed;
        
        private void Awake() 
        {
            _initialSpeed = speed;
        }

        private void OnEnable()
        {
            speed = _initialSpeed;
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            _movement = Vector2.left * (speed / 10 * Time.deltaTime);
            if(independentSpeed) _movement *= GameManager.Instance.LevelManager.Speed == 0 ? 4 : GameManager.Instance.LevelManager.Speed;
            else _movement *= GameManager.Instance.LevelManager.Speed;
            transform.Translate(_movement, Space.World);
        }
    }
}
