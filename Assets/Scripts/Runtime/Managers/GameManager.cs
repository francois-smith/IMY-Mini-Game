/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using System.Collections;
using Runtime.Gameplay;
using Runtime.InputSystem;
using Runtime.Objects;
using Runtime.Player;
using Runtime.SaveSystem;
using Runtime.SoundSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.Managers
{
    [DefaultExecutionOrder(2)]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        #region Header ASSET REFERENCES
        [Space(10)]
        [Header("ASSET REFERENCES")]
        #endregion
        [SerializeField] private InputReader inputReader;
        [SerializeField] private AudioClip clickSound;
        [SerializeField] private AudioClip helpSound;

        // ============ MANAGERS ============
        private PlayerController Player { get; set; }
        public LevelManager LevelManager { get; private set; }
        private SaveManager SaveManager { get; set; }
        public SoundManager SoundManager { get; private set; }
        public PauseMenu PauseMenu { get; private set; }

        
        // ============ GAMEPLAY ============
        private float PointsPerSecond { get; set; }
        private float Points { get; set; }
        private Coroutine _autoIncrementCoroutine;
        
        
        // ============ MENUS ============
        private MainMenu MainMenu { get; set; }
        //public PauseMenu PauseMenu { get; private set; }
        private DeathScreen DeathScreen { get; set; }
        public HUD HUD { get; private set; }
        private bool _isMainMenu = true;
        
        
        // ============ VARIABLES ============
        public bool gameStarted;
        private bool _gamePaused;
        private bool _playerInactive;
        private bool _deathScreenActive;
        private bool _helpOpen;
        
        // ============ UNITY METHODS ============
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.Log("Instance already exists, destroying object!");
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            SaveManager = FindObjectOfType<SaveManager>(true);
            MainMenu = FindObjectOfType<MainMenu>(true);
            SaveManager.LoadData();
            MainMenu.SetHighScore(SaveManager.GetHighScore());
            
            HUD = FindObjectOfType<HUD>(true);
            //PauseMenu = FindObjectOfType<PauseMenu>(true);
            DeathScreen = FindObjectOfType<DeathScreen>(true);
            SoundManager = GetComponent<SoundManager>();
            PauseMenu = FindObjectOfType<PauseMenu>(true);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            inputReader.PauseEvent += HandlePause;
            inputReader.CloseUIEvent += HandleEscape;
            inputReader.StartGameEvent += StartGame;
            inputReader.HelpEvent += OpenHelpScreen;
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            inputReader.PauseEvent -= HandlePause;
            inputReader.CloseUIEvent -= HandleEscape;
            inputReader.StartGameEvent -= StartGame;
            inputReader.HelpEvent += OpenHelpScreen;
        }

        private void OpenHelpScreen()
        {
            if (!_isMainMenu) return;
            
            SoundManager.Play(helpSound, 0.8f);
            if (_helpOpen)
            {
                MainMenu.CloseHelp();
                _helpOpen = false;
            }
            else
            {
                MainMenu.OpenHelp();
                _helpOpen = true;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            inputReader.SetUI();
            //PauseMenu = FindObjectOfType<PauseMenu>(true);
            Player = FindObjectOfType<PlayerController>(true);
            LevelManager = FindObjectOfType<LevelManager>(true);
        }
        
        // ============ PUBLIC METHODS ============

        private void StartGame()
        {
            if (_deathScreenActive)
            {
                gameStarted = false;
                _deathScreenActive = false;
                DeathScreen.HideDeathScreen();
                LevelManager.Reset();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                SoundManager.Play(clickSound, 0.3f);
            }
            else if (gameStarted && _playerInactive)
            {
                Continue();
            }
            else if(!gameStarted)
            {
                gameStarted = true;
                AutoIncrementScore(false);
                LevelManager.StartGame();
                ResetInput();
                Player.StartGame();
                MainMenu.StartGame();
                _isMainMenu = false;
                HUD.ShowHud();
                SoundManager.Play(clickSound, 0.3f);
                
                var spawners = FindObjectsOfType<ObjectSpawner>();
                foreach (var spawner in spawners)
                {
                    spawner.startTime = Time.time;
                }
            }
            else if(_gamePaused)
            {
                PauseMenu.ResumeGame();
                HUD.ShowHud();
                AutoIncrementScore(false);
                LevelManager.ResumeGame();
                Time.timeScale = 1;
                _gamePaused = false;
                ResetInput();
            }
        }

        private void HandlePause()
        {
            AutoIncrementScore(true);
            LevelManager.PauseGame();
            Time.timeScale = 0;
            _gamePaused = true;
            DisableInput();
            PauseMenu.PauseGame();
            HUD.HideHud();
        }
        
        private void HandleEscape()
        {
            if(_helpOpen && _isMainMenu)
            {
                MainMenu.CloseHelp();
                _helpOpen = false;
                SoundManager.Play(helpSound, 0.8f);
            }
            else if (_isMainMenu || _deathScreenActive || _gamePaused)
            {
                MainMenu.QuitGame();
            }
        }
        
        public void LooseLife()
        {
            if(_playerInactive) return;
            DisableInput();
            _playerInactive = true;
            AutoIncrementScore(true);
            Player.LooseLife();
            LevelManager.PauseGame();
        }

        private void Continue()
        {
            _playerInactive = false;
            AutoIncrementScore(false);
            Player.Continue();
            ResetInput();
            LevelManager.ResumeGame();
        }
        
        public void GameOver()
        {
            if(SaveManager.GetHighScore() < Points) SaveManager.SaveData((int)Points);
            DisableInput();
            AutoIncrementScore(true);
            LevelManager.PauseGame();
            StartCoroutine(ShowGameOver());
        }

        private IEnumerator ShowGameOver()
        {
            yield return new WaitForSeconds(0.8f);
            HUD.HideHud();
            DeathScreen.ShowDeathScreen((int)Points);
            _deathScreenActive = true;
        }

        private void ResetInput()
        {
            inputReader.SetGameplay();
        }
        
        private void DisableInput()
        {
            inputReader.SetUI();
        }

        // ============ SCORE ============
        
        private void AutoIncrementScore(bool stop)
        {
            if(stop)
            {
                if(_autoIncrementCoroutine != null) StopCoroutine(_autoIncrementCoroutine);
            }
            else
            {
                _autoIncrementCoroutine = StartCoroutine(IncrementScore());
            }
        }
        
        private IEnumerator IncrementScore()
        {
            AddPoints(PointsPerSecond / 100);
            HUD.UpdateScore((int)Points);
            yield return new WaitForSeconds(0.01f);

            _autoIncrementCoroutine = StartCoroutine(IncrementScore());
        }
        
        public void SetPointsPerSecond(float newPointsPerSecond)
        {
            PointsPerSecond = newPointsPerSecond;
        }

        public void AddPoints(float pointsToAdd)
        {
            Points += pointsToAdd;
        }
    }
}