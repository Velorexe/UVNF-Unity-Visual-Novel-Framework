using System;
using System.Collections;
using UnityEngine;
using UVNF.Core.UI;
using XNode;

namespace UVNF.Core.Story
{
    [NodeWidth(300)]
    public abstract class StoryElement : Node, IComparable
    {
        public abstract string ElementName { get; }
        public abstract Color32 DisplayColor { get; }
        public abstract StoryElementTypes Type { get; }

        public virtual bool IsVisible() { return true; }

        [HideInInspector]
        public bool Active = false;
        [HideInInspector]
        public StoryElement Next;

        [HideInInspector]
        [Input(ShowBackingValue.Never, ConnectionType.Multiple)]
        public NodePort PreviousNode;

        [HideInInspector]
        [Output(ShowBackingValue.Never, ConnectionType.Override)]
        public NodePort NextNode;

        public override object GetValue(NodePort port)
        {
            if (port.IsConnected)
                return port.Connection.node;
            return null;
        }

        public virtual void OnCreate() { }
        public virtual void OnDelete() { }

        public virtual void Connect()
        {
            if (GetOutputPort("NextNode").IsConnected)
                Next = GetOutputPort("NextNode").Connection.node as StoryElement;
        }

        public abstract IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas);

#if UNITY_EDITOR
        public abstract void DisplayLayout(Rect layoutRect, GUIStyle label = null);

        public virtual void DisplayNodeLayout(Rect layoutRect) { DisplayLayout(layoutRect); }

#endif

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (!(obj is StoryElement)) return 1;
            return string.Compare(ElementName, ((StoryElement)obj).ElementName);
        }
    }
}