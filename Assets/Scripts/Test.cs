using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NaughtyAttributes;
using RedditSharp;
using RedditSharp.Things;
using UniRx;
using UniRx.WebRequest;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

public class Test : MonoBehaviour
{
    public string appId     = "56Y3i0_w5IpckA";
    public string appSecret = "0r1pZk-RLm32pX7x72fYmrtXKss";

    [Serializable]
    public class Headers
    {
        public string key;
        public string value;
    }

    [SerializeField]
    public List<Headers> header = new List<Headers>() {
        new Headers() {key = "client_id"},
        new Headers() {key = "response_type"},
        new Headers() {key = "state"},
        new Headers() {key = "redirect_uri"},
        new Headers() {key = "duration"},
        new Headers() {key = "scope"}
    };

    public BotWebAgent BotWebAgent;
    public string                  AccessToken;

    [Button]
    void TestLogin() {
        BotWebAgent = new RedditSharp.BotWebAgent
        ("GottaHateMySelf",
         "dungto1997",
         "A3KES3pqsn9Ctw",
         "3obD1OtkpjfNmcXSRQX_XD6mnOs",
         "http://127.0.0.1");
       var auth = new AuthProvider("A3KES3pqsn9Ctw", "3obD1OtkpjfNmcXSRQX_XD6mnOs", "http://127.0.0.1", BotWebAgent);
        AccessToken = BotWebAgent.AccessToken;
        

        // string base_url = "https://www.reddit.com/api/v1/authorize?";
        // var    sb  = new System.Text.StringBuilder(base_url);
        // foreach (var i in header) {
        //     sb.Append($"{i.key}={i.value}&"); 
        //     if(i.key == "state")i.value = System.Guid.NewGuid().ToString();
        // }
        //
        // sb.Remove(sb.Length - 1, 1);
        // var uri = sb.ToString();
        // TextEditor te      = new TextEditor() {text = uri};
        // te.SelectAll();
        // te.Copy();
        //
        // var request = UnityWebRequest.Get(uri);
        //
        // Dictionary<string,string> dict  = new Dictionary<string, string>();
        // foreach (var h in header) {
        //     dict.Add(h.key,h.value);
        // }
        //
        // //var request = UnityWebRequest.Post(base_url,dict);
        // //Observable.FromCoroutine<UnityWebRequest>(TestAsync).Subscribe();
        // // request.ToObservable().CatchIgnore((UnityWebRequestErrorException x)
        // //                                        =>Debug.LogError(x.RawErrorMessage))
        // //        .Subscribe(success => Debug.Log(success), 
        // //                   error => Debug.LogError(error.Message)
        // //        );
        // StartCoroutine(TestAsync(request));
        // IEnumerator TestAsync(UnityWebRequest www) {
        //     yield return www.SendWebRequest();
        //     while (!www.isDone) yield return null;
        //     var x = 20;
        //     while (x > 0) {
        //         Debug.Log(www.downloadHandler.text);
        //         x--;
        //         yield return new WaitForSeconds(1);
        //     }
        // }
        // Debug.Log(request.url);
        // Application.OpenURL(request.url);
    }

    public string subreddit;

    
    
    [Button]
    void TestPost() {

        SubmitPost();
        
        async void SubmitPost()
        {
            RedditSharp.Reddit   reddit = new RedditSharp.Reddit(BotWebAgent,true);

            var sub  = await reddit.GetSubredditAsync(subreddit);
            var user = await reddit.GetUserAsync("GottaHateMySelf");
            
            await user.GetPosts(10).ForEachAsync(page => {
                Debug.Log(page.Title);
            });
            
            
            
            // await sub.SubmitPostAsync("Test post title","uri");

            //var post = await sub.SubmitPostAsync("ThisIsASubmittedPost", "https://github.com/CrustyJew/RedditSharp/issues/76", resubmit: true);
            //await post.DelAsync();
        }
    }
}