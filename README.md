# Cadence
payment processing, leaderboards, save states, etc for putting your Unity3D game in an arcade machine

## TokenManager
A MonoBehaviour that handles payment processing for tokens.

### initialization
`TokenManager.instance`

### static fields
`tokensPerCredit` - -n = 1/n tokens per credit = n credits per token, 0 = freeplay

### static properties
`tokensInserted` - total number of coins inserted so far

`credits` - number of credits available for play

### static events
`onTokenInserted (int tokensSoFar, int tokensNeeded)`

`onCreditAdded (int totalCredits)`

`onCreditUsed (int totalCredits)`

### static methods
`void LoadSession ()` - loads tokensPerCredit, tokensInserted, and credits from a previous session

`void SaveSession ()` - saves tokensPerCredit, tokensInserted, and credits for use in later sessions

`void AddCredit (int credits)` - called when enough coins are inserted, call explicitly when awarding free plays

`bool UseCredit ()` - call when the player presses start, returns false if there aren't enough credits

## Leaderboards
coming soon

## Save States
coming soon