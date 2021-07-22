using System.Collections.Generic;
using UnityEngine;

namespace BossSlothsCards.Extensions
{
    public static class PlayerManagerExtension
    {
        public static Player GetRandomEnemy(this PlayerManager playerManager,Player player)
        {
            if (playerManager.players.Count == 1) return null;
            var players = new List<Player>(playerManager.players);
            foreach (var _player in playerManager.GetPlayersInTeam(player.teamID))
            {
                players.Remove(_player);
            }

            return players[Random.Range(0, players.Count)];
        }
    }
}