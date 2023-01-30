namespace CocodriloDog.Animation {

	using CocodriloDog.Core;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	/// <summary>
	/// This is the component that will be used to contain and manage <see cref="AnimateComponent"/>s.
	/// </summary>
	/// <remarks>
	/// Its interface has been designed for ease of use by <c>UnityEvents</c>.
	/// </remarks>
	public class AnimateBehaviour : MonoCompositeRoot {


		#region Public Properties

		/// <summary>
		/// The first <see cref="AnimateComponent"/> of this <see cref="AnimateBehaviour"/> if any, or <c>null</c>.
		/// </summary>
		public AnimateComponent DefaultAnimateComponent => AnimateComponentFields.Count > 0 ? AnimateComponentFields[0].Object : null;

		#endregion


		#region Public Methods

		/// <summary>
		/// Plays the <see cref="DefaultAnimateComponent"/>.
		/// </summary>
		public void Play() {
			if(DefaultAnimateComponent != null) {
				DefaultAnimateComponent.Play();
			}
		}

		/// <summary>
		/// Plays the <see cref="AnimateComponent"/> with the specified <paramref name="componentName"/>.
		/// </summary>
		/// <param name="componentName">The name of the component.</param>
		public void Play(string componentName) {
			var component = GetAnimateComponent(componentName);
			if (component != null) {
				component.Play();
			}
		}

		/// <summary>
		/// Plays all the <see cref="AnimateComponent"/>s managed by this behaviour.
		/// </summary>
		public void PlayAll() => AnimateComponentFields.ForEach(af => {
			if (af.Object != null) {
				af.Object.Play();
			}
		});

		/// <summary>
		/// Stops the <see cref="DefaultAnimateComponent"/>.
		/// </summary>
		public void Stop() {
			if (DefaultAnimateComponent != null) {
				DefaultAnimateComponent.Stop();
			}
		}

		/// <summary>
		/// Stops the <see cref="AnimateComponent"/> with the specified <paramref name="componentName"/>.
		/// </summary>
		/// <param name="componentName">The name of the component.</param>
		public void Stop(string componentName) {
			var component = GetAnimateComponent(componentName);
			if (component != null) {
				component.Stop();
			}
		}

		/// <summary>
		/// Stops all the <see cref="AnimateComponent"/>s managed by this behaviour.
		/// </summary>
		public void StopAll() => AnimateComponentFields.ForEach(af => {
			if (af.Object != null) {
				af.Object.Stop();
			}
		});

		/// <summary>
		/// Pauses the <see cref="DefaultAnimateComponent"/>.
		/// </summary>
		public void Pause() {
			if (DefaultAnimateComponent != null) {
				DefaultAnimateComponent.Pause();
			}
		}

		/// <summary>
		/// Pauses the <see cref="AnimateComponent"/> with the specified <paramref name="componentName"/>.
		/// </summary>
		/// <param name="componentName">The name of the component.</param>
		public void Pause(string componentName) {
			var component = GetAnimateComponent(componentName);
			if (component != null) {
				component.Pause();
			}
		}

		/// <summary>
		/// Pauses all the <see cref="AnimateComponent"/>s managed by this behaviour.
		/// </summary>
		public void PauseAll() => AnimateComponentFields.ForEach(af => {
			if (af.Object != null) {
				af.Object.Pause();
			}
		});

		/// <summary>
		/// Resumes the <see cref="DefaultAnimateComponent"/>.
		/// </summary>
		public void Resume() {
			if (DefaultAnimateComponent != null) {
				DefaultAnimateComponent.Resume();
			}
		}

		/// <summary>
		/// Resumes the <see cref="AnimateComponent"/> with the specified <paramref name="componentName"/>.
		/// </summary>
		/// <param name="componentName">The name of the component.</param>
		public void Resume(string componentName) {
			var component = GetAnimateComponent(componentName);
			if (component != null) {
				component.Resume();
			}
		}

		/// <summary>
		/// Resumes all the <see cref="AnimateComponent"/>s managed by this behaviour.
		/// </summary>
		public void ResumeAll() => AnimateComponentFields.ForEach(af => {
			if (af.Object != null) {
				af.Object.Resume();
			}
		});

		/// <summary>
		/// Resets the default motion.
		/// </summary>
		public void ResetMotion() {
			if (DefaultAnimateComponent != null) {
				if(DefaultAnimateComponent is IMotionBaseComponent) {
					((IMotionBaseComponent)DefaultAnimateComponent).ResetMotion();
				}
			}
		}

		/// <summary>
		/// Resets the <see cref="AnimateComponent"/> with the specified <paramref name="componentName"/>
		/// if it is a motion.
		/// </summary>
		/// <param name="componentName">The name of the component.</param>
		public void ResetMotion(string componentName) {
			var component = GetAnimateComponent(componentName);
			if (component != null && component is IMotionBaseComponent) {
				((IMotionBaseComponent)component).ResetMotion();
			}
		}

		/// <summary>
		/// Calls <see cref="IMotionBaseComponent.ResetMotion()"/> in all the motions that it finds.
		/// </summary>
		public void ResetAllMotions() => AnimateComponentFields.ForEach(af => _ResetMotion(af.Object));

		/// <summary>
		/// Disposes all the <see cref="AnimateComponent"/>s of this compopnent.
		/// </summary>
		public void Dispose() => AnimateComponentFields.ForEach(af => {
			if (af.Object != null) {
				af.Object.Dispose();
			}
		});

		/// <summary>
		/// Gets the <see cref="AnimateComponent"/> named <paramref name="componentName"/>.
		/// </summary>
		/// <remarks>
		/// This method can be used for further control over the managed <see cref="AnimateComponent"/>s.
		/// </remarks>
		/// <param name="componentName">The <see cref="AnimateComponent.ObjectName"/></param>
		/// <returns>The <see cref="AnimateComponent"/></returns>
		public AnimateComponent GetAnimateComponent(string componentName) {
			var assetField = AnimateComponentFields.FirstOrDefault(af => af.Object != null && af.Object.ObjectName == componentName);
			return assetField?.Object;
		}

		public override MonoCompositeFieldBase[] GetChildren() {
			return new List<MonoCompositeFieldBase>(AnimateComponentFields).ToArray();
		}

		public override void ConfirmChildren() {
			foreach (var field in GetChildren()) {
				if (field.ObjectBase != null) {
					field.ObjectBase.SetParent(this);
				}
			}
		}

		#endregion


		#region Unity Methods

		private void Start() {
			if (PlayAllOnStart) {
				PlayAll();
			} else {
				// This avoids errors OnDestroy in case it was not played at all.
				Initialize();
			}
		}

		private void OnDestroy() {
			Dispose();
		}

		#endregion


		#region Private Fields

		/// <summary>
		/// A list of <see cref="AnimateComponentField"/>.
		/// </summary>
		/// <remarks>
		/// This name is friendlier than m_AnimateAssetFields in the inspector.
		/// </remarks>
		[SerializeField]
		private List<AnimateComponentField> m_AnimateComponents = new List<AnimateComponentField>();

		[SerializeField]
		private bool m_PlayAllOnStart;

		#endregion


		#region Private Properties		

		private List<AnimateComponentField> AnimateComponentFields => m_AnimateComponents;

		private bool PlayAllOnStart => m_PlayAllOnStart;

		#endregion


		#region Private Methods

		/// <summary>
		/// Initializes all the <see cref="AnimateComponent"/>s of this compopnent.
		/// </summary>
		private void Initialize() => AnimateComponentFields.ForEach(af => {
			if (af.Object != null) {
				af.Object.Initialize();
			}
		});

		private void _ResetMotion(AnimateComponent animateComponent) {
			if(animateComponent == null) {
				return;
			}
			if (animateComponent is IMotionBaseComponent) {
				var motionComponent = animateComponent as IMotionBaseComponent;
				motionComponent.ResetMotion();
			}
			if (animateComponent is SequenceComponent) {
				var sequenceAsset = animateComponent as SequenceComponent;
				foreach (var itemField in sequenceAsset.SequenceItemsFields) {
					// Recursion
					_ResetMotion(itemField.Object);
				}
			}
			if (animateComponent is ParallelComponent) {
				var parallelAsset = animateComponent as ParallelComponent;
				foreach (var itemField in parallelAsset.ParallelItemsFields) {
					// Recursion
					_ResetMotion(itemField.Object);
				}
			}
		}

		#endregion


	}

}