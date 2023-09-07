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
        private VisualTreeAsset winVT { get => WhichKeyManager.mUILoader.BindWindow; }

        private VisualElement labelFrame;
        private Action<int[]> OnComplete;
        private string Title;
        public static void ShowWindow(Action<int[]> onComplete, int[] keySeq, string title = "WhichKey Binding")
        {
            ShowWindow(onComplete, title);
            instance.mKeySeq = keySeq.ToList();
        }
        public static void ShowWindow(Action<int[]> onComplete, string title = "WhichKey Binding")
        {
            if (instance == null)
                instance = ScriptableObject.CreateInstance<BindingWindow>();

            Vector2 size = new(200, 150);
            Vector2 center = EditorGUIUtility.GetMainWindowPosition().center;
            instance.position = new Rect(center.x - size.x / 2, center.y - size.y / 2, size.x, size.y);
            instance.mKeySeq.Clear();
            instance.OnComplete = onComplete;
            instance.Title = title;
            instance.ShowPopup();
        }
        private void CreateGUI()
        {
            var root = winVT.CloneTree();
            root.Q<Label>("Title").text = Title;
            labelFrame = root.Q<VisualElement>("LabelFrame");
            // Button 
            root.Q<Button>("confirm").clicked += Bind;
            root.Q<Button>("cancel").clicked += Cancel;

            rootVisualElement.Add(root);
            // rootVisualElement.style.alignContent = Align.Center;
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
                        var key = keyCode.ToAscii(e.shift);
                        mKeySeq.Add(key);
                        var keyLabel = keyVT.CloneTree();
                        keyLabel.Q<Label>().text = key.ToLabel();
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

