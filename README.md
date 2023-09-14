# ⚡ WhichKey.Unity
vim-like key sequence shortcut manager for unity inspired by whichkey.nvim.**faster** and **better** way to control Unity
## ❓What is WhichKey and why
### 1. More shortcuts and esay to remember<br>
Instead of using modifiers key combination,WhichKey accepts key sequence follow by ``Active``,for example you assign ``Space`` as your active key:
- Unity : **Move To view** ``ctrl``+``alt``+``f`` **Align with view** ``ctrl``+``shift``+``f``
- WhichKey: **Move to View** ``space`` ``v`` ``m``  , **Align with View** ``space`` ``v`` ``a``

The key sequence is way more clear,and you dont have to press them at same time. WhichKey does **NOT** conflict with any vanilla unity shortcut.

### 2. Hint window
There wil be lots of shortcuts you may only use a few times,so its hard to remember these ones,thats why we have a **Cheatsheet** hint window,you can wait like hal a second(configuable),then it will show up and tell you what to do next
![Alt text](/Images/HintWIn.png)
### 3. Less switches bewtten mouse and keyboard
For a vim user like me,the most annoying thing is moving hand back and forth between keyboard and mouse.With WhichKey,you can set all shortcut by one hand,and some 2-handed shortcuts when swtich between IDE and Unity.It's not only time-saving but also more natrual!

<!-- ### 4. Workflow -->
 <!-- Theoretically speaking,you can have infinite shotrcuts with WhichKey,who doesnt love more shortcuts?Its easy to write some editor scripts,but bulit-shortcut is never enough,so its time to stop moving you mouse around,navigation through multi-level menu,lets build a better workflow by using WhichKey,give it a week to take in,you will find the magic. -->

## 📦Installation

Whichkey is not stable yet,OpenUPM recommand


## ⚙️Configuration
You **MUST** assign a Unity built-in shortcut for WhichKey/Active first

>🚀There is a quick start Example in WhichKey/Extra/Load Quickstart Example

Open preferences window and Selcect WhichKey tab,add keybinding in Mapping section

### ⌨️KeyBinding
You can bind key sequence by click the **Bind** button,other than ``shift``+ (a-z) (0-1) ,no modifier key support for now,
![Alt text](/Images/Bindwin.png)
### Types:
- Layer

	Treat it like a folder,for exmaple,you have some gameobject related shortcuts follow by ``g``,you can add a layer ``g`` and set hints to "Gameobject"
	> you can also add layers in the Layer section,

- Menu

  Its a wrapper for ``EditorApplication.ExecuteMenuItem``, you can set menuItemPath in Argument field,
  >The Menu listview has a helper button,but **not all** item is listed,some built-in menutiem is missing

  >the path is case-sensitive and dont forget ``...``

	>No unity localizaiton package surpport ,poth has to be all english

- Method

  If you dont want have too many MenuItem,WhichKey provided an attribute,put the id (in this case ``101``) in Argument field
```cs
using PCP.WhichKey;
...
[WhichKeyMethod(101)]
public static void WKHelloWorld()
{
	Debug.Log("Whickey:Hi");
}
```

### Extra
>Consdier this part as exmaple for API,no further support provided.
- AssetNav
	
	Create an ``AssetNavData`` ScriptableObject,then assign it at projectsettings whichkey asset navigation

	make a keybind like this ,argument means the index of projectSettings

![Alt text](/Images/navset.jpg)
	you can bind like this,use ``f`` ``A``  ``[key]`` to add bookmark,then ``f`` ``a`` ``[key]`` to locate the bookmark,you can change hint from the scriptableobject.

- SceneNav  
	Scene gameobject version of AssetNav,but instead of scriptable object,we use a window ( WhichKey/Extra/Scene Nav Window )


## 🔥Custom comand and handler
Check Extra and Wiki


## 🎨 Theme
add these two files in your asset folder,open ``ThemePreview`` and edit the uss.
the assign the `Theme.uss` to custom USS in WhichKey ProjectSettings 
<details>
<summary>Theme.uss</summary>

```css
.main {
    background-color: rgb(56, 56, 56);
    border-left-color: rgb(47, 47, 47);
    border-right-color: rgb(47, 47, 47);
    border-top-color: rgb(47, 47, 47);
    border-bottom-color: rgb(47, 47, 47);
    border-left-width: 5px;
    border-right-width: 5px;
    border-top-width: 5px;
    border-bottom-width: 5px;
    flex-direction: column;
    padding-left: 15px;
    padding-right: 15px;
    padding-top: 15px;
    padding-bottom: 15px;
    justify-content: center;
    align-items: center;
}

.frame {
    flex-grow: 1;
    background-color: rgba(0, 0, 0, 0);
    flex-direction: row;
    padding-top: 0;
    padding-bottom: 0;
    border-left-color: rgb(47, 47, 47);
    border-right-color: rgb(47, 47, 47);
    border-top-color: rgb(47, 47, 47);
    border-bottom-color: rgb(47, 47, 47);
    border-left-width: 2px;
    border-right-width: 2px;
    border-top-width: 2px;
    border-bottom-width: 2px;
    align-items: center;
    padding-left: 5px;
}

.key {
    font-size: 20px;
    color: rgb(210, 210, 210);
    width: auto;
    background-color: rgb(42, 42, 42);
    -unity-text-align: middle-center;
    border-left-color: rgb(91, 91, 91);
    border-right-color: rgb(91, 91, 91);
    border-top-color: rgb(91, 91, 91);
    border-bottom-color: rgb(91, 91, 91);
    border-left-width: 3px;
    border-right-width: 3px;
    border-top-width: 3px;
    border-bottom-width: 3px;
    border-top-left-radius: 5px;
    border-bottom-left-radius: 5px;
    border-top-right-radius: 5px;
    border-bottom-right-radius: 5px;
    min-width: 30px;
}

.hint {
    font-size: 20px;
    margin-left: 10px;
    color: rgb(166, 173, 200);
    white-space: nowrap;
}

.title {
    font-size: 20px;
    color: rgb(166, 173, 200);
    white-space: nowrap;
    align-self: center;
    -unity-font-style: bold;
    -unity-text-align: upper-center;
    align-items: auto;
    margin-bottom: 10px;
}
```

</details>

<details>
<summary>ThemePreview.uxml</summary>

```xml
<ui:UXML xmlns:ui="UnityEngine.UIElements" xmlns:uie="UnityEditor.UIElements" xsi="http://www.w3.org/2001/XMLSchema-instance" engine="UnityEngine.UIElements" editor="UnityEditor.UIElements" noNamespaceSchemaLocation="../../UIElementsSchema/UIElements.xsd" editor-extension-mode="True">
    <Style src="project://database/Assets/Default/Theme.uss?fileID=7433441132597879392&amp;guid=a9ebbc19d7d87044f9a8356a5dd9f474&amp;type=3#Theme" />
    <ui:VisualElement name="Main" class="main">
        <ui:Label tabindex="-1" text="Label" display-tooltip-when-elided="true" name="Title" class="title" />
        <ui:VisualElement name="Frame" class="frame">
            <ui:Label tabindex="-1" text="A" display-tooltip-when-elided="true" name="Key" class="key" />
            <ui:Label tabindex="-1" text="Hello this is a test Hint" display-tooltip-when-elided="true" name="Hint" class="hint" />
        </ui:VisualElement>
    </ui:VisualElement>
</ui:UXML>

```

</details>

## Compatibility
Tested in 2023.1 and 2022.3
