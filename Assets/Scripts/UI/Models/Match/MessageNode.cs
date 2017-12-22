namespace Game.UI.Models.Match
{
    public class MessageNode
    {
        string message;
        string sender;

        public MessageNode(string message, string sender)
        {
            this.message = message;
            this.sender = sender;
        }

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
            }
        }

        public string Sender
        {
            get { return sender; }
            set
            {
                sender = value;
            }
        }
    }
}