using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Controllers.Lobby
{
    public class LobbyController : MonoBehaviour
    {
        #region Private Variables

        [Header("References")]
        [SerializeField]
        RoomsController roomsController;
        [SerializeField]
        CanvasGroup leaveButtonCanvas;
        [SerializeField]
        CanvasGroup startMatchButtonCanvas;
        [SerializeField]
        CanvasGroup reconnectButtonCanvas;
        [SerializeField]
        Text statusText;
        #endregion


        #region MonoBehaviour CallBacks


        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            GameManager.Instance.LobbyController = this;

            DisableEverything();

            // Photon Connection Events
            PhotonManager.Instance.OnConnectionToPhoton += OnConnectedToPhoton;
            PhotonManager.Instance.OnConnectionToMaster += OnConnectionToMaster;
            PhotonManager.Instance.OnDisconnectFromPhoton += OnDisconnectFromPhoton;

            // Lobby Events
            PhotonManager.Instance.OnLobbyJoined += OnLobbyJoined;
            PhotonManager.Instance.OnLobbyLeft += OnLobbyLeft;
            PhotonManager.Instance.OnReceivedRoomListUpdated += OnReceivedRoomListUpdated;

            // Room Events
            PhotonManager.Instance.OnRoomJoined += OnRoomJoined;
            PhotonManager.Instance.OnRoomLeft += OnRoomLeft;
            PhotonManager.Instance.OnRoomCreated += OnRoomCreated;
            PhotonManager.Instance.OnMasterClientSwitch += OnMasterClientSwitch;

            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
            PhotonManager.Instance.Connect();
            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
        }

        void OnDestroy()
        {
            // Photon Connection Events
            PhotonManager.Instance.OnConnectionToPhoton -= OnConnectedToPhoton;
            PhotonManager.Instance.OnConnectionToMaster -= OnConnectionToMaster;
            PhotonManager.Instance.OnDisconnectFromPhoton -= OnDisconnectFromPhoton;

            // Lobby Events
            PhotonManager.Instance.OnLobbyJoined -= OnLobbyJoined;
            PhotonManager.Instance.OnLobbyLeft -= OnLobbyLeft;
            PhotonManager.Instance.OnReceivedRoomListUpdated -= OnReceivedRoomListUpdated;

            // Room Events
            PhotonManager.Instance.OnRoomJoined -= OnRoomJoined;
            PhotonManager.Instance.OnRoomLeft -= OnRoomLeft;
            PhotonManager.Instance.OnRoomCreated -= OnRoomCreated;
            PhotonManager.Instance.OnMasterClientSwitch -= OnMasterClientSwitch;
        }

        void OnApplicationPause(bool pauseStatus)
        {
            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
            if (pauseStatus)
            {
                Debug.LogError("OnApplicationPause: Paused!");
                Debug.LogError("Status: " + PhotonNetwork.connectionState.ToString());
                Debug.LogError("Status: " + PhotonNetwork.connectionStateDetailed.ToString());
                Debug.LogError("NetworkingPeer Status: " + PhotonNetwork.networkingPeer.State.ToString());

                // app moved to background
                //PhotonNetwork.Disconnect();
            }
            else
            {
                Debug.LogError("OnApplicationPause: UnPaused!");
                Debug.LogError("Status: " + PhotonNetwork.connectionState.ToString());
                Debug.LogError("NetworkingPeer Status: " + PhotonNetwork.networkingPeer.State.ToString());

                // app is foreground again
                //if (PhotonNetwork.connectionState == ConnectionState.Disconnected)
                //    SceneManager.LoadScene("StartScene");
            }
        }
        #endregion

        #region PhotonEvents

        private void OnConnectedToPhoton()
        {
            reconnectButtonCanvas.interactable = false;
            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
        }

        private void OnConnectionToMaster()
        {
            Debug.LogError("LobbyManager.OnConnectedToMaster Called.");
            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
            PhotonManager.Instance.JoinLobby();
            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
            roomsController.Visible = true;
        }

        private void OnDisconnectFromPhoton()
        {
            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
            Debug.LogError("LobbyManager.OnDisconnectFromPhoton Called.");
            roomsController.Enabled = false;
            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
            reconnectButtonCanvas.interactable = true;
            //SceneManager.LoadScene("StartScene");
        }

        private void OnLobbyJoined()
        {
            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
            Debug.LogError("LobbyManager.OnLobbyJoined Called.");
            roomsController.Enabled = true;
            leaveButtonCanvas.alpha = 1f;
            startMatchButtonCanvas.alpha = 1f;

            leaveButtonCanvas.interactable = false;
            startMatchButtonCanvas.interactable = false;

            PopulateRoomsList();
        }

        private void OnLobbyLeft()
        {
            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
            Debug.LogError("LobbyManager.OnLobbyLeft Called.");
            roomsController.Enabled = false;
        }

        void OnReceivedRoomListUpdated()
        {
            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
            Debug.LogError("LobbyManager.OnReceivedRoomListUpdated Called.");
            PopulateRoomsList();
        }

        private void OnRoomCreated()
        {
            Debug.LogError("LobbyManager.OnRoomCreated Called.");
        }

        private void OnRoomJoined()
        {
            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
            Debug.LogError("LobbyManager.OnRoomJoined Called.");
            leaveButtonCanvas.interactable = true;

            startMatchButtonCanvas.interactable = PhotonManager.Instance.IsMasterClient;
        }

        private void OnRoomLeft()
        {
            statusText.text = "Status: " + PhotonNetwork.connectionStateDetailed.ToString();
            Debug.LogError("LobbyManager.OnRoomLeft Called.");
            leaveButtonCanvas.interactable = false;
            startMatchButtonCanvas.interactable = false;
        }

        private void OnMasterClientSwitch(PhotonPlayer newMasterClient)
        {
            if (PhotonManager.Instance.LocalPlayer.Equals(newMasterClient))
                startMatchButtonCanvas.interactable = true;
        }

        #endregion PhotonEvents


        #region ViewEvents

        public void OnLeaveRoomButtonClicked()
        {
            PhotonManager.Instance.LeaveRoom();
        }

        public void OnStartMatchButtonClicked()
        {
            if (PhotonManager.Instance.IsMasterClient)
            {
                Debug.LogError("Start the match.");
                // TODO: Start the Match
                SceneManager.LoadScene("RoomScene");
            }
        }

        #endregion ViewEvents


        #region PrivateMethods

        private void DisableEverything()
        {
            roomsController.Visible = false;
            roomsController.Enabled = false;

            leaveButtonCanvas.alpha = 0;
            leaveButtonCanvas.interactable = false;

            startMatchButtonCanvas.alpha = 0;
            startMatchButtonCanvas.interactable = false;
        }

        void PopulateRoomsList()
        {
            RoomsController.DataList = PhotonNetwork.GetRoomList().Select(room => new RoomRowModel(room.Name, room.PlayerCount)).ToList();
            Debug.LogError("Count: " + RoomsController.DataList.Count);
        }

        #endregion PrivateMethods


        #region Public Methods

        public void ConnectToPhoton()
        {
            PhotonManager.Instance.Connect();
        }

        public void RefreshRoomsList()
        {
            PopulateRoomsList();
        }

        public void CreateRoomWithName(string roomName)
        {
            PhotonManager.Instance.CreateRoomWithName(roomName);
        }

        public void JoinRoomWithName(string roomName)
        {
            PhotonManager.Instance.JoinRoomWithName(roomName);
        }

        public void JoinRandomRoom()
        {
            PhotonManager.Instance.JoinRandom();
        }

        public RoomsController RoomsController
        {
            get
            {
                return roomsController;
            }

            set
            {
                roomsController = value;
            }
        }

        #endregion
    }

}
