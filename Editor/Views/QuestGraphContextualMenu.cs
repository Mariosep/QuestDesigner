using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuestDesigner.Editor
{
    public static class QuestGraphContextualMenu
    {
        enum GraphMenuActions
        {
            CreateNode,
            Separator1,
            Cut,
            Copy,
            Paste,
            Separator2,
            Delete,
            Separator3,
            Duplicate
        }

        enum NodeMenuActions
        {
            DisconnectAll,
            Separator1,
            Cut,
            Copy,
            Separator2,
            Delete,
            Separator3,
            Duplicate
        }

        public static void Populate(QuestGraphView questGraphView, ContextualMenuPopulateEvent evt)
        {
            if (evt.target is GraphView)
            {
                SetGraphViewContextualMenu(questGraphView, evt);
            }
            else if (evt.target is NodeView)
            {
                SetNodeViewContextualMenu(questGraphView, evt);
            }
        }

        private static void SetGraphViewContextualMenu(QuestGraphView questGraphView, ContextualMenuPopulateEvent evt)
        {
            evt.menu.RemoveItemAt((int)GraphMenuActions.Duplicate);
            evt.menu.RemoveItemAt((int)GraphMenuActions.Separator3);
            evt.menu.RemoveItemAt((int)GraphMenuActions.Paste);

            DropdownMenuAction.Status status = questGraphView.nodesToCopy.Count > 0
                ? DropdownMenuAction.Status.Normal
                : DropdownMenuAction.Status.Disabled;

            evt.menu.InsertAction(
                (int)GraphMenuActions.Paste,
                "Paste",
                (actionEvent) =>
                {
                    Vector2 position = questGraphView.GetLocalMousePosition(actionEvent.eventInfo.localMousePosition);
                    questGraphView.PasteNode(position);
                }, status);


            evt.menu.RemoveItemAt((int)GraphMenuActions.Cut);
        }

        private static void SetNodeViewContextualMenu(QuestGraphView questGraphView, ContextualMenuPopulateEvent evt)
        {
            evt.menu.RemoveItemAt((int)NodeMenuActions.Duplicate);
            evt.menu.RemoveItemAt((int)NodeMenuActions.Separator3);
            evt.menu.RemoveItemAt((int)NodeMenuActions.Cut);
        }
    }
}