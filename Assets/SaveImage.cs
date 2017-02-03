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

	public struct ImagePre
	{
		public int id;
		public Texture2D imageQ;
		public Texture2D imageA;

		public ImagePre (int i, Texture2D Q = null, Texture2D A = null)
		{
			id = i;
			imageQ = Q;
			imageA = A;
		}
	}

	public List<ImagePre> images;

	public ImageP nowWaku;

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

	public void SaveMulti ()
	{
		DbProcess dp = this.GetComponent<DbProcess> ();

		for (int i = 0; i < images.Count; i++) {
			ImagePre ip = images [i];
			QuesData qd = dp.GetDbData (ip.id);

			if (ip.imageQ != null) {
				byte[] pngarr = ip.imageQ.EncodeToPNG ();

				string path = System.IO.Path.Combine (Application.persistentDataPath, qd.ID.ToString () + "_Q.png");


				try {
					Debug.Log ("save to" + path);
					File.WriteAllBytes (path, pngarr);

					qd.IMAGE_Q = (qd.ID.ToString () + "_Q.png");

				} catch (System.Exception ex) {
					Debug.Log (ex);
				}

			} else {
				qd.IMAGE_Q = ("なし");

				try {
					string path = System.IO.Path.Combine (Application.persistentDataPath, qd.ID.ToString () + "_Q.png");
					Debug.Log ("delete" + path);

					File.Delete (path);

				} catch (System.Exception ex) {
					Debug.Log (ex);
				}
			}

			if (ip.imageA != null) {
				byte[] pngarr = ip.imageA.EncodeToPNG ();

				string path = System.IO.Path.Combine (Application.persistentDataPath, qd.ID.ToString () + "_A.png");
				try {

					Debug.Log ("save to" + path);
					File.WriteAllBytes (path, pngarr);

					qd.IMAGE_A = (qd.ID.ToString () + "_A.png");

				} catch (System.Exception ex) {
					Debug.Log (ex);
				}

			} else {
				qd.IMAGE_A = ("なし");

				try {
					string path = System.IO.Path.Combine (Application.persistentDataPath, qd.ID.ToString () + "_A.png");
					Debug.Log ("delete" + path);

					File.Delete (path);

				} catch (System.Exception ex) {
					Debug.Log (ex);
				}
			}

			this.GetComponent<DbProcess> ().UpdateQuesData (qd);
		}

		this.GetComponent<PopupWindow> ().Popdown (14);
	}

	public void ViewFalse ()
	{
		mainView.SetActive (false);
	}

	public void ViewTrue ()
	{
		mainView.SetActive (true);
	}

	public void SetTempImages (int quesId, bool isQ, Texture2D tex)
	{

		bool texnakami;
		if (tex == null) {
			texnakami = false;
		} else {
			texnakami = true;
		}

		Debug.Log (quesId + ":" + isQ + ":" + texnakami);


		for (int i = 0; i < images.Count; i++) {
			ImagePre ip = images [i];

			if (ip.id == quesId) {
				if (isQ == true) {
					ip.imageQ = tex;
					Debug.Log ("setQtex:" + quesId);

				} else {
					ip.imageA = tex;
					Debug.Log ("setAtex:" + quesId);

				}
				images [i] = ip;
			}

		}

	}

	public void SetTempNull (int quesId, bool isQ)
	{
		for (int i = 0; i < images.Count; i++) {
			ImagePre ip = images [i];

			if (ip.id == quesId) {
				if (isQ == true) {
					ip.imageQ = null;
					Debug.Log ("setQnull");

				} else {
					ip.imageA = null;
					Debug.Log ("setAnull");

				}
				images [i] = ip;
			}

		}
	}

	public void SetMainView (Sprite tex, ImageP t)
	{
		nowWaku = t;

		GameObject iv = mainView.transform.Find ("image").gameObject;
		iv.GetComponent<Image> ().sprite = tex;
	}

	public void CreateImageList (List<int> senList)
	{
		images = new List<ImagePre> ();

		foreach (int i in senList) {
			images.Add (new ImagePre (i));
		}

		Debug.Log ("CreateImageList" + images.Count);

	}

	public void SetImageInView ()
	{

		const bool allowEditing = true;
		const float compressionQuality = 0.03f;

		IGImagePicker.PickImageFromPhotosAlbum (tex => {
			Debug.Log ("Successfully picked image from photos album");
			nowWaku.Set2dImage (tex);

		}, 
			() => {
				Debug.Log ("Picking image from photos album cancelled");
				nowWaku.ResetImage ();
			}, 
			compressionQuality,
			allowEditing);

		// IMPORTANT! Call this method to clean memory if you are picking and discarding images
		Resources.UnloadUnusedAssets ();

		ViewFalse ();
	}
}
