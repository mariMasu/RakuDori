using System.Collections;
using System.Collections.Generic;
using DeadMosquito.IosGoodies;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class SaveImage : MonoBehaviour
{
	public GameObject mainView;

	public Texture2D imageQ = null;
	public Texture2D imageA = null;

	QuesData nowQ;


	public void SaveSimpleQS (int i)
	{
		QuesSentakuMaster qsm = this.GetComponent<QuesSentakuMaster> ();
		nowQ = qsm.nowQData;

		SaveSimple ();

		this.GetComponent<PopupWindow> ().Popdown (i);
	}

	public void SaveSimpleDA (int i)
	{
		DrillAnsMaster dsm = this.GetComponent<DrillAnsMaster> ();
		nowQ = dsm.nowQData;

		SaveSimple ();

		this.GetComponent<PopupWindow> ().Popdown (i);
	}

	public void SaveSimple ()
	{
		if (imageQ != null) {
			byte[] pngarr = imageQ.EncodeToPNG ();

			string path = System.IO.Path.Combine (Application.persistentDataPath, nowQ.ID.ToString () + "_Q.png");


			try {
				Debug.Log ("save to" + path);
				File.WriteAllBytes (path, pngarr);

				nowQ.IMAGE_Q = (nowQ.ID.ToString () + "_Q.png");

			} catch (System.Exception ex) {
				Debug.Log (ex);
			}
				
		} else if (imageQ == null) {
			nowQ.IMAGE_Q = ("なし");

			try {
				string path = System.IO.Path.Combine (Application.persistentDataPath, nowQ.ID.ToString () + "_Q.png");
				Debug.Log ("delete" + path);

				File.Delete (path);

			} catch (System.Exception ex) {
				Debug.Log (ex);
			}
		}

		if (imageA != null) {
			byte[] pngarr = imageA.EncodeToPNG ();

			string path = System.IO.Path.Combine (Application.persistentDataPath, nowQ.ID.ToString () + "_A.png");
			try {

				Debug.Log ("save to" + path);
				File.WriteAllBytes (path, pngarr);

				nowQ.IMAGE_A = (nowQ.ID.ToString () + "_A.png");

			} catch (System.Exception ex) {
				Debug.Log (ex);
			}

		} else if (imageA == null) {
			nowQ.IMAGE_A = ("なし");

			try {
				string path = System.IO.Path.Combine (Application.persistentDataPath, nowQ.ID.ToString () + "_A.png");
				Debug.Log ("delete" + path);

				File.Delete (path);

			} catch (System.Exception ex) {
				Debug.Log (ex);
			}
		}

		this.GetComponent<DbProcess> ().UpdateQuesData (nowQ);
	}

	public void ViewFalse ()
	{
		mainView.SetActive (false);
	}

}
