using System;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Code
{
    public class PlayFabGetLeaderboard
    {
        public event Action<string> OnSuccess;

        public void GetLeaderboardEntries(int startPosition, int maxResultsCount, string leaderboardName)
        {
            // para obtener los 100 primeros (0, 100, LeaderboardName)
            var request = new GetLeaderboardRequest
            {
                StartPosition = startPosition, // posicion inicial desde la que contar
                MaxResultsCount = maxResultsCount, // entre 10 y 100. 100 MAX para sacar 101 son 2 peticiones (100, 100, LeaderboardName)
                StatisticName = leaderboardName // nombre de la tabla

            };
            PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailure);
        }

        private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
        {
            var leaderboard = new StringBuilder();
            foreach (var playerLeaderboardEntry in result.Leaderboard)
            {
                leaderboard.AppendLine($"{playerLeaderboardEntry.Position}.- {playerLeaderboardEntry.PlayFabId} {playerLeaderboardEntry.StatValue}");
            }

            OnSuccess?.Invoke(leaderboard.ToString());
        }

        private void OnGetLeaderboardFailure(PlayFabError error)
        {
            Debug.LogError($"Debug info: {error.GenerateErrorReport()}");
        }
    }
}

