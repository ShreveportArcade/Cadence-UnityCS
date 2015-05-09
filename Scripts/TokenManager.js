#pragma strict

static var _instance : TokenManager;
public var messageListeners : GameObject[];
public var tokensPerCredit : int = 1;
private var tokensInserted : int = 0;
private var credits : int = 0;

public static function instance() : TokenManager {
    if (_instance == null) {
		var tokenMan = new GameObject("Cadence.TokenManager");
		_instance = tokenMan.AddComponent(TokenManager);
		LoadSession();
	}
	return _instance;
}

function Awake () {
	if (_instance == null) {
		_instance = this;
		LoadSession();
	}
	else if (_instance != this) {
		Debug.LogWarning("TokenManager already initialized, destroying duplicate");
		GameObject.Destroy(this);
	}
}

static function LoadSession () {
	instance().tokensPerCredit = PlayerPrefs.GetInt("Cadence.tokensPerCredit", 1);
	instance().tokensInserted = PlayerPrefs.GetInt("Cadence.tokensInserted", 0);
	instance().credits = PlayerPrefs.GetInt("Cadence.credits", 0);
}

static function SaveSession () {
	PlayerPrefs.SetInt("Cadence.tokensPerCredit", instance().tokensPerCredit);
	PlayerPrefs.SetInt("Cadence.tokensInserted", instance().tokensInserted);
	PlayerPrefs.SetInt("Cadence.credits", instance().credits);
}

function InsertToken() {
	tokensInserted++;
	if (tokensPerCredit > 0) {
		var tokensSoFar = tokensInserted % tokensPerCredit;
		var tokensNeeded = tokensPerCredit - tokensSoFar;
		instance().SendMessageToListeners("OnTokensChanged");
		if (tokensSoFar == 0) AddCredit(1);
	}
	else if (tokensPerCredit < 0) {
		AddCredit(-tokensPerCredit);
		instance().SendMessageToListeners("OnTokensChanged");
	}
	else {
		Debug.LogError("Token inserted during freeplay.");
	}		
}

static function AddCredit(credits : int) {
	instance().credits += credits;
	instance().SendMessageToListeners("OnCreditChanged");
}

static function UseCredit() : boolean {
	if (instance().credits > 0) {
		instance().credits--;
		instance().SendMessageToListeners("OnCreditChanged");
		return true;
	}
	else {
		return false;
	}
}

static function TokensPerCreditText () : String {
	var tokensPerCredit = instance().tokensPerCredit;
	if (tokensPerCredit == 0) {
		return "FREE PLAY";
	}
	else if (Mathf.Abs(tokensPerCredit) == 1) {
		return "1 CREDIT PER COIN";
	}	
	else if (tokensPerCredit > 1) {
		var tokensSoFar = instance().tokensInserted % instance().tokensPerCredit;
		if (tokensSoFar == 0) {
			return String.Format("{1} COINS PER CREDIT", tokensPerCredit);
		}
		else {
			return String.Format("{0}/{1} COINS", tokensSoFar, tokensPerCredit);
		}
	}
	else if (tokensPerCredit < -1) {
		return String.Format("{1} CREDITS PER COIN", Mathf.Abs(tokensPerCredit));
	}	
}

static function CreditsText () : String {
	if (instance().tokensPerCredit == 0) {
		return "";
	}

	if (instance().credits == 0) {
		return "INSERT COIN";
	}
	else if (instance().credits == 1) {
		return "1 CREDIT";
	}
	return String.Format("{0} CREDITS", instance().credits);
}

function Update () {
	Debug.Log(Input.GetButtonUp("Token"));
	if (Input.GetButtonUp("Token")) InsertToken();
}

function OnApplicationQuit() {
    SaveSession();
}

function SendMessageToListeners (message : String) {
	for (messageListener in messageListeners) {
		messageListener.SendMessage(message);
	}
}