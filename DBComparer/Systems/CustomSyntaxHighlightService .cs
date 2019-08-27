using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Services;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DBComparer.Systems
{
    public class CustomSyntaxHighlightService : ISyntaxHighlightService
    {
        private readonly Document document;
        private SyntaxHighlightProperties defaultSettings = new SyntaxHighlightProperties()
        { ForeColor = Color.Black };
        private SyntaxHighlightProperties keywordSettings = new SyntaxHighlightProperties()
        { ForeColor = Color.Blue };
        private SyntaxHighlightProperties stringSettings = new SyntaxHighlightProperties()
        { ForeColor = Color.Green };
        private string[] keywords = new string[] {
                "INSERT", "SELECT", "FROM", "CREATE", "TABLE", "USE", "IDENTITY", "ON", "OFF", "NOT", "NULL", "WITH", "SET",
                "UPDATE", "WHERE", "AND", "INDEX", "COLUMN", "CONSTRAINT", "DECLARE", "TRANSACTION", "COMMIT", "ROLLBACK",
                "AS", "CASE", "WHEN", "THEN", "OR", "LIKE", "GROUP", "BY", "ORDER", "LEFT", "JOIN", "INNER", "ON", "IN", "IS", "EXISTS" };

        public CustomSyntaxHighlightService(Document document)
        {
            this.document = document;
        }

        private List<SyntaxHighlightToken> ParseTokens()
        {
            List<SyntaxHighlightToken> tokens = new List<SyntaxHighlightToken>();
            DocumentRange[] ranges = null;

            ranges = document.FindAll("'", SearchOptions.None);

            for (int i = 0; i < ranges.Length / 2; i++)
            {
                tokens.Add(new SyntaxHighlightToken(ranges[i * 2].Start.ToInt(),
                ranges[i * 2 + 1].Start.ToInt() - ranges[i * 2].Start.ToInt() + 1,
                stringSettings));
            }

            for (int i = 0; i < keywords.Length; i++)
            {
                ranges = document.FindAll(keywords[i].ToLower(), SearchOptions.WholeWord);

                for (int j = 0; j < ranges.Length; j++)
                {
                    if (!IsRangeInTokens(ranges[j], tokens))
                    {
                        tokens.Add(new SyntaxHighlightToken(ranges[j].Start.ToInt(),
                        ranges[j].Length,
                        keywordSettings));
                    }
                }
            }

            tokens.Sort(new SyntaxHighlightTokenComparer());
            AddPlainTextTokens(tokens);
            return tokens;
        }

        private void AddPlainTextTokens(List<SyntaxHighlightToken> tokens)
        {
            int count = tokens.Count;

            if (count == 0)
            {
                tokens.Add(new SyntaxHighlightToken(0, document.Range.End.ToInt(),
                defaultSettings));
                return;
            }

            tokens.Insert(0, new SyntaxHighlightToken(0, tokens[0].Start, defaultSettings));

            for (int i = 1; i < count; i++)
            {
                tokens.Insert(i * 2, new SyntaxHighlightToken(tokens[i * 2 - 1].End,
                tokens[i * 2].Start - tokens[i * 2 - 1].End, defaultSettings));
            }

            tokens.Add(new SyntaxHighlightToken(tokens[count * 2 - 1].End,
            document.Range.End.ToInt() - tokens[count * 2 - 1].End, defaultSettings));
        }

        private bool IsRangeInTokens(DocumentRange range, List<SyntaxHighlightToken> tokens)
        {
            return tokens.Any(t => IsIntersect(range, t));
        }

        private bool IsIntersect(DocumentRange range, SyntaxHighlightToken token)
        {
            int start = range.Start.ToInt();

            if (start >= token.Start && start < token.End)
            {
                return true;
            }

            int end = range.End.ToInt() - 1;

            if (end >= token.Start && end < token.End)
            {
                return true;
            }

            return false;
        }

        public void Execute()
        {
            document.ApplySyntaxHighlight(ParseTokens());
        }

        public void ForceExecute()
        {
            Execute();
        }

        public class SyntaxHighlightTokenComparer : IComparer<SyntaxHighlightToken>
        {
            public int Compare(SyntaxHighlightToken x, SyntaxHighlightToken y)
            {
                return x.Start - y.Start;
            }
        }
    }
}