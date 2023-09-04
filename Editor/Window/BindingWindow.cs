using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UIElements;
using System.ComponentModel;
using System.Linq;

namespace PCP.Tools.WhichKey
{
    public class BindingWindow : EditorWindow
    {
        private static BindingWindow instance;
        private List<int> mKeySeq = new();
        private VisualTreeAsset keyVT { get => WhichKeyManager.mUILoader.KeyLabel; }
        private VisualElement labelFrame;
        private Action<int[]> OnComplete;
        private string Hint;
        public static void ShowWindow(Action<int[]> onComplete, int[] keySeq, string hint = "WhichKey Binding")
        {
            ShowWindow(onComplete, hint);
            instance.mKeySeq = keySeq.ToList();
        }
        public static void ShowWindow(Action<int[]> onComplete, string hint = "WhichKey Binding")
        {
            if (instance == null)
                instance = ScriptableObject.CreateInstance<BindingWindow>();

            Vector2 size = new(200, 105);
            Vector2 center = EditorGUIUtility.GetMainWindowPosition().center;
            instance.position = new Rect(center.x - size.x / 2, center.y - size.y / 2, size.x, size.y);
            instance.mKeySeq.Clear();
            instance.OnComplete = onComplete;
            instance.ShowPopup();
        }
        private void CreateGUI()
        {
            labelFrame = new VisualElement();
            labelFrame.style.flexDirection = FlexDirection.Row;
            labelFrame.style.flexWrap = Wrap.Wrap;
            labelFrame.style.alignItems = Align.Center;
            labelFrame.style.justifyContent = Justify.Center;
            labelFrame.style.height = 80;
            // Button 
            var buttonFrame = new VisualElement();
            buttonFrame.style.flexDirection = FlexDirection.Row;
            buttonFrame.style.justifyContent = Justify.Center;
            Button confirmButton = new Button(() => Bind());
            confirmButton.text = "Confirm";
            Button cancelButton = new Button(() => Cancel());
            cancelButton.text = "Cancel";
            buttonFrame.Add(confirmButton);
            buttonFrame.Add(cancelButton);
            rootVisualElement.Add(new Label(Hint));
            rootVisualElement.Add(labelFrame);
            rootVisualElement.Add(buttonFrame);

            rootVisualElement.style.alignContent = Align.Center;
        }

        private void OnGUI()
        {
            var e = Event.current;
            if (e == null) return;
            if (e.isKey && e.type == EventType.KeyDown)
            {
                var keyCode = e.keyCode;
                if (!keyCode.IsValid()) return;
                e.Use();
                switch (e.keyCode)
                {
                    case KeyCode.Escape:
                        Cancel();
                        break;
                    case KeyCode.Return:
                        Bind();
                        break;
                    case KeyCode.Backspace:
                        if (mKeySeq.Count > 0)
                        {
                            mKeySeq.RemoveAt(mKeySeq.Count - 1);
                            labelFrame.RemoveAt(labelFrame.childCount - 1);
                        }
                        break;
                    default:
                        mKeySeq.Add((int)keyCode);
                        var keyLabel = keyVT.CloneTree();
                        keyLabel.Q<Label>().text = keyCode.ToLabel();
                        labelFrame.Add(keyLabel);
                        break;
                }
            }
        }
        private void Bind()
        {
            OnComplete?.Invoke(mKeySeq.ToArray());
            Close();
        }
        private void LostFocus() => Cancel();

        private void Cancel() => Close();
        // private void 
        [MenuItem("WhichKey/Binding Test")]
        public static void Test()
        {
            BindingWindow.ShowWindow((keys) =>
            {
                Debug.Log(keys[1]);
            });
        }
    }
}

