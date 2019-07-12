using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NaughtyAttributes;
using Reddit;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class RedditAuthorization : MonoBehaviour
{
    public HeadersDictionary headersSettings = new HeadersDictionary() {
        {"client_id", "pKkVQjJwzk7Jrg"}, //installed app ID
        {"response_type", "token"},
        {"state", ""},
        {"redirect_uri", "http://localhost:21345/?/"}, //do not change this address
        {"duration", "temporary"},
        {"scope", "identity,edit,read,save,submit,modposts,mysubreddits,history"},
    };

    //Port to use for listening server after reddit redirect URI to localhost:port/reddit-token.
    public int listeningPort = 21345;

    private float resetTimer = float.MaxValue;
    
    public string AccessToken {
        get { return accessToken;}
        set {
            resetTimer = Time.time + 3500; //Reddit need to refresh token after 1 hour
            accessToken = value;
        }
    }
    
    [ReadOnly]
    public string accessToken;

    [Button()]
    public async void StartRedditAuthentication() {
        //Randomize state and change url to port you want.
        ConfigureStateAndRedirectUri();

        OpenBrowserForRedditAuth(headersSettings);

        var result  = await StartListeningForResponse();
        var uriData = result.GetHeadersFromUriData();
        //if (uriData == null) return;
        if (uriData.Contains("error")) Debug.LogError(uriData["error"]);
        
        AccessToken = uriData["access_token"];
        if(this.GetComponent<RedditManager>()!= null) GetComponent<RedditManager>().InitReddit();
        
        async Task<string> StartListeningForResponse() {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:{listeningPort}/");
            listener.Start();
            //HACK: Hard code file. tell browser to open this html file after get token key. So now this html file will tell unity what is token key.
            // Kinda shitty but it work. All thks to browser security.
            string textFile = Resources.Load<TextAsset>("magic_redirect_fragment_to_data").text;
            byte[] htmlWeb        = Encoding.UTF8.GetBytes(textFile);
            string getValue; // the value after /? in url
            while (true) {
                var context = await listener.GetContextAsync().ConfigureAwait(false);
                try {
                    HttpListenerRequest  request  = context.Request;
                    HttpListenerResponse response = context.Response;

                    response.ContentLength64 = htmlWeb.Length;
                    Stream output = response.OutputStream;
                    output.Write(htmlWeb, 0, htmlWeb.Length);
                    output.Close();
                    
                    // URl with /# symbol (fragment part of URI that not send to http) will be redirect using html code just to get fragments part.
                    // http://localhost:21345/?%2F=#access_token=214252035530-umX7LjfFWPvK7g82F0FaxrgE69k&token_type=bearer&state=6cc0633b-dda7-4f06-9408-0ae5a129fb04&expires_in=3600&scope=edit+history+identity+modposts+mysubreddits+read+save+submit
                    if (request.RawUrl.Contains("access_token")) {
                        //Debug.Log(request.RawUrl);
                        // Meaning we have a full url : http://localhost:21345/?access_token=token & stuff
                        //RawURL look like: /access_token=214252035530-GKCNDtDglx8_3vYakCPcLYed0Pw&token_type=bearer&state=2a7e89b8-2aad-488a-b59b-97702a94f859&expires_in=3600&scope=edit+history+identity+modposts+mysubreddits+read+save+submit
                        getValue = request.RawUrl.Replace("/", ""); // Remove 1st character
                        break;
                    }
                }
                catch (Exception ex) { Debug.LogError(ex); }
            }

            listener.Stop();
            listener.Close();
            return getValue;
        }
    }

    private void ConfigureStateAndRedirectUri() {
        headersSettings["state"] = Guid.NewGuid().ToString();
        headersSettings["redirect_uri"] =
            $"http://localhost:{listeningPort.ToString()}/?/"; //HACK IpAddress actually return [::1] and not localhost
    }

    //Only for case when user refuse token or error.
    // public void ReopenBrowser() { OpenBrowserForRedditAuth(headers); }

    public void OpenBrowserForRedditAuth(HeadersDictionary settings) {
        string base_url = "https://www.reddit.com/api/v1/authorize?";
        var    sb       = new System.Text.StringBuilder(base_url);
        var    header   = string.Join("&", settings.GetFormmatedHeadersList());
        var    uri      = sb.Append(header).ToString();
        Application.OpenURL(uri); 
    }

}