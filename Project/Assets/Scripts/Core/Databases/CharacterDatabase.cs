using System.Collections.Generic;
using UnityEngine;
using Entities;
using System.Linq;

public class CharacterDatabase : ScriptableObject
{
    [SerializeField]
    public List<Character> Characters = new List<Character>();

    public string[] GetCharacterNames()
    {
        return Characters.Select(x => x.FirstName + " " + x.LastName + $" ({x.NickName})").ToArray();
    }

    public Character GetCharacterAtIndex(int index)
    {
        if(index > 0 && index < Characters.Count)
            return Characters[index];
        return null;
    }

    public int CharacterCount()
    {
        return Characters.Count;
    }

    public string GetCharacterNameAtIndex(int index)
    {
        if (index > 0 && index < Characters.Count)
            return Characters[index].FirstName + " " + Characters[index].LastName;
        return string.Empty;
    }

    public void AddCharacter()
    {
        Characters.Add(new Character());
    }
}
