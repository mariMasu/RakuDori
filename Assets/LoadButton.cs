using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

using NendUnityPlugin.AD;

public class LoadButton : MonoBehaviour
{

	public GameObject nend;

	public void LoadQuesSentaku ()
	{
		NendReset ();
		SceneManager.LoadScene ("quesSentaku");

	}

	public void LoadQuesInput ()
	{
		NendReset ();
		SceneManager.LoadScene ("quesInput");

	}

	public void LoadDrillSentaku ()
	{
		NendReset ();
		SceneManager.LoadScene ("drillSentaku");

	}

	public void LoadDrillAns ()
	{
		NendReset ();
		SceneManager.LoadScene ("drillAns");

	}

	public void LoadDrillAnsSingle ()
	{
		NendReset ();
		Statics.ResetReviewList ();
		Statics.AddReviewList ();
		SceneManager.LoadScene ("drillAns");

	}

	public void LoadTitle ()
	{
		NendReset ();
		SceneManager.LoadScene ("title");

	}

	void NendReset ()
	{

		if (nend != null) {
			// NendAdBannerをアタッチしたGameObjectにアタッチされた別スクリプトでの実装と仮定します
			var banner = nend.GetComponent <NendAdBanner> ();
			banner.Pause ();
			banner.Hide ();
		}
	}
}
