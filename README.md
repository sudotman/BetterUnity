# Better Unity
A collection of modifications/additions (scripts/prefabs) which enhance [for me] the overall flow of using the Unity Engine.

## Purpose
There are some key features pan other game engines that seem to be lacking in Unity and this is a package to resolve those problems.
<br>

## Installation
You can download the latest ```.unitypackage``` [through releases or the [repository]("link") folders] and double-click to import it.


# Contents

## 1. Better Transform
An extension of the existing Transform component to include some ease-to-use features.


!["1 Demo - Better Transform Overview"](Demo/1_BetterTransformOverview.png)

### 1.1 Lock Scale Ratio (Uniform Scale):
Maintain the ratio of the scaling in an object's axes when scaling a GameObject up/down.

!["1.1 Demo - Lock Scale"](Demo/1_1_Scale.gif)

### 1.2 Reset Position, Rotation, Scale:
Reset the position, rotation and scale of any GameObject with a click instead of resetting the entire transform or manually inputting `0,0,0 / 1,1,1`.

!["1.2 Demo - Reset"](Demo/1_2_Reset.gif)

### 1.3 Visibility Toggle:
Sometimes you would want to only disable the Mesh Renderers and not Disable the object so that the scripts/components on them still execute. This disables all the Mesh Renderers in this GameObject and all the children renderers.

!["1.3 Demo - Visibility"](Demo/1_3_Visibility.gif)

### 1.4 Make Unit Scale Parent:
For a lot of use-cases in Unity [wanting to have a unit scale or have positioning, rotation start from zero], we tend to create new objects and then assign it as a parent. This button does that.

!["1.4 Demo - Unit Scale"](Demo/1_4_Unit.gif)

## 2. Inspector Additions

### 2.1 Better Rename
A lot of the times when developing, you have similar children which you want to be named incrementally with your desired prefix. Instead of manually going in and doing it, this module allows me to do more it quickly and effeciently, by attaching a script to the parent.

Further, when duplicating objects and creating new ones, Unity appends (x) [x being the current duplicate] and sometimes you would want the objects to be named differently. All new objects will also be renamed appropriately, automatically.


!["2.1 Demo - Better Rename"](Demo/2_1_Rename.gif)

<br>

### 2.2 Inspector Button
You can expose normally variables and fields to the inspector in Unity and modify them in-realtime, and it works great. Sometimes, you would want to have a button set to easily call a function of your liking from the inspector and I tend to use workarounds (such as, setting a boolean and then calling a function after checking the boolean from Update) or creating your custom inspector and having a button there. This is a much more easier and simpler addition to your existing scripts and can be called using simple property attributes.

Usage:
```C#
[InspectorButton("funcToBeCalled")]
public char myButton;
```

The above snippet will create a button named My Button and will call <i>funcToBeCalled</i> function when pressed.

!["2.2 Demo - Inpsector Button"](Demo/2_2_Button.gif)

You can also specify the size of the button (default size is 80):
```C#
[InspectorButton("funcToBeCalled",100)]
public char myButton;
```

!["2.2 Demo - Inpsector Button"](Demo/2_2_1_Button.png)

<br>

### 2.3 Better Scale
A script which works similar to the BetterTransform component but given incase you dont wish to override default transform and just want the uniform scaling on one object.

<br>

## 3. Debug Tools

### 3.1 Debug Log to Screen:
Unity has a great Debug system but lacks in outputting it while full-screened/inside VR and hence this is an Unreal-Inpsired simple way to display all Debug Log messages on the screen. All messages fade away after a while and also start replacing old logs if too many are outputted quickly.

!["3.1 Demo - Debug To Screen"](Demo/3_1_ToScreen.gif)

<br>

### 3.2 Simple FPS Counter
A very simple skeletal frames-per-second counter. Inverses unscaled delta time to output fps on the screen.

!["3.2 Demo - FPS Counter"](Demo/3_2_FPS.png)

<br>

## 4. Helpers
A collection of extension functions which can be called in other scripts for ease-of-use.

### 4.1 Scale Range
Scale a value from a previous range to a new range (remap it linearly).

```C#
myFloatValue.ScaleRange(0,100,10,30);
```

### 4.2 Destory Children
Destroy all children of a parent.

```C#
transform.DestroyChildren();
```

### 4.3 Minus Float Vector 3
Subtract one float value from x, y, z components of a Vector3.

```C#
myVector3.MinusFloatVector3(2.5f);
```

### 4.4 LookAtY
Look at but rotate only across the Y axis (say, for an enemy that needs to turn to you at all times).

```C#
transform.LookAtY(player.position);
```

### 4.5 Grounded/Distance From Ground
Returns distance between two vectors but its grounded. (ignoring their Y-position during calculation)
```C#
player.position.DistanceFromGround(objective.position);
```

### 4.6 Return a random item from a list
Get a random item from a list
```C#
currentItem = myList.RandomItem();
```

## 5. VR Scripts
[todo: add more]
Useful scripts that aid in my VR development.

### 5.1 Head Level
Make an object always align with our headset's level.

!["5.1 Demo - HeadLevel"](Demo/5_1_Head.gif)

## 6. Miscellaneous

### 6.1 Controllers
Various basic controller templates, such as FPS, VR.

### 6.2 Toolbar Tools
[todo: add more]

Tools to do some one-time functions.
Such as, a toolbar version of the better rename, if we require to do it uniquely once.

!["6.2 Demo - Toolbar"](Demo/6_2_Toolbar.gif)


