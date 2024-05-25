<div style="background-color: #333; overflow: hidden;">
  <a href="../README.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Home</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <span style="float: left; display: block; color: white; padding: 14px 16px;"><b>▸Life Cycle◂</b></span>
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

## Lifecycle: `owner`, `reuseID`, and Clearance

`Motion`, `Timer`, `Sequence`, and `Parallel` objects (collectibly named as `PlaybackObject`s) can be reused for optimal results with a very convenient API. 

For example, you can create a motion that animates the position of an object every time the player clicks the mouse button and moves the object to the clicked location. Instead of creating a new `Motion` every time, you can reuse only one `Motion` object to carry out all the animations. This has a very convenient side effect: It will stop the current movement and start a new one towards the new destination point automatically. You wouldn't need to stop or destroy the current one. (A big advantage over DOTween).

This is what `owner` and `reuseID` are for. `PlaybackObject`s are stored internally assotiated with an owner object and an id string when they are created. When invoking `MotionKit.GetMotion(someOwner, "someReuseID", ...` repeatedly, this line of code will use and return the same `Motion` object always.

Example of owners and reuse IDs:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/d3c5c867-31e2-489d-92fd-b98febc89007" width="600">

### C#
#### `owner` and `reuseID`
In the code below, the `owner` is the `m_Ball` and the `reuseID` is `"Position"`. In order to move the ball many times you may want to create only one `Motion3D` and reuse it as much as possible instead of creating a new one each time. So if the code below is excecuted several times in your script with different `Vector3` values and durations, you can rest assured that the same `Motion3D` object will be used for all the position animations as long as you use the same `owner` and `reuseID`.
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

---

<nav>
  <ul>
    <li><a href="../README.md">Home</a></li>
    <li><b>▸Life Cycle◂</b></li>
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
