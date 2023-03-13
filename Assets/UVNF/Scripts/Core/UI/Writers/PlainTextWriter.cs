using System.Collections;
using TMPro;
using UnityEngine;

namespace UVNF.Core.UI.Writers
{
    public class PlainTextWriter : ITextWriter
    {
        // Timer keeping track of the time inbetween letters being displayed
        private float displayIntervalTimer = 0f;

        public virtual IEnumerator Write(TextMeshProUGUI tmp, string text, float displaySpeed)
        {
            int textIndex = 0;
            while (textIndex < text.Length)
            {
                // Else if the timer is over the time it should take
                // for a character to be shown, show a character
                if (displayIntervalTimer >= displaySpeed)
                {
                    tmp.text += text[textIndex];
                    textIndex++;

                    displayIntervalTimer = 0f;
                }
                else
                {
                    displayIntervalTimer += Time.deltaTime;
                }

                yield return null;
            }
        }

        public virtual void WriteInstant(TextMeshProUGUI tmp, string text)
        {
            tmp.SetText(text);
        }
    }
}