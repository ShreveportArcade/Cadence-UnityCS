using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Cadence {
public class TokensPerCreditSlider : MonoBehaviour {
	
	public Slider slider;
	public Text text;

	void Start () {
		slider.value = TokenManager.instance.tokensPerCredit;
		text.text = TokenManager.TokensPerCreditText(false);
	}

	public void OnSliderMoved (float value) {
		TokenManager.instance.tokensPerCredit = (int)value;
		text.text = TokenManager.TokensPerCreditText(false);
	}
}
}