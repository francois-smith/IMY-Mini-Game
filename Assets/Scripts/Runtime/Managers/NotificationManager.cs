/****************************************************************
* Copyright (c) u21649988 Francois Smith
* All rights reserved.
****************************************************************/

using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Managers
{
    public class NotificationManager : MonoBehaviour
    {
        private UIDocument _uiDocument;
        
        //result notification items
        private VisualElement _resultContainer;
        private Label _resultText;
        private VisualElement _resultIcon;
        
        private Coroutine _resultCoroutine;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            var rootVisualElement = _uiDocument.rootVisualElement;

            _resultIcon = rootVisualElement.Q<VisualElement>("status-image");
            _resultText = rootVisualElement.Q<Label>("status-message");
            _resultContainer = rootVisualElement.Q<VisualElement>("report-status");
        }
        
        public void ShowResultNotification(string result, Sprite resultImage)
        {
            _resultText.text = result;
            _resultIcon.style.backgroundImage = new StyleBackground(resultImage);

            if (_resultCoroutine != null)
            {
                StopCoroutine(_resultCoroutine);
                _resultContainer.RemoveFromClassList("result-show");
            }
            
            _resultContainer.AddToClassList("result-show");
            _resultCoroutine = StartCoroutine(HideResultNotification());
        }
        
        private IEnumerator HideResultNotification()
        {
            yield return new WaitForSecondsRealtime(3.5f);
            _resultContainer.RemoveFromClassList("result-show");
        }
    }
}
