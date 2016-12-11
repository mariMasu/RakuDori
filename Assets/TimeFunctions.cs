using UnityEngine;
using System.Collections;
using System;

public class TimeFunctions : MonoBehaviour
{

	public static string GetLastString (string lastTime)
	{
		string retT;
		DateTime last = DateTime.Parse (lastTime);
		TimeSpan diff = DateTime.Now.Subtract (last);

		int i;

		if (diff.TotalDays > 31) {
			i = (int)(diff.TotalDays / 30);
			retT = (i + "ヶ月前");
		} else if (diff.TotalDays > 1) {
			i = (int)diff.TotalDays;
			retT = (i + "日前");
		} else if (diff.TotalHours > 1) {
			i = (int)diff.TotalHours;
			retT = (i + "時間前");

		} else if (diff.TotalMinutes > 1) {
			i = (int)diff.TotalMinutes;
			retT = (i + "分前");
		} else {
			i = (int)diff.TotalSeconds;
			retT = (i + "秒前");
		}


		return retT;
	}

	public static bool NeedReview (string lastTime, int level)
	{
		DateTime last = DateTime.Parse (lastTime);
		TimeSpan diff = DateTime.Now.Subtract (last);

		int gapD = (int)diff.TotalDays;

		bool r = false;

		if (level == 0) {
			r = true;
		} else if (level == 1) {
			if (gapD > 0) {
				r = true;
			}
		} else if (level == 2) {
			if (gapD > 6) {
				r = true;
			}
		} else if (level == 3) {
			if (gapD > 29) {
				r = true;
			}
		}


		return r;
	}

	public static string GetTimeID ()
	{
		string retT;
		DateTime nt = DateTime.Now;

		retT = nt.ToLongTimeString ();

		return retT;
	}
}
