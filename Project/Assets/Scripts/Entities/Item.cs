using UnityEngine;

public abstract class Item
{
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    private string _name = "Undefined";

    public virtual void Use()
    {

    }

    public virtual void Interact()
    {

    }
}
