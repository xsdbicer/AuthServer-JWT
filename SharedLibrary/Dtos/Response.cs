using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class Response<T> where T: class
    {
        public T Data { get; set; }
        public int StatusCode { get; private set; }
        public ErrorDto Error { get; set; }
        [JsonIgnore]
        public bool isSuccesfull { get; set; }

        public static Response<T> Success(T data, int statusCode) { 
            return new Response<T>() { StatusCode = statusCode, Data = data, isSuccesfull=true };
        }

        public static Response<T> Success(int statusCode) { 
            return new Response<T>() { Data = default, StatusCode = statusCode, isSuccesfull=true }; 
        }

        public static Response<T> Fail(int statusCode, ErrorDto errorDto) {
            return new Response<T>() {  Error = errorDto, StatusCode = statusCode, isSuccesfull=false };
        }


        public static Response<T> Fail(string errorMessage,int statusCode,bool isShow)
        {
            var errorDto= new ErrorDto(errorMessage,isShow);

            return new Response<T>() { Error=errorDto, StatusCode = statusCode,isSuccesfull=false };
        }

    }
}
