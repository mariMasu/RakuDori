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
	public GameObject textPE;

	public GameObject textIP;

	public GameObject longBase;
	public GameObject shortBase;

	public GameObject open;
	public GameObject imageBA;

	public GameObject imageView;

	public Sprite sprQ;
	public Sprite sprA;

	GameObject ansContent;
	GameObject sentakuContent;

	public QuesData nowQData;
	public QuesArray nowQArray;

	public List<string[]> senAnsList;

	public int ansOrder = 0;

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
	GameObject nigate;
	GameObject zyun;
	GameObject correct;
	GameObject wrong;

	GameObject next;
	GameObject nextQ;

	GameObject hide;
	GameObject ok;
	GameObject bad;

	public GameObject LimageBQ;
	public GameObject SimageBQ;
	public GameObject imageEdit;

	bool isLongAns;
	GameObject questionBigContent;

	GameObject QtextImage;
	GameObject AtextImage1;
	GameObject AtextImage2;
	GameObject QtextImageB;

	Vector3 levPosL;
	Vector3 imaQPosL;
	Vector3 levPosB;
	Vector3 imaQPosB;

	void Awake ()
	{

		if (Statics.reviewList.Count == 0) {
			this.GetComponent<LoadButton> ().LoadDrillSentaku ();
			return;

		} else {

			Statics.nowDrill = Statics.reviewList [0];
			Statics.reviewList.RemoveAt (0);

		}

		levPosB = new Vector3 (-272, 236, 0);
		imaQPosB = new Vector3 (256, 206, 0);
		levPosL = new Vector3 (-272, -15, 0);
		imaQPosL = new Vector3 (256, -26.75f, 0);
			
		Statics.prefabGap = ((GameObject.Find ("help").GetComponent<RectTransform> ().sizeDelta.x) / 2);

		List<DrillData> dd = new List<DrillData> (from ps in dbManager.Table<DrillData> ()
		                                          select ps);
		DrillData drill = dd.Find (s => s.ID == Statics.nowDrill);

		Statics.nowSevere = Convert.ToBoolean (drill.SEVERE);
		MakeReviewList (drill);

		longBase.transform.position = new Vector3 (10000, 0, 0);
		shortBase.transform.position = new Vector3 (10000, 0, 0);

		ViewQuestion ();


	}

	void MakeReviewList (DrillData dd)
	{

		Statics.nowColor = dd.COLOR;
		Col = DrillColor.GetColorD (dd.COLOR);

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
			
		ansOrder = dd.ANS_ORDER;


		return;

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
			List<QuesData> removeList = new List<QuesData> ();

			s = "苦手問題が存在しません";

			foreach (QuesData qd in questionList) {

				int suu = (qd.CORRECT + qd.WRONG);
				if (suu != 0) {
					float nigate = ((float)qd.WRONG / (float)suu);

					if (nigate <= PlayerPrefs.GetFloat ("NIGATE", 0.25f)) {
						removeList.Add (qd);
					}
				} else {
					removeList.Add (qd);
				}
			}

			foreach (QuesData q in removeList) {
				questionList.Remove (q);
			}
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
			
			if (ansBase != null) {
				
				ansBase.transform.position = new Vector3 (10000, 0, 0);

			}

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



			char[] chars = strAS.ToCharArray ();
			int[] charsByte = Statics.GetByteLengthSJis (chars);

			int ansLen = Statics.IntSum (charsByte);

			if (ansLen > 26 && q.Ans.Length > 1) {
				ansBase = longBase;
				isLongAns = true;
			} else {
				ansBase = shortBase;
				isLongAns = false;

			}

			ansBase.transform.position = new Vector3 (0, 0, 0);

			if (ansBase.activeSelf == false) {
				ansBase.SetActive (true);
				Debug.Log ("ansbase false");

			}

			correct = ansBase.transform.Find ("correct").gameObject;
			wrong = ansBase.transform.Find ("wrong").gameObject;

			level0 = ansBase.transform.Find ("level0").gameObject;
			level1 = ansBase.transform.Find ("level1").gameObject;
			level2 = ansBase.transform.Find ("level2").gameObject;
			level3 = ansBase.transform.Find ("level3").gameObject;
			level4 = ansBase.transform.Find ("level4").gameObject;
			level5 = ansBase.transform.Find ("level5").gameObject;

			zyun = ansBase.transform.Find ("zyun").gameObject;
			nigate = ansBase.transform.Find ("nigate").gameObject;

			next = ansBase.transform.Find ("next").gameObject;
			nextQ = ansBase.transform.Find ("nextQ").gameObject;

			hide = ansBase.transform.Find ("hide").gameObject;
			ok = ansBase.transform.Find ("ok").gameObject;
			bad = ansBase.transform.Find ("bad").gameObject;

			setTagLevActive ();
			setIconActive ();
			setImage ();

			if (ansOrder == 0) {
				Fisher_Yates_CardDeck_Shuffle ();
			} else if (ansOrder == 1) {
				StringComparer cmp = StringComparer.OrdinalIgnoreCase;
				ansList.Sort (cmp);
			}

			ansContent = ansBase.transform.Find ("choose/scroll/Scroll View/Viewport/AnsContent").gameObject;
			sentakuContent = ansBase.transform.Find ("answer/scroll/Scroll View/Viewport/SentakuContent").gameObject;

			gol.Clear ();

			foreach (Transform n in ansContent.transform) {
				GameObject.Destroy (n.gameObject);
			}
			ansContent.GetComponent<VerticalLayoutGroup> ().enabled = false;
			ansContent.GetComponent<ContentSizeFitter> ().enabled = false;

			foreach (Transform n in sentakuContent.transform) {
				GameObject.Destroy (n.gameObject);
			}




			GameObject ansS = ansBase.transform.Find ("choose/scroll").gameObject;
			GameObject senS = ansBase.transform.Find ("answer/scroll").gameObject;

			hide.transform.Find ("setumei").gameObject.GetComponent<Text> ().text = "右上の開錠アイコンで自己評価、\n下の枠をタップで選択肢を表示します";

			ansContent.GetComponent<RectTransform> ().sizeDelta = ansS.GetComponent<RectTransform> ().sizeDelta;
			sentakuContent.GetComponent<RectTransform> ().sizeDelta = senS.GetComponent<RectTransform> ().sizeDelta;

			for (int i = 0; i < ansList.Count; i++) {
				GameObject ans = Instantiate (ansP);

				gol.Add (ans);

				ans.GetComponent<AnsPrefab> ().ansText = ansList [i];
				ans.transform.Find ("textBack").GetComponent<Image> ().color = Col [1];

				StartCoroutine (SetText (ans, ansContent)); 

			}

			GameObject texto = ansBase.transform.Find ("question/Qtext/Viewport/Content/Text").gameObject;

			if (nowQData.REVIEW > 0) {
				texto.GetComponent<Text> ().text = "<size=30><color=#CE0606FF>＊不正解だったため復習中です。\nあと" + nowQData.REVIEW + "回正解しましょう</color></size>\n\n";
			} else {
				texto.GetComponent<Text> ().text = "";
			}


			if (zyun.activeSelf == true) {
				texto.GetComponent<Text> ().text += q.Ques.Substring ((QuesTextEdit.PerKeyCommon.Length));
			} else {
				texto.GetComponent<Text> ().text += q.Ques;
			}

			if (isLongAns) {
				questionBigContent = ansBase.transform.Find ("questionBig/Qtext/Viewport/Content").gameObject;
				questionBigContent.transform.Find ("Text").GetComponent<Text> ().text = texto.GetComponent<Text> ().text;

				if (sprQ != null) {
					QtextImageB.SetActive (true);
				}

				ansBase.transform.Find ("questionBig").gameObject.SetActive (true);

			}

			ansBase.transform.Find ("question/Qtext/Viewport/Content/Text").GetComponent<RectTransform> ().pivot = new Vector2 (1, 1);

			GameObject view = ansBase.transform.Find ("question/Qtext").gameObject;
			StartCoroutine (CorScrollNormalize (view)); 

			if (sprQ == null) {
				StartCoroutine (CorImageSet ()); 
			}

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


		return;
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

	private IEnumerator CorImageSet ()
	{  
		yield return StartCoroutine (Wait ());  
		QtextImage.SetActive (false);

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

		GameObject view = ansBase.transform.Find ("answer/scroll/Scroll View").gameObject;
		StartCoroutine (CorScrollNormalize (view, 0.1f)); 

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
		hide.transform.Find ("Viewport/Content/expText").gameObject.GetComponent<Text> ().text = "";
		hide.transform.Find ("Viewport/Content/ansText").gameObject.GetComponent<Text> ().text = "";


		correct.SetActive (false);
		wrong.SetActive (false);

		next.SetActive (false);
		nextQ.SetActive (false);

		if (nowQData.TEXT.Substring (0, QuesTextEdit.PerKeyCommon.Length) == QuesTextEdit.PerKeyCommon) {
			zyun.SetActive (true);
		} else {
			zyun.SetActive (false);
		}

		SetNigateIcon ();

		if (isLongAns) {
			level0.GetComponent<RectTransform> ().localPosition = levPosL;
			level1.GetComponent<RectTransform> ().localPosition = levPosL;
			level2.GetComponent<RectTransform> ().localPosition = levPosL;
			level3.GetComponent<RectTransform> ().localPosition = levPosL;
			level4.GetComponent<RectTransform> ().localPosition = levPosL;
			level5.GetComponent<RectTransform> ().localPosition = levPosL;

			LimageBQ.GetComponent<RectTransform> ().localPosition = imaQPosL;
		}

	}

	public void setImage ()
	{

		if (isLongAns) {
			QtextImage = ansBase.transform.Find ("question/Qtext/Viewport/Content/Image").gameObject;
			QtextImageB = ansBase.transform.Find ("questionBig/Qtext/Viewport/Content/Image").gameObject;

			AtextImage1 = ansBase.transform.Find ("hide/Viewport/Content/Image").gameObject;

			QtextImage.SetActive (false);
			AtextImage1.SetActive (false);
			QtextImageB.SetActive (false);
		} else {
			QtextImage = ansBase.transform.Find ("question/Qtext/Viewport/Content/Image").gameObject;
			AtextImage1 = ansBase.transform.Find ("hide/Viewport/Content/Image").gameObject;
			AtextImage2 = ansBase.transform.Find ("question/Qtext/Viewport/Content/ImageA").gameObject;

			QtextImage.SetActive (false);
			AtextImage1.SetActive (false);
			AtextImage2.SetActive (false);
		}

		imageBA.SetActive (false);
		LimageBQ.SetActive (false);
		SimageBQ.SetActive (false);
		imageEdit.SetActive (false);

		sprQ = Statics.pathToSprite (nowQData.IMAGE_Q);
		sprA = Statics.pathToSprite (nowQData.IMAGE_A);

		#if UNITY_EDITOR
		if ((nowQData.ID % 2) == 0) {
			sprQ = Resources.Load <Sprite> ("icon");
			sprA = Resources.Load <Sprite> ("icon");
		}
		#endif

		if (sprQ != null) {
			GameObject activeImage = ansBase.transform.Find ("imageBQ").gameObject;
			activeImage.SetActive (true);

			QtextImage.GetComponent<Image> ().sprite = sprQ;
			QtextImage.SetActive (true);

			if (isLongAns) {
				QtextImageB.GetComponent<Image> ().sprite = sprQ;
			}
		} else {
			QtextImage.GetComponent<Image> ().sprite = Resources.Load <Sprite> ("white");
			QtextImage.SetActive (true);
		}

		if (sprA != null) {
			AtextImage1.GetComponent<Image> ().sprite = sprA;
			if (isLongAns != true)
				AtextImage2.GetComponent<Image> ().sprite = sprA;
		}

		if (nextQ.activeSelf == true || ok.activeSelf == true) {
			if (sprA != null) {
				imageBA.SetActive (true);
			}
			imageEdit.SetActive (true);
		}

	}

	public void AnswerNext ()
	{

		bool youhukusyu = TimeFunctions.NeedReview (nowQData.LAST, nowQData.LEVEL);

		nowQData.LAST = DateTime.Now.ToString ();

		foreach (Transform n in ansContent.transform) {
			GameObject.Destroy (n.gameObject);
		}

		ansContent.GetComponent<VerticalLayoutGroup> ().enabled = true;
		ansContent.GetComponent<ContentSizeFitter> ().enabled = true;


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

			nowQData.CORRECT += 1;
			nowQData.REVIEW -= 1;

			if (nowQData.REVIEW < 0) {


				if (nowQData.LEVEL == 0) {
					nowQData.LEVEL += 1;
				} else if (youhukusyu == true && nowQData.LEVEL != 5) {
					if (Statics.nowSevere == false) {
						nowQData.LEVEL += 1;
					}
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

			nowQData.WRONG += 1;

			if (nowQData.LEVEL > 1) {
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

		imageEdit.SetActive (true);

		if (sprA != null) {
			imageBA.SetActive (true);
			if (isLongAns != true)
				AtextImage2.SetActive (true);
		
			if (isLongAns) {
				GameObject image = Instantiate (textIP);

				image.GetComponent<Image> ().sprite = sprA;

				image.transform.SetParent (ansContent.transform);
				image.GetComponent<RectTransform> ().pivot = new Vector2 (1, 1);
				image.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
			}
		}

		if (Statics.StrNull (nowQArray.Exp) == false) {
			GameObject expText = Instantiate (textPE);

			expText.GetComponent<Text> ().text = ("解説：\n" + nowQArray.Exp);

			expText.transform.SetParent (ansContent.transform);
			expText.GetComponent<RectTransform> ().pivot = new Vector2 (1, 1);
			expText.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

		}

		GameObject view = ansBase.transform.Find ("choose/scroll/Scroll View").gameObject;
		StartCoroutine (CorScrollNormalize (view, 0.1f)); 

		SetNigateIcon ();


		this.GetComponent<DbProcess> ().UpdateQuesData (nowQData);

		nextQ.SetActive (true);


	}

	public void ChooseOpen ()
	{
		hide.transform.Find ("setumei").gameObject.GetComponent<Text> ().text = "";

		if (isLongAns) {
			ansBase.transform.Find ("questionBig").gameObject.SetActive (false);

		}
		ContentNarabi (0);
		open.SetActive (false);
		hide.SetActive (false);

		next.SetActive (true);

		if (isLongAns) {
			level0.GetComponent<RectTransform> ().localPosition = levPosB;
			level1.GetComponent<RectTransform> ().localPosition = levPosB;
			level2.GetComponent<RectTransform> ().localPosition = levPosB;
			level3.GetComponent<RectTransform> ().localPosition = levPosB;
			level4.GetComponent<RectTransform> ().localPosition = levPosB;
			level5.GetComponent<RectTransform> ().localPosition = levPosB;

			LimageBQ.GetComponent<RectTransform> ().localPosition = imaQPosB;
		}

	}

	public void AnswerOpen ()
	{
		open.SetActive (false);

		hide.GetComponent<Button> ().enabled = false;
		ok.SetActive (true);
		bad.SetActive (true);

		imageEdit.SetActive (true);
		if (sprA != null) {
			imageBA.SetActive (true);
		}

		SetNigateIcon ();

		hide.transform.Find ("setumei").gameObject.GetComponent<Text> ().text = "";

		hide.transform.Find ("Viewport/Content/ansText").gameObject.GetComponent<Text> ().text = ("正答:\n" + String.Join (" ", nowQArray.Ans));

		if (Statics.StrNull (nowQArray.Exp) == false || sprA != null) {
			hide.GetComponent<ScrollRect> ().enabled = true;

			if (sprA != null) {
				AtextImage1.SetActive (true);
			}
			if (Statics.StrNull (nowQArray.Exp) == false) {
				hide.transform.Find ("Viewport/Content/expText").gameObject.GetComponent<Text> ().text = ("解説:\n" + nowQArray.Exp);

				StartCoroutine (CorSetAnchor (hide.transform.Find ("Viewport/Content").gameObject, hide.transform.Find ("Viewport/Content/expText").gameObject)); 
			}

		}
	}

	public void SelfAnswerNext (bool b)
	{

		bool youhukusyu = TimeFunctions.NeedReview (nowQData.LAST, nowQData.LEVEL);
		nowQData.LAST = DateTime.Now.ToString ();

		if (b == true) {

			nowQData.CORRECT += 1;
			nowQData.REVIEW -= 1;


			if (nowQData.REVIEW < 0) {

				if (nowQData.LEVEL == 0) {
					nowQData.LEVEL += 1;
				} else if (youhukusyu == true && nowQData.LEVEL != 5) {
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

			nowQData.WRONG += 1;

			if (nowQData.LEVEL > 1) {
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

	private IEnumerator CorScrollNormalize (GameObject scroll, float wait = 0.01f)
	{ 
		yield return StartCoroutine (Wait (wait));  
		scroll.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1f;
	}

	private IEnumerator CorSetAnchor (GameObject contentGo, GameObject textGo)
	{  
		yield return StartCoroutine (Wait ());  

		Vector2 wh = textGo.GetComponent<RectTransform> ().sizeDelta;
		contentGo.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, wh.y);

	}

	public void ViewImageMain (bool Q)
	{ 
		GameObject im = imageView.transform.Find ("image").gameObject;

		if (Q == true) {
			im.GetComponent<Image> ().sprite = sprQ;
		} else {
			im.GetComponent<Image> ().sprite = sprA;

		}
		imageView.SetActive (true);
	}

	public void SetNigateIcon ()
	{ 
		int suu = (nowQData.CORRECT + nowQData.WRONG);
		float wrong = (float)nowQData.WRONG;
		if (suu != 0) {
			float keisu = (wrong / (float)suu);

			if (keisu > PlayerPrefs.GetFloat ("NIGATE", 0.25f)) {
				nigate.SetActive (true);
			} else {
				nigate.SetActive (false);
			}
		} else {
			nigate.SetActive (false);
		}

	}

}

