using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

namespace UVNF.Core.UI.Writers.Settings
{
    [StructLayout(LayoutKind.Auto), Serializable]
    public struct TextWriterSettings
    {
        public float FontSize;
        public TMP_FontAsset Font;

        public FontStyles Styles;
        public Color Color;

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