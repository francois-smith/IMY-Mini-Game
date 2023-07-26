/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using Runtime.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Gameplay
{
    public sealed class LevelManager : MonoBehaviour
    {
        public float Speed { get; private set; }
        public float DistanceTraveled { get; set; }
        public float pointsPerSecond = 20;
        
        [FormerlySerializedAs("InitialSpeed")]
        [Space(10)]
        [Header("Level Speed Settings")]
        public float initialSpeed = 10f;
        public float maximumSpeed = 50f;
        public float speedAcceleration = 1f;
        
        [Space(10)]
        [Header("Level Bounds Setting")]
        public Bounds levelBounds;
        
        [Space(10)]
        [Header("Music Object")]
        public GameObject music;

        
        private float _savedSpeed;
        private bool _gameActive;

        private void Start()
        {
            Speed = 0;
            DistanceTraveled = 0;
            GameManager.Instance.SetPointsPerSecond(pointsPerSecond);
        }
        
        public void Update()
        {
            if(!_gameActive) return;
            DistanceTraveled += Speed * Time.fixedDeltaTime;
            
            if (Speed < maximumSpeed)
            {
                Speed += speedAcceleration * Time.deltaTime;
            }
        }

        public void StartGame()
        {
            _gameActive = true;
            Speed = initialSpeed;
            music.SetActive(true);
        }
        
        public void Reset()
        {
            Speed = 0;
            DistanceTraveled = 0;
            _gameActive = false;
        }

        public void PauseGame()
        {
            _savedSpeed = Speed;
            Speed = 0;
            _gameActive = false;
        }
        
        public void ResumeGame()
        {
            Speed = _savedSpeed;
            _gameActive = true;
        }
        
    }
}
