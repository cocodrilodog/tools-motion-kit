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
		All = 1,
		Easing = 2,
		OnStart = 4,
		OnUpdate = 8,
		OnInterrupt = 16,
		OnComplete = 32
	}

}
