using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Text;

public class Statics : MonoBehaviour
{

	public static int nowDrill = 0;
	public static int nowColor = 0;
	public static string nowName = "";
	public static int ansChoose = 0;
	public static int nowTag = 0;
	public static int nowLevel = 0;

	public static bool youhukusyu = false;

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

	public static int[] GetByteLengthSJis (char[] chars)
	{

		Encoding utfEnc = Encoding.GetEncoding ("utf-8");
		int[] charsByte = new int[chars.Length];

		for (int i = 0; i < chars.Length; i++) {
			int bytec = utfEnc.GetByteCount (chars, i, 1);
			if (bytec > 1) {
				bytec = 2;
			}

			charsByte [i] = bytec;
		}

		return charsByte;

	}

	public static int IntSum (int[] ints)
	{

		int sum = 0;
		for (int i = 0; i < ints.Length; i++) {
			sum += ints [i];
		}

		return sum;
	}

}
