using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UIElements;
using PCP.Tools.WhichKey;
using System.Threading.Tasks.Sources;
public class BindingWindow : EditorWindow
{
    private List<int> mKeySeq = new();
    private VisualTreeAsset keyVT { get => WhichKeyManager.mUILoader.KeyLabel; }
    private VisualElement labelFrame;
    private Action<int[]> OnComplete;
    public static void ShowWindow(Action<int[]> onComplete)
    {
        var win = CreateInstance<BindingWindow>();
        //set win postion to cneter of screen
        Vector2 size = new(200, 105);
        // win.position = new Rect(Screen.currentResolution.width / 2 - size.x / 2, Screen.currentResolution.height / 2 - size.y / 2, size.x, size.y);
        Vector2 center = EditorGUIUtility.GetMainWindowPosition().center;
        // win.position = EditorGUIUtility.ScreenToGUIRect(win.position);
        win.position = new Rect(center.x - size.x / 2, center.y - size.y / 2, size.x, size.y);
        win.mKeySeq.Clear();
        win.OnComplete = onComplete;

        win.ShowPopup();
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
