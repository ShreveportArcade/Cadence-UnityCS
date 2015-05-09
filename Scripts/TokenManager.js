#pragma strict

static var instance : TokenManager;
var tokensPerCredit : int = 1;
var tokensInserted : int = 0;
var credits : int = 0;

function Awake () {
	if (instance == null) {
		instance = this;
		LoadSession();
	}
	else if (instance != this) {
		Debug.LogWarning("TokenManager already initialized, destroying duplicate");
		GameObject.Destroy(this);
	}
}

static function LoadSession () {
	instance.tokensPerCredit = PlayerPrefs.GetInt("Cadence.tokensPerCredit", 1);
	instance.tokensInserted = PlayerPrefs.GetInt("Cadence.tokensInserted", 0);
	instance.credits = PlayerPrefs.GetInt("Cadence.credits", 0);
}

static function SaveSession () {
	PlayerPrefs.SetInt("Cadence.tokensPerCredit", instance.tokensPerCredit);
	PlayerPrefs.SetInt("Cadence.tokensInserted", instance.tokensInserted);
	PlayerPrefs.SetInt("Cadence.credits", instance.credits);
}

function InsertToken() {
	tokensInserted++;
	if (tokensPerCredit > 0) {
		var tokensSoFar = tokensInserted % tokensPerCredit;
		var tokensNeeded = tokensPerCredit - tokensSoFar;
		if (tokensSoFar == 0) AddCredit(1);
	}
	else if (tokensPerCredit < 0) {
		AddCredit(-tokensPerCredit);
	}
	else {
		Debug.LogError("Token inserted during freeplay.");
	}		
}

static function AddCredit(credits : int) {
	instance.credits += credits;
}

static function UseCredit() : boolean {
	if (instance.credits > 0) {
		instance.credits--;
		return true;
	}
	else {
		return false;
	}
}

function Update () {
	if (Input.GetButtonUp("Token")) InsertToken();
}

function OnApplicationQuit() {
    SaveSession();
}