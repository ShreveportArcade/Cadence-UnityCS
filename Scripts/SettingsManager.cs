using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

	public string settingsScene = "CadenceSettings";
	private bool cursorVisibleInGame;

	void Awake () {
		if (_instance == null) {
			_instance = this;
			if (_instance.transform.parent == null) DontDestroyOnLoad(_instance.gameObject);
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
		instance.cursorVisibleInGame = Cursor.visible;
		Cursor.visible = true;
		SceneManager.LoadSceneAsync(instance.settingsScene, LoadSceneMode.Additive);
	}

	public static void ExitSettings () {
		Cursor.visible = instance.cursorVisibleInGame;
		SceneManager.UnloadSceneAsync(instance.settingsScene);
	}
}
}