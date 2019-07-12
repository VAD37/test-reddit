using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class DragableUI : MonoBehaviour, IDragHandler
    {
        public void OnDrag(PointerEventData e) {
            var trans    = transform;
            var position = trans.position;
            var cam      = Camera.main;
            // Screen to world point return world point but offset is from top left not from center.
            // so you have to delete screen size /2f from position
            var screenOffsetWorldPosition = cam.ScreenToWorldPoint(new Vector3(0,0,cam.farClipPlane));
            var deltaPos                  = cam.ScreenToWorldPoint(new Vector3(e.delta.x, e.delta.y, cam.farClipPlane));
            // This kinda hack but it work. Allow object move the same distance as the mouse.
            position       += deltaPos - screenOffsetWorldPosition;
            trans.position =  position;
        }
    }
}