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
	public static List<int> reviewList = new List<int> ();

	public static bool strNull (string s)
	{
		if (s == "" || s == null) {
			return true;
		} else {
			return false;
		}
	}

}
