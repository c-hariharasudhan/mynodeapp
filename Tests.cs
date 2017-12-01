using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommentCleaner;
using CommentCleaner.Helpers;
using System.Text.RegularExpressions;

namespace CleanerTest
{
    [TestClass]
    public class CleanerTests
    {
        #region CSharp
        [TestMethod]
        public void Clean_ForCSharp_MultiLine_RemovesCommentedCodes()
        {
            string input = @"/* 123456 
    blah blah blah
    blah blah blah  and this is more This is not end of cmt /* some more */
    right here";
            var result = CommentCleanHelper.Clean(input, CommentPattern.CSharp);
            Assert.IsFalse(result.Contains("123456"));
        }

        [TestMethod]
        public void Clean_ForCSharp_SingleLine_RemovesCommentedCodes()
        {
            string input = @"//123456 
    This is test";
            var result = CommentCleanHelper.Clean(input, CommentPattern.CSharp);
            Assert.IsFalse(result.Contains("123456"));
        }
        [TestMethod]
        public void Clean_ForCSharp_Mixed_RemovesCommentedCodes()
        {

            string input = @"var s = 100;//123456
    Multiline /* Hey this is my comments @#$
    ^&*
    Test */
    This is test";

            var result = CommentCleanHelper.Clean(input, CommentPattern.CSharp);
            Assert.IsFalse(result.Contains("123456"));
            Assert.IsFalse(result.Contains("^&*"));
        }

        [TestMethod]
        public void Clean_ForCSharp_WithinStrings_ShouldNotRemove()
        {

            //        string input = @"var s = 100;//123456
            //Multiline ""/* should not remove*/"" /*
            //^&*
            //Test */
            //This is test";

            string input = @"// Allow easy reference to the System namespace classes.
using System;
            using System.Collections.Generic;
            using System.Text;

namespace ConsoleApplication1
    {
        class Program
        {
            /* Main is the application's entry point.*/
            static void Main(string[] args) // program start
            {
                Console.WriteLine(""Hello, world! //@Testing@"");
                Console.ReadLine();
                /*
                this space is 
                for adding /*
                            additional functionalities
                see you later
                 */
            }
        }
    }";

            var result = CommentCleanHelper.Clean(input, CommentPattern.CSharp);
            Assert.IsFalse(result.Contains("Allow easy reference"));
            Assert.IsFalse(result.Contains("program start"));
            Assert.IsTrue(result.Contains("//@Testing@"));
            Assert.IsFalse(result.Contains("additional functionalities"));
        }
        #endregion

        #region C
        [TestMethod]
        public void Clean_ForC_Mixed_RemovesCommentedCodes()
        {

            string input = @"#include <stdio.h>
#include <string.h>

int main () {

   char str1[12] = ""Hello"";
   char str2[12] = ""World"";
            char str3[12];
            int len;

            /* copy str1 into str3 */
            strcpy(str3, str1);
            printf(""strcpy( str3, str1) :  %s\n"", str3);

            // concatenates str1 and str2 */
            strcat(str1, str2);
            printf(""strcat( str1, str2):   %s\n"", str1);

            /* total lenghth of str1 after concatenation */
            len = strlen(str1);
            printf(""strlen(str1) :  %d\n"", len);

            return 0;
        }";

            var result = CommentCleanHelper.Clean(input, CommentPattern.C);
            Assert.IsFalse(result.Contains("concatenates str1 and str2"));
            Assert.IsFalse(result.Contains("copy str1 into str3"));
        }
        #endregion

        #region VB
        [TestMethod]
        public void Clean_ForVb_RemovesCommentedCodes()
        {
            string input = @"' Allow easy reference to the System namespace classes.
Imports System

' This module houses the application's entry point.
Public Module modmain
   ' Main is the application's entry point.
   Sub Main() ' this is program's starting point
     ' Write text to the console.
     Console.WriteLine (""Hello World using Microsoft's Visual Basic!"")
   End Sub
End Module";
            var result = CommentCleanHelper.Clean(input, CommentPattern.Vb);
            Assert.IsFalse(result.Contains("program's starting point"));
            Assert.IsTrue(result.Contains("Microsoft's Visual Basic!"));
        }
        #endregion

        #region CSS
        [TestMethod]
        public void Clean_ForCss_RemovesCommentedCodes()
        {
            string input = @"body {
    background-color: green; /* lightblue; */
}

/*h1 { 
    color: white;
    text-align: center;
}*/

p {
    font-family: verdana;
    font-size: 20px;
}";
            var result = CommentCleanHelper.Clean(input, CommentPattern.Css);
            Assert.IsFalse(result.Contains("lightblue"));
            Assert.IsFalse(result.Contains("color: white;"));
            Assert.IsTrue(result.Contains("font-family: verdana;"));
        }
        #endregion

        #region HTML
        [TestMethod]
        public void Clean_ForHtml_RemovesCommentedCodes()
        {
            string input = @"<html>
<head>
<title>Comment test</title>
</head>
<body><!-- this is comment tag -->
<!-- 
Your comment can be as long as you need it to be.
Everything within the comment tags is kept from
affecting the code on the page.
-->
<p>This is the website</p>
</body>
</html>";
            var result = CommentCleanHelper.Clean(input, CommentPattern.Html);
            Assert.IsFalse(result.Contains("Everything within the comment"));
            Assert.IsFalse(result.Contains("this is comment tag"));
            Assert.IsTrue(result.Contains("<body>"));
        }
        #endregion

        #region Javascript
        [TestMethod]
        public void Clean_ForJavascript_RemovesCommentedCodes()
        {
            string input = @"var x = 5;      // Declare x, give it the value of 5
var y = x + 2;  // Declare y, give it the value of x + 2
/*
The code below will change
the heading with id = 'myH'
and the paragraph with id = 'myP'
in my web page:
            */
            document.getElementById('myH').innerHTML = ""My First Page"";
            document.getElementById('myP').innerHTML = ""My /*first*/ paragraph.""; ";
            var result = CommentCleanHelper.Clean(input, CommentPattern.Javascript);
            Assert.IsFalse(result.Contains("Declare x, give it the value of 5"));
            Assert.IsFalse(result.Contains("the heading with id = 'myH'"));
            Assert.IsTrue(result.Contains("My /*first*/ paragraph."));
        }
        #endregion

        #region XML
        [TestMethod]
        public void Clean_ForXml_RemovesCommentedCodes()
        {
            string input = @"<?xml version=""1.0"" encoding=""UTF - 8""?>
  < bookstore >
  <!--First record-->
  
    < book category = ""children"" >
   
       < title > Harry Potter </ title >
      
          < author > J K.Rowling </ author >
         
             < year > 2005 </ year >
         
             < price > 29.99 </ price >
         
           </ book >
         
           <!--Second record
           on the
           fly
  -->
  < book category = ""web"" >
 
     < title > Learning XML </ title >
    
        < author > Erik T.Ray </ author >
       
           < year > 2003 </ year >
       
           < price > 39.95 </ price >
       
         </ book >
       </ bookstore > ";
            var result = CommentCleanHelper.Clean(input, CommentPattern.Xml);
            Assert.IsFalse(result.Contains("First record"));
            Assert.IsFalse(result.Contains("Second record"));
        }
        #endregion

        #region VB
        [TestMethod]
        public void Clean_ForSql_RemovesCommentedCodes()
        {
            string input = @"INSERT INTO CUSTOMERS (ID,NAME,AGE,ADDRESS,SALARY)
VALUES (1, 'Ramesh--Krish', 32, 'Ahmedabad', 2000.00 );

INSERT INTO CUSTOMERS (ID,NAME,AGE,ADDRESS,SALARY)
VALUES (2, 'Khilan', 25, 'Delhi', 1500.00 );

SELECT ID, NAME, AGE, ADDRESS, SALARY
FROM CUSTOMERS
--Where SALARY > 10000
GROUP BY age
HAVING COUNT(age) >= 2;

";
            var result = CommentCleanHelper.Clean(input, CommentPattern.Sql);
            Assert.IsTrue(result.Contains("Ramesh--Krish"));
            Assert.IsTrue(result.Contains("Where SALARY > 10000"));
        }
        #endregion
    }
}
