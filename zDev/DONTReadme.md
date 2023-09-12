# Todo
## u
- think about sceneview
<!-- - rename handler ,its not clear now -->
<!-- 1. basewin shouldnt inherit from editor win,bas encapslution -->
<!-- 2. instead of using iwkwinmodifier,maybe just use changeHandlerCmd? -->
3. how to make an inteface get acess to window, so user can close win mannully:INeedWindow
## meh
<!-- - uiload -->
menuHelper add missing menu item manually
<!-- button to uxml -->
<!-- bindWin button to top -->
1. maybe try serialize reference instead of path
2. depth
- !! dont be lazy !!make an unit test!!
## t
- !!hints pooling has no point if use popup window,and listview of listview is not cool
- cutorm theme /has to be done after hints polling?
- basewindow to interface: maybe just hints window to interface
 - inherit from maain hints win
- Arg to a class that can be setup with a window or inline:could be done using factory
1. caching labels reuse of hint window
<!-- - cmd factory to simple cmd **lets check IL for sure thats make sense** didt found a way to use struct,so cmd facoty is better when impl Wkarg somday -->
**why pref sitll stuck on load**
*** pass userdata to wkbidng unboxing ***
4. maybe wkint is not a bad idea.its bad for memory
1. save tree in scriptableObject? 
2. differ lib to  quick save:acceptalbe refreshing speed now
old

<!-- 2. when lost focus of whichkey window ,unity will lost focus too. -->
<!-- 3. check for duplicated key. -->
<!-- 4. change arg to string[] -->
<!-- 5. check keycode length to exclude unwanted keys -->
<!-- 6. upper case key -->
<!-- 8. layer refector -->
<!-- 9. show hint -->
<!-- 12. setting followmosue -->
<!-- 15. overRideshowHints -->
<!-- 16. Refesh? -->
<!-- 17.window data to static, init data and calculate lineheight, invoke by whichkey, -->
<!-- 12. set set Hint Window Size Correctly -->
<!-- 13. setting  -->
<!-- 14. space -->
<!-- 10. change root -->
<!-- 7. Sep settings and manager? do i really need it?Yes!! -->
<!-- 1.wrapper class for setting and preference -->
<!-- 2.LoadSetting -->

<!-- 1. mkhdl complete to reset; -->
<!-- 1. refactor wk manger -->
<!-- 1. rewite list get -->
<!-- 1. try fonts; -->
<!-- GetHints -->
<!-- UI -->
<!-- 4. defualt value interface -->
<!-- setting  -->
<!-- dropdown size -->
<!-- attributes to ignre factory -->
<!-- bind window -->
<!-- change root -->
<!-- keyset use wkkey -->
<!-- keyset to struct -->

<!-- asset using keystruct -->


<!-- make bind label a template -->
<!-- method attribute static with no arg -->
<!-- method command -->
<!-- - scene -->
  <!-- - scene use active transform -->
  <!-- - scene object and asset object -->
  <!-- - maybe use one dataholder -->


<!-- 1. benchmarking cached window -->
<!-- 2. keynode encapsulate and clear after init -->
<!-- 2. assets auto focus on project view -->
<!-- 2. project settings (test Array) -->
<!-- 2. folder manager -->
<!-- 3. scene manager -->
<!-- 14. static format layer hints -->
<!-- 13. follow mouse on change -->
<!-- 11. better way to find duplicated key=> bind win serach table -->
<!-- 5. abstract the window ,there should be a window ref in manager -->
<!-- 1. window instance ref should get from manger:Assethandler -->
<!-- 2. mk hdl and manager ,hdl as an abstarct base -->
<!-- 2. ?active by keyseq<br> -->
<!-- 5. lineheight -->
<!-- 6. keycode ext to util -->
<!-- *** a wrapper is bad for gc, also may lose some reAference,so is intend to repalce json,so lets just edit yaml *** -->
<!-- *** wk to static class singleton to manager *** -->
<!-- 1. oh i need a project specific keyset also adtional keymap -->
<!-- 2. additional layer map -->
<!-- 3. same layer combination -->
<!-- logger first -->
<!-- 1. wkmanger remake -->
 <!-- - handle keycode and raw key inside manager -->
 <!-- - build tree inside manager -->
  
 <!-- - settings -->
 <!-- - window and input handle, who to who?? -->
<!-- *** hideflag wont work for first time it created fixed, import is ok, only happened when mannually delay*** -->
<!-- Menu title -->
<!-- extra keydepth  -->
<!-- assetNav binder size -->
<!-- extra no item handler? -->
<!-- playerMode Handle -->
<!-- excute and create add try catch -->
# Extra
piemenu
# bug
<!-- key interception failed, key up -->
<!-- not handle shift when binding -->
<!-- assetNavdata so no biding -->
<!-- window doesnot close -->
<!-- ?? prefab is gameobject? -->
# ??
<!-- setting ui stuck why? -->
<!-- keybing use int or keycode to char? -->
<!-- !todo tree -->
<!-- !!!!benchmark  chached 0.01 not cached 0.04 -->
<!-- !!! load :list vs array for reloading? -->
<!-- cmdtype??? -->
<!-- Wkint? -->
<!-- check for list that can switch to array -->
# todo 
<!-- UI Elements cant calculate actual size properly(01245f7a) -->
backward compatibility
<!-- Command mode design pattern -->
<!-- Decouple whichkey to wkmanager and wksetting
maybe go on.. decouple wkmanger to keymanager -->
<!-- show window to delegate so user can customize by them self -->
<!-- a tool that get all menuitem -->
# ?
is typecahce fast enough for bigger project? is there a typecache for specific assemble?
1. if i use normal update how can i get gui event?  fuck lets use eidtorwindow...
<!-- 2. have to think through about layer hints,is sb really good? after sethints the hints can be clear -->
<!-- 3. Maybe Add a setting ,let user choose between sting array and list,it beauty and speed -->
<!-- 4. mkhdl Reset VS Complete? i fogot why i use complete... -->
<!-- wk pref property getter if null create instance? no need.  -->
80e54c
4. init benchmark average 0.01 so its ok
<!-- 5. command arg or cmd factory? command arg or cmd factory? command arg or cmd factory? command arg or cmd factory? -->
