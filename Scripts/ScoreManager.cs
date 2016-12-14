using UnityEngine;
using System.Collections;

namespace Cadence {
public class ScoreManager : MonoBehaviour {

	public static ScoreManager instance; // TODO: should be a dictionary of gameKey to instance
	public string gameKey = "Game";

	public string defaultInitials = "---";
	public int[] defaultScoresArray = new int[] { 100000, 8000, 6000, 4500, 3000, 1000 };

	private string[] _initialsArray;
	public string[] initialsArray {
		get { return _initialsArray; }
	}
	private int[] _scoresArray;
	public int[] scoresArray {
		get { return _scoresArray; }
	}
	
	void Awake () {
		if (instance == null) instance = this;
		InitializeScores();
		LoadHighScores();
	}

	void InitializeScores () {
		_initialsArray = new string[defaultScoresArray.Length];
		for (int i = 0; i < defaultScoresArray.Length; i++) {
			_initialsArray[i] = defaultInitials;
		}
		_scoresArray = defaultScoresArray.Clone() as int[];
	}
	
	public void LoadHighScores () {
		for (int i = 0; i < defaultScoresArray.Length; i++) {
			_initialsArray[i] = PlayerPrefs.GetString(gameKey + ".HighScoreName" + i, defaultInitials);
			_scoresArray[i] = PlayerPrefs.GetInt(gameKey + ".HighScore" + i, defaultScoresArray[i]);
		}
	}

	public void SaveHighScores () {
		for (int i = 0; i < defaultScoresArray.Length; i++) {
			PlayerPrefs.SetString(gameKey + ".HighScoreName" + i, _initialsArray[i]);
			PlayerPrefs.SetInt(gameKey + ".HighScore" + i, _scoresArray[i]);
		}
		PlayerPrefs.Save();
	}

	public void ResetHighScores () {
		for (int i = 0; i < defaultScoresArray.Length; i++) {
			PlayerPrefs.DeleteKey(gameKey + ".HighScoreName" + i);
			PlayerPrefs.DeleteKey(gameKey + ".HighScore" + i);
		}
		PlayerPrefs.Save();
		InitializeScores();
	}

	public bool CheckHighScore (int score) {
		for (int i = 0; i < defaultScoresArray.Length; i++) {
			if (score > _scoresArray[i]) return true;
		}
		return false;
	}

	public void AddHighScore (int score, string initials) {
		int lastScore = 0;
		string lastInitials = "";
		bool highScoreSet = false;
		for (int i = 0; i < defaultScoresArray.Length; i++) {
			if (score > _scoresArray[i] || highScoreSet) {
				highScoreSet = true;
				lastScore = _scoresArray[i];
				lastInitials = _initialsArray[i];
				_scoresArray[i] = score;
				_initialsArray[i] = initials;
				score = lastScore;
				initials = lastInitials;
			}
		}
		SaveHighScores();
	}
}
}