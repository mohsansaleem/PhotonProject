using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageRowViewModel : ListNodeGeneric<MessageNode>
{
    [Header("Refrerences")]
    [SerializeField]
    Text messageText;
    [SerializeField]
    Text senderText;

    #region Properties

    public override MessageNode Data
    {
        get
        {
            return base.Data;
        }

        set
        {
            base.Data = value;

            messageText.text = Data.Message;
            senderText.text = Data.Sender;
        }
    }

    #endregion Properties
}
