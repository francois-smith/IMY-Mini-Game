/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.SoundSystem
{
	public class SoundManager : MonoBehaviour
	{
        public AudioSource musicSourcePrefab;
        
        private AudioListener _audioListener;
        private readonly Dictionary<AudioSource, AudioClip> _activeSoundInstanceSources = new();

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _audioListener = FindObjectOfType(typeof(AudioListener)) as AudioListener;
        }

        public void Play(AudioClip sound, float volume = 1f, Transform origin = null)
        {
            if (sound == null) return;

            var audioSource = new GameObject(sound.name + " source").AddComponent<AudioSource>();
            audioSource.transform.position = origin == null ? _audioListener.transform.position : origin.position;
            DontDestroyOnLoad(audioSource);
            _activeSoundInstanceSources.Add(audioSource, sound);
                
            audioSource.clip = sound;
            audioSource.volume = volume;
            audioSource.loop = false;
            audioSource.spatialBlend = origin == null ? 0 : 1;
            audioSource.Play();
                
            StartCoroutine(DestroySoundSource(audioSource, audioSource.clip.length+0.5f));
        }
        
        public void StopAll()
        {
            foreach (var audioSource in _activeSoundInstanceSources)
            {
                audioSource.Key.Stop();
                Destroy(audioSource.Key.gameObject);
            }
            _activeSoundInstanceSources.Clear();
            
            musicSourcePrefab.Stop();
            Destroy(musicSourcePrefab.gameObject);
        }

        public void PauseAll()
        {
            foreach (var audioSource in _activeSoundInstanceSources)
            {
                audioSource.Key.Pause();
            }
            
            musicSourcePrefab.Pause();
        }

        public void ResumeAll()
        {
            foreach (var audioSource in _activeSoundInstanceSources)
            {
                audioSource.Key.UnPause();
            }
            
            musicSourcePrefab.UnPause();
        }

        private IEnumerator DestroySoundSource(AudioSource audioSource, float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            if(audioSource == null) yield break;
            _activeSoundInstanceSources.Remove(audioSource);
            Destroy(audioSource.gameObject);
        }
        
    }
}
