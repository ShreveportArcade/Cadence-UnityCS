using UnityEngine;
using System.Collections;

namespace Cadence {
public class ExitSettingsButton : MonoBehaviour {

	public void ExitSettings () {
		SettingsManager.ExitSettings();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			ExitSettings();
		}
	}
}
}