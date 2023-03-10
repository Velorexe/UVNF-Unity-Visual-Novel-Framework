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
        /// <summary>
        /// The name of the <see cref="StoryElement"/>
        /// </summary>
        public abstract string ElementName { get; }

        /// <summary>
        /// The category / type of the StoryElement
        /// </summary>
        public abstract StoryElementTypes Type { get; }

        /// <summary>
        /// Defines whether the <see cref="StoryElement"/> is visible in the ContextMenu
        /// </summary>
        /// <returns>Returns <see langword="true"/> if the Node should not be shown in the Graph's ContextMenu</returns>
        public virtual bool IsVisible() { return true; }

        /// <summary>
        /// Is <see langword="true"/> if the <see cref="StoryElement"/> is currently being processed
        /// </summary>
        [HideInInspector]
        public bool Active = false;

        /// <summary>
        /// The next <see cref="StoryElement"/> that should play after the <see cref="Execute(UVNFManager, UVNFCanvas)"/> is finished
        /// </summary>
        [HideInInspector]
        public StoryElement Next;

        /// <summary>
        /// The previous <see cref="StoryElement"/> Input for xNode to render
        /// </summary>
        [HideInInspector]
        [Input(ShowBackingValue.Never, ConnectionType.Multiple)]
        public NodePort PreviousNode;

        /// <summary>
        /// The next <see cref="StoryElement"/> Input for xNode to render
        /// </summary>
        [HideInInspector]
        [Output(ShowBackingValue.Never, ConnectionType.Override)]
        public NodePort NextNode;

        /// <summary>
        /// Returns the value of the next port, but only if it's connected
        /// </summary>
        /// <param name="port">The port to get a value from</param>
        /// <returns><see langword="object"/> if a port is connected, <see langword="null"/> if the port is not connected</returns>
        public override object GetValue(NodePort port)
        {
            if (port.IsConnected)
            {
                return port.Connection.node;
            }

            return null;
        }

        /// <summary>
        /// Called when the Node is created inside the <see cref="UVNF.Entities.Containers.StoryGraph"/>
        /// </summary>
        public virtual void OnCreate() { }

        /// <summary>
        /// Called when the Node is deleted inside the <see cref="UVNF.Entities.Containers.StoryGraph"/>
        /// </summary>
        public virtual void OnDelete() { }

        /// <summary>
        /// Sets the <see cref="Next"/> property when the <see cref="NextNode"/> is connected
        /// </summary>
        public virtual void Connect()
        {
            if (GetOutputPort("NextNode").IsConnected)
            {
                Next = GetOutputPort("NextNode").Connection.node as StoryElement;
            }
        }

        /// <summary>
        /// Executes the <see cref="StoryElement"/>
        /// </summary>
        /// <param name="managerCallback">Used for callbacks and accessing UI resources</param>
        /// <param name="canvas">Used for managing characters and dialogue</param>
        /// <returns>A Unity <see cref="Coroutine"/></returns>
        public abstract IEnumerator Execute(UVNFManager managerCallback, UVNFCanvas canvas);

        /// <summary>
        /// Compares the given <see cref="StoryElement"/>'s names
        /// </summary>
        /// <param name="obj"></param>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is StoryElement))
            {
                return 1;
            }

            return string.Compare(ElementName, ((StoryElement)obj).ElementName);
        }
    }
}