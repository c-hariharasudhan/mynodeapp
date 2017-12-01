using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommentCleaner
{
    public static class CommentCleanHelper
    {
        public static string Clean(string content, CommentPattern commentPattern)
        {
            switch (commentPattern)
            {
                case CommentPattern.C:
                case CommentPattern.CSharp:
                case CommentPattern.Javascript:
                    return CSharpCommentClean(content);
                case CommentPattern.Vb:
                    return VbCommentClean(content);
                case CommentPattern.Css:
                    return CssCommentClean(content);
                case CommentPattern.Html:
                case CommentPattern.Xml:
                    return HtmlCommentClean(content);
                case CommentPattern.Sql:
                    return SqlCommentClean(content);
            }
            return "";
        }


        private static string CSharpCommentClean(string content)
        {
            var blockComments = @"/\*(.*?)\*/";
            var lineComments = @"//(.*?)\r?\n";
            var strings = @"""((\\[^\n]|[^""\n])*)""";
            var verbatimStrings = @"@(""[^""]*"")+";


            string noComments = Regex.Replace(content, 
                blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
                me =>{if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                        return me.Value.StartsWith("//") ? Environment.NewLine : "";
                // Keep the literal strings 
                    return me.Value;},RegexOptions.Singleline);
            return noComments;
        }

        private static string VbCommentClean(string content)
        {
            var lineComments = @"\'(.*?)\r?\n"; ;
            var strings = @"""((\'[^\n]|[^""\n])*)""";
            string noComments = Regex.Replace(content, lineComments + "|" + strings, me => {
                if (me.Value.StartsWith("'"))
                    return me.Value.StartsWith("'") ? Environment.NewLine : "";
                // Keep the literal strings 
                return me.Value;
            }, RegexOptions.Singleline);
            return noComments;
        }

        private static string CssCommentClean(string content)
        {
            var blockComments = @"/\*(.*?)\*/";            

            string noComments = Regex.Replace(content, blockComments,
                me => {
                    if (me.Value.StartsWith("/*"))
                        return me.Value.StartsWith("/*") ? Environment.NewLine : "";
                    // Keep the literal strings 
                    return me.Value;
                }, RegexOptions.Singleline);
            return noComments;
        }

        private static string HtmlCommentClean(string content)
        {
            var blockComments = @"<!--(.*?)-->";

            string noComments = Regex.Replace(content, blockComments,
                me => {
                    if (me.Value.StartsWith("<!--"))
                        return me.Value.StartsWith("<!--") ? Environment.NewLine : "";
                    // Keep the literal strings 
                    return me.Value;
                }, RegexOptions.Singleline);
            return noComments;
        }
        private static string SqlCommentClean(string content)
        {
            var lineComments = @"\--(.*?)\r?\n"; ;
            var strings = @"""((\--[^\n]|[^""\n])*)""";
            var strings1 = @"'((\--[^\n]|[^'\n])*)'";
            string noComments = Regex.Replace(content, lineComments + "|" + strings + "|" + strings1, me => {
                if (me.Value.StartsWith("--"))
                    return me.Value.StartsWith("--") ? Environment.NewLine : "";
                // Keep the literal strings 
                return me.Value;
            }, RegexOptions.Singleline);
            return noComments;
        }
    }
}
