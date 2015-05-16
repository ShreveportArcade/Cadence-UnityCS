# Cadence
payment processing, leaderboards, save states, etc for putting your Unity3D game in an arcade machine

## TokenManager
A MonoBehaviour that handles payment processing for tokens.

### initialization
`TokenManager.instance`

### fields
`tokensPerCredit` - -n = 1/n tokens per credit = n credits per token, 0 = freeplay

### properties
`public int[] tokensInserted` - total number of coins inserted so far for player N

`public int[] credits` - number of credits available for play for player N

`public string freePlayString = "FREE PLAY"`

`public string onePlayPerTokenString = "1 CREDIT PER COIN"`

`public string coinsPerCreditFormatString = "{0} COINS PER CREDIT"`

`public string tokensSoFarFormatString = "{0}/{1} COINS"`

`public string creditsPerCoinFormatString = "{0} CREDITS PER COIN"`

`public string insertCoinsString = "INSERT COIN"`

`public string oneCreditString = "1 CREDIT"`

`public string nCreditsFormatString = "{0} CREDITS"`


### static events
`onTokenInserted (int acceptor, int tokensSoFar, int tokensNeeded)`

`onCreditAdded (int acceptor, int totalCredits)`

`onCreditUsed (int acceptor, int totalCredits)`

`onInsufficientCredit(int acceptor, int tokensNeeded)`


### static methods
`public static int JoystickCount ()`

`public static int CoinAcceptorCount ()`

`public static void LoadSession ()` - loads tokensPerCredit, tokensInserted, and credits from a previous session

`public static void SaveSession ()` - saves tokensPerCredit, tokensInserted, and credits for use in later 
sessions

`public static void AddCredit(int credits, int acceptor = 0)` - called when enough coins are inserted, call 
explicitly when awarding free plays

`public static bool UseCredit(int acceptor = 0)` - call when the player presses start, returns false if there 
aren't enough credits

`public static string TokensPerCreditText (bool checkTokensInserted, int acceptor = 0)` - text indicating how much the game costs to play, shows tokens so far when tokensPerCredit > 1

`public static string CreditsText (int acceptor = 0)` - text indicating how many credits a game has, shows insert coins text when credits == 0

## Leaderboards
coming soon

## Save States
coming soon