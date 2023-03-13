using UnityEngine;
using UVNF.Core.UI.Writers;
using UVNF.Core.UI.Writers.Settings;

namespace UVNF.Entities
{
    public class UVNFGameResources : ScriptableObject
    {
        [SerializeReference]
        public ITextWriter[] TextWriterPool = new ITextWriter[0];

        public TextWriterSettings DefaultWriterSettings;
    }
}

