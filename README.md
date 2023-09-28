# MotionKit Guide

MotionKit is a tool that animates anything. It is very similar to [DOTween](http://dotween.demigiant.com/getstarted.php), but with inspector super powers and a few tweaks that makes it simpler to use and learn.

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
- [9 Extending `MotionKit`](#9-extending-motionkit)
- [10 Common Error](#10-common-error)
- [11 Inspector Superpowers: `MotionKitBlocks`](#11-inspector-superpowers-motionkitblocks)

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
(TODO)

### 4.4 `Pulse`
(TODO)

### 4.5 `Shake`
(TODO)

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
(TODO)

## 7 Parallel
(TODO)

## 8 Handle `Motion` Objects Out of MotionKit

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

## 9 Extending `MotionKit`

`MotionKit` is simple to extend. You can add support to animate different types of values like in the example below, the `MotionColor` class.

```
/// <summary>
/// A motion object that supports <see cref="Color"/> animations.
/// </summary>
public class MotionColor : MotionBase<Color, MotionColor> {

	public MotionColor(MonoBehaviour monoBehaviour, Setter setter) 
		: base(monoBehaviour, setter) { }

	protected override Easing GetDefaultEasing() {
		return Color.Lerp;
	}

}
```

## 10 Common Error

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

## 11 Inspector Superpowers: `MotionKitBlocks`

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/0dc7c842-e038-448f-91a9-305a58906c27" width="500">

(TODO)
