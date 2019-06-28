using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
    public string appId     = "A3KES3pqsn9Ctw";
    public string appSecret = "3obD1OtkpjfNmcXSRQX_XD6mnOs";

    public int port = 21345;
    public TcpListener server;
    [Button()]
    public async Task StartListening() {
        Debug.Log("Starting...");
        
        var ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
        server = new TcpListener(ipAddress, port);
        server.Start();
        Debug.Log($"Started server. {server.LocalEndpoint}");
        while (true)
        {
            var client = await server.AcceptTcpClientAsync().ConfigureAwait(false);
            var cw     = new ClientWorking(client, true);
            await Task.Run((Func<Task>)cw.DoSomethingWithClientAsync);
            if (cw.done) break;
        }
        Debug.Log("End of task...");
    }
    
    [Button()]
    public async Task StartListening2() {
        Debug.Log("Starting...");
        
        var ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
        var server = new TcpListener(ipAddress, port);
        server.Start();
        Debug.Log($"Started server. {server.LocalEndpoint}");
        while (true)
        {
            var client = await server.AcceptTcpClientAsync().ConfigureAwait(false);
            var cw     = new ClientWorking(client, true);
            await Task.Run((Func<Task>)cw.DoSomethingWithClientAsync);
            if (cw.done) break;
        }
    }
    
    class ClientWorking
    {
        TcpClient _client;
        bool      _ownsClient;
        public bool done;
        public ClientWorking(TcpClient client, bool ownsClient)
        {
            _client     = client;
            _ownsClient = ownsClient;
        }

        public async Task DoSomethingWithClientAsync()
        {
            try
            {
                using (var stream = _client.GetStream())
                {
                    using (var sr = new StreamReader(stream))
                    using (var sw = new StreamWriter(stream)) {
                        sw.AutoFlush = true;
                        await sw.WriteLineAsync("Hi. This is x2 TCP/IP easy-to-use server").ConfigureAwait(false);
                        await sw.FlushAsync().ConfigureAwait(false);
                        var data = default(string);
                        while (!String.IsNullOrEmpty(data = await sr.ReadLineAsync().ConfigureAwait(false))) {
                            if(data.Contains("GET"))
                                Debug.Log(data.Split(' ')[1]);
                            await sw.WriteLineAsync(data).ConfigureAwait(false);
                            await sw.FlushAsync().ConfigureAwait(false);
                        }
                        Debug.Log("Exit from while loop");
                    }

                }
            }
            finally
            {
                if (_ownsClient && _client != null)
                {
                    (_client as IDisposable).Dispose();
                    _client = null;
                }
            }
        }
    }

    [Button()]
    public void StopListening() {
        server.Stop();
    }

    // [Serializable]
    // public class Headers
    // {
    //     public string key;
    //     public string value;
    // }
    //
    // [SerializeField]
    // public List<Headers> header = new List<Headers>() {
    //     new Headers() {key = "client_id"},
    //     new Headers() {key = "response_type"},
    //     new Headers() {key = "state"},
    //     new Headers() {key = "redirect_uri"},
    //     new Headers() {key = "duration"},
    //     new Headers() {key = "scope"}
    // };
    //
    // public BotWebAgent BotWebAgent;
    // public string                  AccessToken;
    //
    // [Button]
    // void TestLogin() {
    //     BotWebAgent = new RedditSharp.BotWebAgent
    //     ("GottaHateMySelf",
    //      "dungto1997",
    //      "A3KES3pqsn9Ctw",
    //      "3obD1OtkpjfNmcXSRQX_XD6mnOs",
    //      "http://127.0.0.1");
    //    var auth = new AuthProvider("A3KES3pqsn9Ctw", "3obD1OtkpjfNmcXSRQX_XD6mnOs", "http://127.0.0.1", BotWebAgent);
    //     AccessToken = BotWebAgent.AccessToken;
    //     
    //
    //     // string base_url = "https://www.reddit.com/api/v1/authorize?";
    //     // var    sb  = new System.Text.StringBuilder(base_url);
    //     // foreach (var i in header) {
    //     //     sb.Append($"{i.key}={i.value}&"); 
    //     //     if(i.key == "state")i.value = System.Guid.NewGuid().ToString();
    //     // }
    //     //
    //     // sb.Remove(sb.Length - 1, 1);
    //     // var uri = sb.ToString();
    //     // TextEditor te      = new TextEditor() {text = uri};
    //     // te.SelectAll();
    //     // te.Copy();
    //     //
    //     // var request = UnityWebRequest.Get(uri);
    //     //
    //     // Dictionary<string,string> dict  = new Dictionary<string, string>();
    //     // foreach (var h in header) {
    //     //     dict.Add(h.key,h.value);
    //     // }
    //     //
    //     // //var request = UnityWebRequest.Post(base_url,dict);
    //     // //Observable.FromCoroutine<UnityWebRequest>(TestAsync).Subscribe();
    //     // // request.ToObservable().CatchIgnore((UnityWebRequestErrorException x)
    //     // //                                        =>Debug.LogError(x.RawErrorMessage))
    //     // //        .Subscribe(success => Debug.Log(success), 
    //     // //                   error => Debug.LogError(error.Message)
    //     // //        );
    //     // StartCoroutine(TestAsync(request));
    //     // IEnumerator TestAsync(UnityWebRequest www) {
    //     //     yield return www.SendWebRequest();
    //     //     while (!www.isDone) yield return null;
    //     //     var x = 20;
    //     //     while (x > 0) {
    //     //         Debug.Log(www.downloadHandler.text);
    //     //         x--;
    //     //         yield return new WaitForSeconds(1);
    //     //     }
    //     // }
    //     // Debug.Log(request.url);
    //     // Application.OpenURL(request.url);
    // }
    //
    // public string subreddit;
    //
    //
    //
    // [Button]
    // void TestPost() {
    //
    //     SubmitPost();
    //     
    //     async void SubmitPost()
    //     {
    //         RedditSharp.Reddit   reddit = new RedditSharp.Reddit(BotWebAgent,true);
    //
    //         var sub  = await reddit.GetSubredditAsync(subreddit);
    //         var user = await reddit.GetUserAsync("GottaHateMySelf");
    //         
    //         await user.GetPosts(10).ForEachAsync(page => {
    //             Debug.Log(page.Title);
    //         });
    //         
    //         
    //         
    //         // await sub.SubmitPostAsync("Test post title","uri");
    //
    //         //var post = await sub.SubmitPostAsync("ThisIsASubmittedPost", "https://github.com/CrustyJew/RedditSharp/issues/76", resubmit: true);
    //         //await post.DelAsync();
    //     }
    // }
}

     //
     // public class SimpleHTTPServerComponent : MonoBehaviour
     // {
     //         SimpleHTTPServer myServer;
     //         public string FirstIndexPath = "";
     //
     //         public void StartServer()
     //         {
     //                 myServer = new SimpleHTTPServer(Path.Combine(Application.streamingAssetsPath, "App"));
     //                 Application.OpenURL("http:/localhost:" + myServer.Port + "/" + FirstIndexPath);
     //         }
     //
     //         public void StopServer()
     //         {
     //                 Application.Quit();
     //         }
     //
     //         void OnApplicationQuit()
     //         {
     //                 myServer.Stop();
     //         }
     //
     //
     //         class SimpleHTTPServer
     //         {
     //                 private readonly string[] _indexFiles =
     //                         { 
     //                                 "index.html", 
     //                                 "index.htm", 
     //                                 "default.html", 
     //                                 "default.htm" 
     //                         };
     //                 private Thread _serverThread;
     //                 private HttpListener _listener;
     //                 private int _port;
     //
     //                 public int Port
     //                 {
     //                         get { return _port; }
     //                         private set { }
     //                 }
     //
     //                 /// <summary>
     //                 /// Construct server with given port.
     //                 /// </summary>
     //                 /// <param name="path">Directory path to serve.</param>
     //                 /// <param name="port">Port of the server.</param>
     //                 public SimpleHTTPServer(string path, int port)
     //                 {
     //                         this.Initialize(path, port);
     //                 }
     //
     //                 /// <summary>
     //                 /// Construct server with suitable port.
     //                 /// </summary>
     //                 /// <param name="path">Directory path to serve.</param>
     //                 public SimpleHTTPServer(string path)
     //                 {
     //                         //get an empty port
     //                         TcpListener l = new TcpListener(IPAddress.Loopback, 0);
     //                         l.Start();
     //                         int port = ((IPEndPoint)l.LocalEndpoint).Port;
     //                         l.Stop();
     //                         this.Initialize(path, port);
     //                 }
     //
     //                 /// <summary>
     //                 /// Stop server and dispose all functions.
     //                 /// </summary>
     //                 public void Stop()
     //                 {
     //                         _serverThread.Abort();
     //                         _listener.Stop();
     //                 }
     //
     //                 private void Listen()
     //                 {
     //                         _listener = new HttpListener();
     //                         _listener.Prefixes.Add("http://*:" + _port.ToString() + "/");
     //                         _listener.Start();
     //                         while (true)
     //                         {
     //                                 try
     //                                 {
     //                                         HttpListenerContext context = _listener.GetContext();
     //                                 } catch (Exception ex)
     //                                 {
     //                                         print(ex);
     //                                 }
     //                         }
     //                 }
     //
     //
     //                 private void Initialize(string path, int port)
     //                 {
     //                         this._rootDirectory = path;
     //                         this._port = port;
     //                         _serverThread = new Thread(this.Listen);
     //                         _serverThread.Start();
     //                 }
     //
     //
     //         }
     // }