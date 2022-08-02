# Better Unity
A collection of modifications/additions (scripts/prefabs) which enhance [for me] the overall flow of using the Unity Engine.

## Purpose
There are some key features pan other game engines that seem to be lacking in Unity and this is a package to resolve those problems.
<br>

## Installation
You can download the latest ```.unitypackage``` through [Releases](https://github.com/sudotman/BetterUnity/releases/) and double-click to import it.


# Contents

## 1. Inspector Additions

### 1.1 Inspector Button / Call Function in Editor

Allows you to call/execute functions in your script from your inspector.

You can expose normally to the inspector in Unity and it works great. Sometimes, you would want to have a button set to call a function of your liking from the inspector and I tend to use workarounds (such as, setting a boolean and then calling a function after checking the boolean from Update) or creating your custom inspector and having a button there. 

This is a much more easier and simpler addition to your existing scripts and can be called using a simple attribute.

Usage:
```C#
[CallInEditor]
void HelloGitFunction(){
    ....
```

!["1.1 Demo - Call in Editor"](Demo/2_2_2_HelloGit.png)


You may want finer control over your button that appears in the inspector in the terms of the size and the label. In that case, you can use the alternative attribute field described below.

Usage:
```C#
[InspectorButton("funcToBeCalled")]
public char myButton;
```

The above snippet will create a button named My Button and will call <i>`funcToBeCalled`</i> function from inside your script when pressed. The public character can be any variable type, I prefer `char` for saving miniscule memory.

!["1.1 Demo - Inpsector Button"](Demo/2_2_Button.gif)

You can also specify the size of the button (by default, it will try to scale your button size to your text size):
```C#
[InspectorButton("funcToBeCalled",100)]
public char myButton;
```

!["1.1 Demo - Inpsector Button"](Demo/2_2_1_Button.png)

You can also specify a custom text for your button (by default, it takes button name from the variable name below it [the aforementioned variable will still be required in the script regardless of if we use this]):
```C#
[InspectorButton("funcToBeCalled",100,"Custom Name Here!")]
public char myButton;
```

For dynamic size:
```C#
[InspectorButton("funcToBeCalled","Custom Name Here!")]
public char myButton;
```

*The extra step to create a character and invoke the function through specifying the name is because of trying to workaround OnGUI/OnInspectorGUI; a better solution definitely exists and I will explore it at a later time.
<br>
<br>

### 1.2 Inspector Fields
Various inspector fields which can be used to output text to the Inspector without having to write a custom editor GUI for simple text labels. [a better solution involving attributes without need for a variable should exist, will look into it later]

- InspectorText: Outputs normal text.
- InspectorFocusText: Outputs text with focus, with a rectangle around it.
- NullCheck: Forces a field to not be null, and if a reference isn't assigned to it, it will be highlighted red.

Usage:
```C#
[InspectorText("This is a normal text")]
public char normalText;

[InspectorFocusText("This is a text with focus")]
public char focusText;

[NullCheck]
public Transform myField;
```

!["1.2 Demo - Inpsector Button"](Demo/2_3_InspectorFields.png)

<br>

Output of the aforementioned tools all together:<br>
!["1.2 Demo - Full"](Demo/2_2_2_FullBtn.png)

### 1.3 Better Rename
A lot of the times when developing, you have similar children which you want to be named incrementally with your desired prefix. Instead of manually going in and doing it, this module allows me to do more it quickly and effeciently, by attaching a script to the parent.

Further, when duplicating objects and creating new ones, Unity appends (x) [x being the current duplicate] and sometimes you would want the objects to be named differently. All new objects will also be renamed appropriately, automatically.


!["1.3 Demo - Better Rename"](Demo/2_1_Rename.gif)

<br>

### 1.4 Better Scale
Allows you to always have uniform scaling on any GameObject. Works similar to the BetterTransform component but given incase you dont wish to override default transform and just want the uniform scaling on one object.

<br>

## 2. Helpers
A collection of extension functions which can be called in other scripts for ease-of-use.

### 2.1 Scale Range
Scale a value from a previous range to a new range (remap it linearly).

```C#
myFloatValue.ScaleRange(0,100,10,30);
```

<br>

### 2.2 Destory Children
Destroy all children of a parent.

```C#
transform.DestroyChildren();
```

When calling the function from the editor:
```C#
transform.DestroyChildrenEditor();
```

<br>

### 2.3 Minus/Plus Float Vector 3
Subtract one float value from x, y, z components of a Vector3.

```C#
myVector3.MinusFloatVector3(2.5f);
```

Add one float value to x, y, z components of a Vector3.

```C#
myVector3.PlusFloatVector3(4.2f);
```

<br>

### 2.4 LookAtY
Look at but rotate only across the Y axis (say, for an enemy that needs to turn to you at all times).

```C#
transform.LookAtY(player.position);
```

<br>

### 2.5 Grounded/Distance From Ground
Returns distance between two vectors but its grounded. (ignoring their Y-position during calculation)
```C#
player.position.DistanceFromGround(objective.position);
```

<br>

### 2.6 Return a random item from a list
Get a random item from a list
```C#
currentItem = myList.RandomItem();
```

<br>

### 2.7 Ease In Interpolation
Performs a Ease-In Interpolation on our specified Vector3.
```C#
myVector3.EaseInLerp(myFinalVector3,currentTime);
```
<br>

### 2.8 Ease Out Interpolation
Performs a Ease-Out Interpolation on our specified Vector3.
```C#
myVector3.EaseOutLerp(myFinalVector3,currentTime);
```
<br>

### 2.9 Smooth Step Float Interpolation
Returns a smooth stepping interpolation between two floats.
```C#
HelperFunctions.SmoothStep(currentTime,startFloat,EndFloat);
```

<br>

## 3. Toolbar Tools

Tools to do various functions, called by menus.

### 3.1 Rename Suite
Works similar to BetterRename except its for remaining them only once. [We can attach the script, do the rename, and then remove it if we would want but this has a much cleaner UX]. Further, it allows you to rename any random [from anywhere to anywhere] selection at any place in the heirarchy.

!["3.1 Demo - Rename"](Demo/6_1_Rename.gif)

### 3.2 Setup Default Project
A one click button that set-ups directories in our project folder in a standardish way.

!["3.2 Demo - Default"](Demo/6_2_Default.gif)

## 4. BetterContext
A collection of additions to various context menus throughout Unity.

### 4.1 Solve Import
Right click on any GameObject in the heirarchy to "Resolve Import Issues". A lot of times when importing 3D models, we tend to get empty objects, cameras, lights etc [usually dependent on the way the modeller exports them] and hence this button gets rid of them in one-go.

!["4.1 Demo - Solve Import"](Demo/7_1_SolveImport.gif)

### 4.2 Other Contexts
- Rigidbody: I find myself wanting to freeze X, Z rotation and turn interpolation on and also make sure it's not kinematic. Right click and pressing this button does it all for me.

- AudioSource: One key feature missing from inspecting an AudioSource is the ability to play, pause and stop it at will. Right click and pressing this does the aforementioned.

!["4.2 - AudioSource"](Demo/7_2_AudioSource.gif)


## 5. Debug Tools

### 5.1 Debug Log to Screen:
Unity has a great Debug system but lacks in outputting it while full-screened/inside VR and hence this is an Unreal-Inpsired simple way to display all Debug Log messages on the screen. All messages fade away after a while and also start replacing old logs if too many are outputted quickly.

!["3.1 Demo - Debug To Screen"](Demo/3_1_ToScreen.gif)

<br>

### 5.2 Simple FPS Counter
A very simple skeletal frames-per-second counter. Inverses unscaled delta time to output fps on the screen.

!["3.2 Demo - FPS Counter"](Demo/3_2_FPS.png)

<br>

## 6. Better Transform
An extension of the existing Transform component to include some ease-to-use features.


!["6 Demo - Better Transform Overview"](Demo/1_BetterTransformOverview.png)

### 6.1 Lock Scale Ratio (Uniform Scale):
Maintain the ratio of the scaling in an object's axes when scaling a GameObject up/down.

!["6.1 Demo - Lock Scale"](Demo/1_1_Scale.gif)

### 6.2 Reset Position, Rotation, Scale:
Reset the position, rotation and scale of any GameObject with a click instead of resetting the entire transform or manually inputting `0,0,0 / 1,1,1`.

!["6.2 Demo - Reset"](Demo/1_2_Reset.gif)

### 6.3 Visibility Toggle:
Sometimes you would want to only disable the Mesh Renderers and not Disable the object so that the scripts/components on them still execute. This disables all the Mesh Renderers in this GameObject and all the children renderers.

!["6.3 Demo - Visibility"](Demo/1_3_Visibility.gif)

### 6.4 Make Unit Scale Parent:
For a lot of use-cases in Unity [wanting to have a unit scale or have positioning, rotation start from zero], we tend to create new objects and then assign it as a parent. This button does that.

!["6.4 Demo - Unit Scale"](Demo/1_4_Unit.gif)

<br>



## 7. VR Scripts
Useful scripts that aid in my VR development.

### 7.1 Head Level
Make an object always align with our headset's level.

!["7.1 Demo - HeadLevel"](Demo/5_1_Head.gif)


## 8. Miscellaneous

### 8.1 Controllers
Various basic controller templates, such as FPS, VR.

### 8.2 Navigation System
A simple walking navigation system for NPCs with appropriate turning/path following.

Group all points into one parent and drop the parent into the points prefab and configure everything accordingly. You can keep the entire points children messy and maybe use BetterRename to organize them (wink, wink).

!["8.2 - Navigation System"](Demo/8_2_Navigation.gif)

<br>

# Contribution
Generate a pull request for whatever change you feel is necessary and I will be happy to review and add them.

## Current to-do:
- ~~Update RenameSuite's gif.~~
- Add more controllers templates.
- Add more VR utiltiy scripts.
- Store GIFs on the cloud.
- Organize and document Helper Functions (in categories and with more information and preferably screenshots)
- A better way of installation - possibly with a package through our Git url and inbuilt Unity's package manager.
- Fix README's alt text naming.

### Navigation To-Do:
- ~~Multiple lanes/paths~~
- ~~Create a parent script to automatically configure animation states and attach script to all children gameobjects and also populate if empty objects positions.~~
- ~~Use mixamo characters and give selection for different states such as walking, running etc and have a unit scale for speed/animation sync and procedurally modify walking running speeds.~~
- ~~Continue walking from where the start location as opposed to moving them to the preferred location~~
- ~~Give options to control animation states from the inspector.~~
- Create an accompanying editor script so that the Inspector isnt as confusing as it is right now.
- Add a more robust documentation/screenshots accompanying the parent script/individual scripts and the working of it.



