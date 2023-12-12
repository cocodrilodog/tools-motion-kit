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
The previous methods create `Motion` objects that handle the animation of the properties. They can be either `Motion3D`, `MotionFloat` or `MotionColor`, depending on the property that will be animated.

### Inspector

On the other hand, the classes that handle `Motion` objects via inspector are collectibly named `MotionKitBlock`s. They are somehow wrappers of `Motion` objects. The `MotionKitBlocks` are created and arranged on the `MotionKitComponent` which is a `MonoBehaviour` designed to use the `MotionKit` from the Unity inspector.

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

Instead of using global IDs, I decided to associate the `Motion`s with "owners", because it makes more sense from a development standpoint. For example, you may want to define the `reuseID` of multiple `Motion`s that affect the position of objects as `"Position"`, but each one should be associated with the specific `owner` that it intends to move, so that it is no confused with the others. This is a scalable solution to manage the reusability and disposal of the `Motion` objects.

Example of owners and reuse IDs:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/d3c5c867-31e2-489d-92fd-b98febc89007" width="600">

### C#
#### `owner` and `reuseID`
In the code below, the `owner` is the `m_Ball` and the `reuseID` is `"Position"`. In order to move the ball many times you may want to create only one `Motion3D` and reuse it as much as possible instead of creating a new one each time, so if you write the code below in different places of your script with different positions and durations, you can rest asured that the same `Motion3D` object will be used for all the position animations as long as you use the same `owner` and `reuseID`.
```
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2);
```
This has the very convenient side effect that a reused motion will "interrupt" itself if needed. For example, in the code below, the second animation will interrupt the first one because it is starting before the first one completes. This happens because the animation is being carried out by the same `Motion` object, so it just stops and plays with the new settings. 
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
// MonoBehaviour's OnDestroy
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

The `Motion` objects animate properties between an `initialValue` and a `finalValue` for the specified `duration`. By default the animation will happen with constant speed, which in other words uses a linear function to interpolate between the two values:

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
- `Shake`: Makes things shake as you would expect in video games. This is good for a camera shake, for example.

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
`OnComplete` | When the `Motion` completes the animation.

An example of how to set an `OnComplete` callback, lambda style:
```
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2)
	.SetOnComplete(() => Debug.Log("Motion Completed!!!"));
```
Or this one, with a function:
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
Or this one, with a function:
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

## Playback
### C#
#### Playback Methods
The `Motion` objects can be stored in variables and be controlled for playback with the methods `Play()`, `Pause()`, `Resume()` and `Stop()`:

```
Motion3D m_Motion3D;

void Start() {
	// Create the motion but don't play it, yet. Note that SetValuesAndDuration is used instead of Play()
	m_Motion3D = MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
		.SetValuesAndDuration(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2);
}

// Invoke the methods below from UI button clicks, for example

public void OnPlayButtonClick() {
	m_Motion3D.Play();
}

public void OnPauseButtonClick() {
	m_Motion3D.Pause();
}

public void OnResumeButtonClick() {
	m_Motion3D.Resume();
}

public void OnStopButtonClick() {
	m_Motion3D.Stop();
}
```
#### The `Progress` Property

`Motion` objects can be controlled via their `Progress` property. `Progress` is a number that goes from 0 to 1 and changes the property by interpolating between the `initialValue` and the `finalValue`. One example is if you want to create a slider that changes a property of an object and use a `Motion`, but you don't want to actually play the `Motion`:

```
Motion3D m_Motion3D;

void Start() {
	// Create the motion but don't play it at all. Note that SetValuesAndDuration is used instead of Play()
	m_Motion3D = MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
		.SetEasing(MotionKitEasing.BackOut) // Use easing for a nicer transition, even when using a slider
		.SetValuesAndDuration(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2);
}

public void OnValueChanged(float value) {
	// Set the progress when the slider changes
	m_Motion3D.Progress = value;
}
```

## All the Playback Objects: `Motion`, `Timer`, `Sequence`, `Parallel`

## Known Issues
