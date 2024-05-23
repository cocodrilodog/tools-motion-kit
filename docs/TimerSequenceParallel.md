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
  <span style="float: left; display: block; color: white; padding: 14px 16px;"><b>▸Timer, Sequence, and Parallel◂</b></span>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="AnonymousPlaybackObjects.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Anonymous Playback</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="SharedAssets.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Shared Assets</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="BatchOperations.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Batch Operations</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
</div>

---

## The Other Objects: `Timer`, `Sequence`, `Parallel`

The `MotionKit` has other objects that allow to build complex animation composites: `Timer`, `Sequence` and `Parallel`. These 3 objects and the `Motion` objects are referred to as `Playback` objects. 

Here is a table of the 4 of them:

Object | Description | Features
-------- | -------- | --------
`Motion` | Animates any property. | `owner` and `reuseID`, `setter`, `easing`, `callbacks`, playback methods
`Timer` | Waits for a duration and then ends. | `owner` and `reuseID`, `callbacks`, playback methods 
`Sequence` | Plays the contained `Playbacks` in sequence. | `owner` and `reuseID`, `easing`, `callbacks`, playback methods
`Parallel` | Plays the contained `Playbacks` in parallel. | `owner` and `reuseID`, `easing`, `callbacks`, playback methods

Example of a sequence:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/94200d8e-f57a-4a05-a5af-2fd153036b2c" width="300">

Example of a Parallel:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/33966d53-3297-4c29-8a06-08490adc3a89" width="300">

Example of a more complex composite:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/c437bcd2-5c38-4ed0-a744-da22c94f148d" width="300">

### C#

Timer Example:

```
MotionKit.GetTimer(this, "SomeTimer").Play(5)
	.SetOnComplete(() => Debug.Log("Timer completed after 5 seconds"));
```

Sequence Example:

```
// The nested Motion objects will play one after the other.
MotionKit.GetSequence(this, "SomeSequence",
	MotionKit.GetMotion(this, "MoveBall", p => m_Ball.localPosition = p).SetValuesAndDuration(new Vector3(0, 0, 0), new Vector3(3, 0, 0), 2),
	MotionKit.GetMotion(this, "FadeInCanvas", a => m_CanvasRenderer.SetAlpha(a)).SetValuesAndDuration(0, 1, 2),
	MotionKit.GetTimer(this, "Pause").SetDuration(5), // Make a 5 seconds pause here
	MotionKit.GetMotion(this, "MakeRed", c => m_Image.color = c).SetValuesAndDuration(Color.black, Color.red, 2)
).SetEasing(MotionKitEasing.QuadInOut) // Easing will be applied to the sequence as a whole
.Play(); 
```

Parallel Example:

```
// The nested Motion objects will play starting at the same time.
MotionKit.GetParallel(this, "SomeParallel",
	MotionKit.GetMotion(this, "MoveBall", p => m_Ball.localPosition = p).SetValuesAndDuration(Vector3.zero, Vector3.right * 3, 2),
	MotionKit.GetMotion(this, "FadeInCanvas", a => m_CanvasRenderer.SetAlpha(a)).SetValuesAndDuration(0, 1, 2),
	MotionKit.GetMotion(this, "MakeRed", c => m_Image.color = c).SetValuesAndDuration(Color.black, Color.red, 2)
).SetEasing(MotionKitEasing.QuadInOut)
.Play();
```

### Inspector

When creating nested `MotionKit` animations in the Unity inspector, there is no limit. You can create any composite structure that suits your animation needs. This is an example of an `MotionKitComponent` that has many nested `MotionKitBlocks`:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/129fcbf1-1a2c-44c5-bd8e-954a3b25586b" width="600">

In the top area you will find a very powerful breadcrumb navigation system where you can go to any parent in the hierarchy and also choose any sibling of the currently selected `MotionKitBlock`.

In this example, we are seeing the editor of a `ParallelBlock`, which contain 7 child sequences. Whenever you click `Edit` on any of those, you'll navigate deeper in the hierarchy.

In the bottom part of the image, you can see a section called `Batch Operations` which are some editing actions that can be performed in all children `MotionKitBlock`s with one click. For example, setting an incremental duration. More documentation on this will come later!

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
    <li><b>▸Timer, Sequence, and Parallel◂</b></li>
    <li><a href="AnonymousPlaybackObjects.md">Anonymous Playback</a></li>
    <li><a href="SharedAssets.md">Shared Assets</a></li>
    <li><a href="BatchOperations.md">Batch Operations</a></li>
  </ul>
</nav>
