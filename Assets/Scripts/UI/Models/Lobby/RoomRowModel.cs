using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI.Models.Lobby
{
    public class RoomRowModel
    {
        string roomName;
        int playersCount;

        public RoomRowModel()
        {
            roomName = "";
            playersCount = 0;
        }

        public RoomRowModel(string roomName, int playersCount)
        {
            this.roomName = roomName;
            this.playersCount = playersCount;
        }

        public string RoomName
        {
            get
            {
                return roomName;
            }

            set
            {
                roomName = value;
            }
        }

        public int PlayersCount
        {
            get
            {
                return playersCount;
            }

            set
            {
                playersCount = value;
            }
        }
    }
}