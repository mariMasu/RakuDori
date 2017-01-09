using UnityEngine;
using System.Runtime.InteropServices;

public static class ObC
{

	[DllImport ("__Internal")]
	static extern void CleanIconBadge (int i);

	public static void SetBadge (int i)
	{
		#if UNITY_IOS
		CleanIconBadge (i);
		#endif
	}
}