using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Cadence {
public class SettingsManager : MonoBehaviour {

	public delegate void OnSettingsChanged();
	public static event OnSettingsChanged onSettingsChanged = delegate{};

    public bool allowExit = false;
    public float exitHoldTime = 5;
    public KeyCode[] exitButtonCombo = new KeyCode[] {
        KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R
    };
    private float lastExitRelease;

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
	public LoadSceneMode loadSceneMode = LoadSceneMode.Additive;
	private CursorLockMode cursorLockStateInGame;
	private bool cursorVisibleInGame;
	private string exitToScene;

	void Awake () {
        lastExitRelease = Time.time;
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

        if (allowExit) {
            bool shouldExit = true;
            foreach (KeyCode key in exitButtonCombo) {
                if (!Input.GetKey(key)) {
                    shouldExit = false;
                    break;
                }
            }
            if (!shouldExit) lastExitRelease = Time.time;

            if (Time.time - lastExitRelease > exitHoldTime) {
                Debug.Log("Application Quitting");
                Application.Quit();
            }
        }
	}

	public static void EnterSettings () {
		instance.cursorVisibleInGame = Cursor.visible;
		instance.cursorLockStateInGame = Cursor.lockState;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		instance.exitToScene = SceneManager.GetActiveScene().name;
		SceneManager.LoadScene(instance.settingsScene, instance.loadSceneMode);
	}

	public static void ExitSettings () {
		Cursor.visible = instance.cursorVisibleInGame;
		Cursor.lockState = instance.cursorLockStateInGame;
		if (instance.loadSceneMode == LoadSceneMode.Single) {
			SceneManager.LoadScene(instance.exitToScene);
		}
		else {
			SceneManager.UnloadSceneAsync(instance.settingsScene);
		}

		onSettingsChanged();
	}
}
}