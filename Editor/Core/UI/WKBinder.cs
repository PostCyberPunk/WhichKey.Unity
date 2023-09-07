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
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = WhichKeyManager.mUILoader.WkBinder.Instantiate(property.propertyPath);
            var be = root.Q<VisualElement>() as BindableElement;
            // be.BindProperty(property);
            var btn = root.Q<Button>("Bind");
            btn.clickable = new Clickable(() =>
            {
                BindingWindow.ShowWindow((int[] ks) =>
                {
                    WkKeySeq wkKey = ks;
                    var array = property.FindPropertyRelative("_keySeq");
                    array.arraySize = wkKey.KeySeq.Length;
                    for (int i = 0; i < wkKey.KeySeq.Length; i++)
                    {
                        array.GetArrayElementAtIndex(i).intValue = wkKey.KeySeq[i];
                    }
                    property.FindPropertyRelative("_keyLabel").stringValue = wkKey.KeyLabel;
                    property.serializedObject.ApplyModifiedProperties();
                }, -1, "WhichKey Binding");
            });
            return root;
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
