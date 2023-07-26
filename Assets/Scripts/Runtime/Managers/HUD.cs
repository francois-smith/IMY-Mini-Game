/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Managers
{
    public class HUD : MonoBehaviour
    {
        private Label _score;

        private VisualElement _life1;
        private VisualElement _life2;
        private VisualElement _life3;
        
        private VisualElement _hudWindow;
        private UIDocument _uiDocument;

        private void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            var rootVisualElement = _uiDocument.rootVisualElement;
            _hudWindow = rootVisualElement.Q<VisualElement>("HUD-content");

            // Main Menu Items
            _score = rootVisualElement.Q<Label>("score");
            
            _life1 = rootVisualElement.Q<VisualElement>("heart-1");
            _life2 = rootVisualElement.Q<VisualElement>("heart-2");
            _life3 = rootVisualElement.Q<VisualElement>("heart-3");
        }

        public void ShowHud()
        {
            _hudWindow.style.display = DisplayStyle.Flex;
        }

        public void HideHud()
        {
            _hudWindow.style.display = DisplayStyle.None;
        }
        
        public void UpdateScore(int score)
        {
            _score.text = score.ToString();
        }

        public void UpdateLife(int lives)
        {
            switch(lives)
            {
                case 3:
                    _life1.style.display = DisplayStyle.Flex;
                    _life2.style.display = DisplayStyle.Flex;
                    _life3.style.display = DisplayStyle.Flex;
                    break;
                case 2:
                    _life1.style.display = DisplayStyle.Flex;
                    _life2.style.display = DisplayStyle.Flex;
                    _life3.style.display = DisplayStyle.None;
                    break;
                case 1:
                    _life1.style.display = DisplayStyle.Flex;
                    _life2.style.display = DisplayStyle.None;
                    _life3.style.display = DisplayStyle.None;
                    break;
                case 0:
                    _life1.style.display = DisplayStyle.None;
                    _life2.style.display = DisplayStyle.None;
                    _life3.style.display = DisplayStyle.None;
                    break;
            }
        }
        

        public static void QuitGame()
        {
            Application.Quit();
        }
    }
}