using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Text;

public class Statics : MonoBehaviour
{

	public static int nowDrill = 0;
	public static bool nowSevere = false;
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

	public static bool StringToBool (string s)
	{
		if (s == "0") {
			return false;
		} else if (s == "1") {
			return true;
		} else {
			Debug.Log ("err");
			return false;
		}
	}

	public static Sprite pathToSprite (string str)
	{

		string path = System.IO.Path.Combine (Application.persistentDataPath, str);

		byte[] bytesRead;

		try {
			bytesRead = System.IO.File.ReadAllBytes (path);

		} catch (System.Exception ex) {
			Debug.Log (ex);
			return null;
		}

		Texture2D tex = new Texture2D (1024, 1024);
		tex.LoadImage (bytesRead);

		Sprite texture = SpriteFromTex2D (tex);

		return texture;


	}

	public static Sprite SpriteFromTex2D (Texture2D texture)
	{
		return Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
	}

	public static bool KindOfSpace (string str)
	{
		string[] space = {
			" ",
			"　",
			"\t",
			" ",
			"\u2000",
			"\u2001",
			"\u2002",
			"\u2003",
			"\u2004",
			"\u2005",
			"\u2006",
			"\u2007",
			"\u2008",
			"\u2009",
			"\u200a",
			"\u202f",
			"\u205f"
		};

		foreach (string s in space) {
			if (str == s) {
				return true;
			}
		}

		return false;


	}
}
