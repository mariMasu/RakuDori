using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using System.Collections;


public class DrillAnsMaster : MonoBehaviour
{
	public SimpleSQL.SimpleSQLManager dbManager;
	public GameObject ansP;
	public GameObject sentakuP;
	public GameObject textP;

	public GameObject open;

	GameObject ansContent;
	GameObject sentakuContent;

	public QuesData nowQData;
	public QuesArray nowQArray;

	public List<string[]> senAnsList;

	public bool ansRandom = false;

	GameObject ansBase;

	public List<QuesData> questionList;
	public List<QuesData> dummyKouhoList;

	List<GameObject> gol = new List<GameObject> ();

	public List<string> ansList;

	Color[] Col;

	GameObject level0;
	GameObject level1;
	GameObject level2;
	GameObject level3;
	GameObject level4;
	GameObject level5;

	GameObject tagImage;
	GameObject zyun;
	GameObject correct;
	GameObject wrong;

	GameObject next;
	GameObject nextQ;

	GameObject hide;
	GameObject ok;
	GameObject bad;

	void Awake ()
	{
		if (Statics.reviewList.Count == 0) {
			this.GetComponent<LoadButton> ().LoadDrillSentaku ();
			return;

		} else {

			Statics.nowDrill = Statics.reviewList [0];
			Statics.reviewList.RemoveAt (0);

		}

		Statics.prefabGap = ((GameObject.Find ("help").GetComponent<RectTransform> ().sizeDelta.x) / 2);

		Col = DrillColor.GetColorD (Statics.nowColor);

		List<DrillData> dd = new List<DrillData> (from ps in dbManager.Table<DrillData> ()
		                                          select ps);

		MakeReviewList (dd.Find (s => s.ID == Statics.nowDrill));

		ViewQuestion ();


	}

	void MakeReviewList (DrillData dd)
	{
		List<QuesData> qd = new List<QuesData> (from ps in dbManager.Table<QuesData> ()
		                                        select ps);

		questionList = qd.FindAll (s => s.DRILL_ID == dd.ID);

		string coution = GetChooseQues ();

		if (coution != "") {
			QuesData vqd = new QuesData ();
			vqd.TEXT = (" " + QuesTextEdit.AnsKeyCommon + " ");

			questionList.Add (vqd);

			dummyKouhoList = new List<QuesData> ();
			this.GetComponent<PopupWindow> ().PopupCaution ("エラー\n" + coution, 2);
			return;
		}

		if (dd.DUMMY_USE == 0) {
			dummyKouhoList = new List<QuesData> ();


		} else if (dd.DUMMY_USE == 1) {

			if (dd.DUMMY_TAG == 0) {
				dummyKouhoList = questionList;
			} else {
				dummyKouhoList = questionList.FindAll (s => s.TAG == (dd.DUMMY_TAG - 1));
			}
		} else {
			if (dd.DUMMY_TAG == 0) {
				dummyKouhoList = qd;
			} else {
				dummyKouhoList = qd.FindAll (s => s.TAG == (dd.DUMMY_TAG - 1));
			}
		}

		if (dd.QUES_ORDER == 1) {

			Fisher_Yates_CardDeck_Shuffle (questionList);

		} else if (dd.QUES_ORDER == 2) {
			questionList.Sort ((a, b) => a.JUN - b.JUN);

		} else if (dd.QUES_ORDER == 3) {
			questionList.Sort ((a, b) => b.JUN - a.JUN);
		} else {
			Debug.Log ("err");
		}

		if (dd.ANS_ORDER == 0) {
			ansRandom = true;
		} else {
			ansRandom = false;
		}
	}

	string GetChooseQues ()
	{
		
		string s = "";

		if (Statics.ansChoose == 1) {
			List<QuesData> removeList = new List<QuesData> ();

			s = "要復習問題が存在しません";

			foreach (QuesData qd in questionList) {
				
				if (qd.LAST.Length > 5 && qd.REVIEW == 0) {
					if (TimeFunctions.NeedReview (qd.LAST, qd.LEVEL) == false) {
						removeList.Add (qd);
					}
				}

			}

			foreach (QuesData q in removeList) {
				questionList.Remove (q);
			}


			
		} else if (Statics.ansChoose == 2) {
			s = "未完了問題が存在しません";
			questionList.RemoveAll (q => q.LEVEL == 5);
		} else if (Statics.ansChoose == 3) {
			s = "完了問題が存在しません";
			questionList.RemoveAll (q => q.LEVEL != 5);
		}

		if (questionList.Count == 0) {
			return s;
		} else {
			return "";
		}

	}

	public void Fisher_Yates_CardDeck_Shuffle (List<QuesData> aList)
	{
		DateTime t = DateTime.Now;
		int seed = t.Second;

		System.Random _random = new System.Random (seed);

		QuesData myGO;

		int n = aList.Count;
		for (int i = 0; i < n; i++) {
			int r = i + (int)(_random.NextDouble () * (n - i));
			myGO = aList [r];
			aList [r] = aList [i];
			aList [i] = myGO;
		}

		questionList = aList;
	}

	public void Fisher_Yates_CardDeck_Shuffle ()
	{

		DateTime t = DateTime.Now;
		int seed = t.Second;

		System.Random _random = new System.Random (seed);

		string myGO;

		int n = ansList.Count;
		for (int i = 0; i < n; i++) {
			int r = i + (int)(_random.NextDouble () * (n - i));
			myGO = ansList [r];
			ansList [r] = ansList [i];
			ansList [i] = myGO;
		}
	}

	public void ViewQuestion ()
	{

		if (questionList.Count == 0) {
			this.GetComponent<LoadButton> ().LoadDrillAns ();
		} else {

			nowQData = questionList [0];
			QuesArray q = DbTextToQA.DbToQA (nowQData.TEXT);
			nowQArray = q;

			Statics.nowTag = nowQData.TAG;
			Statics.nowLevel = nowQData.LEVEL;

			ansList = new List<string> ();
			senAnsList = new List<string[]> ();

			ansList.AddRange (q.Ans);

			if (dummyKouhoList.Count != 0) {
			
				for (int i = 0; i < 3; i++) {
				
					int rand = UnityEngine.Random.Range (0, dummyKouhoList.Count);

					QuesArray qa = DbTextToQA.DbToQA (dummyKouhoList [rand].TEXT);

					rand = UnityEngine.Random.Range (0, qa.Ans.Length);

					ansList.Add (qa.Ans [rand]);
				}
			}

			if (q.Dummy.Length != 0) {
			
				ansList.AddRange (q.Dummy);
			}

			ansList = ansList.Distinct ().ToList ();



			string ansStr = String.Join ("", q.Ans);
			string dumStr = String.Join ("", q.Dummy);
			string strAS = ansStr + dumStr;

			if (strAS.Length > 15) {
				ansBase = GameObject.Find ("LongAnswer");
			} else {
				ansBase = GameObject.Find ("ShortAnswer");

			}

			ansBase.transform.position = new Vector3 (0, 0, 0);

			correct = ansBase.transform.Find ("correct").gameObject;
			wrong = ansBase.transform.Find ("wrong").gameObject;

			level0 = ansBase.transform.Find ("level0").gameObject;
			level1 = ansBase.transform.Find ("level1").gameObject;
			level2 = ansBase.transform.Find ("level2").gameObject;
			level3 = ansBase.transform.Find ("level3").gameObject;
			level4 = ansBase.transform.Find ("level4").gameObject;


			zyun = ansBase.transform.Find ("zyun").gameObject;

			next = ansBase.transform.Find ("next").gameObject;
			nextQ = ansBase.transform.Find ("nextQ").gameObject;

			hide = ansBase.transform.Find ("hide").gameObject;
			ok = ansBase.transform.Find ("ok").gameObject;
			bad = ansBase.transform.Find ("bad").gameObject;

			setTagLevActive ();
			setIconActive ();

			if (ansRandom == true) {
				Fisher_Yates_CardDeck_Shuffle ();
			} else {
				StringComparer cmp = StringComparer.OrdinalIgnoreCase;
				ansList.Sort (cmp);
			}


			ansContent = ansBase.transform.Find ("choose/scroll/Scroll View/Viewport/AnsContent").gameObject;
			sentakuContent = ansBase.transform.Find ("answer/scroll/Scroll View/Viewport/SentakuContent").gameObject;

			gol.Clear ();

			foreach (Transform n in ansContent.transform) {
				GameObject.Destroy (n.gameObject);
			}

			foreach (Transform n in sentakuContent.transform) {
				GameObject.Destroy (n.gameObject);
			}

			GameObject ansS = ansBase.transform.Find ("choose/scroll").gameObject;
			GameObject senS = ansBase.transform.Find ("answer/scroll").gameObject;

			ansContent.GetComponent<RectTransform> ().sizeDelta = ansS.GetComponent<RectTransform> ().sizeDelta;
			sentakuContent.GetComponent<RectTransform> ().sizeDelta = senS.GetComponent<RectTransform> ().sizeDelta;


			for (int i = 0; i < ansList.Count; i++) {
				GameObject ans = Instantiate (ansP);

				gol.Add (ans);

				ans.GetComponent<AnsPrefab> ().ansText = ansList [i];
				ans.transform.Find ("textBack").GetComponent<Image> ().color = Col [1];

				StartCoroutine (SetText (ans, ansContent)); 

			}

			if (zyun.activeSelf == true) {
				ansBase.transform.Find ("question/Qtext/Text").GetComponent<Text> ().text = q.Ques.Substring ((QuesTextEdit.PerKeyCommon.Length));
			} else {
				ansBase.transform.Find ("question/Qtext/Text").GetComponent<Text> ().text = q.Ques;
			}
			GameObject view = ansBase.transform.Find ("question/Qtext").gameObject;
			StartCoroutine (CorScrollNormalize (view)); 


			questionList.RemoveAt (0);
		}

	}

	public void ContentNarabi (int i)
	{
		GameObject con;

		if (i == 0) {
			con = ansContent;
		} else {
			con = sentakuContent;
		}

		Vector2 conwh = con.GetComponent<RectTransform> ().sizeDelta;


		float maxWidth = con.GetComponent<RectTransform> ().sizeDelta.x;
		if (maxWidth < 0) {
			maxWidth *= -1f;
		}

		float nowHeight = 0f;

		float rowWidth = 0f;
		float rowHeight = 0f;

		foreach (GameObject go in gol) {

			Vector2 wh = go.GetComponent<RectTransform> ().sizeDelta;

			Vector2 newanpo = new Vector2 (wh.x / 2, -(wh.y / 2));

			if ((rowWidth + wh.x) > maxWidth) {

				nowHeight += rowHeight;
				rowHeight = wh.y;

				newanpo.y -= nowHeight;

				rowWidth = 0f;
				
			} else {
				if (wh.y > rowHeight) {
					rowHeight = wh.y;
				}

				newanpo.x += rowWidth;
				newanpo.y -= nowHeight;
				
			}

			go.GetComponent<RectTransform> ().anchoredPosition = newanpo;

			rowWidth += wh.x;

		}

		nowHeight += rowHeight;

		if (conwh.y < nowHeight) {
			GameObject view;
			if (i == 0) {
				view = ansBase.transform.Find ("answer/scroll/Scroll View").gameObject;
			} else {
				view = ansBase.transform.Find ("choose/scroll/Scroll View").gameObject;

			}
			Vector2 size = new Vector2 (maxWidth, (float)(nowHeight + (maxWidth * 0.2)));
			con.GetComponent<RectTransform> ().sizeDelta = size;
			StartCoroutine (CorScrollNormalize (view)); 
		}


	}

	private IEnumerator SetText (GameObject ans, GameObject parent)
	{  
		yield return StartCoroutine (ans.GetComponent<AnsPrefab> ().SetTextC1 ());  
		ans.transform.SetParent (parent.transform);
		ans.transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

	}

	private IEnumerator CorSentakuNarabi ()
	{  
		yield return StartCoroutine (Wait ());  
		ContentNarabi (1);

	}

	private IEnumerator Wait (float f = 0.1f)
	{  
		yield return new WaitForSeconds (f); 
	}

	private IEnumerator CorContentText (GameObject go, int i)
	{  
		yield return StartCoroutine (Wait ());  
		SetContentText (go, i);

	}

	public void ViewNowSentaku ()
	{
		foreach (Transform n in sentakuContent.transform) {
			GameObject.Destroy (n.gameObject);
		}
		GameObject senS = ansBase.transform.Find ("answer/scroll").gameObject;
		sentakuContent.GetComponent<RectTransform> ().sizeDelta = senS.GetComponent<RectTransform> ().sizeDelta;

		gol.Clear ();


		for (int i = 0; i < senAnsList.Count; i++) {
			GameObject sen = Instantiate (sentakuP);

			gol.Add (sen);

			sen.GetComponent<AnsPrefab> ().id = senAnsList [i] [0];
			sen.GetComponent<AnsPrefab> ().ansText = senAnsList [i] [1];
			sen.transform.Find ("textBack").GetComponent<Image> ().color = Col [1];

			StartCoroutine (SetText (sen, sentakuContent)); 
		}

		StartCoroutine (CorSentakuNarabi ()); 

	}

	public int SearchTimeID (string t)
	{
		
		for (int i = 0; i < senAnsList.Count; i++) {

			if (senAnsList [i] [0] == t) {
				return i;
			}

		}

		return -1;
	}

	public void setTagLevActive ()
	{

		tagImage = ansBase.transform.Find ("tag").gameObject;
		if (nowQData.TAG == 0) {
			tagImage.SetActive (false);
		} else {
			Color[] tagc = DrillColor.GetColorD (nowQData.TAG);

			tagImage.SetActive (true);
			tagImage.GetComponent<Image> ().color = tagc [1];
		}


		switch (nowQData.LEVEL) {

		case 0:
			level0.SetActive (true);
			break;
		case 1:
			level1.SetActive (true);
			level0.SetActive (false);

			break;
		case 2:
			level2.SetActive (true);
			level1.SetActive (false);
			level0.SetActive (false);

			break;
		case 3:
			level3.SetActive (true);
			level2.SetActive (false);
			level1.SetActive (false);
			level0.SetActive (false);

			break;
		case 4:
			level4.SetActive (true);
			level3.SetActive (false);
			level2.SetActive (false);
			level1.SetActive (false);
			level0.SetActive (false);

			break;
		case 5:
			level4.SetActive (false);
			level3.SetActive (false);
			level2.SetActive (false);
			level1.SetActive (false);
			level0.SetActive (false);

			break;

		}
	}

	void setIconActive ()
	{
		hide.GetComponent<Button> ().enabled = true;

		open.SetActive (true);
		ok.SetActive (false);
		bad.SetActive (false);

		hide.SetActive (true);
		hide.GetComponent<ScrollRect> ().enabled = false;
		hide.transform.Find ("expText").gameObject.GetComponent<Text> ().text = "";

		correct.SetActive (false);
		wrong.SetActive (false);

		next.SetActive (false);
		nextQ.SetActive (false);

		if (nowQData.TEXT.Substring (0, QuesTextEdit.PerKeyCommon.Length) == QuesTextEdit.PerKeyCommon) {
			zyun.SetActive (true);
		} else {
			zyun.SetActive (false);
		}

	}

	public void AnswerNext ()
	{
		nowQData.LAST = DateTime.Now.ToString ();

		foreach (Transform n in ansContent.transform) {
			GameObject.Destroy (n.gameObject);
		}

		foreach (Transform n in sentakuContent.transform) {
			GameObject.Destroy (n.gameObject);
		}

		GameObject ansS = ansBase.transform.Find ("choose/scroll").gameObject;
		GameObject senS = ansBase.transform.Find ("answer/scroll").gameObject;

		ansContent.GetComponent<RectTransform> ().sizeDelta = ansS.GetComponent<RectTransform> ().sizeDelta;
		sentakuContent.GetComponent<RectTransform> ().sizeDelta = senS.GetComponent<RectTransform> ().sizeDelta;

		GameObject ansText = Instantiate (textP);


		string[] userAns = GetUserAns ();

		bool result;

		if (zyun.activeSelf == true) {

			result = nowQArray.Ans.SequenceEqual (userAns);

		} else {

			Array.Sort (userAns);

			string[] localQA = (string[])nowQArray.Ans.Clone ();
			Array.Sort (localQA);

			result = localQA.SequenceEqual (userAns);	
		}


		if (result == true) {

			nowQData.REVIEW -= 1;


			if (nowQData.REVIEW < 0) {

				if (nowQData.LEVEL != 5) {
					nowQData.LEVEL += 1;
				}

				nowQData.REVIEW = 0;

			} else if (nowQData.REVIEW == 0) {

				if (nowQData.LEVEL == 0) {
					nowQData.LEVEL = 1;
				}
				
			} else {
				questionList.Add (nowQData);

			}

			ansText.GetComponent<Text> ().text = ("正答:\n" + String.Join (" ", nowQArray.Ans));

			correct.SetActive (true);

		} else {

			if (nowQData.LEVEL != 0) {
				nowQData.LEVEL -= 1;
			}
			if (nowQData.REVIEW != 3) {
				nowQData.REVIEW = 3;
			}

			string[] ss = GetUserAns ();

			ansText.GetComponent<Text> ().text = ("正答:\n" + String.Join (" ", nowQArray.Ans) + "\n\nあなたの回答:\n" + String.Join (" ", ss));

			wrong.SetActive (true);
			questionList.Add (nowQData);
		}

		ansText.transform.SetParent (sentakuContent.transform);
		StartCoroutine (CorContentText (ansText, 1)); 

		if (Statics.StrNull (nowQArray.Exp) == false) {
			GameObject expText = Instantiate (textP);

			expText.GetComponent<Text> ().text = ("解説：\n" + nowQArray.Exp);

			expText.transform.SetParent (ansContent.transform);
			StartCoroutine (CorContentText (expText, 0)); 
		}

		this.GetComponent<DbProcess> ().UpdateQuesData (nowQData);

		nextQ.SetActive (true);


	}

	public void ChooseOpen ()
	{
		ContentNarabi (0);
		open.SetActive (false);
		hide.SetActive (false);

		next.SetActive (true);

	}

	public void AnswerOpen ()
	{
		open.SetActive (false);

		hide.GetComponent<Button> ().enabled = false;
		ok.SetActive (true);
		bad.SetActive (true);


		foreach (Transform n in sentakuContent.transform) {
			GameObject.Destroy (n.gameObject);
		}
		GameObject senS = ansBase.transform.Find ("answer/scroll").gameObject;
		sentakuContent.GetComponent<RectTransform> ().sizeDelta = senS.GetComponent<RectTransform> ().sizeDelta;

		GameObject ansText = Instantiate (textP);

		ansText.GetComponent<Text> ().text = ("正答:\n" + String.Join (" ", nowQArray.Ans));
		ansText.transform.SetParent (sentakuContent.transform);
		StartCoroutine (CorContentText (ansText, 1)); 

		if (Statics.StrNull (nowQArray.Exp) == false) {

			hide.GetComponent<ScrollRect> ().enabled = true;
			hide.transform.Find ("expText").gameObject.GetComponent<Text> ().text = ("解説:\n" + nowQArray.Exp);

			StartCoroutine (CorScrollNormalize (hide)); 
		}
	}

	public void SelfAnswerNext (bool b)
	{

		nowQData.LAST = DateTime.Now.ToString ();

		if (b == true) {

			nowQData.REVIEW -= 1;


			if (nowQData.REVIEW < 0) {

				if (nowQData.LEVEL != 5) {
					nowQData.LEVEL += 1;
				}

				nowQData.REVIEW = 0;

			} else if (nowQData.REVIEW == 0) {
				
				if (nowQData.LEVEL == 0) {
					nowQData.LEVEL = 1;
				}

			} else {
				questionList.Add (nowQData);

			}


		} else {

			if (nowQData.LEVEL != 0) {
				nowQData.LEVEL -= 1;
			}
			if (nowQData.REVIEW != 3) {
				nowQData.REVIEW = 3;
			}

			questionList.Add (nowQData);
		}

		this.GetComponent<DbProcess> ().UpdateQuesData (nowQData);

		ViewQuestion ();

	}

	string[] GetUserAns ()
	{
		string kaigyo = "\n";

		List<string> ls = new List<string> ();

		foreach (string[] s in senAnsList) {
			string text = s [1];

			text = text.Replace (kaigyo.ToString (), "");

			ls.Add (text);
		}

		string[] userAns = ls.ToArray ();

		return userAns;
	}

	private void SetContentText (GameObject text, int i)
	{ 

		GameObject con;

		if (i == 0) {
			con = ansContent;
		} else {
			con = sentakuContent;
		}

		text.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

		Vector2 conwh = con.GetComponent<RectTransform> ().sizeDelta;
		Vector2 wh = text.GetComponent<RectTransform> ().sizeDelta;

		Vector2 newanpo = new Vector2 (0, 0);

		text.GetComponent<RectTransform> ().anchoredPosition = newanpo;


		GameObject view;
		if (i == 0) {
			view = ansBase.transform.Find ("choose/scroll/Scroll View").gameObject;
		} else {
			view = ansBase.transform.Find ("answer/scroll/Scroll View").gameObject;
		}

		if (conwh.y < wh.y) {
			Vector2 size = new Vector2 (conwh.x, (float)(wh.y + (conwh.x * 0.2)));
			con.GetComponent<RectTransform> ().sizeDelta = size;
		}
		StartCoroutine (CorScrollNormalize (view)); 

	}

	private IEnumerator CorScrollNormalize (GameObject scroll)
	{ 
		yield return StartCoroutine (Wait (0.01f));  
		scroll.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1f;
	}
}

