using UnityEngine;
using System.Collections;

namespace Cadence {
public class ResetScoresButton : MonoBehaviour {

	public void ResetScores () {
		ScoreManager.instance.ResetHighScores();
	}
}
}