using System.Collections;
using System.Collections.Generic;
using DeadMosquito.IosGoodies;
using UnityEngine;
using UnityEngine.UI;

public class PhotoImage : MonoBehaviour
{

	public GameObject targetImage;
	public GameObject text;
	public GameObject ImageView;

	public Texture2D tex2d;
	public Sprite texture;


	SaveImage si = null;

	public void ImageButton ()
	{
		Debug.Log ("image button:" + text.activeSelf);


		if (text.activeSelf == true) {
			GetSetImage ();
		} else {

			ImageView.SetActive (true);
		}

	}

	public void GetSetImage ()
	{

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

		ImageView.SetActive (false);
	}

	public void ViewFalse ()
	{
		ImageView.SetActive (false);
	}

	public void ResetImage ()
	{		

		si = GameObject.Find ("EventSystem").GetComponent<SaveImage> ();

		targetImage.GetComponent<Image> ().sprite = Resources.Load ("drill_ansback") as Sprite;

		if (this.name == "wakuQ") {
			Debug.Log ("setQnull");
			si.imageQ = null;
		} else if (this.name == "wakuA") {
			Debug.Log ("setAnull");
			si.imageA = null;
		}
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

		if (this.name == "wakuQ" || this.name == "viewQ") {
			Debug.Log ("setQtex");
			si.imageQ = tex2d;
		} else if (this.name == "wakuA" || this.name == "viewA") {
			Debug.Log ("setAtex");
			si.imageA = tex2d;
		}

		targetImage.GetComponent<Image> ().sprite = texture;

		GameObject iv = ImageView.transform.Find ("image").gameObject;
		iv.GetComponent<Image> ().sprite = texture;

		text.SetActive (false);

	}
}
