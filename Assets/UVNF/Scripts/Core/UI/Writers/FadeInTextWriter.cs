using System.Collections;
using TMPro;
using UnityEngine;

namespace UVNF.Core.UI.Writers
{
    public class FadeInTextWriter : ITextWriter
    {
        // Code originates from the Unity Forums:
        // https://forum.unity.com/threads/teletype-interfering-with-vertex-jitter.611245/#post-4092415
        public IEnumerator Write(TextMeshProUGUI tmp, string text, float displaySpeed)
        {
            Color col = tmp.color;
            col.a = 0;

            tmp.color = col;
            tmp.SetText(text);

            tmp.ForceMeshUpdate();

            TMP_TextInfo textInfo = tmp.textInfo;
            Color32[] newVertexColors;

            int currentCharacter = 0;
            int startingCharacterRange = currentCharacter;

            bool isRangeMax = false;

            while (!isRangeMax)
            {
                int characterCount = textInfo.characterCount;
                // Spread should not exceed the number of characters.
                byte fadeSteps = (byte)Mathf.Max(1, 255 / 10);

                for (int i = startingCharacterRange; i < currentCharacter + 1; i++)
                {
                    // Skip characters that are not visible
                    if (!textInfo.characterInfo[i].isVisible)
                    {
                        continue;
                    }

                    // Get the index of the material used by the current character.
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;

                    // Get the vertex colors of the mesh used by this text element (character or sprite).
                    newVertexColors = textInfo.meshInfo[materialIndex].colors32;

                    // Get the index of the first vertex used by this text element.
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                    // Get the current character's alpha value.
                    byte alpha = (byte)Mathf.Clamp(newVertexColors[vertexIndex + 0].a + fadeSteps, 0, 255);

                    // Set new alpha values.
                    newVertexColors[vertexIndex + 0].a = alpha;
                    newVertexColors[vertexIndex + 1].a = alpha;
                    newVertexColors[vertexIndex + 2].a = alpha;
                    newVertexColors[vertexIndex + 3].a = alpha;

                    // Tint vertex colors
                    // Note: Vertex colors are Color32 so we need to cast to Color to multiply with tint which is Color.
                    newVertexColors[vertexIndex + 0] = newVertexColors[vertexIndex + 0] * Color.white;
                    newVertexColors[vertexIndex + 1] = newVertexColors[vertexIndex + 1] * Color.white;
                    newVertexColors[vertexIndex + 2] = newVertexColors[vertexIndex + 2] * Color.white;
                    newVertexColors[vertexIndex + 3] = newVertexColors[vertexIndex + 3] * Color.white;

                    isRangeMax = alpha == 255;
                }

                // Upload the changed vertex colors to the Mesh.
                tmp.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                if (currentCharacter + 1 < characterCount) currentCharacter += 1;
                yield return new WaitForSeconds(displaySpeed);
            }
        }

        public void WriteInstant(TextMeshProUGUI tmp, string text)
        {
            Color col = tmp.color;
            col.a = 255;

            tmp.color = col;

            tmp.SetText(text);
        }
    }
}