using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PlayGamesController : MonoBehaviour 
{
    /*void Start()
    {
        AuthenticateUser();
    }

    void AuthenticateUser()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Loged in Google Play Services");
            }
            else
            {
                Debug.LogError("Unable to sign in Google Play Services");
            }
        }
        );
    }

    public static void PostToLeaderboard()
    { 
        Social.ReportScore((long)(GameData.gameData.saveData.bestTime * 1000), GPGSIds.leaderboard_survivors, (bool success) =>
        {
            if (success)
            {
                Debug.Log("Posted timee to leaderboard");
            }
            else
            {
                Debug.LogError("Unable to post best score to leaderboard");
            }
        });
    }

    public void ShowLeaderboard()
    {
        PostToLeaderboard();
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_survivors);
    }*/
}
