using Game.UI.ViewModels.Lobby;
using Game.UI.ViewModels.Match;

namespace Game.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public LobbyController LobbyController;
        public MatchController MatchController;

    }
}

