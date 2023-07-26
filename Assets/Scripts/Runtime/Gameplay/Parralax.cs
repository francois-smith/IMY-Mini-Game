/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using Runtime.Managers;
using UnityEngine;

namespace Runtime.Gameplay
{
    public class Parralax : MonoBehaviour
    {
        public float Speed;
        
        private Renderer _renderer;
        private Vector2 _newOffset;

        private float _position;
        private float yOffset;
        
        protected virtual void Start () 
        {
            _renderer = GetComponent<Renderer> ();
        }
        
        protected virtual void Update()
        {
            if (_renderer == null) return;

            if (GameManager.Instance.LevelManager != null)
            { 
                _position += (Speed/300) * GameManager.Instance.LevelManager.Speed * Time.deltaTime;
            }
            else
            {
                _position += (Speed/300) * Time.deltaTime;
            }

            
            if (_position > 1.0f) _position -= 1.0f;

            
            _newOffset.x = _position;
            _newOffset.y = yOffset;
            
            _renderer.material.mainTextureOffset = _newOffset;
        }
    }
}
