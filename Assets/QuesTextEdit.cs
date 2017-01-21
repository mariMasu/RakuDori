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

	public GameObject viewContent;
	public GameObject viewText;

	GameObject es;
	QuesInputMaster sd;

	List<string> q;

	//string QuesKeyCommon = "###";
	public static string AnsKeyCommon = "$$$";
	public static string ExpKeyCommon = ":::";
	public static string DummyKeyCommon = "%%%";
	public static string SepKeyCommon = "&&&";
	public static string PerKeyCommon = ";;;";
	public static string NullKeyCommon = "!!!";

	public static string Kaigyo = "\n";

	void Awake ()
	{
		es = GameObject.Find ("EventSystem");
		sd = es.GetComponent<QuesInputMaster> ();

		viewText.GetComponent<Text> ().text = "";
		InputTextView ();
	}

	public void QaTest ()
	{
		TextCheck ();
		q = TextToArray (sd.inputPat);

		if (q.Count == 0) {
			return;
		}

		es.GetComponent<QuesInputMaster> ().TestQuesView (q);

		es.GetComponent<PopupWindow> ().Popup (3);
	}

	public void ImportQaTest (string[] stra)
	{
		q = new List<string> ();
		q.AddRange (stra);

		es.GetComponent<QuesInputMaster> ().TestQuesView (q);

		es.GetComponent<PopupWindow> ().Popup (3);
	}

	public void QaSend ()
	{
		if (q == null || q.Count == 0) {
			return;
		}

		es.GetComponent<DbProcess> ().AddFirst (q);
		es.GetComponent<LoadButton> ().LoadQuesSentaku ();
	}

	public List<string> TextToArray (int mode = 0)
	{

		string text = inputQuesText.GetComponent<InputField> ().text;
		string sentaku;

		List<string> q = new List<string> ();

		string[] que = { sd.quesKey };
		string[] rowText = text.Split (que, StringSplitOptions.RemoveEmptyEntries);

		List<string> listRow = new List<string> ();
		List<string> listQ = new List<string> ();
		List<string> listA = new List<string> ();
		List<string> listD = new List<string> ();
		List<string> listE = new List<string> ();

		foreach (string s in rowText) {
			if (s != "\n") {
				listRow.Add (s);
			}
		}

		//選択肢があればダミー用を作成
		if (Statics.StrNull (sd.sentaku) == false) {
			string[] kanma = { "," };
			string[] sen = sd.sentaku.Split (kanma, StringSplitOptions.RemoveEmptyEntries);

			AnswerToHankaku (ref sen);
			sentaku = sen [0];

			for (int i = 1; i < sen.Length; i++) {
				sentaku += SepKeyCommon + sen [i];
			}

		} else {
			sentaku = "";
		}
		//問題ー解答ー問題ー、全問題ー全回答
		if (mode < 2) {

			if (listRow.Count % 2 != 0) {
				es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n問題のない正答か\n正答のない問題があります");
				return new List<string> ();
			}
			if (sd.inputPat == 0) {
				for (int i = 0; i < listRow.Count; i++) {
					if (i % 2 == 0) {
						listQ.Add (listRow [i]);
					} else {
						listA.Add (listRow [i]);
					}

				}
			} else {
				for (int i = 0; i < listRow.Count; i++) {
					if (i <= (listRow.Count / 2) - 1) {
						listQ.Add (listRow [i]);
					} else {
						listA.Add (listRow [i]);
					}

				}
			}

			//ダミーかあれば分離
			SeparateDummy (ref listA, ref listD);

			RemoveEnter (ref listQ, false);

			//完全一致コマンドの置き換え
			ConvertPerfectCommand (ref listQ);

			if (Statics.StrNull (sd.sepKey) == false) {

				//解答群の分割
				for (int i = 0; i < listQ.Count; i++) {
					
					string str = listQ [i];

					string[] sep = { sd.sepKey };
					string[] rowAS = listA [i].Split (sep, StringSplitOptions.RemoveEmptyEntries);

					List<string> listAS = new List<string> ();

					foreach (string s in rowAS) {
						listAS.Add (RemoveEnterAll (s));
					}

					AnswerToHankaku (ref listAS);
						
					//### Q $$$ A &&& A &&& A
					str += AnsKeyCommon + MergeList (listAS, SepKeyCommon);

					//ダミー解答の分割
					if (listD.Count > 0 && listD [i] != NullKeyCommon) {
						string[] rowDS = listD [i].Split (sep, StringSplitOptions.RemoveEmptyEntries);
						List<string> listDS = new List<string> ();

						foreach (string s in rowDS) {
							listDS.Add (s);
						}
						RemoveEnter (ref listDS);

						//### Q $$$ A &&& A &&& A %%% D &&& D &&& D
						str += DummyKeyCommon + MergeList (listDS, SepKeyCommon);


						//選択肢があれば結合
						if (sentaku.Length > 0) {
							str += SepKeyCommon + sentaku;
						}

					} else {
						if (sentaku.Length > 0) {
							str += DummyKeyCommon + sentaku;
						}
					}
					q.Add (str);
				}
			} else {
				RemoveEnter (ref listA);
				AnswerToHankaku (ref listA);

				for (int i = 0; i < listQ.Count; i++) {


					string str = listQ [i];

					str += AnsKeyCommon + listA [i];

					if (listD.Count > 0 && listD [i] != NullKeyCommon) {
						str += DummyKeyCommon + RemoveEnterAll (listD [i]);

						if (sentaku.Length > 0) {
							str += SepKeyCommon + sentaku;
						}

					} else {

						if (sentaku.Length > 0) {
							str += DummyKeyCommon + sentaku;
						}
					}

					q.Add (str);
				}
			}




			//問題ー正答ー解説ー問題
		} else if (mode == 2) {

			for (int i = 0; i < listRow.Count; i++) {
				
				string[] sep = { sd.ansKey };
				string[] rowAS = listRow [i].Split (sep, StringSplitOptions.RemoveEmptyEntries);

				if (rowAS.Length > 2) {
					es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n一つの問題に複数の正答開始キーが\n使われています（" + i + 1 + "番目の問題）\n分割キーに置き換えてください");
					return new List<string> ();
				} else if (rowAS.Length < 2) {
					es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n問題のない正答か正答のない\n問題があります（" + i + 1 + "番目の問題）");
					return new List<string> ();
				} else {

					listQ.Add (rowAS [0]);
					listA.Add (rowAS [1]);
				}

			}

			//解説の分離
			if (Statics.StrNull (sd.expKey) == false) {

				for (int i = 0; i < listA.Count; i++) {

					string[] sep = { sd.expKey };
					string[] rowAS = listA [i].Split (sep, StringSplitOptions.RemoveEmptyEntries);

					if (rowAS.Length == 2) {
						listA [i] = rowAS [0];
						listE.Add (rowAS [1]);
					} else if (rowAS.Length == 1) {
						listE.Add (NullKeyCommon);
					} else if (rowAS.Length > 2) {

						es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n一つの問題に複数の解説開始キーが\n使われています（" + i + 1 + "番目の問題）");
						return new List<string> ();
					}
				}

				RemoveEnter (ref listE, false);
			}

			//ダミーかあれば分離
			SeparateDummy (ref listA, ref listD);

			RemoveEnter (ref listQ, false);

			//完全一致コマンドの置き換え
			ConvertPerfectCommand (ref listQ);

			if (Statics.StrNull (sd.sepKey) == false) {

				//解答群の分割
				for (int i = 0; i < listQ.Count; i++) {

					string str = listQ [i];

					string[] sep = { sd.sepKey };
					string[] rowAS = listA [i].Split (sep, StringSplitOptions.RemoveEmptyEntries);

					List<string> listAS = new List<string> ();

					foreach (string s in rowAS) {
						listAS.Add (RemoveEnterAll (s));
					}

					AnswerToHankaku (ref listAS);

					//### Q $$$ A &&& A &&& A
					str += AnsKeyCommon + MergeList (listAS, SepKeyCommon);

					//ダミー解答の分割
					if (listD.Count > 0 && listD [i] != NullKeyCommon) {
						string[] rowDS = listD [i].Split (sep, StringSplitOptions.RemoveEmptyEntries);
						List<string> listDS = new List<string> ();

						foreach (string s in rowDS) {
							listDS.Add (s);
						}
						RemoveEnter (ref listDS);

						//### Q $$$ A &&& A &&& A %%% D &&& D &&& D
						str += DummyKeyCommon + MergeList (listDS, SepKeyCommon);


						//選択肢があれば結合
						if (sentaku.Length > 0) {
							str += SepKeyCommon + sentaku;
						}

					} else {
						if (sentaku.Length > 0) {
							str += DummyKeyCommon + sentaku;
						}
					}

					//説明文結合
					if (listE.Count > 0 && listE [i] != NullKeyCommon) {
						str += ExpKeyCommon + listE [i];
					}

					q.Add (str);
				}
			} else {
				RemoveEnter (ref listA);
				AnswerToHankaku (ref listA);

				for (int i = 0; i < listQ.Count; i++) {


					string str = listQ [i];

					str += AnsKeyCommon + listA [i];

					if (listD.Count > 0 && listD [i] != NullKeyCommon) {
						str += DummyKeyCommon + RemoveEnterAll (listD [i]);

						if (sentaku.Length > 0) {
							str += SepKeyCommon + sentaku;
						}

					} else {

						if (sentaku.Length > 0) {
							str += DummyKeyCommon + sentaku;
						}
					}
						
					if (listE.Count > 0 && listE [i] != NullKeyCommon) {
						str += ExpKeyCommon + listE [i];
					}


					q.Add (str);
				}
			}










			//問題ー解説ー正答ー問題
		} else if (mode == 3) {

			for (int i = 0; i < listRow.Count; i++) {

				string[] sep = { sd.ansKey };
				string[] rowAS = listRow [i].Split (sep, StringSplitOptions.RemoveEmptyEntries);

				if (rowAS.Length > 2) {
					es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n一つの問題に複数の正答開始キーが\n使われています（" + i + 1 + "番目の問題）\n分割キーに置き換えてください");
					return new List<string> ();
				} else if (rowAS.Length < 2) {
					es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n問題のない正答か正答のない\n問題があります（" + i + 1 + "番目の問題）");
					return new List<string> ();
				} else {

					listQ.Add (rowAS [0]);
					listA.Add (rowAS [1]);
				}

			}

			//解説の分離
			if (Statics.StrNull (sd.expKey) == false) {

				for (int i = 0; i < listA.Count; i++) {

					string[] sep = { sd.expKey };
					string[] rowQE = listQ [i].Split (sep, StringSplitOptions.RemoveEmptyEntries);

					if (rowQE.Length == 2) {
						listQ [i] = rowQE [0];
						listE.Add (rowQE [1]);
					} else if (rowQE.Length == 1) {
						listE.Add (NullKeyCommon);
					} else if (rowQE.Length > 2) {

						es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n一つの問題に複数の解説開始キーが\n使われています（" + i + 1 + "番目の問題）");
						return new List<string> ();
					}
				}

				RemoveEnter (ref listE, false);
			}

			//ダミーかあれば分離
			SeparateDummy (ref listA, ref listD);

			RemoveEnter (ref listQ, false);

			//完全一致コマンドの置き換え
			ConvertPerfectCommand (ref listQ);

			if (Statics.StrNull (sd.sepKey) == false) {

				//解答群の分割
				for (int i = 0; i < listQ.Count; i++) {

					string str = listQ [i];

					string[] sep = { sd.sepKey };
					string[] rowAS = listA [i].Split (sep, StringSplitOptions.RemoveEmptyEntries);

					List<string> listAS = new List<string> ();

					foreach (string s in rowAS) {
						listAS.Add (RemoveEnterAll (s));
					}

					AnswerToHankaku (ref listAS);

					//### Q $$$ A &&& A &&& A
					str += AnsKeyCommon + MergeList (listAS, SepKeyCommon);

					//ダミー解答の分割
					if (listD.Count > 0 && listD [i] != NullKeyCommon) {
						string[] rowDS = listD [i].Split (sep, StringSplitOptions.RemoveEmptyEntries);
						List<string> listDS = new List<string> ();

						foreach (string s in rowDS) {
							listDS.Add (s);
						}
						RemoveEnter (ref listDS);

						//### Q $$$ A &&& A &&& A %%% D &&& D &&& D
						str += DummyKeyCommon + MergeList (listDS, SepKeyCommon);


						//選択肢があれば結合
						if (sentaku.Length > 0) {
							str += SepKeyCommon + sentaku;
						}

					} else {
						if (sentaku.Length > 0) {
							str += DummyKeyCommon + sentaku;
						}
					}

					//説明文結合
					if (listE.Count > 0 && listE [i] != NullKeyCommon) {
						str += ExpKeyCommon + listE [i];
					}

					q.Add (str);
				}
			} else {
				RemoveEnter (ref listA);
				AnswerToHankaku (ref listA);


				for (int i = 0; i < listQ.Count; i++) {


					string str = listQ [i];

					str += AnsKeyCommon + listA [i];

					if (listD.Count > 0 && listD [i] != NullKeyCommon) {
						str += DummyKeyCommon + RemoveEnterAll (listD [i]);

						if (sentaku.Length > 0) {
							str += SepKeyCommon + sentaku;
						}

					} else {

						if (sentaku.Length > 0) {
							str += DummyKeyCommon + sentaku;
						}
					}
						
					if (listE.Count > 0 && listE [i] != NullKeyCommon) {
						str += ExpKeyCommon + listE [i];
					}


					q.Add (str);
				}
			}









			//全問題ー正答ー解説ー正答
		} else if (mode == 4) {


			string[] sep = { sd.ansKey };
			string[] rowA = listRow [listRow.Count - 1].Split (sep, StringSplitOptions.RemoveEmptyEntries);

			if (rowA.Length == listRow.Count + 1) {

				listRow [listRow.Count - 1] = rowA [0];

				for (int i = 1; i < rowA.Length; i++) {

					listA.Add (rowA [i]);

				}

				for (int i = 0; i < listRow.Count; i++) {

					listQ.Add (listRow [i]);

				}


			} else {
				es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n問題と正答の数が合いません");
				return new List<string> ();
			}



			//解説の分離
			if (Statics.StrNull (sd.expKey) == false) {

				for (int i = 0; i < listA.Count; i++) {

					string[] sep2 = { sd.expKey };
					string[] rowAS = listA [i].Split (sep2, StringSplitOptions.RemoveEmptyEntries);

					if (rowAS.Length == 2) {
						listA [i] = rowAS [0];
						listE.Add (rowAS [1]);
					} else if (rowAS.Length == 1) {
						listE.Add (NullKeyCommon);
					} else if (rowAS.Length > 2) {

						es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n一つの問題に複数の解説開始キーが\n使われています（ " + i + 1 + "番目の問題）");
						return new List<string> ();
					}
				}

				RemoveEnter (ref listE, false);
			}

			//ダミーがあれば分離
			SeparateDummy (ref listA, ref listD);

			RemoveEnter (ref listQ, false);

			//完全一致コマンドの置き換え
			ConvertPerfectCommand (ref listQ);

			if (Statics.StrNull (sd.sepKey) == false) {

				//解答群の分割
				for (int i = 0; i < listQ.Count; i++) {

					string str = listQ [i];

					string[] sep2 = { sd.sepKey };
					string[] rowAS = listA [i].Split (sep2, StringSplitOptions.RemoveEmptyEntries);

					List<string> listAS = new List<string> ();

					foreach (string s in rowAS) {
						listAS.Add (RemoveEnterAll (s));
					}

					AnswerToHankaku (ref listAS);

					//### Q $$$ A &&& A &&& A
					str += AnsKeyCommon + MergeList (listAS, SepKeyCommon);

					//ダミー解答の分割
					if (listD.Count > 0 && listD [i] != NullKeyCommon) {
						string[] rowDS = listD [i].Split (sep, StringSplitOptions.RemoveEmptyEntries);
						List<string> listDS = new List<string> ();

						foreach (string s in rowDS) {
							listDS.Add (s);
						}
						RemoveEnter (ref listDS);

						//### Q $$$ A &&& A &&& A %%% D &&& D &&& D
						str += DummyKeyCommon + MergeList (listDS, SepKeyCommon);


						//選択肢があれば結合
						if (sentaku.Length > 0) {
							str += SepKeyCommon + sentaku;
						}

					} else {
						if (sentaku.Length > 0) {
							str += DummyKeyCommon + sentaku;
						}
					}

					//説明文結合
					if (listE.Count > 0 && listE [i] != NullKeyCommon) {
						str += ExpKeyCommon + listE [i];
					}

					q.Add (str);
				}
			} else {
				RemoveEnter (ref listA);
				AnswerToHankaku (ref listA);


				for (int i = 0; i < listQ.Count; i++) {


					string str = listQ [i];

					str += AnsKeyCommon + listA [i];

					if (listD.Count > 0 && listD [i] != NullKeyCommon) {
						str += DummyKeyCommon + RemoveEnterAll (listD [i]);

						if (sentaku.Length > 0) {
							str += SepKeyCommon + sentaku;
						}

					} else {

						if (sentaku.Length > 0) {
							str += DummyKeyCommon + sentaku;
						}
					}

					if (listE.Count > 0 && listE [i] != NullKeyCommon) {
						str += ExpKeyCommon + listE [i];
					}


					q.Add (str);
				}
			}










			//全問題ー解説ー正答ー解説
		} else if (mode == 5) {
			string[] sep = { sd.ansKey };
			string[] rowA = listRow [listRow.Count - 1].Split (sep, StringSplitOptions.RemoveEmptyEntries);

			listRow [listRow.Count - 1] = rowA [0];

			if (rowA.Length == listRow.Count + 1) {
				
				//解説の分離[0]
				if (Statics.StrNull (sd.expKey) == false) {
					string[] sep2 = { sd.expKey };
					string lr = listRow [listRow.Count - 1];
					string[] rowE = lr.Split (sep2, StringSplitOptions.RemoveEmptyEntries);

					if (rowE.Length > 1) {
						listRow [listRow.Count - 1] = rowE [0];
						listE.Add (rowE [1]);
					}
				}

				for (int i = 1; i < rowA.Length; i++) {

					listA.Add (rowA [i]);

				}

				for (int i = 0; i < listRow.Count; i++) {

					listQ.Add (listRow [i]);

				}


			} else {
				es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n問題と正答の数が合いません");
				return new List<string> ();
			}



			//解説の分離
			if (Statics.StrNull (sd.expKey) == false) {

				for (int i = 0; i < listA.Count - 1; i++) {

					string[] sep2 = { sd.expKey };
					string[] rowAS = listA [i].Split (sep2, StringSplitOptions.RemoveEmptyEntries);

					if (rowAS.Length == 2) {
						listA [i] = rowAS [0];
						listE.Add (rowAS [1]);
					} else if (rowAS.Length == 1) {
						listE.Add (NullKeyCommon);
					} else if (rowAS.Length > 2) {

						es.GetComponent<PopupWindow> ().PopupCaution ("エラー\n一つの問題に開始キーが重複して\n使われています（" + i + 1 + "番目の問題付近）");
						return new List<string> ();
					}
				}

				RemoveEnter (ref listE, false);
			}

			//ダミーがあれば分離
			SeparateDummy (ref listA, ref listD);

			RemoveEnter (ref listQ, false);

			//完全一致コマンドの置き換え
			ConvertPerfectCommand (ref listQ);

			if (Statics.StrNull (sd.sepKey) == false) {

				//解答群の分割
				for (int i = 0; i < listQ.Count; i++) {

					string str = listQ [i];

					string[] sep2 = { sd.sepKey };
					string[] rowAS = listA [i].Split (sep2, StringSplitOptions.RemoveEmptyEntries);

					List<string> listAS = new List<string> ();

					foreach (string s in rowAS) {
						listAS.Add (RemoveEnterAll (s));
					}

					AnswerToHankaku (ref listAS);

					//### Q $$$ A &&& A &&& A
					str += AnsKeyCommon + MergeList (listAS, SepKeyCommon);

					//ダミー解答の分割
					if (listD.Count > 0 && listD [i] != NullKeyCommon) {
						string[] rowDS = listD [i].Split (sep, StringSplitOptions.RemoveEmptyEntries);
						List<string> listDS = new List<string> ();

						foreach (string s in rowDS) {
							listDS.Add (s);
						}
						RemoveEnter (ref listDS);

						//### Q $$$ A &&& A &&& A %%% D &&& D &&& D
						str += DummyKeyCommon + MergeList (listDS, SepKeyCommon);


						//選択肢があれば結合
						if (sentaku.Length > 0) {
							str += SepKeyCommon + sentaku;
						}

					} else {
						if (sentaku.Length > 0) {
							str += DummyKeyCommon + sentaku;
						}
					}

					//説明文結合
					if (listE.Count > 0 && listE [i] != NullKeyCommon) {
						str += ExpKeyCommon + listE [i];
					}

					q.Add (str);
				}
			} else {
				RemoveEnter (ref listA);
				AnswerToHankaku (ref listA);


				for (int i = 0; i < listQ.Count; i++) {


					string str = listQ [i];

					str += AnsKeyCommon + listA [i];

					if (listD.Count > 0 && listD [i] != NullKeyCommon) {
						str += DummyKeyCommon + RemoveEnterAll (listD [i]);

						if (sentaku.Length > 0) {
							str += SepKeyCommon + sentaku;
						}

					} else {

						if (sentaku.Length > 0) {
							str += DummyKeyCommon + sentaku;
						}
					}

					if (listE.Count > 0 && listE [i] != NullKeyCommon) {
						str += ExpKeyCommon + listE [i];
					}


					q.Add (str);
				}
			}

		}

		return q;
	}

	public bool TextCheck ()
	{
		string text = inputQuesText.GetComponent<InputField> ().text;
		string[] used = { AnsKeyCommon, DummyKeyCommon, SepKeyCommon, ExpKeyCommon, PerKeyCommon };
		int f = 0;

		foreach (string s in used) {
			if (text.IndexOf (s) > -1) {
				f++;
			}
		}

		if (f != 0) {
			es.GetComponent<PopupWindow> ().PopupCaution ("エラー\nテキスト内に" + AnsKeyCommon + ",\n" + ExpKeyCommon + "," + DummyKeyCommon + "," + SepKeyCommon + "," + PerKeyCommon + "は使えません");
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

	public static string RemoveEnterZengo (string s)
	{
		while (s.Substring (0, Kaigyo.Length) == Kaigyo) {
			s = s.Substring (Kaigyo.Length);
		}
		while (s.Substring (s.Length - Kaigyo.Length) == Kaigyo) {
			s = s.Substring (0, s.Length - Kaigyo.Length);
		}

		return s;

	}

	public static string RemoveEnterAll (string s)
	{
		
		string ns = s.Replace (Kaigyo, "");

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

	public void SeparateDummy (ref List<string> listA, ref List<string> listD)
	{

		if (Statics.StrNull (sd.dummyKey) == false) {
			string[] dum = { sd.dummyKey };
			for (int i = 0; i < listA.Count; i++) {
				string[] strD = listA [i].Split (dum, StringSplitOptions.RemoveEmptyEntries);
				if (strD.Length > 1) {
					listA [i] = strD [0];
					listD.Add (strD [1]);
				} else {
					listD.Add (NullKeyCommon);
				}
			}
		}
	}

	public void AnswerToHankaku (ref List<string> listA)
	{

		if (sd.toHankaku == true) {

			//answer群の英数字半角化
			for (int i = 0; i < listA.Count; i++) {

				listA [i] = CSharp.Japanese.Kanaxs.KanaEx.ToHankaku (listA [i]);
			}
		}
	}

	public void AnswerToHankaku (ref string[] strA)
	{
		if (sd.toHankaku == true) {
			
			//answer群の英数字半角化
			for (int i = 0; i < strA.Length; i++) {

				strA [i] = CSharp.Japanese.Kanaxs.KanaEx.ToHankaku (strA [i]);
			}
		}
	}

	public void ConvertPerfectCommand (ref List<string> listQ)
	{
		if (Statics.StrNull (sd.perKey) == false) {

			for (int i = 0; i < listQ.Count; i++) {
				if (listQ [i].Substring (0, sd.perKey.Length) == sd.perKey) {
					listQ [i] = listQ [i].Substring (sd.perKey.Length);
					listQ [i] = PerKeyCommon + listQ [i];
				}
			}	
		}
	}

	public void InputTextView ()
	{
		string text = inputQuesText.GetComponent<InputField> ().text;

		if (Statics.StrNull (text) == true) {
			text = GetSampleText ();
		}

		viewText.GetComponent<Text> ().text = text;

		StartCoroutine (CorSetAnchor (viewContent, viewText)); 
	}

	private IEnumerator CorSetAnchor (GameObject contentGo, GameObject textGo)
	{  
		yield return StartCoroutine (Wait ());  

		Vector2 wh = textGo.GetComponent<RectTransform> ().sizeDelta;
		contentGo.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, wh.y);

//		yield return StartCoroutine (Wait ());  
//
//		Vector2 newanpo = textGo.GetComponent<RectTransform> ().anchoredPosition;
//		newanpo.y = 0;
//
//
//		textGo.GetComponent<RectTransform> ().anchoredPosition = newanpo;



	}

	private IEnumerator Wait (float f = 0.1f)
	{  
		yield return new WaitForSeconds (f); 
	}


	public string GetSampleText ()
	{
		int mode = sd.inputPat;
		string quesKey = sd.quesKey;

		string pHText = "（問題登録サンプル）\n";

		if (sd.quesKey == Kaigyo) {
			pHText += "注）区切り文字が「改行」のため他の箇所では改行を使えません。\n\n";
			quesKey = "";
		} else {
			pHText += "\n";
		}

		switch (mode) {

		case 0:

			pHText += quesKey + "問題①\n";
			pHText += quesKey + "問題①の答え";

			if (Statics.StrNull (sd.sepKey) == false) {
				pHText += "⑴" + sd.sepKey + "問題①の答え⑵";
			}

			if (Statics.StrNull (sd.dummyKey) == false) {

				if (quesKey != "") {
					pHText += "\n";
				}

				pHText += sd.dummyKey + "問題①のダミー";

				if (Statics.StrNull (sd.sepKey) == false) {

					pHText += "⑴" + sd.sepKey + "問題①のダミー⑵" + sd.sepKey + "問題①のダミー⑶\n";
				} else {
					pHText += "\n";
				}
			} else {
				pHText += "\n";
			}




			if (Statics.StrNull (sd.perKey) == false) {
				pHText += quesKey + sd.perKey + "問題②(答えの並び方を守る)\n";
			
			} else {
				pHText += quesKey + "問題②\n";
			}
			pHText += quesKey + "問題②の答え";

			if (Statics.StrNull (sd.sepKey) == false) {
				pHText += "⑴" + sd.sepKey + "問題②の答え⑵" + sd.sepKey + "問題②の答え⑶";
			}


			if (Statics.StrNull (sd.dummyKey) == false) {

				if (quesKey != "") {
					pHText += "\n";
				}

				pHText += sd.dummyKey + "問題②のダミー";

				if (Statics.StrNull (sd.sepKey) == false) {
					pHText += "⑴" + sd.sepKey + "問題②のダミー⑵\n";
				} else {
					pHText += "\n";
				}
			} else {
				pHText += "\n";
			}

			pHText += quesKey + "問題③\n...\n..\n.\n";

			break;
		case 1:

			pHText += quesKey + "問題①\n";

			if (Statics.StrNull (sd.perKey) == false) {
				pHText += quesKey + sd.perKey + "問題②(答えの並び方を守る)\n";

			} else {
				pHText += quesKey + "問題②\n";
			}

			pHText += quesKey + "問題③\n...\n..\n.\n";


			pHText += quesKey + "問題①の答え";

			if (Statics.StrNull (sd.sepKey) == false) {
				pHText += "⑴" + sd.sepKey + "問題①の答え⑵";
			}

			if (Statics.StrNull (sd.dummyKey) == false) {

				if (quesKey != "") {
					pHText += "\n";
				}

				pHText += sd.dummyKey + "問題①のダミー";

				if (Statics.StrNull (sd.sepKey) == false) {

					pHText += "⑴" + sd.sepKey + "問題①のダミー⑵" + sd.sepKey + "問題①のダミー⑶\n";
				} else {
					pHText += "\n";
				}
			} else {
				pHText += "\n";
			}




			pHText += quesKey + "問題②の答え";

			if (Statics.StrNull (sd.sepKey) == false) {
				pHText += "⑴" + sd.sepKey + "問題②の答え⑵" + sd.sepKey + "問題②の答え⑶";
			}


			if (Statics.StrNull (sd.dummyKey) == false) {

				if (quesKey != "") {
					pHText += "\n";
				}

				pHText += sd.dummyKey + "問題②のダミー";

				if (Statics.StrNull (sd.sepKey) == false) {
					pHText += "⑴" + sd.sepKey + "問題②のダミー⑵\n";
				} else {
					pHText += "\n";
				}
			} else {
				pHText += "\n";
			}

			pHText += quesKey + "問題③の答え\n...\n..\n.\n";
			break;
		case 2:

			pHText += quesKey + "問題①\n";
			pHText += sd.ansKey + "問題①の答え";

			if (Statics.StrNull (sd.sepKey) == false) {
				pHText += "⑴" + sd.sepKey + "問題①の答え⑵";
			}

			pHText += "\n";

			if (Statics.StrNull (sd.dummyKey) == false) {

				pHText += sd.dummyKey + "問題①のダミー";

				if (Statics.StrNull (sd.sepKey) == false) {

					pHText += "⑴" + sd.sepKey + "問題①のダミー⑵" + sd.sepKey + "問題①のダミー⑶\n";
				} else {
					pHText += "\n";
				}
			}

			pHText += sd.expKey + "問題①の解説\n";


			if (Statics.StrNull (sd.perKey) == false) {
				pHText += quesKey + sd.perKey + "問題②(答えの並び方を守る)\n";

			} else {
				pHText += quesKey + "問題②\n";
			}
			pHText += sd.ansKey + "問題②の答え";

			if (Statics.StrNull (sd.sepKey) == false) {
				pHText += "⑴" + sd.sepKey + "問題②の答え⑵" + sd.sepKey + "問題②の答え⑶";
			}

			pHText += "\n";

			if (Statics.StrNull (sd.dummyKey) == false) {

				pHText += sd.dummyKey + "問題②のダミー";

				if (Statics.StrNull (sd.sepKey) == false) {
					pHText += "⑴" + sd.sepKey + "問題②のダミー⑵\n";
				} else {
					pHText += "\n";
				}
			}

			pHText += sd.expKey + "問題②の解説\n";

			pHText += quesKey + "問題③\n...\n..\n.\n";
			break;
		case 3:

			pHText += quesKey + "問題①\n";
			pHText += sd.expKey + "問題①の解説\n";
			pHText += sd.ansKey + "問題①の答え";

			if (Statics.StrNull (sd.sepKey) == false) {
				pHText += "⑴" + sd.sepKey + "問題①の答え⑵";
			}

			pHText += "\n";

			if (Statics.StrNull (sd.dummyKey) == false) {

				pHText += sd.dummyKey + "問題①のダミー";

				if (Statics.StrNull (sd.sepKey) == false) {

					pHText += "⑴" + sd.sepKey + "問題①のダミー⑵" + sd.sepKey + "問題①のダミー⑶\n";
				} else {
					pHText += "\n";
				}
			}

			if (Statics.StrNull (sd.perKey) == false) {
				pHText += quesKey + sd.perKey + "問題②(答えの並び方を守る)\n";

			} else {
				pHText += quesKey + "問題②\n";
			}
			pHText += sd.expKey + "問題②の解説\n";
			pHText += sd.ansKey + "問題②の答え";

			if (Statics.StrNull (sd.sepKey) == false) {
				pHText += "⑴" + sd.sepKey + "問題②の答え⑵" + sd.sepKey + "問題②の答え⑶";
			}


			pHText += "\n";

			if (Statics.StrNull (sd.dummyKey) == false) {

				pHText += sd.dummyKey + "問題②のダミー";

				if (Statics.StrNull (sd.sepKey) == false) {
					pHText += "⑴" + sd.sepKey + "問題②のダミー⑵\n";
				} else {
					pHText += "\n";
				}
			}

			pHText += quesKey + "問題③\n...\n..\n.\n";

			break;
		case 4:

			pHText += quesKey + "問題①\n";

			if (Statics.StrNull (sd.perKey) == false) {
				pHText += quesKey + sd.perKey + "問題②(答えの並び方を守る)\n";

			} else {
				pHText += quesKey + "問題②\n";
			}

			pHText += quesKey + "問題③\n...\n..\n.\n";

			pHText += sd.ansKey + "問題①の答え";

			if (Statics.StrNull (sd.sepKey) == false) {
				pHText += "⑴" + sd.sepKey + "問題①の答え⑵";
			}

			pHText += "\n";

			if (Statics.StrNull (sd.dummyKey) == false) {

				pHText += sd.dummyKey + "問題①のダミー";

				if (Statics.StrNull (sd.sepKey) == false) {

					pHText += "⑴" + sd.sepKey + "問題①のダミー⑵" + sd.sepKey + "問題①のダミー⑶\n";
				} else {
					pHText += "\n";
				}
			}

			pHText += sd.expKey + "問題①の解説\n";


			pHText += sd.ansKey + "問題②の答え";

			if (Statics.StrNull (sd.sepKey) == false) {
				pHText += "⑴" + sd.sepKey + "問題②の答え⑵" + sd.sepKey + "問題②の答え⑶";
			}

			pHText += "\n";

			if (Statics.StrNull (sd.dummyKey) == false) {

				pHText += sd.dummyKey + "問題②のダミー";

				if (Statics.StrNull (sd.sepKey) == false) {
					pHText += "⑴" + sd.sepKey + "問題②のダミー⑵\n";
				} else {
					pHText += "\n";
				}
			}

			pHText += sd.expKey + "問題②の解説\n";

			pHText += sd.ansKey + "問題③の答え\n...\n..\n.\n";

			break;
		case 5:

			pHText += quesKey + "問題①\n";

			if (Statics.StrNull (sd.perKey) == false) {
				pHText += quesKey + sd.perKey + "問題②(答えの並び方を守る)\n";

			} else {
				pHText += quesKey + "問題②\n";
			}

			pHText += quesKey + "問題③\n...\n..\n.\n";

			pHText += sd.expKey + "問題①の解説\n";
			pHText += sd.ansKey + "問題①の答え";

			if (Statics.StrNull (sd.sepKey) == false) {
				pHText += "⑴" + sd.sepKey + "問題①の答え⑵";
			}

			pHText += "\n";

			if (Statics.StrNull (sd.dummyKey) == false) {

				pHText += sd.dummyKey + "問題①のダミー";

				if (Statics.StrNull (sd.sepKey) == false) {

					pHText += "⑴" + sd.sepKey + "問題①のダミー⑵" + sd.sepKey + "問題①のダミー⑶\n";
				} else {
					pHText += "\n";
				}
			}

			pHText += sd.expKey + "問題②の解説\n";
			pHText += sd.ansKey + "問題②の答え";

			if (Statics.StrNull (sd.sepKey) == false) {
				pHText += "⑴" + sd.sepKey + "問題②の答え⑵" + sd.sepKey + "問題②の答え⑶";
			}


			pHText += "\n";

			if (Statics.StrNull (sd.dummyKey) == false) {

				pHText += sd.dummyKey + "問題②のダミー";

				if (Statics.StrNull (sd.sepKey) == false) {
					pHText += "⑴" + sd.sepKey + "問題②のダミー⑵\n";
				} else {
					pHText += "\n";
				}
			}

			pHText += sd.expKey + "問題③の解説\n...\n..\n.\n";

			break;
		default:
			break;
		}
		return pHText;
	}
}
