using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System;

public class QuesTextEdit : MonoBehaviour
{

	public GameObject inputQuesText;

	GameObject es;
	SceneData sd;


	void Awake ()
	{
		es = GameObject.Find ("EventSystem");
		sd = es.GetComponent<SceneData> ();
	}

	public void QaSend ()
	{
		TextCheck ();
		List<string> q = TextToArray (sd.InputPat);

		if (q.Count == 0) {
			return;
		}

		foreach (string d in q) {
			Debug.Log (d);
		}
	}

	public List<string> TextToArray (int mode = 0)
	{

		List<string> q = new List<string> ();

		string text = inputQuesText.GetComponent<InputField> ().text;

		if (text.IndexOf (sd.QuesKey) == 0) {
			text = text.Substring (sd.QuesKey.Length);
		}

		if (mode == 0) {

			string[] rowText = Regex.Split (text, sd.QuesKey);
			//A＃a/B＃b＆bb/C＃c＃cc

			List<string> listRow = new List<string> ();
			List<string> listQ = new List<string> ();
			List<string> listA = new List<string> ();
			List<string> listD = new List<string> ();

			foreach (string s in rowText) {
				if (SceneData.strNull (s)) {
				} else {
					listRow.Add (s);
				}
			}

			if (listRow.Count % 2 != 0) {
				es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n問題のない正答か\n正答のない問題があります");
				return new List<string> ();
			}

			for (int i = 0; i < listRow.Count; i++) {
				if (i % 2 == 0) {
					listQ.Add (listRow [i]);
				} else {
					listA.Add (listRow [i]);
				}

			}

			if (SceneData.strNull (sd.DummyKey) == false) {
				for (int i = 0; i < listA.Count; i++) {
					string[] dum = { sd.DummyKey };
					string[] strD = listA [i].Split (dum, StringSplitOptions.RemoveEmptyEntries);
					if (strD.Length > 1) {
						listA [i] = strD [0];
						listD.Add (strD [1]);
					} else {
						listD.Add ("!!!");
					}
				}

			}

			RemoveEnter (ref listQ, false);

			if (SceneData.strNull (sd.SepKey) == false) {
				
				for (int i = 0; i < listQ.Count; i++) {
					
					string str = "###" + listQ [i];


					string[] rowAS = Regex.Split (listA [i], sd.SepKey);
					List<string> listAS = new List<string> ();

					foreach (string s in rowAS) {
						if (SceneData.strNull (s)) {
							break;
						} else {
							listAS.Add (RemoveEnterAll (s));
						}

					}
						

					str += "$$$" + MergeList (listAS, "&&&");

					if (listD.Count > 0 && listD [i] != "!!!") {
						string[] rowDS = Regex.Split (listD [i], sd.SepKey);
						List<string> listDS = new List<string> ();

						foreach (string s in rowDS) {
							if (SceneData.strNull (s)) {
							} else {
								listDS.Add (s);
							}
						}
						RemoveEnter (ref listDS);

						str += "%%%" + MergeList (listDS, "&&&");
					}
					q.Add (str);
				}
			} else {
				RemoveEnter (ref listA);

				for (int i = 0; i < listQ.Count; i++) {


					string str = "###" + listQ [i];

					str += "$$$" + listA [i];

					if (listD.Count > 0 && listD [i] != "!!!") {
						str += "%%%" + RemoveEnterAll (listD [i]);
					}
					q.Add (str);
				}
			}
		} else {
			
		}

		return q;
	}

	public bool TextCheck ()
	{
		string text = inputQuesText.GetComponent<InputField> ().text;
		string[] used = { "###", "$$$", "%%%", "&&&", ";;;" };
		int f = 0;

		foreach (string s in used) {
			if (text.IndexOf (s) > -1) {
				f++;
			}
		}

		if (f != 0) {
			es.GetComponent<PopupWindow> ().PopupCaution ("エラー\nテキスト内に###,$$$,\n%%%,&&&,;;;は使えません");
			return false;
		}

		return true;
	}

	public string MergeList (List<string> list, string key)
	{
		string mlist = "";
		foreach (string s in list) {
			mlist += key + s;
		}

		return mlist.Substring (key.Length);
	}

	public string RemoveEnterZengo (string s)
	{
		string kaigyo = "\n";

		while (s.Substring (0, kaigyo.Length - 1) == kaigyo) {
			s = s.Substring (kaigyo.Length);
		}
		while (s.Substring (s.Length - kaigyo.Length) == kaigyo) {
			s = s.Substring (0, s.Length - kaigyo.Length - 1);
		}

		return s;

	}

	public string RemoveEnterAll (string s)
	{
		string kaigyo = "\n";
		string ns = "";
		string[] spl = Regex.Split (s, kaigyo);

		foreach (string sp in spl) {
			if (SceneData.strNull (sp)) {
				break;
			} else {
				ns += sp;
			}
		}

		return ns;

	}

	public void RemoveEnter (ref List<string> list, bool b = true)
	{

		for (int i = 0; i < list.Count; i++) {
			if (b) {
				list [i] = RemoveEnterAll (list [i]);
			} else {
				list [i] = RemoveEnterZengo (list [i]);
			}
		}

	}

	public void RemoveEnter (ref string[] list, bool b = true)
	{
		
		for (int i = 0; i < list.Length; i++) {
			if (b) {
				list [i] = RemoveEnterAll (list [i]);
			} else {
				list [i] = RemoveEnterZengo (list [i]);
			}
		}

	}
}
