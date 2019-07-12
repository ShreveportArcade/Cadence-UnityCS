# Cadence
An arcade library. Enter high scores, accept tokens.

```C#
// Add TokenManager.prefab and (optionally) SettingsManager.prefab to your scene.
// Put stuff like this in your main menu

using UnityEngine;
using Cadence;

public class AcceptTokens : MonoBehaviour {

    // listen for TokenManager events
    void OnEnable () {
        TokenManager.onTokenInserted += OnTokenInserted;
        TokenManager.onInsufficientCredit += OnInsufficientCredit;
        TokenManager.onCreditAdded += OnCreditChanged;
    }

    // stop listening when disabled
    void OnDisable () {
        TokenManager.onTokenInserted -= OnTokenInserted;
        TokenManager.onInsufficientCredit -= OnInsufficientCredit;
        TokenManager.onCreditAdded -= OnCreditChanged;
    }

    void OnTokenInserted(int acceptor, int tokensSoFar, int tokensNeeded) {
        string tokensPerCredit = TokenManager.TokensPerCreditText(true);
    }

    void OnInsufficientCredit(int acceptor, int tokensNeeded) {
        // play error sound, shake the credits text, etc.
    }

    void OnCreditChanged(int acceptor, int totalCredits) {
        string creditsText = TokenManager.CreditsText();
    }

    void Update () {
        if (TokenManager.CanPlay()) {
            // prompt player to start game
        }
        else {
            // prompt player to insert coins
        }

        // try to use a credit
        if (Input.GetKeyDown(KeyCode.Enter) && TokenManager.UseCredit()) {
            // start the game
        }
    }
}
```
