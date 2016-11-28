﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;

public class DbProcess : MonoBehaviour
{

	public SimpleSQL.SimpleSQLManager dbManager;

	public void AddFirst (List<string> text)
	{

		foreach (string t in text) {
			QuesData data = new QuesData { DRILL_ID = SceneData.nowDrill, TEXT = t, IMAGE = "なし", LAST = "なし"
			};

			dbManager.Insert (data);
		}

		List<QuesData> dbData = new List<QuesData> (from ps in dbManager.Table<QuesData> ()
		                                            select ps);
		List<QuesData> newData = new List<QuesData> ();

		for (int i = dbData.Count - text.Count; i < dbData.Count; i++) {
			newData.Add (dbData [i]);
		}

		foreach (QuesData qd in newData) {

			int jun;
			if (qd.ID == 0) {
				jun = -1000;
			} else {
				jun = qd.ID * 1000;
			}

			QuesData dat = new QuesData {
				ID = qd.ID,
				DRILL_ID = qd.DRILL_ID,
				TEXT = qd.TEXT,
				JUN = jun,
				REVIEW = 3,
				IMAGE = "なし",
				LAST = "なし"
			};
			dbManager.UpdateTable (dat);
		}
	}

	public void UpdateTag (int idT, int tag)
	{
		List<QuesData> qd = this.GetComponent<QuesView> ().dbData;

		QuesData data = qd.Find (s => s.ID == idT);

		if (data.TAG == tag) {
			return;
		}

		data.TAG = tag;
		dbManager.UpdateTable (data);
	}

	public void DeleteSelection (List<int> sele)
	{

		foreach (int s in sele) {

			QuesData qd = new QuesData { ID = s };

			dbManager.Delete<QuesData> (qd);

		}
	}

	public void DeleteDrill ()
	{

		DrillData dd = new DrillData { ID = SceneData.nowDrill };

		dbManager.Delete<DrillData> (dd);

		this.GetComponent<LoadButton> ().LoadDrillSentaku ();
	}

	public void UpdateQuesJun (int nowNum, int newNum)
	{
		List<QuesData> qd = this.GetComponent<QuesView> ().dbData;

		QuesData nowData = qd [nowNum];
		QuesData newData = qd [newNum];
		if (newNum == 1) {
			nowData.JUN = newData.JUN - 1;
		} else {
			nowData.JUN = newData.JUN + 1;
		}

		dbManager.UpdateTable (nowData);
	}

	public void UpdateDrillNC (int col, string name)
	{

		if (name == null) {
			name = " ";
		}

		if (name == SceneData.nowName && col == SceneData.nowColor) {
			return;
		}

		List<DrillData> dd = new List<DrillData> (from ps in dbManager.Table<DrillData> ()
		                                          select ps);

		DrillData data = dd.Find (s => s.ID == SceneData.nowDrill);

		data.COLOR = col;
		data.NAME = name;

		dbManager.UpdateTable (data);
	}

}