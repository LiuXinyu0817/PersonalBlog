namespace PersonalBlog.WebApi.utils.ApiResult
{
    public class ApiResult
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
        public int Total { get; set; }
    }
}
