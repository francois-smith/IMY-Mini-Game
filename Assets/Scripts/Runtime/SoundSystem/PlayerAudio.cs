/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using System.Collections.Generic;
using Runtime.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.SoundSystem
{
    public class PlayerAudio : MonoBehaviour
    {
        [Header("SOUNDS")]
        [SerializeField] private List<AudioClip> footsteps;
        [SerializeField] private List<AudioClip> jumpSounds;
        [SerializeField] private List<AudioClip> deathSounds;
        [SerializeField] private AudioClip attackSound;
        [SerializeField] private AudioClip slideSound;
        
        public void PlayFootstepSound()
        {
            GameManager.Instance.SoundManager.Play(footsteps[Random.Range(0, footsteps.Count)], 0.18f);
        }
        
        public void PlayJumpSound()
        {
            GameManager.Instance.SoundManager.Play(jumpSounds[Random.Range(0, jumpSounds.Count)], 0.22f);
        }
        
        public void PlayAttackSound()
        {
            GameManager.Instance.SoundManager.Play(attackSound, 0.18f);
        }
        
        public void PlayDeathSound()
        {
            GameManager.Instance.SoundManager.Play(deathSounds[Random.Range(0, deathSounds.Count)], 0.18f);
        }
        
        public void PlaySlideSound()
        {
            GameManager.Instance.SoundManager.Play(slideSound, 0.18f);
        }
    }
}
