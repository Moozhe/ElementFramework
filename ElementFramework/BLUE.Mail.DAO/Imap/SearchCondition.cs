using System;
using System.Collections.Generic;
using System.Text;

namespace BLUE.Mail.DAO.Imap
{
    public class SearchCondition
    {
        #region Static
        public static SearchCondition Text(string text) { return new SearchCondition { Field = SearchFields.Text, Value = text }; }
        public static SearchCondition BCC(string text) { return new SearchCondition { Field = SearchFields.BCC, Value = text }; }
        public static SearchCondition Before(DateTime date) { return new SearchCondition { Field = SearchFields.Before, Value = date }; }
        public static SearchCondition Body(string text) { return new SearchCondition { Field = SearchFields.Body, Value = text }; }
        public static SearchCondition Cc(string text) { return new SearchCondition { Field = SearchFields.Cc, Value = text }; }
        public static SearchCondition From(string text) { return new SearchCondition { Field = SearchFields.From, Value = text }; }
        public static SearchCondition Header(string name, string text) { return new SearchCondition { Field = SearchFields.Header, Value = name + " " + Utilities.QuoteString(text) }; }
        public static SearchCondition Keyword(string name, string text) { return new SearchCondition { Field = SearchFields.Keyword, Value = text }; }
        public static SearchCondition Larger(long size) { return new SearchCondition { Field = SearchFields.Larger, Value = size }; }
        public static SearchCondition Smaller(long size) { return new SearchCondition { Field = SearchFields.Smaller, Value = size }; }
        public static SearchCondition SentBefore(DateTime date) { return new SearchCondition { Field = SearchFields.SentBefore, Value = date }; }
        public static SearchCondition SentOn(DateTime date) { return new SearchCondition { Field = SearchFields.SentOn, Value = date }; }
        public static SearchCondition SentSince(DateTime date) { return new SearchCondition { Field = SearchFields.SentSince, Value = date }; }
        public static SearchCondition Subject(string text) { return new SearchCondition { Field = SearchFields.Subject, Value = text }; }
        public static SearchCondition To(string text) { return new SearchCondition { Field = SearchFields.To, Value = text }; }
        public static SearchCondition UID(string ids) { return new SearchCondition { Field = SearchFields.UID, Value = ids }; }
        public static SearchCondition Unkeyword(string text) { return new SearchCondition { Field = SearchFields.Unkeyword, Value = text }; }
        public static SearchCondition Answered() { return new SearchCondition { Field = SearchFields.Answered }; }
        public static SearchCondition Deleted() { return new SearchCondition { Field = SearchFields.Deleted }; }
        public static SearchCondition Draft() { return new SearchCondition { Field = SearchFields.Draft }; }
        public static SearchCondition Flagged() { return new SearchCondition { Field = SearchFields.Flagged }; }
        public static SearchCondition New() { return new SearchCondition { Field = SearchFields.New }; }
        public static SearchCondition Old() { return new SearchCondition { Field = SearchFields.Old }; }
        public static SearchCondition Recent() { return new SearchCondition { Field = SearchFields.Recent }; }
        public static SearchCondition Seen() { return new SearchCondition { Field = SearchFields.Seen }; }
        public static SearchCondition Unanswered() { return new SearchCondition { Field = SearchFields.Unanswered }; }
        public static SearchCondition Undeleted() { return new SearchCondition { Field = SearchFields.Undeleted }; }
        public static SearchCondition Undraft() { return new SearchCondition { Field = SearchFields.Undraft }; }
        public static SearchCondition Unflagged() { return new SearchCondition { Field = SearchFields.Unflagged }; }
        public static SearchCondition Unseen() { return new SearchCondition { Field = SearchFields.Unseen }; }

        private static SearchCondition Join(string condition, SearchCondition left, params SearchCondition[] right)
        {
            condition = condition.ToUpper();

            if (left.Operator != condition)
            {
                left = new SearchCondition { Operator = condition, Conditions = new List<SearchCondition> { left } };
            }

            left.Conditions.AddRange(right);
            return left;
        }
        #endregion

        public virtual object Value { get; set; }
        public virtual SearchFields? Field { get; set; }

        internal List<SearchCondition> Conditions { get; set; }
        internal string Operator { get; set; }

        public virtual SearchCondition And(params SearchCondition[] other)
        {
            return Join(string.Empty, this, other);
        }

        public virtual SearchCondition Not(params SearchCondition[] other)
        {
            return Join("NOT", this, other);
        }

        public virtual SearchCondition Or(params SearchCondition[] other)
        {
            return Join("OR", this, other);
        }

        public override string ToString()
        {
            if (Conditions != null && Conditions.Count > 0 && Operator != null)
            {
                List<string> conditions = new List<string>();

                foreach (var condition in Conditions)
                    conditions.Add(condition.ToString());

                return (Operator.ToUpper() + " (" + string.Join(") (", conditions.ToArray()) + ")").Trim();
            }

            var builder = new StringBuilder();
            if (Field != null) builder.Append(Field.ToString().ToUpper());

            if (Value != null)
            {
                var value = Value;
                switch (Field)
                {
                    case SearchFields.BCC:
                    case SearchFields.Body:
                    case SearchFields.From:
                    case SearchFields.Subject:
                    case SearchFields.Text:
                    case SearchFields.To:
                        value = Utilities.QuoteString(Convert.ToString(value));
                        break;
                }

                if (value is DateTime)
                {
                    value = Utilities.QuoteString(Utilities.GetRFC2060Date(((DateTime)value)));
                }

                if (Field != null) builder.Append(" ");
                builder.Append(value);
            }

            return builder.ToString();
        }

        public enum SearchFields
        {
            BCC,
            Before,
            Body,
            Cc,
            From,
            Header,
            Keyword,
            Larger,
            On,
            SentBefore,
            SentOn,
            SentSince,
            Since,
            Smaller,
            Subject,
            Text,
            To,
            UID,
            Unkeyword,
            All,
            Answered,
            Deleted,
            Draft,
            Flagged,
            New,
            Old,
            Recent,
            Seen,
            Unanswered,
            Undeleted,
            Undraft,
            Unflagged,
            Unseen
        }
    }
}
