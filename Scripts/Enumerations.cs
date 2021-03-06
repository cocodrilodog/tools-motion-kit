namespace CocodriloDog.Animation {

	using System;

	public enum TimeMode {
		Normal,
		Unscaled,
		Smooth,
		Fixed,
		FixedUnscaled
	}

	// TODO: Implement an All option
	// https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute?view=netcore-3.1
	[Flags]
	public enum CleanFlag {
		None = 0,
		Easing = 1,
		OnStart = 2,
		OnUpdate = 4,
		OnInterrupt = 8,
		OnComplete = 16
	}

}
