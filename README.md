#  WhichKey.Unity
vim-like key sequence shortcut manager for unity inspired by whichkey.nvim.faster and better way to control Unity
## What is WhichKey and why
### 1. More shortcuts and esay to remember<br>
Instead of using modifiers key combination,WhichKey accepts key sequence follow by ``Active``,for example you assign ``Space`` as your active key:
- Unity : **Move To view** ``ctrl``+``alt``+``f`` **Align with view** ``ctrl``+``shift``+``f``
- WhichKey: **Move to View** ``space`` ``v`` ``m``  , **Align with View** ``space`` ``v`` ``a``

The key sequence is way more clear,and you dont have to press them at same time,and whichkey does **NOT** conflict with any vanilla unity shortcut.

### 2. Hint window
There wil be lots of shortcut you may only use a few times,so its hard to remember these ones,thats why we have a **Cheatsheet** hint window,you can wait like hal a second(configuable),then it will show up and tell you what to do next

### 3. Less switches bewtten mouse and keyboard
For a vim user like me,the most annoying thing is moving hand back and forth between keyboard and mouse.With WhichKey,you can set all shortcut by one hand,and some 2-handed shortcuts when swtich between IDE and Unity.It's not only time-saving but also more natrual!

### 4. Workflow
 Theoretically speaking,you can have infinite shotrcuts with WhichKey,who doesnt love more shortcuts?Its easy to write some editor scripts,but bulit-shortcut is never enough,so its time to stop moving you mouse around,navigation through multi-level menu,lets build a better workflow by using WhichKey,give it a week to take in,you will find the magic.

## Installation
Whichkey is not stable yet,by using OpenUPM you can get an update notification.so no unitypackage for now.

## Configuration
You **MUST** assign a Unity built-in shortcut for WhichKey/Active first,then open preferences window and Selcect WhichKey tab,
- Timeout means how many seconds before hint window to show up

Lets take a look at key mappings,unfold the **mapping** listview,
### KeyBinding
	you can bind key sequence by click the **Bind** button,other than ``shift``+ (a-z) (0-1) ,no modifier key support for now,
### Types:
- Layer

  you can treat it like a folder,for exmaple,you have some gameobject related shortcuts follow by ``g``,you can add a layer ``g`` and set hints to "Gameobject"
	> you can use the Layer listview if you have lots of layer

- Menu

  Its a wrapper for ``EditorApplication.ExecuteMenuItem``, you can set menuItemPath in Argument field
  >The Menu listview has a helper button,but **not all** menu is listed,

  >the path is case-sensitive and dont forget ``...``

	>No unity localizaiton package surpport ,poth has to be all english

- Method

  In case you dont want have too many MenuItem,WhichKey provided an attribute,put the id (in this case ``101``) in Argument field
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
	
	Create an ``AssetNavData`` scriptalbe object

	then assign it at projectsettings whichkey asset navigation

	make a keybind like this ,argument means the index of projectSettings

	you can bind like this,use ``f`` ``A``  ``[key]`` to add book,then ``f`` ``a`` ``[key]`` to locate the bookmark,you can change hint from the scriptableobject.

- SceneNav  
	Scene gameobject version of AssetNav,but instead of scriptable object,we use a window ( WhichKey/Extra/Scene Nav Window )


## Customization
Check Wiki
