using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Traits
{
    public string Name 
    { 
        get { return _name; } 
        set { _name = value; }
    }
    private string _name;
}
