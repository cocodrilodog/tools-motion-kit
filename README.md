<div style="background-color: #333; overflow: hidden;">
  <a href="README.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Home</a>
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

# MotionKit Guide

MotionKit is a Unity engine tool that animates anything. It is very similar to [DOTween](http://dotween.demigiant.com/getstarted.php), but with inspector super powers and a few tweaks that makes it simpler to use and learn.

([Watch overview on YouTube](http://www.youtube.com/watch?v=1knaaxQQs3I))

<a href="http://www.youtube.com/watch?feature=player_embedded&v=1knaaxQQs3I
" target="_blank"><img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/baeb690d-5465-443e-b246-d1ea51f39611" 
alt="IMAGE ALT TEXT HERE" width="300" border="10" /></a>

## How to Install
`MotionKit` depends on the Cocodrilo Dog `Core` tool. You need to install this dependency first, via the Unity Package Manager.

To install both in your Unity project, open the Package Manager and click the plus button, "Add package from URL..." and the use these URLs:
- Cocodrilo Dog Core: https://github.com/cocodrilodog/tools-core.git
- MotionKit: https://github.com/cocodrilodog/tools-motion-kit.git

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

On the other hand, the classes that handle `Motion` objects via inspector are collectibly named `MotionKitBlock`s. They are wrappers of `Motion` objects. The `MotionKitBlocks` are created and arranged on the `MotionKitComponent` which is a `MonoBehaviour` designed to use the `MotionKit` from the Unity inspector.

To create the same example as the `m_Ball` code above via inspector, add a `MotionKitComponent`:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/ec29de39-63d8-431c-8641-e7d8d4627065" height="120">

Create a `Motion3DBlock`:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/3da577ab-569a-432e-9268-681f3cdb9861" width="400">

Tick `Play on start` or play later with `MotionKitComponent.Play()`:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/79888941-bbc2-48b8-a5b2-ab77f89e2914" width="400">

Click `Edit` and assign the relevant properties:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/c080c21a-644b-495d-a7de-2ce0eba601ca" width="400">

To replicate the examples of the `m_CanvasRenderer` and `m_Image` code above via inspector, it is the same process, but you create `MotionFloatBlock` to animate the `m_CanvasRenderer.alpha` property, and `MotionColorBlock` to animate the `m_Image.color`.

In the `MotionKitComponent`, many `MotionKitBlock`s can be created and handled independently:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/74da028c-fba9-42bd-bd71-e65cdf666210" width="400">

---

## Documentation

You can read and experiment in your own project by following the documentation laid out below to gain deep insights about this animation engine. The documents are designed so that you can study one by one in the order of the list. Have fun!

<nav>
  <ul>
    <li><a href="README.md">Home</a></li>
    <li><a href="docs/LifeCycle.md">Life Cycle</a></li>
    <li><a href="docs/Setter.md">Setter</a></li>
    <li><a href="docs/Easing.md">Easing</a></li>
    <li><a href="docs/Callbacks.md">Callbacks</a></li>
    <li><a href="docs/Playback.md">Playback</a></li>
    <li><a href="docs/RelativeValues.md">Relative Values</a></li>
    <li><a href="docs/TimerSequenceParallel.md">Timer, Sequence, and Parallel</a></li>
    <li><a href="docs/AnonymousPlaybackObjects.md">Anonymous Playback</a></li>
    <li><a href="docs/SharedAssets.md">Shared Assets</a></li>
    <li><a href="docs/BatchOperations.md">Batch Operations</a></li>
  </ul>
</nav>
