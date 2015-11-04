namespace Lunggo.Framework.Http.Rest
{
    interface IRestResponse
    {

    }

    interface IRestResponse<T> : IRestResponse
    {
        T Data { get; set; }
    }
}
