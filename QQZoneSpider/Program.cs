using QQZoneSpider;
using System.Text;

Console.WriteLine("请输入Cookie:\n");
var cookie = Console.ReadLine();
var api = new QQZoneHelper(cookie);

while (true)
{
    Console.WriteLine("输入操作 5-退出");
    int flag = Convert.ToInt32(Console.ReadLine());
    switch (flag)
    {
        case 1:
            Console.WriteLine("num:");
            int num = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("start:");
            int start = Convert.ToInt32(Console.ReadLine());

            var result = QQZoneHelper.GetMsgs(num, start);
            if (result.Message.Contains("未登入"))
            {
                Console.WriteLine(result.Message);
                break;
            }

            var comments = new StringBuilder();
            foreach (var commentListItem in result.Data.CommentList)
            {
                var comment =
                    $"Id:{commentListItem.Id} Content:{commentListItem.UbbContent} PubTime:{commentListItem.Pubtime} \r\n ";

                comments.Append(comment);
            }

            Console.WriteLine(comments.ToString());
            break;

        case 2:
            Console.WriteLine("留言内容:\n");
            var txt = new StringBuilder();
            while (true)
            {
                var text = Console.ReadLine();
                if (text.Equals("quit"))
                {
                    break;
                }
                txt.Append(text + "\n");
            }
            Console.WriteLine(QQZoneHelper.PostMsg(txt.ToString()));
            break;
        case 3:
            Console.WriteLine("MsgId:");
            string msgId = Console.ReadLine();
            Console.WriteLine("Content:");
            string content = Console.ReadLine();
            Console.WriteLine(QQZoneHelper.PostReplyMsg(msgId, content));
            break;
        case 4:
            Console.WriteLine("关键词:");
            string keyword = Console.ReadLine();
            Console.WriteLine(QQZoneHelper.SearchMsg(keyword));
            return;
        case 5:
            Console.WriteLine("Cookie");
            QQZoneHelper.Cookie = Console.ReadLine();
            break;
        default:
            Console.WriteLine("请输入规定操作");
            break;
    }
}