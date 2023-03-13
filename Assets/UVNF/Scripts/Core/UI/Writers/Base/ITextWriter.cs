using System.Collections;
using TMPro;

namespace UVNF.Core.UI.Writers
{
    public interface ITextWriter
    {
        public IEnumerator Write(TextMeshProUGUI tmp, string text, float displaySpeed);

        public void WriteInstant(TextMeshProUGUI tmp, string text);
    }
}
