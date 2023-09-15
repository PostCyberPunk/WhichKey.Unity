# ⚡ WhichKey.Unity
一种类似Vim的高效舒适Unity快捷键管理方式，受Whichkey.nvim启发。
![showcase-s](https://github.com/PostCyberPunk/WhichKey.Unity/assets/134976996/0bd1dec9-9239-4a88-ae3e-8108680ab0de)
![sv-s](https://github.com/PostCyberPunk/WhichKey.Unity/assets/134976996/20693c41-47ae-4f22-81de-9533fbd27182)
![fa-s](https://github.com/PostCyberPunk/WhichKey.Unity/assets/134976996/a5bd8af1-f52b-42dc-93c3-8012565b46b7)
![ad-s](https://github.com/PostCyberPunk/WhichKey.Unity/assets/134976996/da2ccc49-ee31-4e0b-87a8-c84d06234d2c)



## ❓WhichKey是什么
### 1. 更多更好记的快捷键
WhichKey 使用``激活``加序列键而不使用组合键,for 比如说你使用 ``Space`` 作为激活键:
- Unity : **Move To view** ``ctrl``+``alt``+``f`` **Align with view** ``ctrl``+``shift``+``f``
- WhichKey: **Move to View** ``space`` ``v`` ``m``  , **Align with View** ``space`` ``v`` ``a``

序列键比起组合键更加清晰，并且不用同时按折磨手指。WhichKey和Unity原味快捷键也没有冲突.

### 2. 提示窗口
有很多不常用的快捷键会很容易忘掉，如果实在想不起来后面的快捷键是什么，WhichKey会在一小段时间（可设置）后打卡一个窗口提示你.
![Alt text](/Images/HintWIn.png)
### 3. 减少手的移动
我感觉很多向我一样的vim用户最讨厌的就是手要在鼠标和键盘之间来回切换。因为WhichKey可以提供更多快捷键，你可以将大部分快捷键设置在单手区域来避免切换鼠标键盘。也可以加上一些双手快捷键在IDE和Unity切换时使用

<!-- ### 4. Workflow -->
 <!-- Theoretically speaking,you can have infinite shortcuts with WhichKey,who doesn`t love more shortcuts?Its easy to write some editor scripts,but bulit-shortcut is never enough,so its time to stop moving you mouse around,navigation through multi-level menu,lets build a better workflow by using WhichKey,give it a week to take in,you will find the magic. -->

## 📦安装

WhichKey还在开发状态，可能会有很多更新，建议使用OpenUPM安装
<details>
<summary>OpenUPM</summary>
 
- 打开 ``Edit/Project Settings/Package Manager``
- 添加 Scoped Registry:
  ```
  Name: OpenUPM
  URL:  https://package.openupm.cn/
  Scope(s): com.postcyberpunk.whichkey
  ```
- 点击<kbd>Save</kbd>
- 打开 Package Manager
- 点击左上角的 <kbd>+</kbd>
- 选择 <kbd>Add from Git URL</kbd>
- 输入 ``com.postcyberpunk.whichkey``
- 点击 <kbd>Add</kbd> 
</details>


## ⚙️设置
你需要先用Unity内置快捷键绑定WhichKey/Actie

>🚀你可以通过WhichKey/Extra/Load Quickstart Example来获得一个简单的预设

你可以在Preferences窗口的Whichkey选项卡中的Mapping部分添加快捷键，ProjectSetting中也有同样的选项，

### ⌨️快捷键绑定
点击``Bind``按钮来绑定快捷键，注意WhichKey目前不支持``Ctrl`` ``Alt``等组合键，仅可以使用``shift``与字母数字的组合假案
![Alt text](/Images/Bindwin.png)
### Types:
- Layer

    你可以把他当作目录，比如你有一系列以``g``开头和Gameobject相关的组合键，你就可以添加一个Gamobject的layer作为提示
	Treat it like a folder,for example,you have some GameObject related shortcuts follow by ``g``,you can add a layer ``g`` and set hints to "GameObject"
	> 你也可以在Layer列表中添加layer

- Menu

  作用就是 ``EditorApplication.ExecuteMenuItem``,用来绑定菜单的，Argument里填菜单路径
  >在Menu列表中会有一个辅助按钮，但是一些内置的菜单路径并不能通过反射获得，仍然需要手动添加

  >菜单路径必须使用全英文，区分大小写，有的还会有``...``，总而言之 一字不差才行


- Method
    如果你有很多自制的方法想要绑定，但是不想菜单过于混乱，可以为你的方法添加WhichKeyMethod(id)属性，然后把id填在argument里,
```cs
using PCP.WhichKey;
...
[WhichKeyMethod(101)]
public static void WKHelloWorld()
{
	Debug.Log("WhichKey:Hi");
}
```

### Extra
>该部分作为api示例，不提供过多支持
- AssetNav
	
	创建一个 ``AssetNavData`` ScriptableObject,在ProjectSettings窗口WhichKey/Assets Navigation选项卡中添加他，像图里一样绑定快捷键

![Alt text](/Images/navset.jpg)
现在你可以通过``f`` ``A``  ``[key]`` 为asset添加标记,使用 ``f`` ``a`` ``[key]``来定位Asset,你可以他创建多个AssetNav对象，在argument填入对应的索引值

- SceneNav  
    场景中Gameobject的书签管理，可以通过WhichKey/Extra/Scene Nav Window进行设置


## 🔥自定义 command 和 handler
参照 Extra 或者 [Wiki](https://github.com/PostCyberPunk/WhichKey.Unity/wiki)


## 🎨 主题
在Assets文件夹里添加下面两个文件,在 ``ThemePreview`` 里编辑USS.
在ProjectSettings窗口中的Whichkey选项卡里把`Theme.uss`分配到Custom USS即可
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

## 兼容性
2023.1 和 2022.3都测试过