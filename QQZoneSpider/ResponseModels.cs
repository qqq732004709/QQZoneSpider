namespace QQZoneSpider
{
    #region Models

    public class ReplyListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Uin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Nick { get; set; }
    }

    public class CommentListItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Secret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Pasterid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Bmp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Pubtime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Modifytime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Effect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Uin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Nickname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? HtmlContent { get; set; }

        public string? UbbContent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Signature { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ReplyListItem>? ReplyList { get; set; }
    }

    public class ApiResult
    {
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Subcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Default { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data? Data { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int AuditNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? AuditON { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AuthorInfo? AuthorInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CommentListItem>? CommentList { get; set; }
    }

    public class AuthorInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? HtmlMsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Sign { get; set; }
    }
    #endregion

}
