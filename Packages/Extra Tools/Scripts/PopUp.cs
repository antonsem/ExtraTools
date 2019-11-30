using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ExtraTools
{

    public class Message
    {
        public Sprite img;
        public string message;
        public UnityAction yes;
        public UnityAction no;
        public UnityAction ok;
        public UnityAction cancel;

        public Message() { }
        public Message(string msg, UnityAction _ok, UnityAction _yes, UnityAction _no, UnityAction _cancel, Sprite _img)
        {
            message = msg;
            ok = _ok;
            yes = _yes;
            no = _no;
            cancel = _cancel;
            img = _img;
        }
    }

    public class PopUp : MonoBehaviour
    {
        [SerializeField]
        private Text messageLabel;
        [SerializeField]
        private Image messageImage;
        [SerializeField]
        private Text message;
        [SerializeField]
        private Button yes;
        [SerializeField]
        private Button no;
        [SerializeField]
        private Button ok;
        [SerializeField]
        private Button cancel;

        private Queue<Message> messages = new Queue<Message>();

        /// <summary>
        /// Creates and adds a message to the queue
        /// </summary>
        /// <param name="msg">The text of the message</param>
        /// <param name="okAction">A method to invoke when 'ok' button is pressed</param>
        /// <param name="yesAction">A method to invoke when 'yes' button is pressed</param>
        /// <param name="noAction">A method to invoke when 'no' button is pressed</param>
        /// <param name="cancelAction">A method to invoke when 'cancel' button is pressed</param>
        /// <param name="img">A sprite to show alongside the question</param>
        public void Register(string msg, UnityAction okAction = null, UnityAction yesAction = null, UnityAction noAction = null, UnityAction cancelAction = null, Sprite img = null)
        {
            Register(new Message(msg, okAction, yesAction, noAction, cancelAction, img));
        }

        /// <summary>
        /// Adds a message to the queue
        /// </summary>
        /// <param name="msg"></param>
        public void Register(Message msg)
        {
            messages.Enqueue(msg);
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                CheckMessages();
            }
        }

        /// <summary>
        /// Sets the message to the PopUp window
        /// </summary>
        /// <param name="msg"></param>
        private void SetMessage(Message msg)
        {
            //Set image if needed. Disable the image object otherwise
            if (msg.img)
            {
                messageImage.gameObject.SetActive(true);
                messageImage.sprite = msg.img;
            }
            else
            {
                messageImage.gameObject.SetActive(false);
            }

            //Set the message text
            message.text = msg.message;

            //Remove listeners from the buttons
            yes.onClick.RemoveAllListeners();
            no.onClick.RemoveAllListeners();
            ok.onClick.RemoveAllListeners();
            cancel.onClick.RemoveAllListeners();

            //Set the buttons
            if (msg.yes != null)
            {
                yes.gameObject.SetActive(true);
                yes.onClick.AddListener(msg.yes);
                yes.onClick.AddListener(CheckMessages);
            }
            else
            {
                yes.gameObject.SetActive(false);
            }

            if (msg.no != null)
            {
                no.gameObject.SetActive(true);
                no.onClick.AddListener(msg.no);
                no.onClick.AddListener(CheckMessages);
            }
            else
            {
                no.gameObject.SetActive(false);
            }

            if (msg.cancel != null)
            {
                cancel.gameObject.SetActive(true);
                cancel.onClick.AddListener(msg.cancel);
                cancel.onClick.AddListener(CheckMessages);
            }
            else
            {
                cancel.gameObject.SetActive(false);
            }

            //If other buttons are disabled, we probably want the OK button to be enabled
            if (!cancel.gameObject.activeSelf && !no.gameObject.activeSelf && !yes.gameObject.activeSelf)
            {
                ok.gameObject.SetActive(true);
                ok.onClick.AddListener(CheckMessages);
                if (msg.ok != null)
                    ok.onClick.AddListener(msg.ok);
            }
            else if (msg.ok != null)
            {
                ok.gameObject.SetActive(true);
                ok.onClick.AddListener(msg.ok);
                ok.onClick.AddListener(CheckMessages);
            }
            else
            {
                ok.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Check if any messages remains in the queue
        /// </summary>
        private void CheckMessages()
        {
            if (messages.Count > 0)
            {
                //Set message label text
                messageLabel.text = string.Format("Got {0} message{1}", (messages.Count + 1).ToString(), messages.Count == 0 ? string.Empty : "s");
                SetMessage(messages.Dequeue());
            }
            else
                gameObject.SetActive(false);
        }
    }
}
