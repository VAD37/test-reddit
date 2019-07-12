using System;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string textToShow;
    

    public RectTransform rect;
    
    private SingleAssignmentDisposable disposable;
    
    [Button()]
    public void OnPointerEnter(PointerEventData eventData) {
        disposable = Observable.Timer(TimeSpan.FromSeconds(2.33f)).Subscribe(
            x => {
                Debug.Log("Show hover info text"); 
            }) as SingleAssignmentDisposable; // Timer is SingleAssignmentDisposable when UniRx run.
    }

    public void OnPointerExit(PointerEventData eventData) {
        disposable.Dispose();
    }

    //OnPointerExit not update when object is destroyed. Some script will watch if disposable is gone each frame.
    private void OnDestroy() { disposable.Dispose(); }
}