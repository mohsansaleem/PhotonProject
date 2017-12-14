using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhotonRaisedEventsHandler
{
    private System.Collections.Generic.Dictionary<EventsIDs, System.Delegate> eventTable;

    public PhotonRaisedEventsHandler()
    {
        eventTable = new System.Collections.Generic.Dictionary<EventsIDs, System.Delegate>();
        foreach (EventsIDs eventId in Enum.GetValues(typeof(EventsIDs)))
            eventTable.Add(eventId, null);
    }

    //public event Action<System.Object, int> OnChatMessage
    //{
    //    add
    //    {
    //        lock (eventTable)
    //        {
    //            eventTable[EventsIDs.ChatMessage] = (Action<System.Object, int>)eventTable[EventsIDs.ChatMessage] + value;
    //        }
    //    }
    //    remove
    //    {
    //        lock (eventTable)
    //        {
    //            eventTable[EventsIDs.ChatMessage] = (Action<System.Object, int>)eventTable[EventsIDs.ChatMessage] - value;
    //        }
    //    }
    //}


    /// <summary>
    /// Gets or sets the <see cref="T:PhotonRaisedEventsHandler"/> with the specified id.
    /// Setting would just add a new Handler to the previous one.
    /// </summary>
    /// <param name="id">Identifier.</param>
    public Action<System.Object, int> this[EventsIDs id]
    {
        get { return (Action<System.Object, int>)eventTable[id]; }
        set { eventTable[id] = (Action<System.Object, int>)eventTable[id] + value; }
    }

    /// <summary>
    /// Remove the specified Event.
    /// </summary>
    /// <returns>The remove.</returns>
    /// <param name="eventId">Event identifier.</param>
    /// <param name="eventToRemove">Event to remove.</param>
    public void Remove(EventsIDs eventId, Action<System.Object, int> eventToRemove)
    {
        eventTable[eventId] = (Action<System.Object, int>)eventTable[eventId] - eventToRemove;
    }

    public void OnEventRaised(byte eventcode, object content, int senderid)
    {
        Debug.LogError("EventRaised.");
        Action<System.Object, int> handler;
        if (null != (handler = (Action<System.Object, int>)eventTable[(EventsIDs)eventcode]))
        {
            handler(content, senderid);
        }
    }
}


public enum EventsIDs
{
    ChatMessage = 1
}