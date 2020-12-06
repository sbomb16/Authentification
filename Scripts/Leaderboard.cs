﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class Leaderboard : MonoBehaviour
{

    public GameObject leaderboardCanvas;
    public GameObject[] leaderboardEntries;

    public static Leaderboard instance;

    void Awake()
    {
        instance = this;
    }

    public void OnLoggedIn()
    {
        leaderboardCanvas.SetActive(false);
        DisplayLeaderboard();
    }

    public void DisplayLeaderboard()
    {
        GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest
        {
            StatisticName = "FastestTime",
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(getLeaderboardRequest,
            result => UpdateLeaderboardUI(result.Leaderboard),
            error => Debug.Log(error.ErrorMessage)
        );

        leaderboardCanvas.SetActive(true);

    }

    void UpdateLeaderboardUI(List<PlayerLeaderboardEntry> leaderboard)
    {
        
        for (int x = 0; x < leaderboardEntries.Length; x++)
        {
            leaderboardEntries[x].SetActive(x < leaderboard.Count);

            if (x >= leaderboard.Count)
            {
                continue;
            }

            leaderboardEntries[x].transform.Find("Player_Name").GetComponent<TextMeshProUGUI>(
            ).text = (leaderboard[x].Position + 1) + ". " + leaderboard[x].DisplayName;

            leaderboardEntries[x].transform.Find("Score_Text").GetComponent<TextMeshProUGUI>()
            .text = (-(float)leaderboard[x].StatValue * 0.001f).ToString("F2");
        }
    }

    public void SetLeaderboardEntry(int newScore)
    {
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "UpdateHighscore",
            FunctionParameter = new { score = newScore }
        };

        PlayFabClientAPI.ExecuteCloudScript(request,
        result => DisplayLeaderboard(),
        error => Debug.Log(error.ErrorMessage)
        );
    }
}
