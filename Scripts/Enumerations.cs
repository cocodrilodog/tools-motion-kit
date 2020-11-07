namespace CocodriloDog.Animation {

	using System;

	public enum TimeMode {
		Normal,
		Unscaled,
		Smooth,
		Fixed,
		FixedUnscaled
	}

	// https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute?view=netcore-3.1
	[Flags]
	public enum CleanFlag {
		None = 0,
		Easing = 1,
		OnUpdate = 2,
		OnInterrupt = 4,
		OnComplete = 8
	}

}
