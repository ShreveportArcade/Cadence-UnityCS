using UnityEngine;
using System.Collections;

namespace Cadence {
public class TokenUIControler : MonoBehaviour {

	

	void OnEnable() {
		TokenManager.onTokenInserted += OnTokenInserted;
		TokenManager.onCreditAdded += OnCreditAdded;
		TokenManager.onCreditUsed += OnCreditUsed;
	}

	void OnDisable() {
		TokenManager.onTokenInserted += OnTokenInserted;
		TokenManager.onCreditAdded += OnCreditAdded;
		TokenManager.onCreditUsed += OnCreditUsed;
	}

	void OnTokenInserted(int tokensSoFar, int tokensNeeded) {

	}

	void OnCreditAdded(int totalCredits) {

	}

	void OnCreditUsed(int totalCredits) {
		
	}
}
}