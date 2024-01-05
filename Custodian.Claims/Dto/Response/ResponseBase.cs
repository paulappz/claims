using System.Net;

namespace Custodian.Claims.Dto.Response
{
	public class ResponseBase<T> 
	{
		public T? Data { get; set; }
		public int StatusCode { get; set; }
		public bool IsSuccessful { get; set; }
		public string? Message { get; set; }


		public static ResponseBase<T> Success(T data, HttpStatusCode statusCode = HttpStatusCode.OK, string message = "Success")
		{
			return new()
			{
				Data = data,
				StatusCode = (int)statusCode,
				Message = message,
				IsSuccessful = true
			};
		}

		public static ResponseBase<T> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
		{
			return new()
			{
				StatusCode = (int)statusCode,
				Message = message,
			};
		}
	}
}
