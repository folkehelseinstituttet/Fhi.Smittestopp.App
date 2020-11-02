using System;
using System.Collections;
using System.Collections.Generic;

namespace NDB.Covid19.Test.Tests.Models.utils
{
    class ExceptionObj : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new Exception()
            };
            
            yield return new object[]
            {
                new TimeoutException()
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
