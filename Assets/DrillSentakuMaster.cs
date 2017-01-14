using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;


public class DrillSentakuMaster : MonoBehaviour
{

	private List<DrillData> _dbData;

	public GameObject drill;

	private GameObject content;

	public int[] nowNumQA;


	void Awake ()
	{
		content = GameObject.Find ("Content");

		Reset ();
	}

	void View ()
	{

		foreach (DrillData dbData in _dbData) {
			//Debug.Log (dbData.ID + dbData.NAME + dbData.COLOR);

			GameObject go = Instantiate (drill);

			GameObject ba = go.transform.FindChild ("base").gameObject;
			GameObject obi = go.transform.FindChild ("obi").gameObject;
			GameObject name = go.transform.FindChild ("name").gameObject;
			GameObject record = go.transform.FindChild ("record").gameObject;

			Color[] col = DrillColor.GetColorD (dbData.COLOR);
			ba.GetComponent<Image> ().color = col [0];
			obi.GetComponent<Image> ().color = col [1];
			name.GetComponent<Text> ().text = dbData.NAME;

//			string lastTime = dbData.LAST;
//			if (lastTime.Length > 5) {
//				lastTime = DrillTime.GetLastTime (dbData.LAST);
//			}

			int[] num = DbNumCount (dbData.ID);

			record.GetComponent<Text> ().text = (num [0] + "\n" + num [1] + "／" + num [2]);

			go.GetComponent<DrillNum> ().id = dbData.ID;
			go.GetComponent<DrillNum> ().color = dbData.COLOR;
			go.GetComponent<DrillNum> ().nameD = dbData.NAME;
			go.GetComponent<DrillNum> ().orderQ = dbData.QUES_ORDER;
			go.GetComponent<DrillNum> ().numQA = num;


			if (num [2] > 0) {
				go.GetComponent<DrillNum> ().existQ = true;

			}

			go.transform.SetParent (content.transform);
			go.transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

		}

	}

	public int[] DbNumCount (int drillId)
	{
		int rNum = 0;
		int qNum;
		int okNum;

		List<QuesData> dbData = this.GetComponent<DbProcess> ().GetDbDataAll ();

		dbData = dbData.FindAll (s => s.DRILL_ID == drillId);

		qNum = dbData.Count;
		okNum = dbData.FindAll (s => s.LEVEL == 5).Count;

		foreach (QuesData qd in dbData) {
			if (qd.LAST.Length > 5 && qd.REVIEW == 0) {
				if (TimeFunctions.NeedReview (qd.LAST, qd.LEVEL)) {
					rNum++;
				}
			} else {
				rNum++;
			}
		}

		return new int[] { rNum, okNum, qNum };
	}


	public void Reset ()
	{
		
		foreach (Transform n in content.transform) {
			GameObject.Destroy (n.gameObject);
		}

		_dbData = this.GetComponent<DbProcess> ().GetDbDrillDataAll ();

		View ();

	}

	public void AddSimple (int col, string name)
	{
		if (name == null) {
			name = " ";
		}

		DrillData data = new DrillData { NAME = name, COLOR = col };

		this.GetComponent<DbProcess> ().AddDrill (data);

		Reset ();
	}
		
}
