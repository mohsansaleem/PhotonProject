using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Controllers.Lobby
{
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