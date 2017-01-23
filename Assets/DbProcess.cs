using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;

public class DbProcess : MonoBehaviour
{

	public SimpleSQL.SimpleSQLManager dbManager;

	public List<QuesData> GetDbDataAll ()
	{
		List<QuesData> dbData = new List<QuesData> (from ps in dbManager.Table<QuesData> ()
		                                            select ps);

		return dbData;
	}

	public List<QuesData> GetDbDataLevel (int i)
	{
		List<QuesData> dbData = new List<QuesData> (from ps in dbManager.Table<QuesData> ()
		                                            select ps);

		dbData = dbData.FindAll (s => s.LEVEL == i);

		return dbData;
	}

	public List<QuesData> GetDbDataDrillId (int i)
	{
		List<QuesData> dbData = new List<QuesData> (from ps in dbManager.Table<QuesData> ()
		                                            select ps);

		dbData = dbData.FindAll (s => s.DRILL_ID == i);

		return dbData;
	}

	public QuesData GetDbData (int i)
	{
		List<QuesData> dbData = new List<QuesData> (from ps in dbManager.Table<QuesData> ()
		                                            select ps);

		QuesData qd = dbData.Find (s => s.ID == i);

		return qd;
	}

	public List<DrillData> GetDbDrillDataAll ()
	{
		List<DrillData> dbData = new List<DrillData> (from ps in dbManager.Table<DrillData> ()
		                                              select ps);

		return dbData;
	}

	public DrillData GetDbDrillData (int i)
	{
		List<DrillData> dbData = new List<DrillData> (from ps in dbManager.Table<DrillData> ()
		                                              select ps);

		DrillData qd = dbData.Find (s => s.ID == i);

		return qd;
	}


	public void AddFirst (List<string> text)
	{

		foreach (string t in text) {
			QuesData data = new QuesData { DRILL_ID = Statics.nowDrill, TEXT = t, IMAGE_Q = "なし", IMAGE_A = "なし", SOUND = "なし", LAST = "なし", EX2 = "なし"
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
				REVIEW = 0,
				IMAGE_Q = "なし", 
				IMAGE_A = "なし",
				SOUND = "なし",
				LAST = "なし",
				EX2 = "なし"
			};
			dbManager.UpdateTable (dat);
		}
	}

	public void UpdateTag (int idT, int tag)
	{
		List<QuesData> qd = this.GetComponent<QuesSentakuMaster> ().dbData;

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

	public void AddQues (QuesData qd)
	{
		int jun;
		if (qd.ID == 0) {
			jun = -1000;
		} else {
			jun = qd.ID * 1000;
		}

		qd.JUN = jun;

		dbManager.Insert (qd);
	}

	public void AddDrill (DrillData dd)
	{

		dbManager.Insert (dd);
	}

	public void DeleteDrill ()
	{

		DrillData dd = new DrillData { ID = Statics.nowDrill };

		dbManager.Delete<DrillData> (dd);

		this.GetComponent<LoadButton> ().LoadDrillSentaku ();
	}

	public void UpdateQuesJun (int nowNum, int newNum)
	{
		List<QuesData> qd = this.GetComponent<QuesSentakuMaster> ().dbData;

		QuesData nowData = qd [nowNum];
		QuesData newData = qd [newNum];
		if (newNum == 0) {
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

		if (name == Statics.nowName && col == Statics.nowColor) {
			return;
		}

		List<DrillData> dd = new List<DrillData> (from ps in dbManager.Table<DrillData> ()
		                                          select ps);

		DrillData data = dd.Find (s => s.ID == Statics.nowDrill);

		data.COLOR = col;
		data.NAME = name;

		dbManager.UpdateTable (data);
	}

	public void UpdateDrillOrder (int orderQ, int orderA, int dumU, int dumT)
	{


		List<DrillData> dd = new List<DrillData> (from ps in dbManager.Table<DrillData> ()
		                                          select ps);

		DrillData data = dd.Find (s => s.ID == Statics.nowDrill);

		data.QUES_ORDER = orderQ;
		data.ANS_ORDER = orderA;
		data.DUMMY_USE = dumU;
		data.DUMMY_TAG = dumT;


		dbManager.UpdateTable (data);
	}

	public void UpdateQuesData (QuesData qd)
	{

		dbManager.UpdateTable (qd);
	}

}
