using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;
using System;

namespace PCP.Tools.WhichKey
{
    [CustomPropertyDrawer(typeof(WkKeySeq))]
    public class WkBinder : PropertyDrawer
    {
        private int mDepth = -1;
        private string mTitle = "WhichKey Binding";
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {

            var root = UILoader.instance.WkBinder.Instantiate(property.propertyPath);
            var btn = root.Q<Button>("Bind");
            btn.clickable = new Clickable(() =>
            {
                BindingWindow.ShowWindow((ks) =>
                {
                    if (root.parent.userData != null)
                    {
                        var setting = (WkBinderSetting)root.parent.userData;
                        mDepth = setting.Depth;
                        mTitle = setting.Title;
                    }

                    WkKeySeq wkKey = ks;
                    var array = property.FindPropertyRelative("_keySeq");
                    array.arraySize = wkKey.KeySeq.Length;
                    for (int i = 0; i < wkKey.KeySeq.Length; i++)
                    {
                        array.GetArrayElementAtIndex(i).intValue = wkKey.KeySeq[i];
                    }
                    property.FindPropertyRelative("_keyLabel").stringValue = wkKey.KeyLabel;
                    property.serializedObject.ApplyModifiedProperties();
                }, mDepth, mTitle);
            });
            return root;
        }
    }
    public struct WkBinderSetting
    {
        public int Depth;
        public string Title;
        /// <summary>
        /// Create a which key binder darower for WkKeySeq
        /// </summary>
        /// <param name="depth">Max count for your key sequence</param>
        public WkBinderSetting(int depth)
        {
            Depth = depth;
            Title = "WhichKey Binding";
        }
        /// <summary>
        /// Create a which key binder darower for WkKeySeq
        /// </summary>
        /// <param name="depth">Max count for your key sequence</param>
        /// <param name="title">Tilte for biding window</param>
        public WkBinderSetting(int depth, string title)
        {
            Depth = depth;
            Title = title;
        }
    }
    // public class WkBinder : VisualElement
    // {
    //     public new class UxmlTraits : VisualElement.UxmlTraits { }
    //     public WkBinder(Action action)
    //     {
    //         var root = WhichKeyManager.mUILoader.WkBinder.CloneTree();
    //         var e = root.Q<VisualElement>() as BindableElement;
    //         e.bindingPath = "Keys";
    //         var btn = root.Q<Button>("Bind");
    //         btn.clicked += action;
    //         Add(root);
    //     }
    // }
}
