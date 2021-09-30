using System;
using System.Collections.Generic;
using System.Linq;

namespace VoteApp.Application.Commons.ExtensionMethods
{
    public class FilterOperator<T>
    {
        public T S { get; set; }
        public T EQ { get; set; }
        public T NE { get; set; }
        public T GT { get; set; }
        public T GTE { get; set; }
        public T LT { get; set; }
        public T LTE { get; set; }
        public List<T> IN { get; set; } = new List<T>();
        public List<T> NIN { get; set; } = new List<T>();

        public void SetOperator(string op, string value)
        {
            if (op == Operator.EQ) EQ = ConvertValue(value);
            if (op == Operator.NE) NE = ConvertValue(value);
            if (op == Operator.GT) GT = ConvertValue(value);
            if (op == Operator.GTE) GTE = ConvertValue(value);
            if (op == Operator.LT) LT = ConvertValue(value);
            if (op == Operator.LTE) LTE = ConvertValue(value);
            if (op == Operator.S) S = ConvertValue(value);
            if (op == Operator.IN)
            {
                var items = value.Split(',').ToList().Select(x => ConvertValue(x));
                foreach (var item in items)
                {
                    if (item == null) continue;
                    IN.Add(item);
                }
            }
            if (op == Operator.NIN)
            {
                var items = value.Split(',').ToList().Select(x => ConvertValue(x));
                foreach (var item in items)
                {
                    if (item == null) continue;
                    NIN.Add(item);
                }
            }
        }

        public T ConvertValue(string value)
        {
            try
            {
                var t = typeof(T);
                if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    if (value == null) return default;
                    t = Nullable.GetUnderlyingType(t);
                }
                return (T)Convert.ChangeType(value, t);
            }
            catch (Exception)
            {
                return default;
            }

        }
    }

    public class Operator
    {
        public const string S = "S";
        public const string EQ = "EQ";
        public const string NE = "NE";
        public const string GT = "GT";
        public const string GTE = "GTE";
        public const string LT = "LT";
        public const string LTE = "LTE";
        public const string IN = "IN";
        public const string NIN = "NIN";
    }
}
