/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Managers
{
    public class PauseMenu : MonoBehaviour
    {
        // // Main Pause Items
        private VisualElement _pauseMenuWindow;
        private UIDocument _uiDocument;

        private VisualElement _floatButton;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            var rootVisualElement = _uiDocument.rootVisualElement;
            _pauseMenuWindow = rootVisualElement.Q<VisualElement>("pause-menu-content");
        }
        
        public void PauseGame()
        {
            _pauseMenuWindow.style.display = DisplayStyle.Flex;
        }
        
        public void ResumeGame()
        {
            _pauseMenuWindow.style.display = DisplayStyle.None;
        }
    }
}