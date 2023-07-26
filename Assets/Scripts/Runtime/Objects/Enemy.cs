/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using System.Collections.Generic;
using Runtime.Managers;
using Runtime.Player;
using UnityEngine;

namespace Runtime.Objects
{
    public class Enemy : MonoBehaviour
    {
        public bool canDie;
        public float points;
        public AudioSource audioSource;
        public AudioClip deathSound;
        public List<AudioClip> moveSounds;

        private Animator _animator;
        private static readonly int Die = Animator.StringToHash("Die");
        private float _speed;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _speed = GetComponent<MovingObject>().speed;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var player = other.GetComponent<PlayerController>();
                if (player.isAttacking && canDie)
                {
                    GetComponent<Collider2D>().enabled = false;
                    GetComponent<MovingObject>().speed = 10;
                    GameManager.Instance.AddPoints(points);
                    _animator.SetTrigger(Die);
                }
                else
                {
                    GameManager.Instance.LooseLife();
                    GetComponent<Collider2D>().enabled = false;
                }
            }
        }

        private void ResetSpeed()
        {
            GetComponent<MovingObject>().speed = _speed;
        }
        
        private void PlayDeathSound()
        {
            GameManager.Instance.SoundManager.Play(deathSound, 0.15f);
        }
        
        private void PlayMoveSound()
        {
            audioSource.PlayOneShot(moveSounds[Random.Range(0, moveSounds.Count)]);
        }
    }
}
