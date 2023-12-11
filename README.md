# MotionKit Guide

MotionKit is a tool that animates anything. It is very similar to [DOTween](http://dotween.demigiant.com/getstarted.php), but with inspector super powers and a few tweaks that makes it simpler to use and learn.

([Watch overview on YouTube](http://www.youtube.com/watch?v=1knaaxQQs3I))

<a href="http://www.youtube.com/watch?feature=player_embedded&v=1knaaxQQs3I
" target="_blank"><img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/baeb690d-5465-443e-b246-d1ea51f39611" 
alt="IMAGE ALT TEXT HERE" width="300" border="10" /></a>

## How to Install
`MotionKit` depends on the Cocodrilo Dog `Core` tool. You need to install this dependency first, via the Unity Package Manager.

To install both in your Unity project, open the Package Manager and click the plus button, "Add package from URL..." and the use these URLs:
- Cocodrilo Dog Core: https://github.com/cocodrilodog/tools-core
- MotionKit: https://github.com/cocodrilodog/tools-motion-kit

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/6408c284-330b-4abb-a075-5f5452841775" height="150">

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/57a94a49-0474-4744-88d7-f0835c7a6455" height="150">

Let's chat in Discord: https://discord.gg/sZHQPsq

## Quick Start
### C#
Move a `m_Ball` (`Transform`) from 0, 0, 0 to 3, 0, 0 during 2 seconds. Register the motion with owner `m_Ball` and reuse id `"Position"`
```
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2);
```
Fade in a `m_CanvasRenderer`: Animate `alpha` from 0 to 1 during 2 seconds. Register the motion with owner `m_CanvasRenderer` and reuse id `"Alpha"`
```
MotionKit.GetMotion(m_CanvasRenderer, "Alpha", a => m_CanvasRenderer.SetAlpha(a)).Play(0, 1, 2);
```
Animate the color of the `m_Image` from black to red during 2 seconds. Register the motion with owner `m_Image` and reuse id `"Color"`
```
MotionKit.GetMotion(m_Image, "Color", c => m_Image.color = c).Play(Color.black, Color.red, 2);
```
### Inspector
The group of classes that handles `MotionKit` objects via inspector are collectibly named `MotionKitBlock`s.

To create the same example as the `m_Ball` code above via inspector, add a `MotionKitComponent`:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/ec29de39-63d8-431c-8641-e7d8d4627065" height="120">

Create a `Motion3DBlock`:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/3da577ab-569a-432e-9268-681f3cdb9861" width="400">

Tick `Play on start` or play later with `MotionKitComponent.Play()`:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/79888941-bbc2-48b8-a5b2-ab77f89e2914" width="400">

Click `Edit` and assign the relevant properties:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/c080c21a-644b-495d-a7de-2ce0eba601ca" width="400">

To replicate the examples of the `m_CanvasRenderer` and `m_Image` code above via inspector, it is the same process, but you create `MotionFloatBlock` to animate the `m_CanvasRenderer.alpha` property, and `MotionColorBlock` to animate the `m_Image.color`.

## Lifecycle: `owner`, `reuseID`, and Clearance

The idea of the `owner` and `reuseID` is to store the `Motion` objects internally in an ordered way so that they are reusable when it makes sense, and then disposed when not needed anymore. 

Instead of using global IDs, I decided to associate the `Motion`s with "owners", because it makes more sense from a development standpoint. For example, you may want to define the `reuseID` of multiple `Motion`s that affect the position of objects as `"Position"`, but each one should be associated with the specific `owner` that it intends to move, so that it is no confused with any other. This is a scalable solution to manage the reusability and disposal of the `Motion` objects.

Example of owners and reuse IDs:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/d3c5c867-31e2-489d-92fd-b98febc89007" width="600">

### C#
#### `owner` and `reuseID`
In the code below, the `owner` is the `m_Ball` and the `reuseID` is `"Position"`. In order to move the ball many times you may not want to create a new `Motion3D` object each time, so if you write the code below in different places of your script with different positions and durations, you can rest asured that the same `Motion` object will be used for all the position animations as long as you use the same `owner` and `reuseID`.
```
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2);
```
This has the very convenient side effect that a reused motion will "interrupt" itself if needed. For example, in the code below, the second motion will interrupt the first one because it is starting before the first one completes. This happens because the animation is being carried out by the same `Motion` object, so it just stops and plays with the new settings. 
```
// First motion
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2);

// wait for 1.5 seconds

// Second motion (0.5 before the previous one completes)
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(m_Ball.localPosition, new Vector3(0, 5, 0), 3); // New initial position, final position, and duration are set
```
#### Clearance
Once you are done with the motions, you can get rid of them like this:
```
private void OnDestroy() {
	// Dispose the motions that were registered
	MotionKit.ClearPlaybacks(m_Ball);
	MotionKit.ClearPlaybacks(m_CanvasRenderer);
	MotionKit.ClearPlaybacks(m_BallColor);
}
```
The code above will dispose all the motions registered with owners: `m_Ball`, `m_CanvasRenderer`, and `m_BallColor`.
### Inspector
#### `owner` and `reuseID`
To set the `owner` and `reuseID` in a `MotionKitBlock` (the inspector version), click `Edit Owner and/or Reuse ID` and then assign the desired values:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/8f9b960e-0fe3-49db-a15b-c76eb3fc6c9b" height="115">
<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/ee8846b6-df1a-4f93-8476-fb07d10484fb" height="115">

#### Clearance
In the `MotionKitComponent` the clearance is carried out automatically when the component is destroyed.

## Setter: `Vector3`, `Float`, `Color`
### C#
Every motion needs a `setter` to apply changes to the property that we want to animate. In our example, the setter is `p => m_Ball.localPosition = p`, which will animate the position of the ball:
```
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2);
```
It is a lamba expression that receives a `Vector3` parameter and then applies it as needed. It could also be a function, but would need more lines of code:
```
MotionKit.GetMotion(m_Ball, "Position", SetPosition).Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2);

void SetPosition(Vector3 pos) {
	 m_Ball.localPosition = pos;
}
```
The `MotionKit` API is designed so that depending on the provided setter it will internally choose among `Motion3D`, `MotionFloat` and `MotionColor`. You don't have to worry about that. For example, the following code will create a `MotionFloat` object:
```
MotionKit.GetMotion(m_CanvasRenderer, "Alpha", a => m_CanvasRenderer.SetAlpha(a)).Play(0, 1, 2);
```
An this one will create a `MotionColor`:
```
MotionKit.GetMotion(m_Image, "Color", c => m_Image.color = c).Play(Color.black, Color.red, 2);
```
### Inspector
When using the `MotionKitComponent` you **do** have to choose which motion to use, according to the property that you want to animate:
<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/3da577ab-569a-432e-9268-681f3cdb9861" width="400">

Note that there is a `Motion2D` block that is only avaliable in the inspector version.

Once the `MotionKitBlock` is created, the inspector will allow you to choose among all properties and methods that suits the type of `MotionKitBlock`. In this example, all the listed methods and properties receive a `Vector3` parameter:
<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/40b256b1-4204-428c-a77d-e15d9179adf6" width="400">

## Easing

The `Motion` objects animate properties between an `initialValue` and a `finalValue` for the specified `duration`. By default the animation will happed with constant speed, which in other words uses a linear function to interpolate between the two values:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/e9adbf54-f34a-4e5e-b220-0c9b8cc44db7" width="300">

But the `Motion` objects receive an `easing` paramater that is a function that will accelerate/decelerate the animations in different ways.

A good place to visualize easing functions is [Robert Penner's website](http://robertpenner.com/easing/)

<img width="800" alt="imagen" src="https://user-images.githubusercontent.com/8107813/64360751-b5b20200-cfd0-11e9-83a8-4df8a0199707.png">

### C#
In the code below, the easing function is `MotionKitEasing.BackOut` that correspond to one the built-in functions:
```
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2)
 	.SetEasing(MotionKitEasing.BackOut);
```
By using different functions, the animations will look more interesting and will likely have more artistic value.

The class `MotionKitEasing` has many built-in functions that you can visualize in the [Robert Penner's website](http://robertpenner.com/easing/), but there are also some other options:
- `MotionKitCurve` (`AnimationCurve`): Uses a Unity `AnimationCurve` that you can customize in the inspector.
- `Blink`: Changes between the initial and final value repeatedly like blinking. For example, black, white, black, white, etc.
- `Pulse`: Increments a value like in a Bell Curve. Why didn't I named it "Bell Curve"? Only God knows why...
- `Shake`: Makes things shake as you would expect on video games. This is good for a camera shake, for example.

This special functions are called `ParameterizedEasing` and more documentation on this will come later.

### Inspector
In the `MotionKitComponent` you just choose from a list of easing functions:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/c2f82556-5722-428a-9c2a-dbde95b32d91" height="400">  
<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/c7c3cea4-d6fb-4505-8548-e32e49caf8da" height="400">

And if you choose from some of the `ParameterizedEasing` functions (`AnimationCurve`, `Blink`, `Pulse` or `Shake`), additional parameters will appear on the inspector:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/ee1c4990-a3d8-438e-9a4b-649d1ee436b0" width="400">


## Callbacks
### C#
`Motion` objects have the following callbacks:

Callback | Triggered
----------- | ---------------- |
`OnStart` | Just before the `Motion` starts.
`OnUpdate` | On every update, while the `Motion` is playing.
`OnInterrupt` | When the `Motion` is stopped or played before it has completed.
`OnComplete` | When the `Motion` completes tha animation.

An example of how to set an `OnComplete` callback, lambda style:
```
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2)
	.SetOnComplete(() => Debug.Log("Motion Completed!!!"));
```
Or this one with a function:
```
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2)
	.SetOnComplete(OnComplete);

void OnComplete() {
	Debug.Log("Motion Completed!!!");
}
```
To set the other callbacks you can use these methods: `SetOnStart()`, `SetOnUpdate()`, and `SetOnInterrupt()`.

The callbacks can also receive the `Motion` object as a parameter, for example:
```
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2)
	.SetOnUpdate(m => Debug.Log($"Motion progress: {m.Progress}"));
```
Or this one with a function:
```
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2)
	.SetOnUpdate(OnUpdate);

void OnUpdate(Motion3D motion3D) {
	Debug.Log($"Motion progress: {motion3D.Progress}");
}
```
This parameter comes handy in case you want to read or change any of the `Motion`'s properties while its playing. 

### Inspector
In the `MotionKitComponent`, the callbacks can be assigned as Unity events in the bottom part of the inspector:
<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/d96861bd-d91d-4a64-83a3-7c6d9dddf229" width="450">

Note that the callbacks that receive the `Motion` object as a aprameter are not available from the inspector.

## Control the Playback: The `Progress` Property

## All the Playback Objects: `Motion`, `Timer`, `Sequence`, `Parallel`

## Known Issues

## Table of Contents

- [1 Introduction](#1-introduction)
- [2 MotionKit Architecture](#2-motionkit-architecture)
	- [2.1 The `owner` and `reuseID` Parameters](#21-the-owner-and-reuseid-parameters)
	- [2.2 The `setter` Parameter](#22-the-setter-parameter)
	- [2.3 Clearing Motions](#23-clearing-motions)
- [3 More `Motion` Settings](#3-more-motion-settings)
- [4 Easing and `ParameterizedEasing`](#4-easing-and-parameterizedeasing)
	- [4.1 Easing](#41-easing)
	- [4.2 `MotionKitCurve`](#42-motionkitcurve)
   	- [4.3 `Blink`](#43-blink)
	- [4.4 `Pulse`](#44-pulse)
   	- [4.5 `Shake`](#45-shake)
- [5 `Timer`](#5-timer)
- [6 `Sequence`](#6-sequence)
- [7 `Parallel`](#7-parallel)
- [8 Handle `Motion` Objects Out of MotionKit](#8-handle-motion-objects-out-of-motionkit)
- [9 Common Error](#9-common-error)
- [10 Inspector Superpowers: `MotionKitBlocks`](#10-inspector-superpowers-motionkitblocks)

## 1 Introduction

With `MotionKit`, all numeric values of any object can be animated. There are also built-in extensions to animate `Color` and `Vector3` values. The tool is easily extensible to support more animatable types.

<img width="400" alt="imagen" src="https://user-images.githubusercontent.com/8107813/64360668-813e4600-cfd0-11e9-9cd4-c1cb5f592a94.gif">

There is a bunch of examples scenes that can be imported from the Unity Package Manager window.

## 2 MotionKit Architecture

Animating objects with `MotionKit` is very simple. In the example below, we are animating the `localPosition` of `TheObject` from its current value to 3 units right with a duration of 1 second:

```
MotionKit.GetMotion(this, "Position", p => TheObject.localPosition = p)
	.Play(TheObject.localPosition, TheObject.localPosition + Vector3.right * 3, 1f);
```

When we create an animation this way, the `MotionKit` tool creates and returns an instance of `Motion3D` type. Subsequential execution of this code will reuse the same `Motion3D` instance while it uses the same combination of `owner` and `reuseID`. Explained below!

### 2.1 The `owner` and `reuseID` Parameters 

By passing `this` as the `owner` argument we are saying that the `Motion3D` instance conceptually belongs to the object referenced by the `this` keyword (normally a `MonoBehaviour`). It can be any object, though.

The `reuseID` `"Position"` is an arbitrary name that we create in order to reuse the generated `Motion3D` instance for all position animations that the `this` object will play and that will be performed by the same `Motion3D` instance.

There are other types of motions like `MotionFloat` and `MotionColor`, but the returned type will be handled automatically by `MotionKit`, depending on the passed parameters.

Having the `owner` parameter helps to isolate which animations belong to which objects. For example in the table below, we can see a list of possible owner-key related motions:

Owner | Reuse Key | Motion instance type
----- | --------- | --------------------
`CharacterController1` | `"Position"` | Motion3D
`CharacterController2` | `"Position"` | Motion3D
`CharacterController2` | `"Scale"` | Motion3D
`AudioSource1` | `"Volume"` | MotionFloat
`AudioSource2` | `"Volume"` | MotionFloat
`GraphicController` | `"BackgroundColor"` | MotionColor

As it can be seen in the table, both `CharacterController1` and `CharacterController2` share the key `"Position"` but `MotionKit` will create two different instances of `Motion3D` because the `"Position"` is associated with two different owners.

The motions will be reused in sequential `MotionKit.GetMotion(...)` calls only when the owner and the key are the same, otherwise new motion instances will be created.

The reusability system is very useful, because it is very common that a developer will need to animate certain parameter of an object very often, for example: the position, associated with the "Position" key. In this case, this system will reuse the same "Position" motion instance for multiple sequential movements, which results in the animation changing its final position instead of having multiple motions trying to move the same object in different directions and conflicting with each other.

### 2.2 The `setter` Parameter

This is where the property will be actually animated. `MotionKit` calculate values over time and you need to assign them to the target object. The most efficient way to write this parameter is by using a lambda expression. The function must receive a parameter of the type that you want to animate (`Vector3`, `float` or `Color`).

Another alternative would be the normal method syntax.

```
// Lambda style:
MotionKit.GetMotion(this, "Position", p => TheObject.localPosition = p)
	.Play(TheObject.localPosition, TheObject.localPosition + Vector3.right * 3, 1f);

// Normal method style:
MotionKit.GetMotion(this, "Position", PositionSetter)
	.Play(TheObject.localPosition, TheObject.localPosition + Vector3.right * 3, 1f);

void PositionSetter(Vector3 pos) {
	TheObject.localPosition = pos;
}
```

Depending on this parameter, the returned motion will be either `Motion3D`, `MotionFloat` or `MotionColor`.

### 2.3 Clearing Motions

When you invoke `MotionKit.GetMotion(...)`, `MotionKit` stores the returned `Motion` object in dictionaries organized by `owner` and `reuseKey` in order to reuse them when needed. Once an owner is no longer going to play its animations, it is a good idea to remove its `Motion` instances from the dictionary. 

This is done by calling `MotionKit.ClearMotions(owner)`. For example, in the case of `MonoBehaviour` objects, a good place to call it is `OnDestroy()`


## 3 More `Motion` Settings

`MotionKit` motion objects have additional settings like easing parameters, callbacks and playback control:

```
Motion3D positionMotion;

void Start() {
	positionMotion = MotionKit.GetMotion(this, "Position", p => TheObject.localPosition = p)
		.SetEasing(MotionKitEasing.EaseOut)
		.SetOnUpdate(() => Debug.LogFormat("Position animation updated"))
		.SetOnComplete(() => Debug.LogFormat("Position animation Completed"))
		.Play(TheObject.localPosition, TheObject.localPosition + Vector3.right * 3, 1f);
}

void PauseResumeButton_OnClick() {
	if (positionMotion.IsPaused) {
		positionMotion.Resume();
	} else {
		positionMotion.Pause();
	}
}

void Slider_OnValueChanged(float value) {
	positionMotion.Progress = value;
}

void StopButton_OnClick() {
	positionMotion.Stop();
}

```

## 4 Easing and `ParameterizedEasing`

### 4.1 Easing

`MotionKit` can use any easing function to calculate the animated values. By default (if no easing curve is specified), it will animate the values with a linear function. The signature for the easing function follows this template:

`delegate ValueT Easing(ValueT a, ValueT b, float t);`

Where `ValueT` is the type to animate. In the table below you can see the current `Motion` implementations, their corresponding animatable types and default easing functions:

Motion Type | Animatable Type  | Default Easing Function
----------- | ---------------- | -----------------------
`Motion3D` | `Vector3` | `Vector3.Lerp`
`MotionFloat` | `float` | `Mathf.Lerp`
`MotionColor` | `Color` | `Color.Lerp`

A good place to look for easing functions is [Robert Penner's website](http://robertpenner.com/easing/)

<img width="800" alt="imagen" src="https://user-images.githubusercontent.com/8107813/64360751-b5b20200-cfd0-11e9-83a8-4df8a0199707.png">

On the other hand, there are some curves that require some special parameters. They are called `ParameterizedEasing` and are described below.

### 4.2 `MotionKitCurve`

(This section needs to be updated)

Sometimes the existing easing functions won't fit specific needs. For this, there is an additional option called `MotionKitCurves`. It is an asset where you can design custom animation curves that can be passed as parameters to `SetEasing(...)`:

<img width="317" alt="imagen" src="https://user-images.githubusercontent.com/8107813/64360868-fa3d9d80-cfd0-11e9-9ea0-4caa16aea6ed.png">

Once you have a `MotionKitCurve` field in the inspector, you can use it like this:

```
MotionKit.GetMotion(this, "Color", c => ColorObject.Color = c)
	.SetEasing(motionKitCurve.ColorEasing)
	.Play(ColorObject.Color, Random.ColorHSV(), 1);
```
### 4.3 `Blink`
(Coming soon...)

### 4.4 `Pulse`
(Coming soon...)

### 4.5 `Shake`
(Coming soon...)

## 5 Timer

The `Timer` object is used to invoke any method at the end of a time period. In the example below the `"Timer Completed!"` log will be shown in the console 1 second after the call to `MotionKit.GetTimer(...)`.

```
MotionKit.GetTimer(this, "Delay").Play(1).SetOnComplete(() => Debug.LogFormat("Timer Completed!"));
```

The `Timer` object follows the same rules as the `Motion` objects, except that it doesn't receive any `setter` nor easing function. It should be cleared the same way `Motion` objects are cleared.

It can be used in combination with `Motion` objects to create animated sequences:

```
MotionKit.GetMotion(this, "Position", p => PositionObject.localPosition = p)
	.Play(PositionObject.localPosition, Random.onUnitSphere, 1f)
	.SetOnComplete(() => {
		// Wait for the position animation to complete and wait one more second
		MotionKit.GetTimer(this, "Delay").Play(1).SetOnComplete(() => {
			// After the second has passed, animate the color
			MotionKit.GetMotion(this, "Color", c => ColorObject.Color = c).Play(ColorObject.Color, Random.ColorHSV(), 1);
		});
	});
```
## 6 Sequence
(Coming soon...)

## 7 Parallel
(Coming soon...)

## 8 Handle `Motion` Objects Out of MotionKit

(This section needs update)

`Motion` objects can be instantiated independently of the `MotionKit` class. If you do so, you will need to handle their lifecycle by yourself. For example, you would need to properly handle multiple `Motion` objects that will try to animate the same property of an objects in overlaping times.

Examples:

```
Motion3D motion3D = new Motion3D(this, p => PositionObject.localPosition = p)
	.Play(Vector3.zero, Random.onUnitSphere * 3, 1f);
  
MotionFloat motionFloat = new MotionFloat(this, f => FloatObject.FloatProperty = f)
	.Play(FloatObject.FloatProperty, FloatObject.FloatProperty + 3, 1f);
  
MotionColor motionColor = new MotionColor(this, c => ColorObject.ColorProperty = c)
	.Play(ColorObject.ColorProperty, new Color(0, 0, 0, 1), 1f);
```
The first parameter passed to the constructor is the `MonoBehaviour` where the coroutines that generate the animations will be executed.

And when you don't need the motions anymore:
```
motion3D.Dispose();
motionFloat.Dispose();
motionColor.Dispose();
```

## 9 Common Error

It is common to have an error where the `onComplete` callback is invoked by a `Motion` where it wasn't specified. The following example illustrates this case:

```
void ScaleUpButton_OnClick() {
	TheObject.gameObject.SetActive(true);
	MotionKit.GetMotion(this, "Scale", s => TheObject.localScale = s)
		.Play(TheObject.localScale, Vector3.one, 1f);
}

void ScaleDownButton_OnClick() {
	MotionKit.GetMotion(this, "Scale", s => TheObject.localScale = s)
		.Play(TheObject.localScale, Vector3.zero, 1f)
		.SetOnComplete(() => TheObject.gameObject.SetActive(false));
}
```

The idea of this code is that the `GameObject` is disabled after it is downscaled and re-enabled prior to being upscaled. At first glace this code is correct, but what will happen is that the object will be disabled also when the upscaling animation completes, despite there was no `onComplete`callback assigned when upscaling.

This is due to the fact that the `Motion3D` instance is the same in both cases, because the `this` and `"Scale"` parameters are the same. So the `onComplete` callback that was assigned in the downscale animation remains when the upscale animation is triggered again. The solution for this problem is setting it to null` in the upscale animation:

```
void ScaleUpButton_OnClick() {
	TheObject.gameObject.SetActive(true);
	MotionKit.GetMotion(this, "Scale", s => TheObject.localScale = s)
		.Play(TheObject.localScale, Vector3.one, 1f)
		.SetOnComplete(null);
}
```

## 10 Inspector Superpowers: `MotionKitBlocks`

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/0dc7c842-e038-448f-91a9-305a58906c27" width="500">

(Coming soon...)
