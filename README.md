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

!["1.4 Demo - Unit Scale"](Demo/1_3_Visibility.gif)

## 2. Debug Tools

### 2.1 Debug Log to Screen:
Unity has a great Debug system but lacks in outputting it while full-screened/inside VR and hence this is an Unreal-Inpsired simple way to display all Debug Log messages on the screen.

!["2.1 Demo - Debug To Screen"](Demo/2_1_ToScreen.gif)