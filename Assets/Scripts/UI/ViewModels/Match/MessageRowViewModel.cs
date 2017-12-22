using Game.UI.Models.Match;
using Game.UI.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.ViewModels.Match
{
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
}