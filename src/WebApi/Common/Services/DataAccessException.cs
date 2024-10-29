using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class DataAccessException : Exception
    {
        public DataAccessException()
        {
        }

        public DataAccessException(
            string? message) : base(
                message)
        {
        }

        public DataAccessException(
            string? message,
            Exception? innerException) : base(
                message,
                innerException)
        {
        }

        public DataAccessException(
            HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
    }
}
