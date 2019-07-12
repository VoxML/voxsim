﻿#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Converters;
#if DNXCORE50
using Xunit;
using Test = Xunit.FactAttribute;
using Assert = Newtonsoft.Json.Tests.XUnitAssert;
#else
using NUnit.Framework;

#endif

namespace Newtonsoft.Json.Tests.Documentation.Samples.Serializer
{
    [TestFixture]
    public class JsonConverterAttributeProperty : TestFixtureBase
    {
        #region Types
        public enum UserStatus
        {
            NotConfirmed,
            Active,
            Deleted
        }

        public class User
        {
            public string UserName { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public UserStatus Status { get; set; }
        }
        #endregion

        [Test]
        public void Example()
        {
            #region Usage
            User user = new User
            {
                UserName = @"domain\username",
                Status = UserStatus.Deleted
            };

            string json = JsonConvert.SerializeObject(user, Formatting.Indented);

            Console.WriteLine(json);
            // {
            //   "UserName": "domain\\username",
            //   "Status": "Deleted"
            // }
            #endregion

            StringAssert.AreEqual(@"{
  ""UserName"": ""domain\\username"",
  ""Status"": ""Deleted""
}", json);
        }
    }
}