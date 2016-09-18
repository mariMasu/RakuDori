using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;


public class QuesAdd : MonoBehaviour {

	private List<DrillData> _dbData;

	public SimpleSQL.SimpleSQLManager dbManager;

	public GameObject drill;

	private GameObject content;


	void Start()
	{
		content = GameObject.Find ("Content");

		Reset();
	}

	void View()
	{

		foreach (DrillData dbData in _dbData)
		{
			Debug.Log (dbData.ID + dbData.NAME + dbData.COLOR + dbData.LAST);

			GameObject go = Instantiate (drill);

			GameObject ba =  go.transform.FindChild ("base").gameObject;
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

			go.transform.SetParent(content.transform);
			go.transform.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);

		}

	}

	private void Reset()
	{

		foreach ( Transform n in content.transform )
		{
			GameObject.Destroy(n.gameObject);
		}

		// Loads the player stats from the database using Linq
		_dbData = new List<DrillData> (from ps in dbManager.Table<DrillData> () select ps);

		View();

	}

	/// <summary>
	/// Saves the player stats by using the PlayerStats class structure. No need for SQL here.
	/// </summary>
	/// <param name='playerName'>
	/// Player name.
	/// </param>
	/// <param name='totalKills'>
	/// Total kills.
	/// </param>
	/// <param name='points'>
	/// Points.
	/// </param>
	public void AddSimple()
	{

		PopupWindow pw = this.GetComponent<PopupWindow> ();

		string name = pw.InputText;
		//string last = DateTime.Now.ToString();

		if (name == null) {
			name = " ";
		}

		DrillData data = new DrillData { NAME = name, COLOR = pw.ColNum , LAST = "なし"};

		dbManager.Insert(data);

		Reset ();
	}

	/// <summary>
	/// Saves the player stats by executing a SQL statement. Note that no data is returned, this only modifies the table
	/// </summary>
	/// <param name='playerName'>
	/// Player name.
	/// </param>
	/// <param name='totalKills'>
	/// Total kills.
	/// </param>
	/// <param name='points'>
	/// Points.
	/// </param>
	public void AddQuery()
	{
		// Call our SQL statement using ? to bind our variables
		//dbManager.Execute("INSERT INTO DBdrillData (PlayerName, TotalKills, Points) VALUES (?, ?, ?)", playerName, totalKills, points);
	}

}
