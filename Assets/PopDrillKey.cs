﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class PopDrillKey : MonoBehaviour
{

	public GameObject senDrop;
	public GameObject senInput;
	public GameObject sepDrop;
	public GameObject sepInput;
	public GameObject dummyDrop;
	public GameObject dummyInput;
	public GameObject ansInput;
	public GameObject ansDrop;
	public GameObject quesInput;
	public GameObject quesDrop;
	public GameObject expDrop;
	public GameObject expInput;
	public GameObject perDrop;
	public GameObject perInput;
	public GameObject toggle1;

	public GameObject[] drops = new GameObject[7];
	public GameObject[] inputs = new GameObject[7];

	void Awake ()
	{
		drops [0] = senDrop;
		drops [1] = quesDrop;
		drops [2] = ansDrop;
		drops [3] = sepDrop;
		drops [4] = expDrop;
		drops [5] = dummyDrop;
		drops [6] = perDrop;

		inputs [0] = senInput;
		inputs [1] = quesInput;
		inputs [2] = ansInput;
		inputs [3] = sepInput;
		inputs [4] = expInput;
		inputs [5] = dummyInput;
		inputs [6] = perInput;
	}



	public void PopDrillKeyView ()
	{
		QuesInputMaster sd = this.GetComponent<QuesInputMaster> ();

		float gapX = quesInput.GetComponent<RectTransform> ().localPosition.x - quesDrop.GetComponent<RectTransform> ().localPosition.x;
		float gapY = (float)((quesInput.GetComponent<RectTransform> ().sizeDelta.y) * 2.3);
		Vector3 quesPos = quesDrop.GetComponent<RectTransform> ().localPosition;
		quesPos.y -= gapY / 2;

		if (sd.inputPat == 0 || sd.inputPat == 1) {
			quesDrop.GetComponent<Dropdown> ().options [0].text = "改行";

			ansDrop.transform.position = new Vector3 (10000, 0, 0);
			ansInput.transform.position = new Vector3 (10000, 0, 0);
			expDrop.transform.position = new Vector3 (10000, 0, 0);
			expInput.transform.position = new Vector3 (10000, 0, 0);

			sepDrop.GetComponent<RectTransform> ().localPosition = new Vector3 (quesPos.x, quesPos.y - gapY, 0);
			sepInput.GetComponent<RectTransform> ().localPosition = new Vector3 ((quesPos.x + gapX), quesPos.y - gapY, 0);
			dummyDrop.GetComponent<RectTransform> ().localPosition = new Vector3 (quesPos.x, quesPos.y - gapY * 2, 0);
			dummyInput.GetComponent<RectTransform> ().localPosition = new Vector3 ((quesPos.x + gapX), quesPos.y - gapY * 2, 0);
			perDrop.GetComponent<RectTransform> ().localPosition = new Vector3 (quesPos.x, quesPos.y - gapY * 3, 0);
			perInput.GetComponent<RectTransform> ().localPosition = new Vector3 ((quesPos.x + gapX), quesPos.y - gapY * 3, 0);

			//popKeyText.GetComponent<Text> ().text = "・選択式問題の選択肢設定\n\n\n・問題文の区切り\n\n・正答、ダミー内の分割\n\n・ダミー選択肢の開始\n\n・正答の順序を守る問題";
		} else {
			quesDrop.GetComponent<Dropdown> ().options [0].text = "@@(半角)";

			int m = 1;

			for (int i = 2; i < 7; i++) {
				drops [i].GetComponent<RectTransform> ().localPosition = new Vector3 (quesPos.x, quesPos.y - gapY * m, 0);
				inputs [i].GetComponent<RectTransform> ().localPosition = new Vector3 ((quesPos.x + gapX), quesPos.y - gapY * m, 0);

				m++;
			}

			//popKeyText.GetComponent<Text> ().text = "・選択式問題の選択肢設定\n\n\n・問題文の区切り\n\n・正答文の開始\n\n・正答、ダミー内の分割\n\n・解説文の開始\n\n・ダミー選択文の開始\n\n・正答の順序を守る問題";
		}
		quesDrop.GetComponent<Dropdown> ().value = 1;
		quesDrop.GetComponent<Dropdown> ().value = 0;

		DropViewReset ();

		toggle1.GetComponent<Toggle> ().isOn = sd.toHankaku;


	}


	public void DropViewReset ()
	{

		foreach (GameObject d in drops) {
			d.GetComponent<DrillKeyButton> ().ChangeDropByText ();
		}

	}

	public bool CustomCheck ()
	{
		foreach (GameObject i in inputs) {
			if (i.activeSelf && (i.GetComponent<InputField> ().text == "")) {
				return false;
			}
		}

		return true;
	}

}
