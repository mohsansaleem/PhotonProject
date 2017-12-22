using Game.Managers;
using Game.UI.Models.Lobby;
using Game.UI.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.ViewModels.Lobby
{
    /// <summary>
    /// Rooms controller.
    /// </summary>
    public class RoomsController : ListViewGeneric<RoomRowModel, RoomRowViewModel>
    {
        [Header("References")]
        [SerializeField]
        Text roomNameText;

        #region ButtonEvents

        public void OnCreateRoomButtonClicked()
        {
            if (GameManager.Instance.LobbyController != null)
                GameManager.Instance.LobbyController.CreateRoomWithName(roomNameText.text);
        }

        public void OnJoinRandomButtonClicked()
        {
            if (GameManager.Instance.LobbyController != null)
                GameManager.Instance.LobbyController.JoinRandomRoom();
        }

        public void OnRefreshRoomsListButtonClicked()
        {
            if (GameManager.Instance.LobbyController != null)
                GameManager.Instance.LobbyController.RefreshRoomsList();
        }

        public void OnConnectButtonClicked()
        {
            if (GameManager.Instance.LobbyController != null)
                GameManager.Instance.LobbyController.ConnectToPhoton();
        }

        #endregion ButtonEvents
    }
}