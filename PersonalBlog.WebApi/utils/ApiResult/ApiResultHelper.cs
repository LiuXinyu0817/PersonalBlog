namespace PersonalBlog.WebApi.utils.ApiResult
{
    public static class ApiResultHelper
    {
        public static ApiResult Success(dynamic data = null, int total = 0)
        {
            return new ApiResult
            {
                Code = 200,
                Message = "操作成功",
                Data = data,
                Total = total
            };
        }

        public static ApiResult Failed(string message = "操作失败")
        {
            return new ApiResult
            {
                Code = 500,
                Message = message,
                Data = null
            };
        }
    }
}