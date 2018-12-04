using System;
using System.Collections.Generic;
using System.Text;

namespace DRS.Data.BusinessEntities
{
    public class PagedResult
    {
        public List<Object> Result { get; set; } = new List<object>();
        public Int64 Total { get; set; } = 0;
        public ErrorWrapper Error { get; set; } = new ErrorWrapper();
    }
    public class ErrorWrapper
    {
        public bool IsError { get; set; } = false;
        public string Message { get; set; } = String.Empty;
        public string Stacktrace { get; set; } = String.Empty;
    }
}
