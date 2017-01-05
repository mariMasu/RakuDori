using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;


public class Statics : MonoBehaviour
{

	public static int nowDrill = 0;
	public static int nowColor = 0;
	public static string nowName = "";
	public static int ansChoose = 0;
	public static int nowTag = 0;
	public static int nowLevel = 0;

	public static List<int> reviewList = new List<int> ();
	public static float prefabGap = 0;


	public static void ResetReviewList ()
	{
		reviewList = new List<int> ();
	}

	public static void AddReviewList ()
	{
		reviewList.Add (nowDrill);
	}

	public static bool HaveKaigyo (string s)
	{
		if (0 <= s.IndexOf ("\n")) {
			return true;
		} else {
			return false;

		}
	}

	public static bool StrNull (string s)
	{
		if (s == "" || s == null) {
			return true;
		} else {
			return false;
		}
	}

}
