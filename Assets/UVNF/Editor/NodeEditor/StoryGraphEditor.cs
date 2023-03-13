using System;
using UnityEditor;
using UnityEngine;
using UVNF.Core.Story;
using UVNF.Core.Story.Dialogue;
using UVNF.Editor.Settings;
using UVNF.Entities.Containers;
using UVNF.Extensions;
using XNode;
using XNodeEditor;

namespace UVNF.Editor.Story.Nodes
{
    [CustomNodeGraphEditor(typeof(StoryGraph))]
    public class StoryGraphEditor : NodeGraphEditor
    {
        public override void OnOpen()
        {
            base.OnOpen();
        }

        public override string GetNodeMenuName(Type type)
        {
            if (type.BaseType == typeof(Node) || type.IsSubclassOf(typeof(Node)))
            {
                if (type.IsSubclassOf(typeof(StoryElement)))
                {
                    StoryElement element = ScriptableObject.CreateInstance(type) as StoryElement;
                    string returnString = element.Type.ToString() + "/" + type.Name.Replace("Element", "");
                    UnityEngine.Object.DestroyImmediate(element);
                    return returnString;
                }
                else
                {
                    return base.GetNodeMenuName(type).Replace("Node", "");
                }
            }
            else return null;
        }

        public override void OnGUI()
        {
            base.OnGUI();
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 2)
            {
                CreateNode(typeof(DialogueElement), window.WindowToGridPosition(Event.current.mousePosition).OffsetY(20));
            }
            else if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Space)
            {
                GenericMenu menu = new GenericMenu();
                AddContextMenuItems(menu);
                menu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
            }
        }

        public override Node CreateNode(Type type, Vector2 position)
        {
            Node node = base.CreateNode(type, position);
            if (node is StoryElement element)
            {
                element.OnCreate(UVNFEditorSettings.Instance.MainResources);
            }

            return node;
        }
    }
}