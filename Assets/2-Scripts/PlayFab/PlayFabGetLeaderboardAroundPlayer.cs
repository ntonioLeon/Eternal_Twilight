using System;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code
{
    public class PlayFabGetLeaderboardAroundPlayer
    {
        public event Action<string> OnSuccess;

        public void GetLeaderboardAroundPlayer(string playerId, int maxResoultsCount, string leaderboardName)
        {
            var request = new GetLeaderboardAroundPlayerRequest
            {
                PlayFabId = playerId,
                MaxResultsCount = maxResoultsCount, //11
                StatisticName = leaderboardName
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetLeaderboardArroundPlayerSuccess, OnGetLeaderboardArroundPlayerFailure);
        }

        private void OnGetLeaderboardArroundPlayerSuccess(GetLeaderboardAroundPlayerResult result)
        {
            var leaderboard = new StringBuilder();
            foreach (var playerLeaderboardEntry in result.Leaderboard)
            {
                leaderboard.AppendLine($"{playerLeaderboardEntry.Position}.- {playerLeaderboardEntry.DisplayName} {playerLeaderboardEntry.StatValue}");
            }
            OnSuccess?.Invoke(leaderboard.ToString());
        }

        private void OnGetLeaderboardArroundPlayerFailure(PlayFabError error)
        {
            Debug.LogError($"Debug informa: {error.GenerateErrorReport()}");
        }
    }
}

