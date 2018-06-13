using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Paraphernalia.Components;

namespace Cadence {
public class TokenTextController : MonoBehaviour {

    public string tokenInsertedSound = "coin";
    public bool checkTokensInserted = true;
    public int acceptor = 0;

    public Text creditsText;
    public Text tokensPerCreditText;

    void OnEnable () {
        TokenManager.onTokenInserted += OnTokenInserted;
        TokenManager.onCreditAdded += OnCreditsChanged;
        TokenManager.onCreditUsed += OnCreditsChanged;
        SettingsManager.onSettingsChanged += UpdateText;
    }

    void OnDisable () {
        TokenManager.onTokenInserted -= OnTokenInserted;
        TokenManager.onCreditAdded -= OnCreditsChanged;
        TokenManager.onCreditUsed -= OnCreditsChanged;
        SettingsManager.onSettingsChanged -= UpdateText;
    }

    void Start () {
        UpdateText();
    }

    void OnTokenInserted(int acceptor, int tokensSoFar, int tokensNeeded) {
        if (acceptor != this.acceptor) return;
        AudioManager.PlayEffect(tokenInsertedSound);
        tokensPerCreditText.text = TokenManager.TokensPerCreditText(checkTokensInserted, acceptor);
    }

    void OnCreditsChanged(int acceptor, int totalCredits) {
        if (acceptor != this.acceptor) return;
        creditsText.text = TokenManager.CreditsText(acceptor);
    }

    void UpdateText() {
        creditsText.text = TokenManager.CreditsText(acceptor);
        tokensPerCreditText.text = TokenManager.TokensPerCreditText(checkTokensInserted, acceptor);
    }
}
}