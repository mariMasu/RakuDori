using System.Collections;
using System.Collections.Generic;
using DeadMosquito.IosGoodies;
using UnityEngine;
using UnityEngine.UI;

public class ImageP : MonoBehaviour
{

	public GameObject targetImage;
	public GameObject text;

	public Texture2D tex2d;
	public Sprite texture;

	public int quesId;
	public bool isQ;

	SaveImage si = null;

	public void SetDefault ()
	{
		DbProcess dp = GameObject.Find ("EventSystem").GetComponent<DbProcess> ();
		QuesData qd = dp.GetDbData (quesId);

		if (isQ == true && qd.IMAGE_Q != "なし") {

			Debug.Log ("gettex:" + qd.IMAGE_Q);

			string path = System.IO.Path.Combine (Application.persistentDataPath, (qd.IMAGE_Q));

			byte[] bytesRead;

			try {
				bytesRead = System.IO.File.ReadAllBytes (path);

			} catch (System.Exception ex) {
				Debug.Log (ex);
				return;
			}

			Texture2D tex = new Texture2D (1024, 1024);
			tex.LoadImage (bytesRead);

			Set2dImage (tex);
		}

		if (isQ == false && qd.IMAGE_A != "なし") {
			Debug.Log ("gettex:" + qd.IMAGE_A);

			string path = System.IO.Path.Combine (Application.persistentDataPath, (qd.IMAGE_A));
			byte[] bytesRead;

			try {
				bytesRead = System.IO.File.ReadAllBytes (path);

			} catch (System.Exception ex) {
				Debug.Log (ex);
				return;
			}
			Texture2D tex = new Texture2D (1024, 1024);
			tex.LoadImage (bytesRead);

			Set2dImage (tex);
		}
	}

	public void ImageButton ()
	{
		si = GameObject.Find ("EventSystem").GetComponent<SaveImage> ();

		Debug.Log ("image button:" + text.activeSelf);


		if (text.activeSelf == true) {
			GetSetImage ();
		} else {
			si.SetMainView (texture, this.GetComponent<ImageP> ());
			si.ViewTrue ();
		}

	}

	public void GetSetImage ()
	{
		si = GameObject.Find ("EventSystem").GetComponent<SaveImage> ();

		const bool allowEditing = true;
		const float compressionQuality = 0.1f;

		IGImagePicker.PickImageFromPhotosAlbum (tex => {
			Debug.Log ("Successfully picked image from photos album");

			Set2dImage (tex);

		}, 
			() => {
				Debug.Log ("Picking image from photos album cancelled");
				ResetImage ();
			}, 
			compressionQuality,
			allowEditing);

		// IMPORTANT! Call this method to clean memory if you are picking and discarding images
		Resources.UnloadUnusedAssets ();

		si.ViewFalse ();
	}

	public void ResetImage ()
	{		

		si = GameObject.Find ("EventSystem").GetComponent<SaveImage> ();

		targetImage.GetComponent<Image> ().sprite = Resources.Load ("drill_ansback") as Sprite;

		si.SetTempNull (quesId, isQ);

		text.SetActive (true);
	}

	public void Set2dImage (Texture2D tex)
	{
		si = GameObject.Find ("EventSystem").GetComponent<SaveImage> ();

		if (tex == null) {
			Debug.Log ("texnull");
			return;
		}

		tex2d = tex;
		texture = Statics.SpriteFromTex2D (tex2d);

		si.SetTempImages (quesId, isQ, tex);

		targetImage.GetComponent<Image> ().sprite = texture;

		text.SetActive (false);

	}
}
