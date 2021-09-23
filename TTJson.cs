using System;
using System.Collections.Generic;


namespace TTJson {
    public class Value {
        public enum EType {
            Int,
            Double,
            String,
            List,
            Object
        };

        public readonly EType valueType;
        public readonly int intValue;
        public readonly double doubleValue;
        public readonly string stringValue;
        public readonly List<Value> listValue;
        public readonly Dictionary<string, Value> objectValue;

        public Value(string stringValue) {
            valueType = EType.String;
            this.stringValue = stringValue;
        }

        public Value(int intValue) {
            valueType = EType.Int;
            this.intValue = intValue;
        }

        public Value(double doubleValue) {
            valueType = EType.Double;
            this.doubleValue = doubleValue;
        }

        public Value(List<Value> listValue) {
            valueType = EType.List;
            this.listValue = listValue;
        }

        public Value(Dictionary<string, Value> objectValue) {
            valueType = EType.Object;
            this.objectValue = objectValue;
        }
    }

    public class Parser {
        public Parser(string jsonData) {
            _text = jsonData;
            _cursor = 0;
        }

        public Value Parse() {
            _cursor = 0;
            SkipWhitespace();
            return ConsumeValue();
        }

        private readonly string _text;
        private int _cursor;

        private void SkipWhitespace() {
            while (true) {
                string chr = _text[_cursor].ToString();
                if (!(chr == " " || chr == "\t" || chr == "\r" || chr == "\n"))
                    break;
                _cursor += 1;
            }
        }

        private void AssertChr(char require) {
            if (_text[_cursor] != require)
                throw new Exception("Parse error, expected '" + require + "', got '" + _text[_cursor] + "' at index " + _cursor);
        }

        private Value ConsumeNumber() {
            const string validCharacters = "0123456789.eE+-xXabcdefABCDEF";
            // Advance until the end
            int start = _cursor;
            while (true) {
                if (!validCharacters.Contains(_text[_cursor].ToString()))
                    break;
                _cursor += 1;
            }
            // Parse the number
            string substr = _text.Substring(start, _cursor - start).ToLower();
            if (substr.Contains(".") || substr.Contains("e"))
                return new Value(double.Parse(substr));
            return new Value(int.Parse(substr));
        }

        private string ConsumeString() {
            AssertChr('"');
            // Advance past the start
            _cursor += 1;
            // Track escape characters
            bool skip = false;
            // Advance until the next un-escaped double quote
            int start = _cursor;
            while (true) {
                char chr = _text[_cursor];
                _cursor += 1;
                if (skip) continue;
                if (chr == '"') break;
                skip = chr == '\\';
            }
            return _text.Substring(start, _cursor - start - 1);
        }

        private List<Value> ConsumeArray() {
            AssertChr('[');
            _cursor += 1;
            List<Value> result = new List<Value>();
            while (true) {
                SkipWhitespace();
                // Detect end of list
                if (_text[_cursor] == ']') {
                    _cursor += 1;
                    break;
                }
                if (result.Count > 0) {
                    // Detect comma
                    AssertChr(',');
                    _cursor += 1;
                    SkipWhitespace();
                }
                // Else read value and proceed
                result.Add(ConsumeValue());
            }
            return result;
        }

        private Value ConsumeValue() {
            char chr = _text[_cursor];
            if (chr == '"') {
                return new Value(ConsumeString());
            }
            if (chr == '[') {
                return new Value(ConsumeArray());
            }
            if (chr == '{') {
                return new Value(ConsumeObject());
            }
            return ConsumeNumber();
        }

        private Dictionary<string, Value> ConsumeObject() {
            AssertChr('{');
            _cursor += 1;
            Dictionary<string, Value> result = new Dictionary<string, Value>();
            while (true) {
                SkipWhitespace();
                // Detect end of object
                if (_text[_cursor] == '}') {
                    _cursor += 1;
                    break;
                }
                if (result.Count > 0) {
                    // Detect comma
                    AssertChr(',');
                    _cursor += 1;
                    SkipWhitespace();
                }
                // Read key
                string key = ConsumeString();
                // Detect separator
                SkipWhitespace();
                AssertChr(':');
                _cursor += 1;
                SkipWhitespace();
                // Read value
                result[key] = ConsumeValue();
            }
            return result;
        }
    }
}
