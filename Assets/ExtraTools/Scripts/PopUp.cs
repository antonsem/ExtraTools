using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExtraTools
{
	public readonly struct MessageButton
	{
		public readonly string label;
		public readonly Action callback;

		public MessageButton(string labelText, Action callbackAction)
		{
			label = labelText;
			callback = callbackAction;
		}
	}

	public readonly struct Message
	{
		public readonly Sprite img;
		public readonly string message;
		public readonly MessageButton[] buttons;

		public Message(string msg, Sprite messageIcon = null, params MessageButton[] messageButtons)
		{
			message = msg;
			img = messageIcon;

			buttons = messageButtons?.Length > 0
				? messageButtons
				: new[] { new MessageButton("OK", () => {}) };
		}
	}

	public class PopUp : MonoBehaviour
	{
		[SerializeField] private Canvas canvas;
		[SerializeField] private Text messageLabel;
		[SerializeField] private Image messageImage;
		[SerializeField] private Text message;
		[SerializeField] private GameObject buttonPrefab;
		[SerializeField] private Transform buttonsParent;

		private List<Button> _buttons = new List<Button>();
		private Queue<Message> _messages = new Queue<Message>();

		/// <summary>
		/// Creates and adds a message to the queue
		/// </summary>
		/// <param name="msg">The text of the message</param>
		/// <param name="buttons">Buttons with corresponding actions</param>
		public void Register(string msg, params MessageButton[] buttons)
		{
			Register(new Message(msg, null, buttons));
		}

		/// <summary>
		/// Creates and adds a message to the queue
		/// </summary>
		/// <param name="msg">The text of the message</param>
		/// <param name="img">A sprite to show alongside the question</param>
		/// <param name="buttons">Buttons with corresponding actions</param>
		public void Register(string msg, Sprite img = null, params MessageButton[] buttons)
		{
			Register(new Message(msg, img, buttons));
		}

		/// <summary>
		/// Adds a message to the queue
		/// </summary>
		public void Register(in Message msg)
		{
			_messages.Enqueue(msg);

			if (canvas.enabled)
			{
				UpdateLabel();
				return;
			}

			canvas.enabled = true;
			CheckMessages();
		}

		/// <summary>
		/// Sets the message to the PopUp window
		/// </summary>
		private void SetMessage(in Message msg)
		{
			//Remove all previously set buttons
			ClearButtons();

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

			for (var i = 0; i < msg.buttons.Length; i++)
			{
				var newBtn = GetButton(buttonsParent);
				newBtn.onClick.AddListener(msg.buttons[i].callback.Invoke);
				newBtn.GetComponentInChildren<Text>().text = msg.buttons[i].label;
			}
		}

		/// <summary>
		/// Deactivates buttons and removes the onClick listeners 
		/// </summary>
		private void ClearButtons()
		{
			for (var i = 0; i < _buttons.Count; i++)
			{
				if (!_buttons[i].gameObject.activeSelf) continue;
				_buttons[i].onClick.RemoveAllListeners();
				_buttons[i].gameObject.SetActive(false);
			}
		}

		/// <summary>
		/// Checks if there is an inactive button. Activates and returns it if there is.
		/// Instantiates a new one otherwise.
		/// </summary>
		/// <param name="bParent">Parent for the new button</param>
		/// <returns>A button</returns>
		private Button GetButton(Transform bParent)
		{
			for (var i = 0; i < _buttons.Count; i++)
			{
				if (_buttons[i].gameObject.activeSelf) continue;
				_buttons[i].onClick.RemoveAllListeners();
				_buttons[i].onClick.AddListener(CheckMessages);
				_buttons[i].gameObject.SetActive(true);
				return _buttons[i];
			}

			var newButton = Instantiate(buttonPrefab, bParent).GetComponent<Button>();
			newButton.onClick.AddListener(CheckMessages);
			_buttons.Add(newButton);
			return _buttons[_buttons.Count - 1];
		}

		/// <summary>
		/// Check if any messages remains in the queue
		/// </summary>
		private void CheckMessages()
		{
			if (_messages.Count == 0)
			{
				canvas.enabled = false;
				return;
			}

			SetMessage(_messages.Dequeue());
			UpdateLabel();
		}

		/// <summary>
		/// Updates the message count label
		/// </summary>
		private void UpdateLabel()
		{
			//Set message label text
			messageLabel.text = $"Got {(_messages.Count + 1).ToString()} message{(_messages.Count == 0 ? "" : "s")}";
		}

		/// <summary>
		/// Checks all canvases present in the scene and sets the sort order of itself to
		/// maximum found sorting order + 1
		/// </summary>
		public void SetMaxSortOrder()
		{
			var canvases = FindObjectsOfType<Canvas>(true);
			canvas.overrideSorting = true;
			var maxOrder = canvas.sortingOrder;

			foreach (var c in canvases)
			{
				if (c.sortingOrder >= maxOrder)
				{
					maxOrder = c.sortingOrder;
				}
			}

			canvas.sortingOrder = maxOrder + 1;
		}
	}
}