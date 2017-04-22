using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cadence {
public class CoinDoorToggle : MonoBehaviour {

	public Text checkbox;
	public GameObject toggleableGameObject;

	public string checkedString = "☑";
	public string uncheckedString = "☐";

	void Start () {
		SetChecked(TokenManager.instance.hasCoinDoor);
	}

	void SetChecked (bool isChecked) {
		checkbox.text = isChecked ? checkedString : uncheckedString;
		toggleableGameObject.SetActive(isChecked);
	}

	public void Toggle() {
		TokenManager.instance.hasCoinDoor = !TokenManager.instance.hasCoinDoor;
		SetChecked(TokenManager.instance.hasCoinDoor);
	}
}
}
