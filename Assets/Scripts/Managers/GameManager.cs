using System.Collections;
using System.Collections.Generic;
using Game.Controllers.Lobby;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public LobbyController LobbyController;
        public MatchController MatchController;

    }
}

