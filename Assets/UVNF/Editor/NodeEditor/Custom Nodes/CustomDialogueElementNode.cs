using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UVNF.Core.Story.Dialogue;
using UVNF.Core.UI.Writers;
using UVNF.Editor.Settings;

namespace UVNF.Editor.Story.Nodes
{
    [CustomNodeEditor(typeof(DialogueElement))]
    public class CustomDialogueNode : CustomStoryElementNode
    {
        private bool _checkedType = false;

        public override void DrawBody()
        {
            base.DrawBody();

            DrawTextWriter((DialogueElement)Node);
            DrawTextWriterSettings((DialogueElement)Node);
        }

        /// <summary>
        /// Draws the <see cref="ITextWriter"/> field
        /// </summary>
        /// <param name="node">The <see cref="DialogueElement"/> node from which the TextWriter should be displayed</param>
        internal void DrawTextWriter(DialogueElement node)
        {
            ITextWriter[] textWriters = UVNFEditorSettings.Instance.MainResources.TextWriterPool;

            if (!_checkedType && node.TextWriter != null)
            {
                node.TextWriter = textWriters.First(x => x.GetType() == node.TextWriter.GetType());
                _checkedType = true;
            }

            List<string> textWritersNames = textWriters.Select(x => x.GetType().Name).ToList();
            textWritersNames.Insert(0, textWritersNames[0] + " (Default)");

            // If the Dialogue Node is Default
            if (node.DefaultTextWriter && node.TextWriter != textWriters[0])
            {
                node.TextWriter = textWriters[0];
            }

            int currentIndex = node.DefaultTextWriter ? 0 : Array.IndexOf(textWriters, node.TextWriter) + 1;
            int index = EditorGUILayout.Popup("Text Writer", currentIndex, textWritersNames.ToArray());

            if (index != currentIndex)
            {
                // Custom
                if (index > 0)
                {
                    node.TextWriter = textWriters[index - 1];
                    node.DefaultTextWriter = false;
                }
                // Default
                else
                {
                    node.DefaultTextWriter = true;
                }
            }
        }

        /// <summary>
        /// Draws the settings that should be applied to the <see cref="ITextWriter"/>
        /// </summary>
        /// <param name="node">The <see cref="DialogueElement"/> node from which the TextWriterSettings should be displayed</param>
        internal void DrawTextWriterSettings(DialogueElement node)
        {
            node.DefaultWriterSettings = EditorGUILayout.Toggle("Default Writer Settings", node.DefaultWriterSettings);

            if (node.DefaultWriterSettings)
            {
                node.WriterSettings = UVNFEditorSettings.Instance.MainResources.DefaultWriterSettings;
            }
            else
            {
                DrawTextWriterSettingsFields(node);
            }
        }

        private void DrawTextWriterSettingsFields(DialogueElement node)
        {
            node.WriterSettings.FontSize = EditorGUILayout.FloatField("Font Size", node.WriterSettings.FontSize);

            node.WriterSettings.Font = (TMP_FontAsset)EditorGUILayout.ObjectField(
                "Font",
                node.WriterSettings.Font,
                typeof(TMP_FontAsset),
                false);

            node.WriterSettings.Styles = (FontStyles)EditorGUILayout.EnumFlagsField("Styles", node.WriterSettings.Styles);
            node.WriterSettings.Color = EditorGUILayout.ColorField("Color", node.WriterSettings.Color);

            node.WriterSettings.TextDisplaySpeed = EditorGUILayout.FloatField("Display Speed", node.WriterSettings.TextDisplaySpeed);
        }
    }
}
