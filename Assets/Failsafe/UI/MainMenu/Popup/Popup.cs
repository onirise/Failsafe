using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Failsafe.UI.MainMenu.Popup
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] public bool startVisible;
        [SerializeField] public string title = "TITLE";
        [TextArea(3, 7)] public string textContent = "";
        [SerializeField] public UnityEvent onSubmit;
        [SerializeField] public UnityEvent onCancel;

        private VisualElement _container;
        private Label _title;
        private Label _text;
        private Button _submitButton;
        private Button _cancelButton;

        private void Awake()
        {
            _container = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Container");
            _container.RegisterCallback<NavigationSubmitEvent>(_ => Submit());
            _container.RegisterCallback<NavigationCancelEvent>(_ => Cancel());

            _title = _container.Q<Label>("Title");
            _title.text = title;

            _text = _container.Q<Label>("Text");
            _text.text = textContent;

            _submitButton = _container.Q<Button>("SubmitButton");
            _submitButton.RegisterCallback<ClickEvent>(_ => Submit());

            _cancelButton = _container.Q<Button>("CancelButton");
            _cancelButton.RegisterCallback<ClickEvent>(_ => Cancel());

            if (startVisible)
                Show();
            else
                Hide();
        }

        public void Show()
        {
            _container.style.display = DisplayStyle.Flex;
            _container.Focus();
        }

        public void Hide()
        {
            _container.style.display = DisplayStyle.None;
        }

        private void Cancel()
        {
            onCancel?.Invoke();
        }
        private void Submit()
        {
            onSubmit.Invoke();
        }
    }
}