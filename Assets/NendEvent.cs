using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using NendUnityPlugin.AD;
using NendUnityPlugin.Common;

public class NendEvent : MonoBehaviour
{

	public GameObject offBanner;

	void Awake ()
	{
		offBanner.SetActive (false);
		NendAdBanner banner = this.GetComponent<NendAdBanner> ();

		banner.AdLoaded += OnFinishLoadBannerAd;
		banner.AdReceived += OnReceiveBannerAd;
		banner.AdFailedToReceive += OnFailToReceiveBannerAd;
		banner.AdClicked += OnClickBannerAd;
		banner.AdBacked += OnDismissScreen;
		banner.InformationClicked += OnClickInformation;
	}

	public void OnFinishLoadBannerAd (object sender, EventArgs args)
	{
		if (Debug.isDebugBuild)
			UnityEngine.Debug.Log ("広告のロードが完了しました。");
	}

	public void OnClickBannerAd (object sender, EventArgs args)
	{
		if (Debug.isDebugBuild)
			UnityEngine.Debug.Log ("広告がクリックされました。");
	}

	public void OnReceiveBannerAd (object sender, EventArgs args)
	{
		if (Debug.isDebugBuild)
			UnityEngine.Debug.Log ("広告の受信に成功しました。");

	}

	public void OnFailToReceiveBannerAd (object sender, NendAdErrorEventArgs args)
	{
		if (Debug.isDebugBuild)
			UnityEngine.Debug.Log ("広告の受信に失敗しました。エラーメッセージ: " + args.Message);

		offBanner.SetActive (true);
	}

	public void OnDismissScreen (object sender, EventArgs args)
	{
		if (Debug.isDebugBuild)
			UnityEngine.Debug.Log ("広告が画面上に復帰しました。");
		
	}

	public void OnClickInformation (object sender, EventArgs args)
	{
		if (Debug.isDebugBuild)
			UnityEngine.Debug.Log ("インフォメーションボタンがクリックされオプトアウトページに遷移しました。");
	}
}
