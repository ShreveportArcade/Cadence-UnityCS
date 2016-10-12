using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public static string[] initialsArray = new string[] {"---", "---", "---", "---", "---", "---"};
	public static int[] scoresArray = new int[] { 100000, 8000, 6000, 4500, 3000, 1000 };

	void Awake () {
		LoadHighScores();
	}
	
	public void LoadHighScores () {
		for (int i = 0; i < 6; i++) {
			initialsArray[i] = PlayerPrefs.GetString("VectorZ.HighScoreName" + i, "---");
			scoresArray[i] = PlayerPrefs.GetInt("VectorZ.HighScore" + i, scoresArray[i]);
		}
	}

	public void SaveHighScores () {
		for (int i = 0; i < 6; i++) {
			PlayerPrefs.SetString("VectorZ.HighScoreName" + i, initialsArray[i]);
			PlayerPrefs.SetInt("VectorZ.HighScore" + i, scoresArray[i]);
		}
	}

	public bool CheckHighScore (int score) {
		for (int i = 0; i < 6; i++) {
			if (score > scoresArray[i]) return true;
		}
		return false;
	}

	public void AddHighScore (int score, string initials) {
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
