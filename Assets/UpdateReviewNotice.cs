using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

using System.Collections.Generic;
using System;
using System.Linq;

using NotificationType = UnityEngine.iOS.NotificationType;
using LocalNotification = UnityEngine.iOS.LocalNotification;
using NotificationServices = UnityEngine.iOS.NotificationServices;

public class UpdateReviewNotice : MonoBehaviour
{

	//public float timeOut = 3600f;
	//private float timeElapsed = 0f;
	//bool isTitle = false;

	ReviewTimes[] rta;

	public struct ReviewTimes
	{
		public int levelNum;
		public float maxTime;

		public ReviewTimes (int p1, float p2)
		{
			levelNum = p1;
			maxTime = p2;
		}
	}

	void Start ()
	{
		NotificationServices.RegisterForNotifications (
			NotificationType.Alert |
			NotificationType.Badge |
			NotificationType.Sound);
	}

	public void SetNotification (string message, int delayTime, int badgeNumber = 1)
	{
		var l = new LocalNotification ();
		l.applicationIconBadgeNumber = badgeNumber;
		l.fireDate = System.DateTime.Now.AddSeconds (delayTime);
		l.alertBody = message;
		NotificationServices.ScheduleLocalNotification (l);
	}

	public void SetBadgeNotification (int badgeNumber)
	{
		var l = new LocalNotification ();
		l.applicationIconBadgeNumber = badgeNumber;
		NotificationServices.ScheduleLocalNotification (l);
	}

	//	void Update ()
	//	{
	//		timeElapsed += Time.deltaTime;
	//
	//		if (timeElapsed >= timeOut) {
	//			// Do anything
	//
	//			timeElapsed = 0.0f;
	//		}
	//	}

	void OnApplicationPause (bool pauseStatus)
	{
		if (pauseStatus) {
			SetReviewNotice ();
		} else {
			NotificationServices.CancelAllLocalNotifications ();
		}
	}

	void OnApplicationQuit ()
	{
		SetReviewNotice ();
	}

	void SetReviewNotice ()
	{

		NotificationServices.CancelAllLocalNotifications ();


		int nowNeedReview = 0;
		rta = new ReviewTimes[4];

		List<QuesData> dbzero = this.GetComponent<DbProcess> ().GetDbDataLevel (0);

		if (dbzero != null) {
			nowNeedReview = dbzero.Count;

			for (int i = 1; i < 5; i++) {
				List<QuesData> dbData = this.GetComponent<DbProcess> ().GetDbDataLevel (i);

				DateTime oldest = new DateTime (9999, 1, 1, 0, 0, 0);

				foreach (QuesData qd in dbData) {
					if (qd.LAST.Length > 5) {
						DateTime d = DateTime.Parse (qd.LAST);

						if (TimeFunctions.NeedReview (qd.LAST, i)) {
							nowNeedReview++;
						}

						if (oldest < d) {
							oldest = d;
						}
					}
				}
				if (oldest.Year != 9999) {
					rta [i - 1].maxTime = TimeFunctions.GetNextTime (oldest.ToString (), i);
				} else {
					rta [i - 1].maxTime = 0f;
				}

				if (i == 1) {
					rta [i - 1].levelNum = (dbzero.Count + dbData.Count);
				} else {
					rta [i - 1].levelNum = dbData.Count;
				}
			}

			for (int i = 2; i < 5; i++) {
				rta [i - 1].levelNum += rta [i - 2].levelNum;
			}

			foreach (ReviewTimes rt in rta) {
				if (rt.levelNum != 0) {
					SetNotification ("要復習の問題が" + rt.levelNum + "問あります", (int)rt.maxTime, rt.levelNum);
				}
			}

			SetBadgeNotification (nowNeedReview);

		}

	}
}
