using NaughtyAttributes.Editor;
using UnityEditor;

namespace Reddit
{
    [CustomEditor(typeof(RedditManager))]
    public class RedditManagerEditor : InspectorEditor
    {
        //For quick test authentication in editor or when refresh editor during dev
        protected override void OnEnable() {
            base.OnEnable();
            var reddit = this.target as RedditManager;
            if (reddit                             != null &&
                reddit.authorization               != null &&
                string.IsNullOrEmpty(reddit.Token) == false) { reddit.InitReddit(); }
        }
    }
}