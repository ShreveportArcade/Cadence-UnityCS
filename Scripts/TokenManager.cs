using UnityEngine;
using System.Collections;

namespace Cadence {
public class TokenManager : MonoBehaviour {

	public delegate void OnTokenInserted(int tokensSoFar, int tokensNeeded);
	public static event OnTokenInserted onTokenInserted = delegate{};

	public delegate void OnCreditChanged(int totalCredits);
	public static event OnCreditChanged onCreditAdded = delegate{};
	public static event OnCreditChanged onCreditUsed = delegate{};

	private static TokenManager _instance;
	public static TokenManager instance {
		get {
			if (_instance == null) {
				GameObject tokenMan = new GameObject("Cadence.TokenManager");
				_instance = tokenMan.AddComponent<TokenManager>();
				LoadSession();
			}
			return _instance;
		}
	}

	[Tooltip("-n = 1/n tokens per credit = n credits per token, 0 = freeplay")]
	public int tokensPerCredit = 1;
	public KeyCode tokenKeyCode = KeyCode.Joystick1Button19;
	#if UNITY_EDITOR
	public KeyCode altTokenKeyCode = KeyCode.T;
	#endif
	
	private int _tokensInserted = 0;
	public int tokensInserted {
		get { return _tokensInserted; }
	}

	private int _credits = 0;
	public int credits {
		get { return _credits; }
	}

	public string freePlayString = "FREE PLAY";
	public string onePlayPerTokenString = "1 CREDIT PER COIN";
	public string coinsPerCreditFormatString = "{0} COINS PER CREDIT";
	public string tokensSoFarFormatString = "{0}/{1} COINS";
	public string creditsPerCoinFormatString = "{0} CREDITS PER COIN";
	public string insertCoinsString = "INSERT COIN";
	public string oneCreditString = "1 CREDIT";
	public string nCreditsFormatString = "{0} CREDITS";
	
	void Awake () {
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad(_instance.gameObject);
			LoadSession();
		}
		else if (_instance != this) {
			Debug.LogWarning("TokenManager already initialized, destroying duplicate");
			GameObject.Destroy(this);
		}
	}

	public static void LoadSession () {
		instance.tokensPerCredit = PlayerPrefs.GetInt("Cadence.tokensPerCredit", 1);
		instance._tokensInserted = PlayerPrefs.GetInt("Cadence.tokensInserted", 0);
		instance._credits = PlayerPrefs.GetInt("Cadence.credits", 0);
	}

	public static void SaveSession () {
		PlayerPrefs.SetInt("Cadence.tokensPerCredit", instance.tokensPerCredit);
		PlayerPrefs.SetInt("Cadence.tokensInserted", instance.tokensInserted);
		PlayerPrefs.SetInt("Cadence.credits", instance.credits);
	}

	public void InsertToken() {
		_tokensInserted++;
		if (tokensPerCredit > 0) {
			int tokensSoFar = tokensInserted % tokensPerCredit;
			int tokensNeeded = tokensPerCredit - tokensSoFar;
			onTokenInserted(tokensSoFar, tokensNeeded);
			if (tokensSoFar == 0) AddCredit(1);
		}
		else if (tokensPerCredit < 0) {
			AddCredit(-tokensPerCredit);
		}
		else {
			Debug.LogError("Token inserted during freeplay.");
		}		
	}

	public static void AddCredit(int credits) {
		instance._credits += credits;
		onCreditAdded(instance.credits);
	}

	public static bool UseCredit() {
		if (instance.credits > 0) {
			instance._credits--;
			onCreditUsed(instance.credits);
			return true;
		}
		else {
			return false;
		}
	}

	public static string TokensPerCreditText (bool checkTokensInserted) {
		var tokensPerCredit = instance.tokensPerCredit;
		if (tokensPerCredit == 0) {
			return instance.freePlayString;
		}
		else if (Mathf.Abs(tokensPerCredit) == 1) {
			return instance.onePlayPerTokenString;
		}	
		else if (tokensPerCredit > 1) {
			var tokensSoFar = instance.tokensInserted % instance.tokensPerCredit;
			if (tokensSoFar == 0 || !checkTokensInserted) {
				return string.Format(instance.coinsPerCreditFormatString, tokensPerCredit);
			}
			else {
				return string.Format(instance.tokensSoFarFormatString, tokensSoFar, tokensPerCredit);
			}
		}
		else {
			return string.Format(instance.creditsPerCoinFormatString, Mathf.Abs(tokensPerCredit));
		}	
	}

	public static string CreditsText () {
		if (instance.tokensPerCredit == 0) {
			return "";
		}

		if (instance.credits == 0) {
			return instance.insertCoinsString;
		}
		else if (instance.credits == 1) {
			return instance.oneCreditString;
		}
		return string.Format(instance.nCreditsFormatString, instance.credits);
	}

	void Update () {
		if (Input.GetKeyUp(tokenKeyCode)) InsertToken();

		#if UNITY_EDITOR
		if (Input.GetKeyUp(altTokenKeyCode)) InsertToken();
		#endif
	}

	void OnApplicationQuit() {
        SaveSession();
    }
}
}
