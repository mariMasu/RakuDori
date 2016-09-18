using UnityEngine;
using System.Collections;
using System;

public class DrillTime : MonoBehaviour {

	public static string GetLastTime (string lastTime)
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

		}else if(diff.TotalMinutes > 1){
			i = (int)diff.TotalMinutes;
			retT = (i + "分前");
		} else {
			i = (int)diff.TotalSeconds;
			retT = (i + "秒前");
		}


		return retT;
	}
}
