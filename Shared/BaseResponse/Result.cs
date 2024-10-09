﻿namespace Shared.BaseResponse
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error { get; set; }
        public string Message { get; set; }
        public string ResponseCode { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="error"></param>
        /// <exception cref="InvalidOperationException"></exception>
        protected Result(bool isSuccess, string error)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException();
            IsSuccess = isSuccess;
            Error = error;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="error"></param>
        /// <param name="responseCode"></param>
        /// <exception cref="InvalidOperationException"></exception>
        protected Result(bool isSuccess, string error, string responseCode)
        {
            if (isSuccess && !string.IsNullOrEmpty(error))
                throw new InvalidOperationException();
            IsSuccess = isSuccess;
            Error = error;
            ResponseCode = responseCode;

        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message, T t)
        {
            return new Result<T>(t, false, string.Empty) { Message = message };
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }

        public static Result<T> Fail<T>(string message, string responseCode)
        {
            var response = new Result<T>(default(T), false, message, responseCode);
            response.Message = message;
            return response;
        }

        public static Result<T> Fail<T>(T value, string message, string responseCode)
        {
            var response = new Result<T>(value, false, message, responseCode);
            response.Message = message;
            return response;
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result Ok(string message)
        {
            return new Result(true, string.Empty) { Message = message };
        }

        public static Result Ok(object value, string message)
        {
            return new Result<object>(value, true, string.Empty) { Message = message };
        }
        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result<T> Ok<T>(T value, string message)
        {
            return new Result<T>(value, true, string.Empty) { Message = message };
        }
        public static Result Combine(params Result[] results)
        {
            foreach (var result in results)
            {
                if (!result.IsSuccess)
                    return result;
            }
            return Ok();
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;
        public T Value
        {
            get
            {
                return _value;
            }

        }

        protected internal Result(T value, bool isSuccess, string error) : base(isSuccess, error)
        {
            _value = value;
        }

        protected internal Result(T value, bool isSuccess, string error, string responseCode) : base(isSuccess, error, responseCode)
        {
            _value = value;
        }
    }
}
