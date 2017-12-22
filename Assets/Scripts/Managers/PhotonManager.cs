using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Game.Managers
{
    public class PhotonManager : Singleton<PhotonManager>, IPunCallbacks
    {
        #region PrivateVariables
        /// <summary>
        /// This client's version number. Users are separated from each other by gameversion (which allows you to make breaking changes).
        /// </summary>
        string _gameVersion = "1";

        public PhotonRaisedEventsHandler raisedEventsHandler = new PhotonRaisedEventsHandler();
        #endregion PrivateVariables

        #region PhotonEvents
        public event Action OnConnectionToPhoton;
        public event Action OnConnectionToMaster;
        public event Action<DisconnectCause> OnConnectingFail;
        public event Action OnRoomCreated;
        public event Action<string> OnCustomAuthenticationFail;
        public event Action<Dictionary<string, object>> OnCustomAuthenticateResponse;
        public event Action OnDisconnectFromPhoton;
        public event Action<DisconnectCause> OnFailedToConnectToThePhoton;
        public event Action OnLobbyJoined;
        public event Action OnRoomJoined;
        public event Action OnLobbyLeft;
        public event Action OnRoomLeft;
        public event Action OnLobbyStatisticUpdate;
        public event Action<PhotonPlayer> OnMasterClientSwitch;
        public event Action<object[]> OnOwnershipRequested;
        public event Action<object[]> OnOwnershipTransfer;
        public event Action<object[]> OnCreateRoomFailed;
        public event Action<ExitGames.Client.Photon.Hashtable> OnCustomRoomPropertiesChanged;
        public event Action<PhotonMessageInfo> OnPhotonInstantiated;
        public event Action<object[]> OnJoinRoomFailed;
        public event Action OnPhotonMaxCccuReach;
        public event Action<PhotonPlayer> OnPlayerActivityChanged;
        public event Action<PhotonPlayer> OnPlayerConnected;
        public event Action<PhotonPlayer> OnPlayerDisconnected;
        public event Action<object[]> OnPlayerPropertiesChanged;
        public event Action<object[]> OnRandomJoinFailed;
        public event Action OnReceivedRoomListUpdated;
        public event Action OnUpdateFriendList;
        public event Action<OperationResponse> OnWebRpcResponsed;
        #endregion PhotonEvents

        #region MonoBehaviorMagicFunctions
        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // we don't join the lobby. There is no need to join a lobby to get the list of rooms.
            PhotonNetwork.autoJoinLobby = false;

            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.automaticallySyncScene = true;

            Debug.LogError("PhotonManager Awake.");
            PhotonNetwork.OnEventCall += raisedEventsHandler.OnEventRaised;

        }

        void OnDestroy()
        {
            base.OnDestroy();
            PhotonNetwork.OnEventCall -= raisedEventsHandler.OnEventRaised;
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        //void Start()
        //{
        //    Connect();
        //}

        #endregion MonoBehaviorMagicFunctions

        #region PublicFunctions

        /// <summary>
        /// Start the connection process. 
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.connected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed() and we'll create one.
                // PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings(_gameVersion);
            }
        }

        /// <summary>
        /// Raises the event. Call this function to do the thing.
        /// </summary>
        /// <param name="eventId">Event identifier.</param>
        /// <param name="content">Content.</param>
        /// <param name="sendReliable">If set to <c>true</c> send reliable.</param>
        /// <param name="options">Options.</param>
        public void RaiseEvent(EventsIDs eventId, object content, bool sendReliable, RaiseEventOptions options)
        {
            //byte evCode = 0;    // my event 0. could be used as "group units"
            //byte[] content = new byte[] { 1, 2, 5, 10 };    // e.g. selected unity 1,2,5 and 10
            //bool reliable = true;
            Debug.LogError("Raising Event.");
            PhotonNetwork.RaiseEvent((byte)eventId, content, sendReliable, options);
        }

        /// <summary>
        /// Joins the lobby.
        /// </summary>
        public void JoinLobby()
        {
            PhotonNetwork.JoinLobby();
        }

        /// <summary>
        /// Leaves the lobby.
        /// </summary>
        public void LeaveLobby()
        {
            PhotonNetwork.LeaveLobby();
        }

        /// <summary>
        /// Creates the room with name.
        /// </summary>
        /// <param name="roomName">Room name.</param>
        public void CreateRoomWithName(string roomName)
        {
            Debug.LogError("PhotonController.CreateRoomWithName Called.");
            PhotonNetwork.CreateRoom(roomName);
        }

        /// <summary>
        /// Creates the custom room.
        /// </summary>
        /// <param name="roomName">Room name.</param>
        /// <param name="emptyRoomTtl">Empty room ttl.</param>
        /// <param name="playerTtl">Player ttl.</param>
        /// <param name="isOpen">If set to <c>true</c> is open.</param>
        /// <param name="isVisible">If set to <c>true</c> is visible.</param>
        /// <param name="maxPlayers">Max players.</param>
        /// <param name="lobbyType">Lobby type.</param>
        public void CreateCustomRoom(string roomName, int emptyRoomTtl = 300000, int playerTtl = 90000000, bool isOpen = true, bool isVisible = true, byte maxPlayers = 4, LobbyType lobbyType = LobbyType.Default)
        {
            Debug.LogError("PhotonController.CreateCustomRoom Called.");
            //PhotonNetwork.CreateRoom(roomName);
            PhotonNetwork.CreateRoom(roomName, new RoomOptions() { EmptyRoomTtl = emptyRoomTtl, PlayerTtl = playerTtl, IsOpen = isOpen, IsVisible = isVisible, MaxPlayers = maxPlayers }, TypedLobby.Default);
        }

        public void CreateCustomRoom(string roomName, RoomOptions roomOptions, LobbyType lobbyType = LobbyType.Default)
        {
            Debug.LogError("PhotonController.CreateCustomRoom Called.");

            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
        }

        public void CreateCustomRoom(string roomName, RoomOptions roomOptions, LobbyType lobbyType, string[] expectedUsers)
        {
            Debug.LogError("PhotonController.CreateCustomRoom Called.");

            PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default, expectedUsers);
        }

        public void JoinRoomWithName(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void JoinRandom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion PublicFunctions

        #region Properties

        public bool IsConnected
        {
            get
            {
                return PhotonNetwork.connected;
            }
        }

        public bool IsMasterClient
        {
            get
            {
                return PhotonNetwork.isMasterClient;
            }
        }

        public PhotonPlayer LocalPlayer
        {
            get
            {
                return PhotonNetwork.player;
            }
        }

        public string LocalPlayerName
        {
            set
            {
                PhotonNetwork.playerName = value;
            }

            get
            {
                return PhotonNetwork.playerName;
            }
        }

        public PhotonPlayer[] PlayersInRoom
        {
            get
            {
                return PhotonNetwork.playerList;
            }
        }
        #endregion Properties

        /// <summary>
        /// Implementing the IPunCallbacks
        /// </summary>
        #region PhotonCallbacks

        public void OnConnectedToMaster()
        {
            if (OnConnectionToMaster != null)
                OnConnectionToMaster();
        }

        public void OnConnectedToPhoton()
        {
            if (OnConnectionToPhoton != null)
                OnConnectionToPhoton();
        }

        public void OnConnectionFail(DisconnectCause cause)
        {
            if (OnConnectingFail != null)
                OnConnectingFail(cause);
        }

        public void OnCreatedRoom()
        {
            if (OnRoomCreated != null)
                OnRoomCreated();
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            if (OnCustomAuthenticationFail != null)
                OnCustomAuthenticationFail(debugMessage);
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            if (OnCustomAuthenticateResponse != null)
                OnCustomAuthenticateResponse(data);
        }

        public void OnDisconnectedFromPhoton()
        {
            if (OnDisconnectFromPhoton != null)
                OnDisconnectFromPhoton();
        }

        public void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            if (OnFailedToConnectToThePhoton != null)
                OnFailedToConnectToThePhoton(cause);
        }

        public void OnJoinedLobby()
        {
            if (OnLobbyJoined != null)
                OnLobbyJoined();
        }

        public void OnJoinedRoom()
        {
            if (OnRoomJoined != null)
                OnRoomJoined();
        }

        public void OnLeftLobby()
        {
            if (OnLobbyLeft != null)
                OnLobbyLeft();
        }

        public void OnLeftRoom()
        {
            if (OnRoomLeft != null)
                OnRoomLeft();
        }

        public void OnLobbyStatisticsUpdate()
        {
            if (OnLobbyStatisticUpdate != null)
                OnLobbyStatisticUpdate();
        }

        public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            if (OnMasterClientSwitch != null)
                OnMasterClientSwitch(newMasterClient);
        }

        public void OnOwnershipRequest(object[] viewAndPlayer)
        {
            if (OnOwnershipRequested != null)
                OnOwnershipRequested(viewAndPlayer);
        }

        public void OnOwnershipTransfered(object[] viewAndPlayers)
        {
            if (OnOwnershipTransfer != null)
                OnOwnershipTransfer(viewAndPlayers);
        }

        public void OnPhotonCreateRoomFailed(object[] codeAndMsg)
        {
            if (OnCreateRoomFailed != null)
                OnCreateRoomFailed(codeAndMsg);
        }

        public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
        {
            if (OnCustomRoomPropertiesChanged != null)
                OnCustomRoomPropertiesChanged(propertiesThatChanged);
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            if (OnPhotonInstantiated != null)
                OnPhotonInstantiated(info);
        }

        public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
        {
            if (OnJoinRoomFailed != null)
                OnJoinRoomFailed(codeAndMsg);
        }

        public void OnPhotonMaxCccuReached()
        {
            if (OnPhotonMaxCccuReach != null)
                OnPhotonMaxCccuReach();
        }

        public void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
        {
            if (OnPlayerActivityChanged != null)
                OnPlayerActivityChanged(otherPlayer);
        }

        public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            if (OnPlayerConnected != null)
                OnPlayerConnected(newPlayer);
        }

        public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
        {
            if (OnPlayerDisconnected != null)
                OnPlayerDisconnected(otherPlayer);
        }

        public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
        {
            if (OnPlayerPropertiesChanged != null)
                OnPlayerPropertiesChanged(playerAndUpdatedProps);
        }

        public void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            if (OnRandomJoinFailed != null)
                OnRandomJoinFailed(codeAndMsg);
        }

        public void OnReceivedRoomListUpdate()
        {
            if (OnReceivedRoomListUpdated != null)
                OnReceivedRoomListUpdated();
        }

        public void OnUpdatedFriendList()
        {
            if (OnUpdateFriendList != null)
                OnUpdateFriendList();
        }

        public void OnWebRpcResponse(OperationResponse response)
        {
            if (OnWebRpcResponsed != null)
                OnWebRpcResponsed(response);
        }

        #endregion PhotonCallbacks
    }
}