<div style="background-color: #333; overflow: hidden;">
  <a href="../README.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Home</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="docs/LifeCycle.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Life Cycle</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="docs/Setter.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Setter</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="docs/Easing.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Easing</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="docs/Callbacks.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Callbacks</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="docs/Playback.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Playback</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="docs/RelativeValues.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Relative Values</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="docs/TimerSequenceParallel.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Timer, Sequence, and Parallel</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="docs/AnonymousPlaybackObjects.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Anonymous Playback</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="docs/SharedAssets.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Shared Assets</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="docs/BatchOperations.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Batch Operations</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
</div>

---

## Setter: `Vector3`, `Float`, `Color`
### C#
Every motion needs a `setter` to apply changes to the property that we want to animate. In our example, the setter is `p => m_Ball.localPosition = p`, which will animate the position of the ball:
```
MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
	.Play(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2);
```
It is a lamba expression that receives a `Vector3` parameter and then applies it as needed. It could also be a function, but it would need more lines of code:
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

The `Motion` objects animate properties between an `initialValue` and a `finalValue` for the specified `duration`. By default the animation will happen with constant speed, which in other words, uses a linear function to interpolate between the two values:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/e9adbf54-f34a-4e5e-b220-0c9b8cc44db7" width="300">

But the `Motion` objects receive an `easing` paramater that is a function which will accelerate/decelerate the animations in different ways.

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
In the `MotionKitComponent` you just assign an object to the setter field, and then choose from a list of easing functions:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/c2f82556-5722-428a-9c2a-dbde95b32d91" height="400">  
<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/c7c3cea4-d6fb-4505-8548-e32e49caf8da" height="400">

And if you choose from some of the `ParameterizedEasing` functions (`AnimationCurve`, `Blink`, `Pulse` or `Shake`), additional parameters will appear on the inspector:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/ee1c4990-a3d8-438e-9a4b-649d1ee436b0" width="400">