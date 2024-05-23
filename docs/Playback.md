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

## Playback
### C#
#### Playback Methods
The `Motion` objects can be stored in variables and be controlled for playback with the methods `Play()`, `Pause()`, `Resume()` and `Stop()`:

```
Motion3D m_Motion3D;

void Start() {
	// Create the motion but don't play it, yet. Note that SetValuesAndDuration is
	// used instead of Play()
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

`Motion` objects can be controlled via their `Progress` property. `Progress` is a number that goes from 0 to 1 and changes the property by interpolating between the `initialValue` and the `finalValue`. One example where the `Progress` property is useful is if you want to create a slider that changes a property of an object by using a `Motion`, but you don't want to actually play the `Motion`:

```
Motion3D m_Motion3D;

void Start() {
	// Create the motion but don't play it at all. Note that SetValuesAndDuration is
	// used instead of Play()
	m_Motion3D = MotionKit.GetMotion(m_Ball, "Position", p => m_Ball.localPosition = p)
		.SetEasing(MotionKitEasing.BackOut) // Use easing for a nicer transition, even when using a slider
		.SetValuesAndDuration(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2);
}

public void OnValueChanged(float value) {
	// Set the progress when the slider changes.
	// The slider will make the animation to go to any part of the animation at any time,
	// between the initial and final values.
	m_Motion3D.Progress = value;
}
```
### Inspector

The playback of the `MotionKitBlock`s created in a `MotionKitComponent` can be controlled independently by their names:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/70b29d23-1530-4989-af2e-3122e944ab86" width="450">

This is an example of how to play each one:

```
m_MotionKitComponent.Play("Motion3D");
m_MotionKitComponent.Play("MotionFloat");
m_MotionKitComponent.Play("MotionColor");
```

The same will apply to `Pause()`, `Resume()`, `Stop()`.

The names can be customized when you edit each `MotionKitBlock`.

If you want more control over the actual `Motion` object that belongs to the wrapper `MotionKitBlock` you can obtain direct access to it by first getting the `MotionKitBlock` and then the `Motion` object:

```
// Get the MotionKitBlock that contains the `Motion3D`
Motion3DBlock motion3DBlock = m_MotionKitComponent.GetChild("Motion3D") as Motion3DBlock;

// Get the `Motion3D
var motion3D = motion3DBlock.Motion;

// Do anything that you need to do
motion3D.Progress = 0.5f;
```