# Better Unity
A collection of modifications/additions (scripts/prefabs) which enhance [for me] the overall flow of using the Unity Engine.

## Purpose
There are some key features pan other game engines that seem to be lacking in Unity and this is a package to resolve those problems.
<br>

## Installation
You can download the latest ```.unitypackage``` through [Releases](https://github.com/sudotman/BetterUnity/releases/) and double-click to import it.

or

You can import it through Unity's package manager using the url: 
```html
https://github.com/sudotman/BetterUnity.git
```

<i>Installation instruction:</i>
!["Installation Demo"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/InstallationGit.gif)

## Contribute to the project
[Click here to see general collaboration information](#contribution)


# Contents / Documentation

Table of Contents:
* [Contents / Documentation](#contents--documentation)
  * [1\. Inspector Additions](#1-inspector-additions)
    * [1\.1 Inspector Button / Call Function in Editor](#11-inspector-button--call-function-in-editor)
    * [1\.2 Inspector Fields](#12-inspector-fields)
    * [1\.3 Better Rename](#13-better-rename)
    * [1\.4 Better Scale](#14-better-scale)
  * [2\. Helpers](#2-helpers)
    * [2\.1 Scale Range](#21-scale-range)
    * [2\.2 Destory Children](#22-destory-children)
    * [2\.3 Minus/Plus Float Vector 3](#23-minusplus-float-vector-3)
    * [2\.4 LookAtY](#24-lookaty)
    * [2\.5 Grounded/Distance From Ground](#25-groundeddistance-from-ground)
    * [2\.6 Return a random item from a list](#26-return-a-random-item-from-a-list)
    * [2\.7 Ease In Interpolation](#27-ease-in-interpolation)
    * [2\.8 Ease Out Interpolation](#28-ease-out-interpolation)
    * [2\.9 Smooth Step Float Interpolation](#29-smooth-step-float-interpolation)
    * [2\.10 Hex To Color](#210-hex-to-color)
    * [2\.11 Serializers](#211-serializers)
    * [2\.12 Better Raycast](#212-betterraycast)
  * [3\. Toolbar Tools](#3-toolbar-tools)
    * [3\.1 Rename Suite](#31-rename-suite)
    * [3\.2 Setup Default Project](#32-setup-default-project)
    * [3\.3 Select All With](#33-select-all-with)
    * [3\.4 Mesh Combine Wizard](#34-mesh-combine-wizard-tool)
  * [4\. BetterContext](#4-bettercontext)
    * [4\.1 Move ATB](#41-move-atb)
    * [4\.2 Solve Import](#42-solve-import)
    * [4\.3 Other Contexts](#43-other-contexts)
  * [5\. Debug Tools](#5-debug-tools)
    * [5\.1 Debug Log to Screen:](#51-debug-log-to-screen)
    * [5\.2 BD:](#52-bd)
    * [5\.3 Simple FPS Counter](#53-simple-fps-counter)
  * [6\. Better Transform](#6-better-transform)
    * [6\.0 Local/Global Switch](#60-localglobal-switch)
    * [6\.1 Lock Scale Ratio (Uniform Scale):](#61-lock-scale-ratio-uniform-scale)
    * [6\.2 Reset Position, Rotation, Scale:](#62-reset-position-rotation-scale)
    * [6\.3 Visibility Toggle:](#63-visibility-toggle)
    * [6\.4 Make Unit Scale Parent:](#64-make-unit-scale-parent)
	* [6\.5 Unreal Styled Camera Bookmark Hotkeys:](#65-unreal-styled-camera-bookmark-hotkeys)
  * [7\. World Settings](#7-world-settings)
    * [7\.1 KillY (KillZ from UE)](#71-killy-killz-from-ue)
  * [8\. VR Scripts](#8-vr-scripts)
    * [8\.1 Head Level](#81-head-level)
  * [9\. Miscellaneous](#9-miscellaneous)
    * [9\.1 Controllers](#91-controllers)
    * [9\.2 Navigation System](#92-navigation-system)
    * [9\.3 Flow Controller](#93-flow-controller)
* [Contribution](#contribution)
  * [Current to\-do:](#current-to-do)
    * [Navigation To\-Do:](#navigation-to-do)

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

!["1.1 Demo - Call in Editor"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/2_2_2_HelloGit.png)


You may want finer control over your button that appears in the inspector in the terms of the size and the label. In that case, you can use the alternative attribute field described below.

Usage:
```C#
[InspectorButton("funcToBeCalled")]
public char myButton;
```

The above snippet will create a button named My Button and will call <i>`funcToBeCalled`</i> function from inside your script when pressed. The public character can be any variable type, I prefer `char` for saving miniscule memory.

!["1.1 Demo - Inpsector Button"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/2_2_Button.gif)

You can also specify the size of the button (by default, it will try to scale your button size to your text size):
```C#
[InspectorButton("funcToBeCalled",100)]
public char myButton;
```

!["1.1 Demo - Inpsector Button"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/2_2_1_Button.png)

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
- InspectorFocusText: Outputs text with focus, with a rectangle around it. [has an overload for aligning it to the left]
- NullCheck: Forces a field to not be null, and if a reference isn't assigned to it, it will be highlighted red.
- Optional: Designates a field as Optional which changes it's appearance accordingly.
- Layer: Allows selection of a single layer as opposed to a layermask.

Usage:
```C#
[Optional]
public int additionalPoints;

// Standard Variable
public float mainPoints;

[InspectorText("This is a normal text")]
public char normalText;

[InspectorFocusText("This is a text with focus")]
public char focusText;

[InspectorFocusText("This is a text with focus aligned to the left",true)]
public char leftAlignedFocusText;

[SerializeField, Layer]
int layer;

[NullCheck]
public Transform myField;
```
*Individual Char variable definition not necessary for Text Fields but I prefer to include it to keep it more organized since the order gets reversed otherwise.

!["1.2 Demo - Inpsector Button"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/2_3_InspectorFields.png)

<br>


Output of the aforementioned tools all together:<br>
!["1.2 Demo - Full"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/2_2_2_FullBtn.png)

### 1.3 Better Rename
A lot of the times when developing, you have similar children which you want to be named incrementally with your desired prefix. Instead of manually going in and doing it, this module allows me to do more it quickly and effeciently, by attaching a script to the parent.

Further, when duplicating objects and creating new ones, Unity appends (x) [x being the current duplicate] and sometimes you would want the objects to be named differently. All new objects will also be renamed appropriately, automatically.


!["1.3 Demo - Better Rename"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/2_1_Rename.gif)

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

### 2.10 Hex To Color
Allows you to pass a Hexadecimal color string and use in the native Unity's RGB color system.
```C#
Color myColor;
myColor = myColor.HexColor("#60c47e");
```
!["2.10 - Hex to Color"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/4_colorConversion.png)

If an incorrect hex string is passed and/or it can't be parsed, a warning is logged while a default black color is returned.

!["2.10 - Hex to Color Error"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/4_colorConversionError1.png)
!["2.10 - Hex to Color Error"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/4_colorConversionError2.png)



There are plenty more functions which are not mentioned here because of their usages being quite insigniicant. Look through [HelperFunctions.cs](https://github.com/sudotman/BetterUnity/blob/main/Runtime/BetterUnity/Helpers/HelperFunctions.cs) for all the remaining functions, they are aptly documented.

<br>

### 2.11 Serializers
Gives you different serializers which allows you to easily pass common Unity data structures into files and vice versa.

```js
SerializedVector3 serializedVector3;
SerializedQuaternion serializedQuaternion;
```

Quick code to get started with conversion of Vector3 into Serialized JSON data:
```c#
List<SerializedVector3> anchorPositions = new List<SerializedVector3>();
List<SerializedQuaternion> anchorRotations = new List<SerializedQuaternion>();

foreach (var gameObjects in arrayOfGameObjects)
{
    Vector3 currentPositionData = anchorGameObject.transform.position;

    Quaternion currentRotData = anchorGameObject.transform.rotation;

    anchorPositions.Add(Vector3Extensions.FromVector3(currentPositionData));
    anchorRotations.Add(QuaternionExtensions.FromQuaternion(currentRotData));
}

SerializedVector3[] actualPositions = anchorPositions.ToArray();
SerializedQuaternion[] actualRotations = anchorRotations.ToArray();

string jsonPositions = JsonConvert.SerializeObject(actualPositions);
string jsonRotations = JsonConvert.SerializeObject(actualRotations);

byte[] dataPos = Encoding.ASCII.GetBytes(jsonPositions);
byte[] dataRot = Encoding.ASCII.GetBytes(jsonRotations);

UnityEngine.Windows.File.WriteAllBytes(localJSONPath, dataPos);
UnityEngine.Windows.File.WriteAllBytes(localJSONPath, dataRot);

```

[Serializables.cs](https://github.com/sudotman/BetterUnity/blob/main/Runtime/BetterUnity/Helpers/Serializers/Serializables.cs) has more information about every function/class and extensions. Deserialization/Serialization is also available in the same script.

<br>

### 2.12 BetterRaycast
A simple one-liner to allow a better raycast function. Usually, when Raycasting, we also like to Debug and see the visual output of our raycast being hit. This encapsulates it all into one single line.

The hit is passed by reference and is updated automatically.

```ts
RaycastHit hit;
hit.BetterRaycast(startPosition, direction, raycastDistance);
```

<br>

## 3. Toolbar Tools

Tools to do various functions, called by menus.

### 3.1 Rename Suite
Works similar to BetterRename except its for remaining them only once. [We can attach the script, do the rename, and then remove it if we would want but this has a much cleaner UX]. Further, it allows you to rename any random [from anywhere to anywhere] selection at any place in the heirarchy.

!["3.1 Demo - Rename"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/6_1_Rename.gif)

### 3.2 Setup Default Project
A one click button that set-ups directories in our project folder in a standardish way.

!["3.2 Demo - Default"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/6_2_Default.gif)

### 3.3 Select All With
Another wizard to select all the objects with the specified:
- tag
- layer
- name
- having cameras
- having audio sources
- having meshes

!["3.3 Demo - Select"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/3_3_SelectAll.png)

### 3.3.1 Select All with Generic
Often times we end up with frankenstienian structure in the heierarchy of our project. Imagine a dashboard inside of a car being setup using a canvas with various TextMeshPro's being nested inside more Canvas Elements until there are atleast 20-30 elements inside other sub components. The normal search function (including the aforementioned BetterUnity's select all with) ends up showing objects in the entire heirarchy during a search even if we search by type. 

'Select all with generic' allows you to select any object type that exists below the currently selected object - allowing faster modifications of components at the same time. 

!["3.3 Demo - Select Generic"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/3_3_1_SelectAllGeneric.png)

### 3.4 Mesh Combine Wizard Tool
Made from taking the tool by @sirgru as the base of it. Allows you to combine multiple meshes into one in one easy click.
Allows the generation of Secondary Meshes too if needed. Combines mesh renderer and the mesh filter together of any two meshes into one also making it into prefab.

!["3.4 Demo - Mesh Combine Wizard"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/3_4_MeshCombineTool.png)


## 4. BetterContext
A collection of additions to various context menus throughout Unity.

### 4.1 Move ATB
Right click on any two GameObjects in the heirarchy and move the objects to each other as needed.

### 4.2 Solve Import
Right click on any GameObject in the heirarchy to "Resolve Import Issues". A lot of times when importing 3D models, we tend to get empty objects, cameras, lights etc [usually dependent on the way the modeller exports them] and hence this button gets rid of them in one-go.

!["4.1 Demo - Solve Import"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/7_1_SolveImport.gif)

### 4.3 Other Contexts
- Rigidbody: I find myself wanting to freeze X, Z rotation and turn interpolation on and also make sure it's not kinematic. Right click and pressing this button does it all for me.

- AudioSource: One key feature missing from inspecting an AudioSource is the ability to play, pause and stop it at will. Right click and pressing this does the aforementioned.

!["4.2 - AudioSource"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/7_2_AudioSource.gif)


## 5. Debug Tools

All of the following is without any unnecessary canvases.

<b>How to use:</b>
Add LogToScreen to any GameObject without doing anything else.

### 5.1 Debug Log to Screen:
Unity has a great Debug system but lacks in outputting it while full-screened/inside VR and hence this is an Unreal-Inpsired simple way to display all Debug Log messages on the screen. All messages fade away after a while and also start replacing old logs if too many are outputted quickly.

!["3.1 Demo - Debug To Screen"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/3_1_ToScreen.gif)

<br>

### 5.2 BD:
BD (BetterDebug) is a bunch of extensions to the default Debug class which allows greater control over Debugging/Logging.

- <b> BD.Log </b> : Is able to log and print any string or array. (tiny code but effective for quick debugging)

```C#
BD.Log("Test");
```

### 5.3 Simple FPS Counter
A very simple skeletal frames-per-second counter. Inverses unscaled delta time to output fps on the screen.

!["3.2 Demo - FPS Counter"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/3_2_FPS.png)

<br>

## 6. Better Transform
An extension of the existing Transform component to include some ease-to-use features.

!["6 Demo - Better Transform Overview"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/1_0_Global.png)


### 6.0 Local/Global Switch
!["6 Demo - Better Transform Overview"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/1_0_Local.png)

### 6.1 Lock Scale Ratio (Uniform Scale):
Maintain the ratio of the scaling in an object's axes when scaling a GameObject up/down. Gives a slider with an option to configure the ceiling value for the slider.

!["6.1 Demo - Lock Scale"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/1_1_Scale.png)

### 6.2 Reset Position, Rotation, Scale:
Reset the position, rotation and scale of any GameObject locally/globally individually.

!["6.2 Demo - Reset"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/1_2_Reset.gif)

### 6.3 Visibility Toggle:
Sometimes you would want to only disable the Mesh Renderers and not Disable the object so that the scripts/components on them still execute. This disables all the Mesh Renderers in this GameObject and all the children renderers.

!["6.3 Demo - Visibility"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/1_3_Visibility.gif)

### 6.4 Make Unit Scale Parent:
For a lot of use-cases in Unity [wanting to have a unit scale or have positioning, rotation start from zero], we tend to create new objects and then assign it as a parent. This button does that.

!["6.4 Demo - Unit Scale"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/1_4_Unit.gif)

### 6.5 Unreal-Styled Camera Bookmark Hotkeys
Unreal offers a way in the scene to set "camera" position/rotation as bookmarks with "Ctrl+1/Ctrl+2/Ctrl+3" to set the bookmarks and then "1,2,3" respectively to jump to those bookmarks. [This shortcut might already be bound to something else. Resolve any conflicts as you please]

!["6.4 Demo - Unit Scale"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/6_UnrealBookmarks.gif)

<br>

## 7. World Settings
A bunch of world settings inspired by the same-name Unreal settings which have a bunch of functionalites missing in Unity.

### 7.1 KillY (KillZ from UE)
The value of KillY dictates the Y-Position threshold of any object below which it gets automatically destroyed.


## 8. VR Scripts
Useful scripts that aid in my VR development.

### 8.1 Head Level
Make an object always align with our headset's level.

Mainly derived from MRTK's head aligning script with some extra small modifications to the whole script that best suits my (and hopefully, other developers') needs.

!["8.1 Demo - HeadLevel"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/5_1_1_Head.png)
 

!["8.1 Demo - HeadLevel"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/5_1_Head.gif)


## 9. Miscellaneous

### 9.1 Controllers
Various basic controller templates, such as FPS, VR.

### 9.2 Navigation System
A NPC walking/navigation system which is modular and fully configurable.

Demo:

!["7.1 Demo - HeadLevel"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/8_2_1_NavigationMaster.gif)

<br>

You can reconfigure individual points/characters after using the Master Creator or you can choose to attach scripts individually and configure settings manually - both would work, and that would also work in conjunction to both 
approaches.

Import the sample from the package manager to get a preconfigured scene if needed.

<br>

<b>Doing it manually</b>:
Group all the points you wish to navigate into one parent and drop the parent into the points prefab and configure everything accordingly. You can keep the PointParnet's children messy and maybe use BetterRename to organize them (wink, wink).

Individual scripts:
!["8.2 - Navigation System"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/8_NavigationManual.png)

<br>

A quick guiding GIF on a quick setup is given below:
!["8.2 - Navigation System"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/8_2_2_Setup.gif)

<b> General Information</b>:

* If using Master Creator, remember to attach an animator to your prefab and have the two walking and running state inside the animator (No state machine connections necessary). This will also be displayed as error but seems to be a common issue so I am spelling it out here.
* When using master creator, a generic walking/running speed is calculated. Feel free to modify it individually or in-group as convenient.
* Avoid turns which are too steep, even though there is a custom interpolation built-in, if turns are too steep, it will look not so great.

<br>

Navigation System working as it should:

!["8.2 - Navigation System"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/8_2_Navigation.gif)

<br>

### 9.3 Flow Controller
A lot of times, there is a need to script events linearly (usually to follow a particular storyboard in a particular order). This scripts entails having multiple variables, multiple functions and in general just a very messy frankensteinian codebase.

This hopes to alleviate some of those messyness - trying to make a more generic flow controller which allows for triggering of events with ease also eliminating the need to do this everytime.

Also, has a Custom Inspector GUI that streamlines it some more.

!["9.3 Demo - Flow Controller"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/8_3_FlowController.png)

General outline:

- <b>Events Associated</b> are the current events to be called when this "Storyboard Event" is hit. Events associated has an array where the first Event element of the Event[] array is called instantly while the rest are called with a 2 seconds delay. Note: The first element of the Event[] array - multiple listeners (functions) can be called inside one UnityEvent.
- <b>Phase Name</b> defines the name of this storyboard event and also is used as the index for the overall storyboard - gets mapped as an integer in the backend.
- <b>Phase Description</b> is purely cosmetical and doesn't alter functionality - just for record keeping.
- <b>Moving Ahead Requirements</b> define the event that you need to trigger to trigger the wait conditions. Once, these booleans are assigned, you will be assigned one bool out of a generic set of bools that you can modify or pass by ref if needed to further modify it and execute the condition on completion.

The script would look this with sample data of a storyboard:
!["9.3 Demo - Flow Controller"](https://github.com/sudotman/sudotman/blob/main/demos/BetterUnity/8_3_2_FlowController.png)



<br>

# Contribution
Generate a pull request for whatever change you feel is necessary and I will be happy to review and add them.

The project should be good to go as is on most modern Unity versions. Prefer to not update any dependencies or any other prompts auto-detected by Unity.

## Current to-do:
- ~~Update RenameSuite's gif.~~
- ~~Make ScreenToLog scale according to our window size. [possible solution is to use GUI.matrix]~~
- ~~<b>Store GIFs on the cloud</b>. [Very essential, the repo is growing fast]~~
- ~~Update dynamic scaling of InspectorText. [using a bandaid fix for now which is really triggering]~~	
- Assembly definitions are not properly architectured, yet. Need to refactor and organize components into proper categories.
- Add more controllers templates.
- Add more VR utiltiy scripts.
- Organize and document Helper Functions (in categories and with more information and preferably screenshots)
- A better way of installation - possibly with a package through our Git url and inbuilt Unity's package manager.
- Fix README's alt text naming.
- Create one universal prefab to enable all functionality inside of our scene.
- Modify CallInEditor to encompass text-change parameters and size instead of a seperate wrapper of InspectorButton. [Debating this as I really like the simplicity of CallInEditor]
- The eulers of the rotation on Inspector only modify the global rotation -  do something to make it so that the local rotation could also be modified in the inspector itself

### Navigation To-Do:
- ~~Multiple lanes/paths~~
- ~~Create a parent script to automatically configure animation states and attach script to all children gameobjects and also populate if empty objects positions.~~
- ~~Use mixamo characters and give selection for different states such as walking, running etc and have a unit scale for speed/animation sync and procedurally modify walking running speeds.~~
- ~~Continue walking from where the start location as opposed to moving them to the preferred location~~
- ~~Give options to control animation states from the inspector.~~
- ~~Create an accompanying editor script so that the Inspector isnt as confusing as it is right now.~~
- Add a more robust documentation/screenshots accompanying the parent script/individual scripts and the working of it. // sorta done for now but will remove this after some consideration.
- Add a dynamic intersection detection with other NPCs.




