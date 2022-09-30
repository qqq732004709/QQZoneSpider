
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace QQZoneSpider;

public class QQZoneHelper
{
    public static string Cookie { get; set; }

    public static string QQ { get; set; }

    public static string Tk { get; set; }

    public static string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.81 Safari/537.36 Edg/94.0.992.47";

    public QQZoneHelper(string cookie)
    {
        Cookie = cookie;
        GetGtk();
    }

    public static void Login()
    {
        var login_url = "https://ssl.ptlogin2.qq.com/ptqrshow?appid=549000912&e=2&l=M&s=3&d=72&v=4&t=0." +
                        string.Format("{0}6252926{1}2285{2}86&daid=5", new Random().Next(0, 9), new Random().Next(0, 9), new Random().Next(0, 9));

        var cookies = @"version=0, name='_qz_referrer', value='qzone.qq.com', port=None,
            port_specified = False,
            domain = 'qq.com',
            domain_specified = False, domain_initial_dot = False, path = '/', path_specified = True,
            secure = False, expires = None, discard = True, comment = None, comment_url = None,
            rest ={ 'HttpOnly': None}, rfc2109 = False";
        HttpWebRequest? request = null;

        try
        {
            request = WebRequest.Create(login_url) as HttpWebRequest;
            request.AllowAutoRedirect = true;
            request.Headers["Host"] = "ssl.ptlogin2.qq.com";
            request.Headers["Referer"] = "https://qzone.qq.com/";
            request.Timeout = 2000;
            request.Headers["Cookie"] = cookies;
            request.Headers["Content-Type"] = "application/x-www-form-urlencoded;charset=UTF-8";
            request.UserAgent = @"Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";
            using (WebResponse webResponse = request.GetResponse())
            {
                var json = JsonConvert.SerializeObject(webResponse);

                var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static string PostMsg(string msg)
    {
        var body =
            $@"content={msg}&hostUin={QQ}&uin={QQ}&format=fs&inCharset=utf-8&outCharset=utf-8&iNotice=1&ref=qzone
&json=1&g_tk={Tk}&qzreferrer=https%3A%2F%2Fuser.qzone.qq.com%2Fproxy%2Fdomain%2Fqzs.qq.com%2Fqzone%2Fmsgboard%2Fmsgbcanvas.html%23page%3D2";

        var url = @$"https://h5.qzone.qq.com/proxy/domain/m.qzone.qq.com/cgi-bin/new/add_msgb?&g_tk={Tk}";
        var request = InitRequest(url);
        var data = Encoding.UTF8.GetBytes(body);
        request.Method = "POST";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        var response = (HttpWebResponse)request.GetResponse();
        string responseBody;
        using (Stream stream = response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                responseBody = reader.ReadToEnd();
            }
        }

        return responseBody;
    }

    public static string PostReplyMsg(string msgId, string msg)
    {
        var body =
            $@"hostUin={QQ}&msgId={msgId}&format=fs&content={msg}&uin={QQ}&iNotice=1&inCharset=utf-8&outCharset=utf-8&ref=qzone&json=1&g_tk={Tk}
&qzreferrer=https%3A%2F%2Fuser.qzone.qq.com%2Fproxy%2Fdomain%2Fqzs.qq.com%2Fqzone%2Fmsgboard%2Fmsgbcanvas.html%23page%3D1";

        var url = @$"https://h5.qzone.qq.com/proxy/domain/m.qzone.qq.com/cgi-bin/new/add_reply?&g_tk={Tk}";
        var request = InitRequest(url);
        var data = Encoding.UTF8.GetBytes(body);
        request.Method = "POST";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        var response = (HttpWebResponse)request.GetResponse();
        string responseBody;
        using (Stream stream = response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                responseBody = reader.ReadToEnd();
            }
        }

        return responseBody;
    }

    public static ApiResult GetMsgs(int num, int start)
    {
        var url =
            $@"https://user.qzone.qq.com/proxy/domain/m.qzone.qq.com/cgi-bin/new/get_msgb?uin={QQ}&hostUin={QQ}&num={num}&start={start}&hostword=0&essence=1&r=0.7681618785673934&iNotice=0&inCharset=utf-8&outCharset=utf-8&format=jsonp&ref=qzone&g_tk={Tk}&g_tk={Tk}";

        var request = InitRequest(url);
        request.Method = "GET";

        var response = (HttpWebResponse)request.GetResponse();
        string responseBody;
        using (Stream stream = response.GetResponseStream())
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                responseBody = reader.ReadToEnd();
            }
        }

        var result = GetResponseMsg(responseBody);

        return result;

    }

    public static HttpWebRequest InitRequest(string url)
    {
        var request = (HttpWebRequest)WebRequest.Create(url);
        request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
        request.Headers["authority"] = "h5.qzone.qq.com";
        request.UserAgent = UserAgent;
        request.Referer = "https://user.qzone.qq.com/";
        request.Headers["origin"] = "https://user.qzone.qq.com";
        request.Headers["cookie"] = Cookie;
        request.Headers["accept-language"] = "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6";

        return request;
    }

    public static void GetGtk()
    {
        int h = 5381;

        var index1 = Cookie.IndexOf("p_skey", StringComparison.Ordinal) + 7;
        var index2 = Cookie.IndexOf(';', index1);
        var pskey = Cookie.Substring(index1, index2 - index1);

        foreach (var c in pskey)
        {
            h += (h << 5) + (int)c;
        }

        Tk = (h & 2147483647).ToString();
    }

    public static List<CommentListItem> GetAllMsgs()
    {
        var result = new List<CommentListItem>();

        var isStop = false;
        var pageIndex = 0;
        var pageSize = 10;

        while (true)
        {
            if (isStop)
            {
                break;
            }

            var apiResult = GetMsgs(pageSize, pageIndex * pageSize);
            result.AddRange(apiResult.Data.CommentList);

            if (apiResult.Code != 0 || apiResult.Data.CommentList.Count < pageSize)
            {
                isStop = true;
            }

            pageIndex++;
        }

        return result;
    }

    public static Dictionary<string, int> StaticKeyWord(List<CommentListItem> commentListItems)
    {

        var comments = commentListItems.Select(c => c.UbbContent).ToList();

        return new Dictionary<string, int>();
    }

    public static string SearchMsg(string keyword)
    {
        for (int i = 0; i < 10; i++)
        {
            var msg = GetMsgs(20, i);
        }

        return "";
    }

    private static ApiResult GetResponseMsg(string responseMsg)
    {
        var pa = @$"(?<=ubbContent"":"")[\s\S]*(?<="",
            ""signature"")";
        string PATTERN = @"(?<=commentList"":)[\{[\s\S]*\]";
        string PATTERN2 = @"(?<=_Callback\()[\{[\s\S]*\}";

        var matched = Regex.Match(responseMsg, PATTERN2).Value;

        var result = JsonConvert.DeserializeObject<ApiResult>(matched);

        foreach (var msg in result.Data.CommentList)
        {
            msg.UbbContent = msg.UbbContent.Replace("\n", " ");
        }
        return result;
    }
}

