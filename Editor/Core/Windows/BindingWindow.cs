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
        private Stack<int> mKeySeq = new();
        private VisualTreeAsset keyVT { get => WhichKeyManager.mUILoader.KeyLabel; }
        private VisualTreeAsset winVT { get => WhichKeyManager.mUILoader.BindWindow; }

        private VisualElement labelFrame;
        private Action<int[]> OnComplete;
        private string Title;
        private int depth;
        private bool mouseFound = false;
        private Vector2 mSize = new(200, 150);
        public static void ShowWindow(Action<int[]> onComplete, int depth, string title)
        {
            if (instance == null)
                instance = ScriptableObject.CreateInstance<BindingWindow>();

            // Vector2 mSize = new(200, 150);
            // Vector2 center = EditorGUIUtility.GetMainWindowPosition().center;
            // instance.position = new Rect(center.x - mSize.x / 2, center.y - mSize.y / 2, mSize.x, mSize.y);

            instance.mKeySeq.Clear();
            instance.OnComplete = onComplete;
            instance.Title = title;
            instance.depth = depth;
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
            root.Q<Button>("delete").clicked += Delete;

            rootVisualElement.Add(root);
            // rootVisualElement.style.alignContent = Align.Center;
        }

        private void FollowMouse()
        {
            Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            position = new Rect(mousePos.x, mousePos.y, mSize.x, mSize.y);
        }
        private void OnGUI()
        {
            var e = Event.current;
            if (e == null) return;
            if (!mouseFound)
            {
                mouseFound = true;
                FollowMouse();
            }
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
                        Delete();
                        break;
                    default:
                        if (depth > 0 && mKeySeq.Count >= depth) return;
                        var key = keyCode.ToAscii(e.shift);
                        mKeySeq.Push(key);
                        var keyLabel = keyVT.CloneTree();
                        keyLabel.Q<Label>().text = key.ToLabel();
                        labelFrame.Add(keyLabel);
                        break;
                }
            }
        }
        private void Bind()
        {
            OnComplete?.Invoke(mKeySeq.Reverse().ToArray());
            Close();
        }
        private void Delete()
        {
            if (mKeySeq.Count > 0)
            {
                mKeySeq.Pop();
                labelFrame.RemoveAt(labelFrame.childCount - 1);
            }
        }
        private void OnLostFocus() => Close();

        private void Cancel() => Close();
    }
}

