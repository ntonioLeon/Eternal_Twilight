using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

namespace Code
{
    public class PlayFabUpdatePlayerStatistics
    {
        public void UpdatePlayerStatistics(string leaderboardName, int score)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardName,
                    Value = score
                }
            }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request,
                                                    OnPlayerStatisticsSuccess,
                                                    OnPlayerStatisticsFailure);
        }
        private void OnPlayerStatisticsSuccess(UpdatePlayerStatisticsResult result)
        {
            Debug.Log("Score Updated");
        }

        private void OnPlayerStatisticsFailure(PlayFabError error)
        {
            Debug.LogError($"Debug info: {error.GenerateErrorReport()}");
        }
    }
}

