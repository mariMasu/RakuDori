using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;

public class TitleMaster : MonoBehaviour
{

	private List<DrillData> _dbData;

	public GameObject notice;
	public GameObject orderName;
	List<int> reviewDrillId;

	void Awake ()
	{
		notice.SetActive (false);
		SearchReviewQues ();
	}

	public void SearchReviewQues ()
	{
		List<QuesData> dbData = this.GetComponent<DbProcess> ().GetDbDataAll ();

		if (dbData != null) {
			List<int> reviewId = new List<int> ();

			int reviewC = 0;

			dbData = dbData.FindAll (s => s.LEVEL != 5);

			foreach (QuesData qd in dbData) {
				if (qd.LAST.Length > 5 && qd.REVIEW == 0) {
					if (TimeFunctions.NeedReview (qd.LAST, qd.LEVEL)) {
						reviewId.Add (qd.DRILL_ID);
						reviewC++;
					}
				} else {
					reviewId.Add (qd.DRILL_ID);
					reviewC++;

				}
			}


			if (reviewC > 0) {

				notice.transform.Find ("Text").gameObject.GetComponent<Text> ().text = reviewC.ToString ();
				notice.SetActive (true);


				reviewDrillId = reviewId.Distinct ().ToList ();
			}

		}
		return;
	}

	public void MatomeNext ()
	{


		if (notice.activeSelf == true) {
			List<DrillData> ddList = this.GetComponent<DbProcess> ().GetDbDrillDataAll ();

			foreach (int i in reviewDrillId) {
				DrillData dd = ddList.Find (s => s.ID == i);

				if (dd.QUES_ORDER == 0) {
					Statics.nowDrill = dd.ID;
					orderName.GetComponent<Text> ().text = "ドリル名:" + dd.NAME;
					this.GetComponent<PopupWindow> ().PopDrillOrder (1);
					return;
				}
			}

			Statics.ansChoose = 1;
			Statics.reviewList = reviewDrillId;
			Statics.youhukusyu = true;

			this.GetComponent<LoadButton> ().LoadDrillAns ();
		} else {
			this.GetComponent<PopupWindow> ().PopupCaution ("現在要学習の問題はありません", 2);

		}
	}

	void OnApplicationPause (bool isPause)
	{
		if (isPause == false) {
			SearchReviewQues ();

			if (Debug.isDebugBuild)
				Debug.Log ("タイトル最表示");
		}
	}
}
