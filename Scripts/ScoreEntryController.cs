using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Paraphernalia.Components;
using Cadence;

public class ScoreEntryController : MonoBehaviour {

	public delegate void OnNameSubmitted(string name);
	public static event OnNameSubmitted onNameSubmitted = delegate {};

	public enum EntryType {
		Joystick,
		Trackball
	}
	public EntryType entryType = EntryType.Joystick;

	public int numLetters = 3;
	public Text[] letterSelectors;
	public Button okButton;
	public string selectSound = "select";
	public string confirmSound = "confirm";

	static char[] initials;
	static int[] letterIndices;
	int index = 0;
	bool ready = false;

	char[] letters = new char[] {
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 
		'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 
		'U', 'V', 'W', 'X', 'Y', 'Z' ,
		'@', '#', '$', '%', '^', '&', '*',
		'|', '.', '!', '?', '-',
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'_'		
	};

	void OnEnable () {
		StartCoroutine("Setup");
	}

	IEnumerator Setup () {
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		index = 0;
		EventSystem.current.SetSelectedGameObject(letterSelectors[index].gameObject);
		ready = true;
	}

	void Start () {
		if (initials == null) {
			initials = new char[] {'A', 'A', 'A'};
			letterIndices = new int[initials.Length];
		}

		for (int i = 0; i < initials.Length; i++) {
			letterSelectors[i].text = initials[i].ToString();
		}
	}

	public void UpdateIndex (int dir) {
		AudioManager.PlayEffect(selectSound);
		index = Mathf.Max(index + dir, 0);
		if (index < letterSelectors.Length) {
			EventSystem.current.SetSelectedGameObject(letterSelectors[index].gameObject);
		}
		else {
			index = letterSelectors.Length;
			EventSystem.current.SetSelectedGameObject(okButton.gameObject);
		}
	}

	public void UpdateLetter (int dir) {
		if (index >= letterIndices.Length) return;
		AudioManager.PlayEffect(selectSound);
		letterIndices[index] = (letterIndices[index] + dir + letters.Length) % letters.Length;
		char letter = letters[letterIndices[index]];
		initials[index] = letter;
		letterSelectors[index].text = letter.ToString();
	}

	public void NextPressed () {
		UpdateIndex(1);
	}

	public void OKPressed () {
		ready = false;
		AudioManager.PlayEffect(confirmSound);
		onNameSubmitted(new string(initials));
	}

	Vector2 lastDir;
	void Update () {
		if (!ready) return;

		Vector2 dir = Vector2.zero;
		
		switch (entryType) {
			case EntryType.Joystick:
				dir.x = Input.GetAxisRaw("Horizontal");
				dir.y = Input.GetAxisRaw("Vertical");
				break;
			case EntryType.Trackball:
				if (Input.GetMouseButtonDown(0)) {
					if (index < letterIndices.Length) NextPressed();
					else OKPressed();
				}
				dir.x = Input.GetAxisRaw("Mouse X");
				dir.y = Input.GetAxisRaw("Mouse Y");
				break;
			default:
				break;
		}

		if (lastDir.magnitude < 0.5f && dir.magnitude >= 0.5f) {
			if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) UpdateIndex((int)Mathf.Sign(dir.x));
			else UpdateLetter((int)Mathf.Sign(dir.y));
		}

		lastDir = dir;
	}
}
