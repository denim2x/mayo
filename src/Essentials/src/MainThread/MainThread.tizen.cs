using System;
using ElmSharp;

namespace Microsoft.Maui.ApplicationModel
{
	public static partial class MainThread
	{
		static void PlatformBeginInvokeOnMainThread(Action action)
		{
			if (PlatformIsMainThread)
				action();
			else
				EcoreMainloop.PostAndWakeUp(action);
		}

		static bool PlatformIsMainThread
			=> EcoreMainloop.IsMainThread;
	}
}
