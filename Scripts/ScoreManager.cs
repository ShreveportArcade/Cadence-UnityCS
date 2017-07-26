using UnityEngine;
using System.Collections;

namespace Cadence {
public class ScoreManager : MonoBehaviour {

	public static ScoreManager instance; // TODO: should be a dictionary of gameKey to instance
	public string gameKey = "Game";

	public string defaultInitials = "---";
	public int[] defaultScores = new int[] { 100000, 8000, 6000, 4500, 3000, 1000 };

	private string[] _initials;
	public string[] initials {
		get { return _initials; }
	}
	
	private int[] _scores;
	public int[] scores {
		get { return _scores; }
	}
	public static int highScore {
		get { return instance._scores[0]; }
	}
	
	void Awake () {
		if (instance == null) instance = this;
		InitializeScores();
		LoadHighScores();
	}

	void InitializeScores () {
		_initials = new string[defaultScores.Length];
		for (int i = 0; i < defaultScores.Length; i++) {
			_initials[i] = defaultInitials;
		}
		_scores = defaultScores.Clone() as int[];
	}
	
	public void LoadHighScores () {
		for (int i = 0; i < defaultScores.Length; i++) {
			_initials[i] = PlayerPrefs.GetString(gameKey + ".HighScoreName" + i, defaultInitials);
			_scores[i] = PlayerPrefs.GetInt(gameKey + ".HighScore" + i, defaultScores[i]);
		}
	}

	public void SaveHighScores () {
		for (int i = 0; i < defaultScores.Length; i++) {
			PlayerPrefs.SetString(gameKey + ".HighScoreName" + i, _initials[i]);
			PlayerPrefs.SetInt(gameKey + ".HighScore" + i, _scores[i]);
		}
		PlayerPrefs.Save();
	}

	public void ResetHighScores () {
		for (int i = 0; i < defaultScores.Length; i++) {
			PlayerPrefs.DeleteKey(gameKey + ".HighScoreName" + i);
			PlayerPrefs.DeleteKey(gameKey + ".HighScore" + i);
		}
		PlayerPrefs.Save();
		InitializeScores();
	}

	public bool CheckHighScore (int score) {
		for (int i = 0; i < defaultScores.Length; i++) {
			if (score > _scores[i]) return true;
		}
		return false;
	}

	public void AddHighScore (int score, string initials) {
		int lastScore = 0;
		string lastInitials = "";
		bool highScoreSet = false;
		for (int i = 0; i < defaultScores.Length; i++) {
			if (score > _scores[i] || highScoreSet) {
				highScoreSet = true;
				lastScore = _scores[i];
				lastInitials = _initials[i];
				_scores[i] = score;
				_initials[i] = initials;
				score = lastScore;
				initials = lastInitials;
			}
		}
		SaveHighScores();
	}
}
}