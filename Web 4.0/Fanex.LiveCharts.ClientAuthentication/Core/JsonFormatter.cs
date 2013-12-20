// --------------------------------------------------------------------------
// <copyright file="JsonFormatter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.11">Created</change>
// </history>

namespace Fanex.LiveCharts.ClientAuthentication.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Web.UI;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum JSONSerializationMode
    {
        /// <summary>
        /// Slower, but takes advantage of override ToJSON methods.
        /// </summary>
        UseReflection,

        /// <summary>
        /// Faster, but ignores override ToJSON methods.
        /// </summary>
        NoReflection
    }

    /// <summary>
    /// This class encodes and decodes JsonFormatter strings.
    /// </summary>
    [SuppressMessage(
            "StyleCop.CSharp.Nexcel.NexcelCustomRules",
            "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1629:DocumentationTextMustEndWithAPeriod", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1507:CodeMustNotContainMultipleBlankLinesInARow", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1106:CodeMustNotContainEmptyStatements", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1515:SingleLineCommentMustBePrecededByBlankLine", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1106:CodeMustNotContainEmptyStatements", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1512:SingleLineCommentsMustNotBeFollowedByBlankLine", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1503:CurlyBracketsMustNotBeOmitted", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1500:CurlyBracketsForMultiLineStatementsMustNotShareLine", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1306:FieldNamesMustBeginWithLowerCaseLetter", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1122:UseStringEmptyForEmptyStrings", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1013:ClosingCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1012:OpeningCurlyBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1011:ClosingSquareBracketsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1002:SemicolonsMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "EX1003:MethodMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009:ClosingParenthesisMustBeSpacedCorrectly", Justification = "Reviewed.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1005:SingleLineCommentsMustBeginWithSingleSpace", Justification = "Reviewed.")]
    public static class JsonFormatter
    {
        public static string ToJSON(object obj)
        {
            if (obj == null)
                return "null";

            return ToJSON(obj, JSONSerializationMode.UseReflection);
        }

        public static string ToJSON(object obj, JSONSerializationMode mode)
        {
            if (obj == null)
                return "null";

            if (mode == JSONSerializationMode.UseReflection)
            {
                Type type = obj.GetType();
                MethodInfo[] methods = type.GetMethods();
                MethodInfo method = null;
                foreach (var mi in methods)
                {
                    if (mi.Name == "ToJSON" && mi.ReturnType == typeof(string)
                    && mi.DeclaringType == type && mi.GetParameters().Length == 0)
                    {
                        method = mi;
                        break;
                    }
                }
                //methods = methods.Where(c => c.Name == "ToJSON" && c.ReturnType == typeof(string)).ToArray();
                //MethodInfo method = methods.SingleOrDefault(c => c.DeclaringType == _type && c.GetParameters().Length == 0);

                if (method != null)
                    return (string)method.Invoke(obj, new object[0]);
            }

            return JsonFormatter.Serialize(obj, mode);
        }

        public static string EscapeString(string obj)
        {
            return obj.Replace("\\", "\\\\").Replace("/", "\\/").Replace("\"", "\\\"")
            .Replace("\b", "\\b").Replace("\f", "\\f").Replace("\n", "\\n")
            .Replace("\r", "\\r").Replace("\t", "\\t");
        }

        public static string UnescapeString(string obj)
        {
            if (!obj.Contains("\\"))
                return obj;

            return obj.Replace("\\t", "\t").Replace("\\r", "\r").Replace("\\n", "\n")
            .Replace("\\f", "\f").Replace("\\b", "\b").Replace("\\\"", "\"")
            .Replace("\\/", "/").Replace("\\\\", "\\");
        }

        public static string Serialize(object obj)
        {
            return Serialize(obj, JSONSerializationMode.UseReflection);
        }

        public static string Serialize(object obj, JSONSerializationMode mode)
        {
            if (obj == null)
                return "null";

            Type type = obj.GetType();

            if (IsNumeric(type))
            {
                return obj.ToString();
            }
            else if (type == typeof(string) || type == typeof(char))
            {
                return "\"" + EscapeString("" + obj) + "\"";
            }
            else if (type == typeof(bool))
            {
                bool x = (bool)obj;

                return x ? "true" : "false";
            }
            else if (type == typeof(DateTime))
            {
                DateTime dt = (DateTime)obj;

                return "\"" + dt.ToString("MM/dd/yyyy hh:mm:ss tt") + "\"";
            }

            StringBuilder result = new StringBuilder();

            if (obj.GetType().IsArray)
            {
                result.Append("[");

                Array array = (Array)obj;

                foreach (object value in array)
                {
                    result.Append(ToJSON(value, mode) + ",");
                }

                string json = result.ToString();

                if (json.EndsWith(","))
                    json = json.Substring(0, json.Length - 1);

                json += "]";

                return json;
            }
            else if (obj is IDictionary)
            {
                IDictionary d = (IDictionary)obj;
                IDictionaryEnumerator i = d.GetEnumerator();

                result.Append("{");

                while (i.MoveNext())
                {
                    result.Append("\"" + i.Key + "\":");
                    result.Append(ToJSON(i.Value, mode));
                    result.Append(",");
                }

                string json = result.ToString();

                if (json.EndsWith(","))
                    json = json.Substring(0, json.Length - 1);

                json += "}";

                return json;
            }
            else if (obj is NameValueCollection)
            {
                NameValueCollection col = (NameValueCollection)obj;

                result.Append("{");

                for (int i = 0; i < col.Count; i++)
                {
                    string key = col.Keys[i];
                    string value = col[key];

                    result.Append("\"" + key + "\":");
                    result.Append(ToJSON(value, mode));
                    result.Append(",");
                }

                string json = result.ToString();

                if (json.EndsWith(","))
                    json = json.Substring(0, json.Length - 1);

                json += "}";

                return json;
            }
            else if (obj is IEnumerable)
            {
                IEnumerable item = (IEnumerable)obj;
                IEnumerator i = item.GetEnumerator();

                result.Append("[");

                while (i.MoveNext())
                {
                    result.Append(ToJSON(i.Current, mode) + ",");
                }

                string json = result.ToString();

                if (json.EndsWith(","))
                    json = json.Substring(0, json.Length - 1);

                json += "]";

                return json;
            }
            else
            {
                result.Append("{");

                PropertyInfo[] props = obj.GetType().GetProperties();

                foreach (PropertyInfo prop in props)
                {
                    if (IgnoreProperty(prop))
                        continue;

                    object value = prop.GetValue(obj, null);

                    result.Append("\"" + prop.Name + "\":");
                    result.Append(ToJSON(prop.GetValue(obj, null), mode));
                    result.Append(",");
                }

                string json = result.ToString();

                if (json.EndsWith(","))
                    json = json.Substring(0, json.Length - 1);

                json += "}";

                return json;
            }
        }

        private static bool IgnoreProperty(PropertyInfo prop)
        {
            if (prop.GetIndexParameters().Length > 0)
                return true;

            string Namespace = prop.PropertyType.Namespace;

            if (Namespace != null && (Namespace.StartsWith("System.Reflection") || Namespace.StartsWith("System.Security")))
                return true;

            Type[] ignoredTypes = new Type[] { typeof(Type), typeof(HtmlTextWriter), typeof(TextWriter), typeof(Stream) };

            foreach (Type type in ignoredTypes)
            {
                if (prop.PropertyType == type || prop.PropertyType.IsSubclassOf(type))
                    return true;
            }

            return false;
        }

        public static object Deserialize(string json, Type type)
        {
            object o = Deserialize(json);
            PropertyCache.Clear();
            ConstructorCache.Clear();
            return Deserialize(o, type);
        }

        public static T Deserialize<T>(string json)
        {
            return (T)Deserialize(json, typeof(T));
        }

        private static Hashtable PropertyCache = new Hashtable();
        private static Hashtable ConstructorCache = new Hashtable();
        private static Hashtable PropCanWrite = new Hashtable();

        private static object Deserialize(object obj, Type type)
        {
            if (obj == null)
                return null;

            Type t = obj.GetType();

            if (t == typeof(ArrayList))
            {
                if (type.IsArray)
                    type = type.GetElementType();

                ArrayList elems = new ArrayList();

                foreach (object o in (ArrayList)obj)
                {
                    if (o != null)
                        elems.Add(Deserialize(o, type));
                }

                return elems.ToArray(type);
            }

            if (t == typeof(Hashtable))
            {
                Hashtable hash = (Hashtable)obj;
                PropertyInfo[] props;
                ConstructorInfo construct;

                if (PropertyCache.ContainsKey(type))
                {
                    props = (PropertyInfo[])PropertyCache[type];
                    construct = (ConstructorInfo)ConstructorCache[type];
                }
                else
                {
                    props = type.GetProperties();
                    PropertyCache[type] = props;

                    construct = type.GetConstructor(new Type[] { });
                    ConstructorCache[type] = construct;
                }

                object elem = construct.Invoke(new object[] { });

                foreach (PropertyInfo prop in props)
                {
                    if (!prop.CanWrite)
                        continue;

                    object temp = hash[prop.Name];

                    if (temp == null)
                        temp = hash[prop.Name.ToLower()];

                    if (temp == null)
                        temp = hash[char.ToLower(prop.Name[0]) + prop.Name.Substring(1)];

                    if (temp != null)
                        prop.SetValue(elem, Deserialize(temp, prop.PropertyType), null);
                }

                return elem;
            }

            if (obj is double)
            {
                double val = (double)obj;

                if (type == typeof(int))
                    return (int)val;
                else if (type == typeof(short))
                    return (short)val;
                else if (type == typeof(decimal))
                    return (decimal)val;
                else if (type == typeof(long))
                    return (long)val;
                else if (type == typeof(double))
                    return (double)val;
                else if (type == typeof(float))
                    return (float)val;
                else if (type == typeof(byte))
                    return (byte)val;
                else
                    return val;
            }

            if (type == typeof(bool))
            {
                return (bool)obj;
            }

            if (type == typeof(string))
                return obj.ToString();

            if (type == typeof(DateTime))
            {
                DateTime outdt;

                if (DateTime.TryParse(obj.ToString(), out outdt))
                    return outdt;
                else
                    return new DateTime(1, 1, 1);
            }

            if (type == typeof(object))
                return obj;

            return null;
        }

        private static double outDouble = 0;

        public static object Deserialize(string json)
        {
            JSONDeserializer jd = new JSONDeserializer(json);
            return jd.Deserialize();
        }

        /// <summary>
        /// This is just a test.
        /// </summary>
        private class JSONTypeDeserializer
        {
            private string _json;
            private Type _type;
            private int _index;

            public JSONTypeDeserializer(string json, Type type)
            {
                this._json = json;
                this._type = type;
            }

            public object Deserialize()
            {
                _index = 0;

                if (_type.IsArray)
                {
                    var t = _type.GetElementType();
                    return ProcessArray(t);
                }
                else
                {
                    return ProcessHash(_type);
                }
            }

            private object ProcessValue(Type type)
            {
                SkipWhitespace();

                if (_json[_index] == '[')
                {
                    if (_json == "[]")
                        return new object[0];

                    return ProcessArray(type);
                }

                if (_json[_index] == '{')
                {
                    if (_json == "{}")
                    {
                        ConstructorInfo constructor = type.GetConstructor(new Type[0]);

                        if (constructor != null)
                            return constructor.Invoke(new object[0]);
                        else
                            return null;
                    }

                    return ProcessHash(type);
                }

                if (_json[_index] == '"')
                {
                    string val = UnescapeString(ProcessString());

                    DateTime outDate;

                    if (DateTime.TryParse(val, out outDate))
                    {
                        return outDate;
                    }

                    return val;
                }

                int startIndex = _index;

                while (_index < _json.Length && _json[_index] != ',' && _json[_index] != '}' && _json[_index] != ']' && !Char.IsWhiteSpace(_json[_index]))
                    _index++;

                string jval = _json.Substring(startIndex, _index - startIndex).Trim();

                if (jval == "true")
                    return true;

                if (jval == "false")
                    return false;

                if (jval == "null")
                    return null;

                if (type == typeof(int))
                    return int.Parse(jval);
                else if (type == typeof(short))
                    return short.Parse(jval);
                else if (type == typeof(decimal))
                    return decimal.Parse(jval);
                else if (type == typeof(long))
                    return long.Parse(jval);
                else if (type == typeof(double))
                    return double.Parse(jval);
                else if (type == typeof(float))
                    return float.Parse(jval);

                return null;
            }

            private object[] ProcessArray(Type type)
            {
                _index++;

                List<object> list = new List<object>();

                while (true)
                {
                    SkipWhitespace();

                    list.Add(ProcessValue(type));

                    if (!MoveNext())
                        break;
                }

                return list.ToArray();
            }

            private object ProcessHash(Type type)
            {
                ConstructorInfo constructor = type.GetConstructor(new Type[0]);

                object obj;

                if (constructor != null)
                    obj = constructor.Invoke(new object[0]);
                else
                    return null;

                _index++;

                PropertyInfo[] props = type.GetProperties();

                Type t;

                while (true)
                {
                    SkipWhitespace();

                    string key = ProcessHashKey().Trim();
                    SkipWhitespace();

                    //PropertyInfo prop = props.FirstOrDefault(c => c.Name == key);
                    PropertyInfo prop = null;
                    foreach (var p in props)
                    {
                        if (p.Name == key)
                        {
                            prop = p;
                            break;
                        }
                    }

                    if (prop == null)
                    {
                        //prop = props.FirstOrDefault(c => c.Name.ToLower() == key.ToLower());
                        foreach (var p in props)
                        {
                            if (p.Name.ToLower() == key.ToLower())
                            {
                                prop = p;
                                break;
                            }
                        }
                    }

                    if (prop == null)
                        t = typeof(object);
                    else
                        t = prop.PropertyType;

                    object value = ProcessValue(t);

                    prop.SetValue(obj, value, null);

                    if (!MoveNext())
                        break;
                }

                return obj;
            }

            private bool MoveNext()
            {
                while (_index < _json.Length && _json[_index] != ',' &&
                _json[_index] != ']' && _json[_index] != '}')
                    _index++;

                if (_index >= _json.Length)
                    return false;

                if (_json[_index] == ']' || _json[_index] == '}')
                {
                    _index++;
                    return false;
                }

                _index++;

                SkipWhitespace();

                if (_index >= _json.Length)
                    return false;

                return true;
            }

            private void SkipWhitespace()
            {
                while (_index < _json.Length && Char.IsWhiteSpace(_json[_index]))
                    _index++;
            }

            private string ProcessHashKey()
            {
                int startIndex = _index;
                while (_json[_index++] != ':')
                    ;
                string result = _json.Substring(startIndex, _index - startIndex - 1).Trim();
                result = ChopStart(result, "\"");
                result = ChopEnd(result, "\"");
                return result;
            }

            private string ProcessString()
            {
                int startIndex = _index;
                StringBuilder result = new StringBuilder();

                while (true)
                {
                    _index++;

                    while (_json[_index] != '"')
                    {
                        result.Append(_json[_index]);
                        _index++;
                    }

                    int j = _index - 1;
                    int count = 0;

                    while (_json[j] == '\\')
                    {
                        j--;
                        count++;
                    }

                    //if there are an even number of backslashes,
                    //then they are all just backslashes and they aren't escaping the quote
                    //otherwise, the quote is being escaped and we need to keep searching for the close quote
                    if (count % 2 == 0)
                        break;
                    else
                        result.Append(_json[_index]);
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// JSON Deserializer.
        /// </summary>
        private class JSONDeserializer
        {
            private string _json;
            private int _index;

            public JSONDeserializer(string JSON)
            {
                _index = 0;
                _json = JSON;
            }

            public object Deserialize()
            {
                _index = 0;
                SkipWhitespace();
                return ProcessValue();
            }

            private object ProcessValue()
            {
                if (_json[_index] == '[')
                {
                    return ProcessArray();
                }

                if (_json[_index] == '{')
                {
                    return ProcessHash();
                }

                if (_json[_index] == '"')
                {
                    string val = UnescapeString(ProcessString());

                    //DateTime outDate;

                    //if (DateTime.TryParse(val, out outDate))
                    //{
                    // return outDate;
                    //}

                    return val;
                }

                int startIndex = _index;

                while (_index < _json.Length && _json[_index] != ',' && _json[_index] != '}' && _json[_index] != ']' && !Char.IsWhiteSpace(_json[_index]))
                    _index++;

                string jval = _json.Substring(startIndex, _index - startIndex);

                if (jval == "true")
                    return true;

                if (jval == "false")
                    return false;

                if (jval == "null")
                    return null;

                if (double.TryParse(jval, out outDouble))
                    return outDouble;

                return null;
            }

            private ArrayList ProcessArray()
            {
                _index++;

                ArrayList list = new ArrayList();

                SkipWhitespace();

                if (_json[_index] == ']')
                {
                    _index++;
                    return list;
                }

                while (true)
                {
                    list.Add(ProcessValue());

                    if (!MoveNext())
                        break;
                }

                return list;
            }

            private Hashtable ProcessHash()
            {
                _index++;

                Hashtable hash = new Hashtable();

                SkipWhitespace();

                if (_json[_index] == '}')
                {
                    _index++;
                    return hash;
                }

                while (true)
                {
                    string key = ProcessHashKey();

                    SkipWhitespace();

                    object value = ProcessValue();

                    hash.Add(key, value);

                    if (!MoveNext())
                        break;
                }

                return hash;
            }

            private bool MoveNext()
            {
                while (_index < _json.Length && _json[_index] != ',' &&
                _json[_index] != ']' && _json[_index] != '}')
                    _index++;

                if (_index >= _json.Length)
                    return false;

                if (_json[_index] == ']' || _json[_index] == '}')
                {
                    _index++;
                    return false;
                }

                _index++;

                SkipWhitespaceEnd();

                if (_index >= _json.Length)
                    return false;

                return true;
            }

            private void SkipWhitespaceEnd()
            {
                while (_index < _json.Length && Char.IsWhiteSpace(_json[_index]))
                    _index++;
            }

            private void SkipWhitespace()
            {
                while (Char.IsWhiteSpace(_json[_index]))
                    _index++;
            }

            private string ProcessHashKey()
            {
                int startIndex = _index + 1;
                while (_json[_index++] != ':')
                    ;
                string result = _json.Substring(startIndex, _index - startIndex - 2).TrimEnd();
                return result;
            }

            private string ProcessString()
            {
                int startIndex = _index + 1;

                while (true)
                {
                    _index++;

                    while (_json[_index] != '"')
                        _index++;

                    int j = _index - 1;
                    int count = 0;

                    while (_json[j] == '\\')
                    {
                        j--;
                        count++;
                    }

                    //if there are an even number of backslashes,
                    //then they are all just backslashes and they aren't escaping the quote
                    //otherwise, the quote is being escaped and we need to keep searching for the close quote
                    if (count % 2 == 0)
                        break;
                }

                return _json.Substring(startIndex, _index - startIndex);
            }
        }

        private static string ChopStart(string str, string x)
        {
            if (str.StartsWith(x))
                return str.Substring(x.Length);

            return str;
        }

        private static string ChopEnd(string str, string x)
        {
            if (str.EndsWith(x))
                return str.Substring(0, str.Length - x.Length);

            return str;
        }

        private static bool IsNumeric(Type type)
        {
            Type[] types = new Type[]{typeof(int), typeof(short), typeof(long),
                typeof(double), typeof(decimal), typeof(byte)};

            foreach (var t in types)
            {
                if (t == type)
                {
                    return true;
                }
            }

            return false;
            //return types.Contains(_type);
        }
    }
}