/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Managers
{
    [DefaultExecutionOrder(1)]
    public class MainMenu : MonoBehaviour
    {
        // // Main Pause Items
        private Label _highScore;
        private VisualElement _mainMenuWindow;
        private UIDocument _uiDocument;

        private VisualElement _floatButton;
        private VisualElement _helpMenu;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            var rootVisualElement = _uiDocument.rootVisualElement;
            _mainMenuWindow = rootVisualElement.Q<VisualElement>("main-menu-content");

            // Main Menu Items
            _highScore = rootVisualElement.Q<Label>("HighScore");
            _helpMenu = rootVisualElement.Q<VisualElement>("HelpMenu");
            
            _floatButton = rootVisualElement.Q<VisualElement>("StartGameButton");
            StartCoroutine(FloatButton());
        }

        private IEnumerator FloatButton()
        {
            _floatButton.AddToClassList("float-on");
            yield return new WaitForSeconds(0.55f);
            _floatButton.RemoveFromClassList("float-on");
            yield return new WaitForSeconds(0.55f);

            StartCoroutine(FloatButton());
        }

        public void StartGame()
        {
            _mainMenuWindow.style.display = DisplayStyle.None;
        }

        public void SetHighScore(int score)
        {
            _highScore.text = "HIGH SCORE: "+ score;
        }

        public static void QuitGame()
        {
            Application.Quit();
        }
        
        public void OpenHelp()
        {
            _helpMenu .style.display = DisplayStyle.Flex;
        }

        public void CloseHelp()
        {
            _helpMenu.style.display = DisplayStyle.None;
        }
    }
}