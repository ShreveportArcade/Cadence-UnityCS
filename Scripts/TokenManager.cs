using UnityEngine;
using System.Collections;

namespace Cadence {
public class TokenManager : MonoBehaviour {

	public delegate void OnTokenInserted(int acceptor, int tokensSoFar, int tokensNeeded);
	public static event OnTokenInserted onTokenInserted = delegate{};

	public delegate void OnCreditChanged(int acceptor, int totalCredits);
	public static event OnCreditChanged onCreditAdded = delegate{};
	public static event OnCreditChanged onCreditUsed = delegate{};

	public delegate void OnInsufficientCredit(int acceptor, int tokensNeeded);
	public static event OnInsufficientCredit onInsufficientCredit = delegate{};

	private static TokenManager _instance;
	public static TokenManager instance {
		get {
			if (_instance == null) {
				SetupAsInstance(FindObjectOfType(typeof(TokenManager)) as TokenManager);
			}
			if (_instance == null) {
				GameObject tokenMan = new GameObject("Cadence.TokenManager");
				SetupAsInstance(tokenMan.AddComponent<TokenManager>());
			}
			return _instance;
		}
	}

	[Tooltip("-n = 1/n tokens per credit = n credits per token, 0 = freeplay")]
	public int tokensPerCredit = 1;
	[Range(0,19)] public int tokenButton = 19;
	#if UNITY_EDITOR
	public KeyCode[] editorTokenKeyCodes = new KeyCode[] {
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4
	};	
	#endif
	
	private int[] _tokensInserted;
	public int[] tokensInserted {
		get { return _tokensInserted; }
	}

	private int[] _credits;
	public int[] credits {
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
			SetupAsInstance(this);
		}
		else if (_instance != this) {
			Debug.LogWarning("TokenManager already initialized, destroying duplicate");
			GameObject.Destroy(this);
		}
	}

	static void SetupAsInstance (TokenManager tokenMan) {
		if (tokenMan == null) return;
		_instance = tokenMan;
		_instance._tokensInserted = new int[CoinAcceptorCount()];
		_instance._credits = new int[CoinAcceptorCount()];
		DontDestroyOnLoad(_instance.gameObject);
		LoadSession();
	}

	public static int CoinAcceptorCount () {
		#if UNITY_EDITOR
		return instance.editorTokenKeyCodes.Length;
		#else
		return Input.GetJoystickNames().Length;
		#endif
	}

	public static void LoadSession () {
		instance.tokensPerCredit = PlayerPrefs.GetInt("Cadence.tokensPerCredit", 1);
		for (int i = 0; i < CoinAcceptorCount(); i++) {
			instance._tokensInserted[i] = PlayerPrefs.GetInt("Cadence.tokensInserted." + i, 0);
			instance._credits[i] = PlayerPrefs.GetInt("Cadence.credits." + i, 0);
		}
	}

	public static void SaveSession () {
		PlayerPrefs.SetInt("Cadence.tokensPerCredit", instance.tokensPerCredit);
		for (int i = 0; i < CoinAcceptorCount(); i++) {
			PlayerPrefs.SetInt("Cadence.tokensInserted." + i, instance.tokensInserted[i]);
			PlayerPrefs.SetInt("Cadence.credits." + i, instance.credits[i]);
		}
	}

	[ContextMenu("Clear Session")]
	public void ClearSession () {
		for (int i = 0; i < CoinAcceptorCount(); i++) {
			instance.credits[i] = 0;
			instance.tokensInserted[i] = 0;
			onTokenInserted(i, 0, 0);
			onCreditAdded(i, 0);
		}
		SaveSession();
	}

	void InsertToken(int acceptor = 0) {
		_tokensInserted[acceptor]++;
		if (tokensPerCredit > 0) {
			int tokensSoFar = tokensInserted[acceptor] % tokensPerCredit;
			int tokensNeeded = tokensPerCredit - tokensSoFar;
			onTokenInserted(acceptor, tokensSoFar, tokensNeeded);
			if (tokensSoFar == 0) AddCredit(1, acceptor);
		}
		else if (tokensPerCredit < 0) {
			AddCredit(-tokensPerCredit, acceptor);
		}
		else {
			Debug.LogError("Token inserted during freeplay.");
		}		
	}

	public static void AddCredit(int credits, int acceptor = 0) {
		instance._credits[acceptor] += credits;
		onCreditAdded(acceptor, instance.credits[acceptor]);
	}

	public static bool UseCredit(int acceptor = 0) {
		if (instance.credits[acceptor] > 0) {
			instance._credits[acceptor]--;
			onCreditUsed(acceptor, instance.credits[acceptor]);
			return true;
		}
		else {
			int tokensSoFar = instance.tokensInserted[acceptor] % instance.tokensPerCredit;
			int tokensNeeded = instance.tokensPerCredit - tokensSoFar;
			onInsufficientCredit(acceptor, tokensNeeded);
			return false;
		}
	}

	public static string TokensPerCreditText (bool checkTokensInserted, int acceptor = 0) {
		int tokensPerCredit = instance.tokensPerCredit;
		if (tokensPerCredit == 0) {
			return instance.freePlayString;
		}
		else if (Mathf.Abs(tokensPerCredit) == 1) {
			return instance.onePlayPerTokenString;
		}	
		else if (tokensPerCredit > 1) {
			int tokensSoFar = instance.tokensInserted[acceptor] % instance.tokensPerCredit;
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

	public static string CreditsText (int acceptor = 0) {
		if (instance.tokensPerCredit == 0) {
			return "";
		}

		if (instance.credits[acceptor] == 0) {
			return instance.insertCoinsString;
		}
		else if (instance.credits[acceptor] == 1) {
			return instance.oneCreditString;
		}
		return string.Format(instance.nCreditsFormatString, instance.credits[acceptor]);
	}

	void Update () {
		for (int i = 0; i < CoinAcceptorCount(); i++) {
			#if UNITY_EDITOR
			if (Input.GetKeyUp(editorTokenKeyCodes[i])) InsertToken(i);
			#else
			string key = string.Format("joystick {0} button {1}", i+1, tokenButton);
			if (Input.GetKeyUp(key)) InsertToken(i);
			#endif
		}
	}

	void OnApplicationQuit() {
        SaveSession();
    }
}
}
