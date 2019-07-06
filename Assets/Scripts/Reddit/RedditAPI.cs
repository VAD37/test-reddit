using System;
using System.Collections.Generic;
using UniRx;
using UniRx.WebRequest;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace Reddit
{
    /// <summary>
    /// For convenient than practical use
    /// </summary>
    public static partial class RedditConfig
    {
        public static readonly Dictionary<string, string> PostHeader = new Dictionary<string, string>() {
            {"Authorization", "value set by new RedditApi(token) "},
            {"User-Agent", "window:test.installed.app:v0.1 (by /u/GottaHateMyself)"},
            {"X-Ratelimit-Remaining",""}
        };

        public static string redditUrl = "https://oauth.reddit.com";


        public static UnityWebRequest GetBasicGetRequest(string api) {
            var request = UnityWebRequest.Get(redditUrl + api);
            foreach (var header in PostHeader) { request.SetRequestHeader(header.Key, header.Value); }
            return request;
        }

        public static UnityWebRequest GetBasicPostRequest(string api, WWWForm form) {
            var request = UnityWebRequest.Post(redditUrl + api, form);
            foreach (var header in PostHeader) { request.SetRequestHeader(header.Key, header.Value); }

            return request;
        }

        public static WWWForm GetBasicForm() {
            var form = new WWWForm();
            foreach (var h in PostHeader) { form.AddField(h.Key, h.Value); }
            return form;
        }
    }

    //Serializable so unity not reset this value every reload
    [Serializable]
    public class RedditApi
    {
        [SerializeField] //Serialize to show inspector
        private string accessToken;

        public RedditApi(string token) {
            accessToken                              = token;
            RedditConfig.PostHeader["Authorization"] = "bearer " + token;
        }

        public void GetUserInfo(Action<string> onSuccess) {
            var request = RedditConfig.GetBasicGetRequest("/api/v1/me");
            request.ToObservable()
                   .Subscribe(onSuccess, PrintError);
        }


        /// <summary>
        /// result look like this {"jquery": [[0, 1, "call", ["body"]], [1, 2, "attr", "find"], [2, 3, "call", [".status"]], [3, 4, "attr", "hide"], [4, 5, "call", []], [5, 6, "attr", "html"], [6, 7, "call", [""]], [7, 8, "attr", "end"], [8, 9, "call", []], [1, 10, "attr", "redirect"], [10, 11, "call", ["https://www.reddit.com/r/test/comments/c9iqck/test_title/"]], [1, 12, "attr", "find"], [12, 13, "call", ["*[name=url]"]], [13, 14, "attr", "val"], [14, 15, "call", [""]], [15, 16, "attr", "end"], [16, 17, "call", []], [1, 18, "attr", "find"], [18, 19, "call", ["*[name=text]"]], [19, 20, "attr", "val"], [20, 21, "call", [""]], [21, 22, "attr", "end"], [22, 23, "call", []], [1, 24, "attr", "find"], [24, 25, "call", ["*[name=drafts_count]"]], [25, 26, "attr", "val"], [26, 27, "call", [0]], [27, 28, "attr", "end"], [28, 29, "call", []], [1, 30, "attr", "find"], [30, 31, "call", ["*[name=title]"]], [31, 32, "attr", "val"], [32, 33, "call", [" "]], [33, 34, "attr", "end"], [34, 35, "call", []]], "success": true}
        /// </summary>
        /// <param name="onSuccess"></param>
        public void MakeNewPost(Action<string> onSuccess) {
            var form = RedditConfig.GetBasicForm();
            form.AddField("sr",   "u_GottaHateMyself");
            form.AddField("title", "test title");
            form.AddField("kind", "self");
            form.AddField("text", "test text with API token 2");
            var request = RedditConfig.GetBasicPostRequest("/api/submit", form);
            request.ToObservable()
                   .Subscribe(onSuccess, PrintError);
        }
        
        public void EditTextPost(string url,string subreddit, string title ,string bodyText,Action<string> onSuccess) {
            var form = RedditConfig.GetBasicForm();
            form.AddField("sr", subreddit);
            form.AddField("title", title);
            form.AddField("kind",  "link");
            form.AddField("url", url);
            form.AddField("text", bodyText);
            form.AddField("resubmit", "true");
            var request = RedditConfig.GetBasicPostRequest("/api/submit", form);
            request.ToObservable()
                   .Subscribe(onSuccess, PrintError);
        }


        void PrintError(Exception ex) { Debug.LogError(ex); }
    }
}