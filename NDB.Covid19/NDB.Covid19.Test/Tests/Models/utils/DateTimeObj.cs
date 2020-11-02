using System;
using System.Collections;
using System.Collections.Generic;

namespace NDB.Covid19.Test.Tests.Models.utils
{
    class DateTimeObj : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new DateTime(2020,04,03),
            };
            yield return new object[]
            {
                 DateTime.Now,
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
