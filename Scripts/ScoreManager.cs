using UnityEngine;
using System.Collections;

namespace Cadence {
public class ScoreManager : MonoBehaviour {

	public static ScoreManager instance;
	public string gameKey = "Game";

	public static string[] initialsArray = new string[] {"---", "---", "---", "---", "---", "---"};
	public static int[] scoresArray = new int[] { 100000, 8000, 6000, 4500, 3000, 1000 };

	void Awake () {
		if (instance == null) instance = this;
		LoadHighScores();
	}
	
	public static void LoadHighScores () {
		for (int i = 0; i < 6; i++) {
			initialsArray[i] = PlayerPrefs.GetString(instance.gameKey + ".HighScoreName" + i, "---");
			scoresArray[i] = PlayerPrefs.GetInt(instance.gameKey + ".HighScore" + i, scoresArray[i]);
		}
	}

	public static void SaveHighScores () {
		for (int i = 0; i < 6; i++) {
			PlayerPrefs.SetString(instance.gameKey + ".HighScoreName" + i, initialsArray[i]);
			PlayerPrefs.SetInt(instance.gameKey + ".HighScore" + i, scoresArray[i]);
		}
		PlayerPrefs.Save();
	}

	public static bool CheckHighScore (int score) {
		for (int i = 0; i < 6; i++) {
			if (score > scoresArray[i]) return true;
		}
		return false;
	}

	public static void AddHighScore (int score, string initials) {
		int lastScore = 0;
		string lastInitials = "";
		bool highScoreSet = false;
		for (int i = 0; i < 6; i++) {
			if (score > scoresArray[i] || highScoreSet) {
				highScoreSet = true;
				lastScore = scoresArray[i];
				lastInitials = initialsArray[i];
				scoresArray[i] = score;
				initialsArray[i] = initials;
				score = lastScore;
				initials = lastInitials;
			}
		}
		SaveHighScores();
	}
}
}