using UnityEngine;
using System.Collections;

namespace Cadence {
public class SettingsManager : MonoBehaviour {

	private static SettingsManager _instance;
	public static SettingsManager instance {
		get {
		    if (_instance == null) {
				GameObject settingsMan = new GameObject("Cadence.SettingsManager");
				_instance = settingsMan.AddComponent<SettingsManager>();
			}
			return _instance;
		}
	}

	private string sceneToLoad;
	private bool cursorVisibleInGame;

	void Awake () {
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad(_instance.gameObject);
		}
		else if (_instance != this) {
			Debug.LogWarning("SettingsManager already initialized, destroying duplicate");
			GameObject.Destroy(this);
		}
	}

	void Update () {
		if ((Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0)) &&
			(Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift)) &&
			(Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt))) {
			EnterSettings();
		}
	}

	public static void EnterSettings () {
		instance.sceneToLoad = Application.loadedLevelName;
		instance.cursorVisibleInGame = Cursor.visible;
		Cursor.visible = true;
		Application.LoadLevel("CadenceSettings");
	}

	public static void ExitSettings () {
		Cursor.visible = instance.cursorVisibleInGame;
		Application.LoadLevel(instance.sceneToLoad);
	}
}
}