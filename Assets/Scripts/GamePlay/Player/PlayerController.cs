using System.Collections;
using System.Collections.Generic;
using ExitGames.Demos.DemoAnimator;
using Game.Managers;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Photon.PunBehaviour, IPunObservable
{
    [SerializeField]
    int hp;
    [SerializeField]
    TextMesh health;

    private void Start()
    {
        hp = 200;

        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();


        if (_cameraWork != null)
        {
            if (photonView.isMine)
            {
                Game.Managers.GameManager.Instance.MatchController.PlayerController = this;
                _cameraWork.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }
    }

    public void SendMessageToOthers(string msg)
    {
        photonView.RPC("MessageRPCReceived", PhotonTargets.Others, msg);
        //PhotonManager.Instance.RaiseEvent(EventsIDs.ChatMessage, msg, true, null);
    }

    [PunRPC]
    void MessageRPCReceived(string msg, PhotonMessageInfo info)
    {
        Game.Managers.GameManager.Instance.MatchController.OnNewMessage(msg, info.sender.ID.ToString());
        Debug.Log(string.Format("ChatMessage {0} {1}", msg, info.sender.ID));
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(hp);
            health.text = "Me: " + hp;
        }
        else
        {
            // Network player, receive data
            this.hp = (int)stream.ReceiveNext();
            health.text = info.sender.NickName + ": " + hp;

        }
    }
}
