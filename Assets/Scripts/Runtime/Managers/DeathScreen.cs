/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Managers
{
    public class DeathScreen : MonoBehaviour
    {
        private Label _score;
        private VisualElement _deathWindow;
        private UIDocument _uiDocument;

        private VisualElement _floatButton;

        private void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            var rootVisualElement = _uiDocument.rootVisualElement;
            _deathWindow = rootVisualElement.Q<VisualElement>("death-content");
            
            _score = rootVisualElement.Q<Label>("Score");
            
            _floatButton = rootVisualElement.Q<VisualElement>("RestartGameButton");
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

        public void HideDeathScreen()
        {
            _deathWindow.style.display = DisplayStyle.None;
        }
        
        public void ShowDeathScreen(int score)
        {
            _deathWindow.style.display = DisplayStyle.Flex;
            _score.text = score.ToString(CultureInfo.InvariantCulture);
        }
    }
}