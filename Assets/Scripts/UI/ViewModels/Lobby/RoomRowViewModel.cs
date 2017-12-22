﻿using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Room row view model.
/// </summary>
public class RoomRowViewModel : ListNodeGeneric<RoomRowModel>
{
    [Header("References")]
    [SerializeField]
    Text roomName;
    [SerializeField]
    Text playersCount;

    #region PublicFunctions

    public void OnJoinButtonClicked()
    {
        if (GameManager.Instance.LobbyController != null)
            GameManager.Instance.LobbyController.JoinRoomWithName(Data.RoomName);
    }

    #endregion PublicFunctions


    #region Properties
    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    /// <value>The data.</value>
    public override RoomRowModel Data
    {
        get
        {
            return base.Data;
        }

        set
        {
            base.Data = value;

            roomName.text = Data.RoomName;
            playersCount.text = Data.PlayersCount.ToString();
        }
    }
    #endregion Properties
}
