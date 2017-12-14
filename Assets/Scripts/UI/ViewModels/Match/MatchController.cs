using System.Collections;
using System.Collections.Generic;
using Game.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchController : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    MessagesPanel messagesPanel;
    [SerializeField]
    InputField messageText;
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    [HideInInspector]
    public PlayerController PlayerController;

    private void Awake()
    {
        Game.Managers.GameManager.Instance.MatchController = this;
        PhotonManager.Instance.raisedEventsHandler[EventsIDs.ChatMessage] = HandleChatMessageAction;
    }

    // Use this for initialization
    void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.Log("We are Instantiating LocalPlayer from " + SceneManager.GetActiveScene().name);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            GameObject player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            var tmp = player.GetComponent<PlayerController>();
            if (tmp != null)
                PlayerController = tmp;
        }
    }

    private void OnDestroy()
    {
        PhotonManager.Instance.raisedEventsHandler.Remove(EventsIDs.ChatMessage, HandleChatMessageAction);
    }

    void HandleChatMessageAction(object arg1, int senderId)
    {
        string msg = arg1 as string;

        OnNewMessage(msg, senderId.ToString());
    }

    public void OnNewMessage(string message, string sender)
    {
        Debug.LogError("OnNewMessage: " + StackTraceUtility.ExtractStackTrace());
        messagesPanel.AddNewRow(new MessageNode(message, sender), true);
    }

    public void OnSendButtonClicked()
    {
        PlayerController.SendMessageToOthers(messageText.text);
        messagesPanel.AddNewRow(new MessageNode(messageText.text, "Me"), true);
        messageText.text = string.Empty;
    }


}
