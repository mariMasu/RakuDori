using UnityEngine;
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

		foreach (QuesData qd in dbData) {
			QuesData dat = new QuesData {
				DRILL_ID = qd.DRILL_ID,
				TEXT = qd.TEXT,
				JUN = qd.DRILL_ID,
				REVIEW = 3,
				IMAGE = "なし",
				LAST = "なし"
			};
			dbManager.UpdateTable (dat);
		}
	}

	public void DeleteSelection (List<int> sele)
	{

		foreach (int s in sele) {

			QuesData qd = new QuesData { ID = s };

			dbManager.Delete<QuesData> (qd);


		}
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
