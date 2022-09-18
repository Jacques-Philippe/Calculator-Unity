using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Calculator.UI.Scripts
{
    public class CalculatorUIManager : MonoBehaviour
    {
        /// <summary>
        /// The UI element containing the text where state information is displayed
        /// </summary>
        [SerializeField]
        protected TextMeshProUGUI text;

        /// <summary>
        /// The value of the angle, in degrees
        /// </summary>
        private string message;

        /// <summary>
        /// The public-facing accessor and mutator for the angle value. <br />
        /// Note changing this value will update the UI
        /// </summary>
        public string Message
        {
            get => message;
            set
            {
                this.ThreadsafeUIUpdate(newMessage: value);
                this.message = value;
            }
        }

        /// <summary>
        /// A queue to contain all enqueued text to update the UI with
        /// </summary>
        /// <remarks>this is done so that updates to the UI are made properly</remarks>
        protected ConcurrentQueue<string> mQueuedText = new ConcurrentQueue<string>();

        void ThreadsafeUIUpdate(string newMessage)
        {
            mQueuedText.Enqueue($"{newMessage}");
        }

        protected virtual void UpdateMessage()
        {
            string newMessage;
            while (mQueuedText.TryDequeue(out newMessage))
            {
                text.text = $"{newMessage}";
            }
        }

        private void Update()
        {
            UpdateMessage();
        }
    }
}
