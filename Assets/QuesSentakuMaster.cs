using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;


public class QuesSentakuMaster : MonoBehaviour
{
	
	public GameObject quesP;
	private GameObject content;

	public GameObject deleteDrillB;

	public bool sentakuMode = false;

	[SerializeField]
	List<int> senList;

	public int sortTag = 6;

	public List<QuesData> dbDataPlain;
	public List<QuesData> dbData;
	public QuesData nowQData;

	public GameObject imageP;

	void Awake ()
	{
		senList = new List<int> ();

		content = GameObject.Find ("QuesContent");
		SentakuQuesView ();
	}


	public void SentakuQuesView ()
	{
		
		dbDataPlain = this.GetComponent<DbProcess> ().GetDbDataDrillId (Statics.nowDrill);
		dbData = dbDataPlain;

		if (sortTag != 6) {
			dbData = dbData.FindAll (s => s.TAG == sortTag);
		}

		dbData.Sort ((a, b) => a.JUN - b.JUN);

		senList.Clear ();

		foreach (Transform n in content.transform) {
			GameObject.Destroy (n.gameObject);
		}


		int i = 1;
		foreach (QuesData d in dbData) {

			GameObject go = Instantiate (quesP);

			GameObject ba = go.transform.FindChild ("base").gameObject;
			GameObject obi = go.transform.FindChild ("obi").gameObject;
			GameObject zyun = go.transform.FindChild ("zyun").gameObject;
			GameObject ques = go.transform.FindChild ("quesText").gameObject;
			GameObject ans = go.transform.FindChild ("ansText").gameObject;
			GameObject num = go.transform.FindChild ("numText").gameObject;
			GameObject tag = go.transform.FindChild ("tag").gameObject;

			GameObject level1 = go.transform.FindChild ("level1").gameObject;
			GameObject level2 = go.transform.FindChild ("level2").gameObject;
			GameObject level3 = go.transform.FindChild ("level3").gameObject;
			GameObject level4 = go.transform.FindChild ("level4").gameObject;
			GameObject level5 = go.transform.FindChild ("level5").gameObject;


			//Debug.Log (d);

			go.GetComponent<QuesBannar> ().id = d.ID;
			go.GetComponent<QuesBannar> ().jun = i;

			string lastTime;
			if (d.LAST.Length > 5) {
				lastTime = TimeFunctions.GetLastString (d.LAST);
			} else {
				lastTime = "なし";
			}

			num.GetComponent<Text> ().text = i + "　最終：" + lastTime;

			i++;

			Color[] col = DrillColor.GetColorD (Statics.nowColor);
			ba.GetComponent<Image> ().color = col [0];
			obi.GetComponent<Image> ().color = col [1];

			if (d.TAG == 0) {
				tag.SetActive (false);
			} else {

				Color[] tCol = DrillColor.GetColorD (d.TAG);
				tag.GetComponent<Image> ().color = tCol [1];
			}

			level1.SetActive (false);
			level2.SetActive (false);
			level3.SetActive (false);
			level4.SetActive (false);
			level5.SetActive (false);


			if (d.LEVEL == 0) {
			} else if (d.LEVEL == 1) {
				level1.SetActive (true);
			} else if (d.LEVEL == 2) {
				level2.SetActive (true);
			} else if (d.LEVEL == 3) {
				level3.SetActive (true);
			} else if (d.LEVEL == 4) {
				level4.SetActive (true);
			} else {
				level5.SetActive (true);
			}

			QuesArray qa = DbTextToQA.DbToQA (d.TEXT);

			if (DbTextToQA.IsPer (qa.Ques)) {
				zyun.SetActive (true);
				ques.GetComponent<Text> ().text = qa.Ques.Substring (QuesTextEdit.PerKeyCommon.Length);
			} else {
				zyun.SetActive (false);
				ques.GetComponent<Text> ().text = qa.Ques;
			}
				
			string ansS = "";

			foreach (string s in qa.Ans) {
				ansS += ("/" + s);
			}

			ans.GetComponent<Text> ().text = ansS.Substring (1);

			go.transform.SetParent (content.transform);
			go.transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

		}

		if (dbDataPlain.Count > 0) {
			deleteDrillB.SetActive (false);
		} else {
			deleteDrillB.SetActive (true);
		}
	}

	public void SetSentakuId (int i)
	{

		if (senList.Contains (i) == true) {
			senList.Remove (i);
		} else {
			senList.Add (i);
		}
	}

	public void SetPopQuesEdit (int id)
	{

		nowQData = this.GetComponent<DbProcess> ().GetDbData (id);
		this.GetComponent<PopupWindow> ().PopQuesEdit (nowQData);

	}

	public void SetSentakuMode (bool b)
	{

		if (b == true) {
			this.sentakuMode = true;

			foreach (Transform n in content.transform) {
				GameObject editB = n.gameObject.transform.FindChild ("editB").gameObject;
				editB.GetComponent<Button> ().enabled = false;
				editB.GetComponent<Image> ().enabled = false;
			}

		} else {
			this.sentakuMode = false;
			foreach (Transform n in content.transform) {
				GameObject editB = n.gameObject.transform.FindChild ("editB").gameObject;
				editB.GetComponent<Image> ().enabled = true;
				editB.GetComponent<Button> ().enabled = true;

				GameObject go = n.gameObject;
				go.GetComponent<QuesBannar> ().SentakuFalse ();
			}
			senList.Clear ();
		}
	}

	public void PopDelete ()
	{
		if (senList.Count == 0) {
			return;
		} else {
			this.GetComponent<PopupWindow> ().Popup (12);

		}
	}

	public void DeleteSenB ()
	{

		this.GetComponent<DbProcess> ().DeleteSelection (senList);
		SentakuQuesView ();
		this.GetComponent<PopupWindow> ().Popdown (12);
		this.GetComponent<PopupWindow> ().Popdown (2);

	}

	public void TagSenB (int tagC)
	{

		foreach (int s in senList) {
			this.GetComponent<DbProcess> ().UpdateTag (s, tagC);

		}
		SentakuQuesView ();

	}

	public void CopyQuesB (int drillId, bool tagCopy, bool levelCopy)
	{

		string time = DateTime.Now.ToString ();

		foreach (int s in senList) {
			
			QuesData qd = this.GetComponent<DbProcess> ().GetDbData (s);
			QuesData newqd = new QuesData { DRILL_ID = drillId, TEXT = qd.TEXT, IMAGE_Q = "なし", IMAGE_A = "なし",  SOUND_Q = "なし", SOUND_A = "なし", LAST = time, EX2 = "なし"
			};

			if (tagCopy) {
				newqd.TAG = qd.TAG;
			}

			if (levelCopy) {
				newqd.LEVEL = qd.LEVEL;
				newqd.LAST = qd.LAST;
				newqd.REVIEW = qd.REVIEW;
			}

			this.GetComponent<DbProcess> ().AddQues (newqd);

		}

		SentakuQuesView ();

	}

	public bool SentakuNull ()
	{
		if (senList.Count == 0) {
			return true;
		} else {
			return false;
		}
	}

	public void SentakuAll ()
	{
		if (SentakuNull () == true) {

			foreach (Transform c in content.transform) {
				c.GetComponent<QuesBannar> ().OnSentaku ();
			}
		} else {
			foreach (Transform c in content.transform) {
				c.GetComponent<QuesBannar> ().SentakuFalse ();
			}
			senList.Clear ();
			
		}
	}

	public int GetQuesCount ()
	{
		return dbData.Count;
	}

	public int GetPlainQuesCount ()
	{
		return dbDataPlain.Count;
	}

	public void PopSetImageMulti ()
	{
		if (senList.Count == 0) {
			return;
		}

		GameObject parent = GameObject.Find ("PopSetMultiImage");
		GameObject icontent = parent.transform.Find ("scroll/Scroll View/Viewport/imageContent").gameObject;

		foreach (Transform n in icontent.transform) {
			GameObject.Destroy (n.gameObject);
		}

		DbProcess dp = this.GetComponent<DbProcess> ();
		this.GetComponent<SaveImage> ().CreateImageList (senList);

		foreach (int seni in senList) {
			//Debug.Log (dbData.ID + dbData.NAME + dbData.COLOR);

			QuesData qd = dp.GetDbData (seni);

			GameObject go = Instantiate (imageP);

			GameObject text = go.transform.Find ("Qtext/quesText").gameObject;
			GameObject wakuQ = go.transform.Find ("wakuQ").gameObject;
			GameObject wakuA = go.transform.Find ("wakuA").gameObject;



			QuesArray qa = DbTextToQA.DbToQA (qd.TEXT);

			string ansText = "";

			foreach (string s in qa.Ans) {
				ansText += ("/" + s);
			}
			ansText = ansText.Substring (1);

			string qtext = qa.Ques;

			if (qa.Ques.Length > QuesTextEdit.PerKeyCommon.Length) {
				if (qa.Ques.Substring (0, QuesTextEdit.PerKeyCommon.Length) == QuesTextEdit.PerKeyCommon) {
					qtext = qa.Ques.Substring (QuesTextEdit.PerKeyCommon.Length);
				}
			}

			string newText = "質問文\n" + qtext + "\n\n正答\n" + ansText + "\n\n解説\n" + qa.Exp;
			text.GetComponent<Text> ().text = newText;

			wakuQ.GetComponent<ImageP> ().quesId = qd.ID;
			wakuA.GetComponent<ImageP> ().quesId = qd.ID;

			go.transform.SetParent (icontent.transform);
			go.transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

			wakuQ.GetComponent<ImageP> ().SetDefault ();
			wakuA.GetComponent<ImageP> ().SetDefault ();

			go.transform.Find ("Qtext").gameObject.GetComponent<ScrollRect> ().verticalNormalizedPosition = 1f;

		}
		this.GetComponent<PopupWindow> ().Popup (14);
	}
}
