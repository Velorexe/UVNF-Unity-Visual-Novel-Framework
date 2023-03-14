using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

namespace UVNF.Core.UI.Writers.Settings
{
    /// <summary>
    /// Settings for <see cref="ITextWriter"/> to adjust certain TMP UI properties
    /// </summary>
    [StructLayout(LayoutKind.Auto), Serializable]
    public struct TextWriterSettings
    {
        /// <summary>
        /// The size at which the font should be displayed
        /// </summary>
        public float FontSize;

        /// <summary>
        /// The font that should be used
        /// </summary>
        public TMP_FontAsset Font;

        /// <summary>
        /// Marks which styles should be used (Bold, Italic, etc.)
        /// </summary>
        public FontStyles Styles;

        /// <summary>
        /// The color of the font
        /// </summary>
        public Color Color;

        /// <summary>
        /// The time between each character being shown
        /// </summary>
        public float TextDisplaySpeed;

        public TextWriterSettings(
            float fontSize,
            TMP_FontAsset font,

            FontStyles styles,
            Color color,

            float textDisplaySpeed = 0.025f)
        {
            FontSize = fontSize;
            Font = font;

            Styles = styles;
            Color = color;

            TextDisplaySpeed = textDisplaySpeed;
        }
    }
}