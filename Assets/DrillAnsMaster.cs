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

	public List<string> ansList;

	Color[] Col;

	GameObject level0;
	GameObject level1;
	GameObject level2;
	GameObject level3;

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

		} else {

			Statics.nowDrill = Statics.reviewList [0];
			Statics.reviewList.RemoveAt (0);

		}

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
		}

		nowQData = questionList [0];
		QuesArray q = DbTextToQA.DbToQA (nowQData.TEXT);
		nowQArray = q;

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
		string dumStr = ansList.ToString ();
		string strAS = ansStr + dumStr;

		if (strAS.Length > 10) {
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

		zyun = ansBase.transform.Find ("zyun").gameObject;

		next = ansBase.transform.Find ("next").gameObject;
		nextQ = ansBase.transform.Find ("nextQ").gameObject;

		hide = ansBase.transform.Find ("hide").gameObject;
		ok = ansBase.transform.Find ("ok").gameObject;
		bad = ansBase.transform.Find ("bad").gameObject;

		setIconActive ();



		if (ansRandom == true) {
			Fisher_Yates_CardDeck_Shuffle ();
		} else {
			StringComparer cmp = StringComparer.OrdinalIgnoreCase;
			ansList.Sort (cmp);
		}


		ansContent = ansBase.transform.Find ("choose/scroll/Scroll View/Viewport/AnsContent").gameObject;
		sentakuContent = ansBase.transform.Find ("answer/scroll/Scroll View/Viewport/SentakuContent").gameObject;


		foreach (Transform n in ansContent.transform) {
			GameObject.Destroy (n.gameObject);
		}

		foreach (Transform n in sentakuContent.transform) {
			GameObject.Destroy (n.gameObject);
		}

		for (int i = 0; i < ansList.Count; i++) {
			GameObject ans = Instantiate (ansP);

			ans.GetComponent<AnsPrefab> ().ansText = ansList [i];
			ans.transform.Find ("textBack").GetComponent<Image> ().color = Col [1];

			StartCoroutine (SetText (ans, ansContent)); 

		}


		ansBase.transform.Find ("question/Qtext/Text").GetComponent<Text> ().text = q.Ques;
		ansBase.transform.Find ("question/Qtext").GetComponent<ScrollRect> ().verticalNormalizedPosition = 1f;


		questionList.RemoveAt (0);

	}

	private IEnumerator SetText (GameObject ans, GameObject parent)
	{  
		yield return StartCoroutine (ans.GetComponent<AnsPrefab> ().SetTextC1 ());  
		ans.transform.SetParent (parent.transform);
		ans.transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

	}

	public void ViewNowSentaku ()
	{
		foreach (Transform n in sentakuContent.transform) {
			GameObject.Destroy (n.gameObject);
		}

		for (int i = 0; i < senAnsList.Count; i++) {
			GameObject sen = Instantiate (sentakuP);

			sen.GetComponent<AnsPrefab> ().id = senAnsList [i] [0];
			sen.GetComponent<AnsPrefab> ().ansText = senAnsList [i] [1];
			sen.transform.Find ("textBack").GetComponent<Image> ().color = Col [1];

			StartCoroutine (SetText (sen, sentakuContent)); 
		}

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

	void setIconActive ()
	{
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
			level3.SetActive (false);
			level2.SetActive (false);
			level1.SetActive (false);
			level0.SetActive (false);

			break;

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

		GameObject ansText = Instantiate (ansP);
		ansText.GetComponent<Button> ().enabled = false;


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

				if (nowQData.LEVEL != 4) {
					nowQData.LEVEL += 1;
				}

				nowQData.REVIEW = 0;

			} else if (nowQData.REVIEW == 0) {
				
			} else {
				questionList.Add (nowQData);

			}

			ansText.GetComponent<AnsPrefab> ().ansText = ("正答:\n" + String.Join (" ", nowQArray.Ans));
			ansText.transform.Find ("textBack").GetComponent<Image> ().color = Col [1];

			StartCoroutine (SetText (ansText, sentakuContent)); 

			correct.SetActive (true);

		} else {

			if (nowQData.LEVEL != 0) {
				nowQData.LEVEL -= 1;
			}
			if (nowQData.REVIEW != 3) {
				nowQData.REVIEW = 3;
			}

			string[] ss = GetUserAns ();

			ansText.GetComponent<AnsPrefab> ().ansText = ("正答:\n" + String.Join (" ", nowQArray.Ans) + "\n\nあなたの回答:\n" + String.Join ("", ss));
			ansText.transform.Find ("textBack").GetComponent<Image> ().color = Col [1];

			StartCoroutine (SetText (ansText, sentakuContent)); 

			wrong.SetActive (true);

		}

		if (Statics.strNull (nowQArray.Exp) == false) {
			GameObject expText = Instantiate (ansP);
			expText.GetComponent<Button> ().enabled = false;

			expText.GetComponent<AnsPrefab> ().ansText = ("解説：\n" + nowQArray.Exp);
			expText.transform.Find ("textBack").GetComponent<Image> ().color = Col [1];

			StartCoroutine (SetText (expText, ansContent)); 
		}

		this.GetComponent<DbProcess> ().UpdateQuesData (nowQData);

		nextQ.SetActive (true);


	}

	public void ChooseOpen ()
	{
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

		GameObject ansText = Instantiate (ansP);
		ansText.GetComponent<Button> ().enabled = false;

		ansText.GetComponent<AnsPrefab> ().ansText = ("正答:\n" + String.Join (" ", nowQArray.Ans));
		ansText.transform.Find ("textBack").GetComponent<Image> ().color = Col [1];

		StartCoroutine (SetText (ansText, sentakuContent)); 

		if (Statics.strNull (nowQArray.Exp) == false) {

			hide.GetComponent<ScrollRect> ().enabled = true;
			hide.transform.Find ("expText").gameObject.GetComponent<Text> ().text = ("解説:\n" + nowQArray.Exp);
			hide.GetComponent<ScrollRect> ().verticalNormalizedPosition = 0f;
		}
	}

	public void SelfAnswerNext (bool b)
	{
		nowQData.LAST = DateTime.Now.ToString ();

		if (b == true) {

			nowQData.REVIEW -= 1;


			if (nowQData.REVIEW < 0) {

				if (nowQData.LEVEL != 4) {
					nowQData.LEVEL += 1;
				}

				nowQData.REVIEW = 0;

			} else if (nowQData.REVIEW == 0) {

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

		}

		this.GetComponent<DbProcess> ().UpdateQuesData (nowQData);

		ViewQuestion ();

	}

	string[] GetUserAns ()
	{
		List<string> ls = new List<string> ();

		foreach (string[] s in senAnsList) {
			ls.Add (s [1]);
		}

		string[] userAns = ls.ToArray ();

		return userAns;
	}

}

