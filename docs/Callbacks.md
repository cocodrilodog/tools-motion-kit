<div style="background-color: #333; overflow: hidden;">
  <a href="../README.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Home</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="LifeCycle.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Life Cycle</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="Setter.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Setter</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="Easing.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Easing</a>
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

Note that the callbacks that receive the `Motion` object as a parameter are not available from the inspector.
---

<nav>
  <ul>
    <li><a href="../README.md">Home</a></li>
    <li><a href="LifeCycle.md">Life Cycle</a></li>
    <li><a href="Setter.md">Setter</a></li>
    <li><a href="Easing.md">Easing</a></li>
    <li><a href="Callbacks.md">Callbacks</a></li>
    <li><a href="Playback.md">Playback</a></li>
    <li><a href="RelativeValues.md">Relative Values</a></li>
    <li><a href="TimerSequenceParallel.md">Timer, Sequence, and Parallel</a></li>
    <li><a href="AnonymousPlaybackObjects.md">Anonymous Playback</a></li>
    <li><a href="SharedAssets.md">Shared Assets</a></li>
    <li><a href="BatchOperations.md">Batch Operations</a></li>
  </ul>
</nav>