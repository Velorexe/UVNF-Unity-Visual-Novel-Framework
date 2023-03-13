using System.Collections;
using TMPro;

namespace UVNF.Core.UI.Writers
{
    /// <summary>
    /// An interface that's used to display text on the TMP UI
    /// </summary>
    public interface ITextWriter
    {
        /// <summary>
        /// Writes text to the given <see cref="TextMeshProUGUI"/> with a set speed
        /// </summary>
        /// <param name="tmp">The <see cref="TextMeshProUGUI"/> that the <paramref name="text"/> should be displayed on</param>
        /// <param name="text">The text that should be displayed</param>
        /// <param name="displaySpeed">The speed at which each character is displayed</param>
        /// <returns>A Unity <see cref="UnityEngine.Coroutine"/></returns>
        public IEnumerator Write(TextMeshProUGUI tmp, string text, float displaySpeed);

        /// <summary>
        /// Instantly writes all text to the given <see cref="TextMeshProUGUI"/>
        /// </summary>
        /// <param name="tmp">The <see cref="TextMeshProUGUI"/> that the <paramref name="text"/> should be displayed on</param>
        /// <param name="text">The text that should be displayed</param>
        public void WriteInstant(TextMeshProUGUI tmp, string text);
    }
}
