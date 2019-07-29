using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestHelper;

namespace SwitchCaseStringAnalyzer.Test
{
    [TestClass]
    public class UnitTest : DiagnosticVerifier
    {

        // No diagnostics expected to show up
        [TestMethod]
        public void TestMethod1()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            public void Test1()
            {
                var g = 1;
                switch(g) { }
            }
        }
    }";

            VerifyCSharpDiagnostic(test);
        }

        // Diagnostic triggered and warning reported
        [TestMethod]
        public void TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            public void Test1()
            {
                var g = " + "\"test\"" + @";
                switch(g) { }
            }
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "SwitchCaseStringAnalyzer",
                Message = "Using strings in switch case is usually a sign that you want to use an enum.",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 16, 17)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new SwitchCaseStringAnalyzerAnalyzer();
        }
    }
}
