<div style="background-color: #333; overflow: hidden;">
  <a href="../README.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Home</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <a href="LifeCycle.md" style="float: left; display: block; color: white; text-align: center; padding: 14px 16px; text-decoration: none;">Life Cycle</a>
  <span style="float: left; display: block; color: white; padding: 14px 16px;">|</span>
  <span style="float: left; display: block; color: white; padding: 14px 16px;"><b>▸Setter◂</b></span>
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

Once the `MotionKitBlock` is created, you need to assign an object in the setter field, and then the inspector will allow you to choose among all properties and methods of that object that suits the type of `MotionKitBlock`. In this example, all the listed methods and properties receive a `Vector3` parameter:

<img src="https://github.com/cocodrilodog/tools-motion-kit/assets/8107813/40b256b1-4204-428c-a77d-e15d9179adf6" width="400">

---

<nav>
  <ul>
    <li><a href="../README.md">Home</a></li>
    <li><a href="LifeCycle.md">Life Cycle</a></li>
    <li><b>▸Setter◂</b></li>
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
