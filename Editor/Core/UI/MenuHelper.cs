using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class MenuHelper : EditorWindow
{
    //Generate Menu Tree
    private static MenuItemNode menuTree;
    private class MenuItemNode
    {
        public string Name;
        public List<MenuItemNode> Children = new List<MenuItemNode>();
        public MenuItemNode(string name)
        {
            Name = name;
        }
    }
    private static List<string> GetMenuItems()
    {
        var list = new List<string>();
        var mlist = TypeCache.GetMethodsWithAttribute<MenuItem>();
        foreach (var item in mlist)
        {
            var attribute = (MenuItem)item.GetCustomAttributes(typeof(MenuItem), false)[0];
            list.Add(attribute.menuItem);
        }
        return list;
    }
    private static List<string> GetMenuItemsReflection()
    {
        Assembly unityEditorAssembly = Assembly.GetAssembly(typeof(MenuItem));
        Type menuItemType = typeof(MenuItem);
        var list = new List<string>();
        foreach (Type type in unityEditorAssembly.GetTypes())
        {
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                MenuItem[] menuItems = (MenuItem[])method.GetCustomAttributes(menuItemType, false);
                if (menuItems.Length > 0)
                {
                    foreach (MenuItem menuItem in menuItems)
                    {
                        list.Add(menuItem.menuItem);
                    }
                }
            }
        }
        return list;
    }
    private static void GenerateMenuTree()
    {

        var slist = GetMenuItems();
        slist.Sort();
        //generate tree
        menuTree = new MenuItemNode("");
        foreach (var item in slist)
        {
            var cItem = Regex.Replace(item, @"\s*[%#_].*", "");
            var node = menuTree;
            var path = cItem.Split('/');
            foreach (var p in path)
            {
                var child = node.Children.Find(n => n.Name == p);
                if (child == null)
                {
                    child = new MenuItemNode(p);
                    node.Children.Add(child);
                }
                node = child;
            }
        }
    }

    private static MenuHelper window;
    private static Action<string> callback;
    private string result;
    private MenuItemNode currentNode;
    private ListView btnList;
    public static void ShowWindow(Action<string> callback)
    {
        if (menuTree == null)
            GenerateMenuTree();
        if (window == null)
        {
            window = CreateInstance<MenuHelper>();
            window.titleContent = new GUIContent("Menu Helper");
        }
        MenuHelper.callback = callback;
        window.result = "";
        window.currentNode = menuTree;
        window.ShowUtility();
    }
    private void CreateGUI()
    {
        btnList = new ListView();
        btnList.makeItem = MakeItem;
        btnList.bindItem = (e, i) =>
        {
            var btn = (Button)e;
            btn.text = currentNode.Children[i].Name;
        };
        btnList.itemsSource = currentNode.Children;
        btnList.itemsSourceChanged += () =>
        {
            btnList.Rebuild();
        };
        
        rootVisualElement.Add(btnList);
    }
    private VisualElement MakeItem()
    {
        var btn = new Button();
        btn.clickable.clicked += () =>
        {
            result += "/" + btn.text;
            currentNode = currentNode.Children.Find(n => n.Name == btn.text);
            if (currentNode == null)
            {
                Debug.LogError("currentNode is null");
                Close();
                return;
            }
            if (currentNode.Children.Count > 0)
            {
                btnList.itemsSource = currentNode.Children;
                window.Repaint();
            }

            else
            {
                callback(result);
                Close();
            }
        };
        return btn;
    }
    [MenuItem("Tools/MenuHelper")]
    public static void TestMenuHelper()
    {
        ShowWindow((s) => Debug.Log(s));
    }
}