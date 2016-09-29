

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace PontoPortaria1510Tests
{
    static class CustomAsserts
    {
        public static void AreSameTime(IEnumerable<DateTime> date1, IEnumerable<DateTime> date2)
        {
            var enum1 = date1.GetEnumerator();
            var enum2 = date2.GetEnumerator();
            enum1.Reset();
            enum2.Reset();
            while (true)
            {
                if (enum1.MoveNext())
                {
                    if (!enum2.MoveNext())
                        Assert.Fail("Tamanho diferente");
                    if(enum1.Current.ToString("HH:mm") != enum2.Current.ToString("HH:mm"))
                    {
                        Assert.Fail($"{enum1.Current.ToString("HH:mm")} diferente de {enum2.Current.ToString("HH:mm")}");
                    }
                }
                else if (enum2.MoveNext())
                {
                    Assert.Fail("Tamanho diferente");
                }else
                {
                    return;
                }
            }
        }
    }
}
