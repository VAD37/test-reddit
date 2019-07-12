using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class PostBox : MonoBehaviour
{
    public TMP_InputField title;
    public TMP_InputField bodyText;
    public Button submitButton;
    public PostStatus     postStatus;

    private void Start() {
        postStatus = new PostStatus();
        title.OnEndEditAsObservable().Subscribe((s) => { postStatus.IsDirty.Value = true; });

        postStatus.IsSubmitted.AsObservable().Subscribe(WhenPostSubmitted);
    }

    private void WhenPostSubmitted(bool s) {
        Debug.Log("post have submitted " + s);
    }

    [Serializable]
    public class PostStatus
    {
        public BoolReactiveProperty IsSubmitted;
        public BoolReactiveProperty IsDirty;

        public PostStatus() {
            IsSubmitted = new BoolReactiveProperty(false);
            IsDirty     = new BoolReactiveProperty(false);
        }
    }
}