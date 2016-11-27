using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System;

public class DbTextToQA : MonoBehaviour
{

	static string[] AnsKeyCommon = { QuesTextEdit.AnsKeyCommon };
	static string[] DummyKeyCommon = { QuesTextEdit.DummyKeyCommon };
	static string[] ExpKeyCommon = { QuesTextEdit.ExpKeyCommon };
	static string[] SepKeyCommon = { QuesTextEdit.SepKeyCommon };


	public static QuesArray DbToQA (string raw)
	{

		//string[] rowQ = raw.Split (QuesKeyCommon, StringSplitOptions.RemoveEmptyEntries);

		//for (int i = 0; i > rowQ.Count; i++) {

		string q = "";
		List<string> a = new List<string> ();
		string e = "";
		List<string> d = new List<string> ();


		string[] rowQA = raw.Split (AnsKeyCommon, StringSplitOptions.RemoveEmptyEntries);

		q = rowQA [0];

		string[] rowAE = rowQA [1].Split (ExpKeyCommon, StringSplitOptions.RemoveEmptyEntries);

		if (rowAE.Length == 1) {

		} else {
			e = rowAE [1];

		}

		string[] rowAD = rowAE [0].Split (DummyKeyCommon, StringSplitOptions.RemoveEmptyEntries);

		if (rowAD.Length == 1) {

			
		} else {
			string[] rowDS = rowAD [1].Split (SepKeyCommon, StringSplitOptions.RemoveEmptyEntries);

			if (rowDS.Length == 1) {
				d.Add (rowDS [0]);
			} else {
				foreach (string s in rowDS) {
					d.Add (s);
				}

			}
		}

		string[] rowAS = rowAD [0].Split (SepKeyCommon, StringSplitOptions.RemoveEmptyEntries);

		if (rowAS.Length == 1) {
			a.Add (rowAS [0]);
		} else {
			foreach (string s in rowAS) {
				a.Add (s);
			}

		}
			

		//}

		QuesArray qa = new QuesArray ();
		qa.Ques = q;
		qa.Ans = a.ToArray ();
		qa.Dummy = d.ToArray ();
		qa.Exp = e;

		return qa;

	}
}
