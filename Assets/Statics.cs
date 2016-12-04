using UnityEngine;
using System.Collections;

public class Statics : MonoBehaviour
{

	public static int nowDrill = 0;
	public static int nowColor = 0;
	public static string nowName = "";

	public static bool strNull (string s)
	{
		if (s == "" || s == null) {
			return true;
		} else {
			return false;
		}
	}

}
