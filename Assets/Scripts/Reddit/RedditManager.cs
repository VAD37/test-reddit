using System;
using NaughtyAttributes;
using UnityEngine;

namespace Reddit
{
    public class RedditManager : MonoBehaviour
    {
        public RedditAuthorization authorization;

        public RedditApi redditApi;

        public string Token => authorization.accessToken;

        public UserMe user;

        public void InitReddit() {
            redditApi = new RedditApi(Token); // This get refresh by Editor Extension OnEnable for easy testing. During running This only called once
        }
        
        [Button()]
        void TestGetUserInfo() {
            redditApi.GetUserInfo(
                s => {
                    Debug.Log(s);
                    user = UserMe.FromJson(s);
                }
            );
        }

        [Button()]
        void TestResubmitPost() { redditApi.EditTextPost("https://www.reddit.com/r/test/comments/c9iqck/test_title/","test", "title resubmmit", "body text re submmit" ,Debug.Log); }

        [Button()]
        void TestMakeNewPost() { redditApi.MakeNewPost(s => { Debug.Log(s); }); }
    }
}