using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;


public class DrillAdd : MonoBehaviour
{

	private List<DrillData> _dbData;

	public SimpleSQL.SimpleSQLManager dbManager;

	public GameObject drill;

	private GameObject content;


	void Awake ()
	{
		content = GameObject.Find ("Content");

		Reset ();
	}

	void View ()
	{

		foreach (DrillData dbData in _dbData) {
			Debug.Log (dbData.ID + dbData.NAME + dbData.COLOR + dbData.LAST);

			GameObject go = Instantiate (drill);

			GameObject ba = go.transform.FindChild ("base").gameObject;
			GameObject obi = go.transform.FindChild ("obi").gameObject;
			GameObject name = go.transform.FindChild ("name").gameObject;
			GameObject record = go.transform.FindChild ("record").gameObject;

			Color[] col = DrillColor.GetColorD (dbData.COLOR);
			ba.GetComponent<Image> ().color = col [0];
			obi.GetComponent<Image> ().color = col [1];
			name.GetComponent<Text> ().text = dbData.NAME;

			string lastTime = dbData.LAST;
			if (lastTime.Length > 5) {
				lastTime = DrillTime.GetLastTime (dbData.LAST);
			}

			record.GetComponent<Text> ().text = (lastTime + "\n" + dbData.OKNUM + "／" + dbData.QNUM);

			go.GetComponent<DrillNum> ().id = dbData.ID;
			go.GetComponent<DrillNum> ().color = dbData.COLOR;
			go.GetComponent<DrillNum> ().name = dbData.NAME;

			go.transform.SetParent (content.transform);
			go.transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

		}

	}

	private void Reset ()
	{
		
		foreach (Transform n in content.transform) {
			GameObject.Destroy (n.gameObject);
		}

		// Loads the player stats from the database using Linq
		_dbData = new List<DrillData> (from ps in dbManager.Table<DrillData> ()
		                               select ps);

		View ();

	}

	public void AddSimple (int col, string name)
	{
		if (name == null) {
			name = " ";
		}

		DrillData data = new DrillData { NAME = name, COLOR = col, LAST = "なし" };

		dbManager.Insert (data);

		Reset ();
	}
}
