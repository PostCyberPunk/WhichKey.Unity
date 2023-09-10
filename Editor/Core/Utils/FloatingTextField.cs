using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PCP.WhichKey
{
    public class FloatingTextField : EditorWindow
    {
        private string mHint;
        private bool shouldFocus;
        private static readonly Vector2 mSize = new(200, 22);
        private TextField textField;
        private System.Action<string> onInputComplete;

        public static void ShowInputField(System.Action<string> onComplete, string hint = "Enter Here:")
        {
            var win = CreateInstance<FloatingTextField>();
            win.mHint = hint;
            win.onInputComplete = onComplete;
            win.shouldFocus = true;
            win.minSize = mSize;
            win.maxSize = mSize;
            win.position = new Rect(0, 0, mSize.x, mSize .y);
            win.ShowPopup();
        }

        private void CreateGUI()
        {
            textField = new TextField(mHint);
            rootVisualElement.Add(textField);
        }
        private void OnGUI()
        {
            if (shouldFocus)
            {
                if (Event.current != null)
                {
                    Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
                    position = new Rect(mousePos.x - mSize.x / 2, mousePos.y - mSize.y / 2, maxSize.x, mSize.y);
                }
                shouldFocus = false;
                textField.Focus();
                textField.RegisterCallback<KeyDownEvent>(e =>
                {
                    if (e.keyCode == KeyCode.Return)
                    {
                        onInputComplete?.Invoke(textField.value);
                        Close();
                    }
                });
            }
        }
        private void OnLostFocus() => Close();
    }

}
