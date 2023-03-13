using System.Collections;
using UnityEngine;
using UVNF.Core.UI;
using UVNF.Core.UI.Writers;
using UVNF.Core.UI.Writers.Settings;
using UVNF.Entities;

namespace UVNF.Core.Story.Dialogue
{
    /// <summary>
    /// A <see cref="StoryElement"/> that displays dialogue on the UI
    /// </summary>
    public class DialogueElement : StoryElement
    {
        public override string ElementName => "Dialogue";

        public override StoryElementTypes Type => StoryElementTypes.Story;

        /// <summary>
        /// The text that should appear in the dialogue's name panel
        /// </summary>
        public string CharacterName;

        /// <summary>
        /// The dialogue that should be displayed on the UI
        /// </summary>
        [TextArea(8, 10)]
        public string Dialogue;

        [SerializeReference, HideInInspector]
        public ITextWriter TextWriter;

        [HideInInspector]
        public TextWriterSettings WriterSettings;

#if UNITY_EDITOR
        public override void OnCreate(UVNFGameResources resources)
        {
            WriterSettings = resources.DefaultWriterSettings;
        }

        [HideInInspector]
        public bool DefaultTextWriter = true;

        [HideInInspector]
        public bool DefaultWriterSettings = true;
#endif

        public override IEnumerator Execute(UVNFManager gameManager, UVNFCanvas canvas)
        {
            return canvas.DisplayText(Dialogue, CharacterName, TextWriter, WriterSettings);
        }
    }
}