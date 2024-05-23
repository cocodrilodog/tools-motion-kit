<div style="background-color: #333; overflow: hidden;">
  <a href="../README.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Home</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="LifeCycle.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Life Cycle</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="Setter.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Setter</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <span style="float: left; display: block; color: white; padding: 14px 16px;"><b>▸Easing◂</b></span>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="Callbacks.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Callbacks</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="Playback.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Playback</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="RelativeValues.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Relative Values</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="TimerSequenceParallel.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Timer, Sequence, and Parallel</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="AnonymousPlaybackObjects.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Anonymous Playback</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="SharedAssets.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Shared Assets</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="BatchOperations.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Batch Operations</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
</div>

---

## Easing

The `Motion` objects animate properties between an `initialValue` and a `finalValue` for the specified `duration`. By default the animation will happen with constant speed, which in other words, uses a linear function to interpolate between the two values:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/e9adbf54-f34a-4e5e-b220-0c9b8cc44db7" width="300">

But the `Motion` objects receive an `easing` paramater that is a function which will accelerate/decelerate the animations in different ways.

A good place to visualize easing functions is [Robert Penner's website](https://easings.net/)

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

---

<nav>
  <ul>
    <li><a href="../README.md">Home</a></li>
    <li><a href="LifeCycle.md">Life Cycle</a></li>
    <li><a href="Setter.md">Setter</a></li>
    <li><b>▸Easing◂</b></li>
    <li><a href="Callbacks.md">Callbacks</a></li>
    <li><a href="Playback.md">Playback</a></li>
    <li><a href="RelativeValues.md">Relative Values</a></li>
    <li><a href="TimerSequenceParallel.md">Timer, Sequence, and Parallel</a></li>
    <li><a href="AnonymousPlaybackObjects.md">Anonymous Playback</a></li>
    <li><a href="SharedAssets.md">Shared Assets</a></li>
    <li><a href="BatchOperations.md">Batch Operations</a></li>
  </ul>
</nav>
