using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

using System.Collections.Generic;
using System;
using System.Linq;

#if UNITY_IOS
using NotificationType = UnityEngine.iOS.NotificationType;
using LocalNotification = UnityEngine.iOS.LocalNotification;
using NotificationServices = UnityEngine.iOS.NotificationServices;
#endif

#if UNITY_ANDROID
using Assets.SimpleAndroidNotifications;
#endif

public class UpdateReviewNotice : MonoBehaviour
{
	public GameObject offBanner;

	ReviewTimes[] rta;

	public struct ReviewTimes
	{
		public int needReviewNum;
		public float maxTime;

		public ReviewTimes (int p1, float p2)
		{
			needReviewNum = p1;
			maxTime = p2;
		}
	}

	void Start ()
	{

		#if UNITY_IOS
		NotificationServices.RegisterForNotifications (
			NotificationType.Alert |
			NotificationType.Badge |
			NotificationType.Sound);
		#endif

	}

	public void SetNotification (string message, int delayTime, int badgeNumber = 1)
	{
		#if UNITY_IOS
		var l = new LocalNotification ();
		l.applicationIconBadgeNumber = badgeNumber;
		l.fireDate = System.DateTime.Now.AddSeconds (delayTime);
		l.alertBody = message;
		NotificationServices.ScheduleLocalNotification (l);

		if (Debug.isDebugBuild)
			Debug.Log ("通知セット" + message + delayTime + "秒後" + badgeNumber);
		#endif
	
	}


	void OnApplicationPause (bool pauseStatus)
	{
		if (pauseStatus) {
			
			if (offBanner != null) {
				offBanner.GetComponent<OfflineBanner> ().enabled = false;
			}

			SetReviewNotice ();
		} else {

			#if UNITY_IOS
			NotificationServices.CancelAllLocalNotifications ();

			#endif

			#if UNITY_ANDROID
			NotificationManager.CancelAll ();
			#endif


			if (offBanner != null) {
				offBanner.GetComponent<OfflineBanner> ().enabled = true;
			}

			if (Debug.isDebugBuild)
				Debug.Log ("通知消去");

		}
	}

	void OnApplicationQuit ()
	{
		SetReviewNotice ();
	}

	void SetReviewNotice ()
	{

		#if UNITY_IOS
		NotificationServices.CancelAllLocalNotifications ();

		#endif

		#if UNITY_ANDROID
		NotificationManager.CancelAll ();
		#endif
		
		List<QuesData> qdall = this.GetComponent<DbProcess> ().GetDbDataAll ();

		if (qdall == null || qdall.Count == 0) {


			#if UNITY_IOS
			ObC.SetBadge (0);

			#endif

			return;
		}

		int nowNeedReview = 0;
		rta = new ReviewTimes[4];

		List<QuesData> dbzero = this.GetComponent<DbProcess> ().GetDbDataLevel (0);

		nowNeedReview = dbzero.Count;

		for (int i = 1; i < 5; i++) {
			List<QuesData> dbData = this.GetComponent<DbProcess> ().GetDbDataLevel (i);

			DateTime latest = new DateTime (1000, 1, 1, 0, 0, 0);

			foreach (QuesData qd in dbData) {
				if (qd.LAST.Length > 5 && qd.REVIEW == 0) {
					DateTime d = DateTime.Parse (qd.LAST);

					if (TimeFunctions.NeedReview (qd.LAST, i)) {
						nowNeedReview++;
					}

					if (latest < d) {
						latest = d;
					}
				}
			}

			if (latest.Year != 1000) {
				rta [i - 1].maxTime = TimeFunctions.GetNextTime (latest.ToString (), i);
			} else {
				rta [i - 1].maxTime = 0f;
			}

			if (i == 1) {
				rta [i - 1].needReviewNum = (dbzero.Count + dbData.Count);
			} else {
				rta [i - 1].needReviewNum = dbData.Count;
			}
		}

		for (int i = 2; i < 5; i++) {
			rta [i - 1].needReviewNum += rta [i - 2].needReviewNum;
		}


		#if UNITY_IOS
		foreach (ReviewTimes rt in rta) {
			if (rt.needReviewNum > 0 && rt.maxTime > 0) {
				SetNotification ("要復習の問題が" + rt.needReviewNum + "問あります", (int)rt.maxTime, rt.needReviewNum);
			}
		}

		ObC.SetBadge (nowNeedReview);
		#endif

		#if UNITY_ANDROID
		foreach (ReviewTimes rt in rta) {
		if (rt.needReviewNum > 0 && rt.maxTime > 0) {
		NotificationManager.SendWithAppIcon (TimeSpan.FromSeconds ((double)rt.maxTime), "要復習のお知らせ", rt.needReviewNum + "問の問題があります", new Color (0, 0.6f, 1), NotificationIcon.Bell);
		}
		}
		#endif


	}
}
