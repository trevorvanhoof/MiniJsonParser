# MiniJsonParser

C# - Read json to generic dictionary ( System.Collections.Dictionary<String, TTJson.Value> )

#### Why?

I am in an old Unity version that does not have System.Text.Json available.
I could not easily find a lightweight solution like this, only larger multi-purpose libraries.

This is a single file drop-in solution, I write it in a short amount of time to solve my immediate need, so be warned that there may be bugs, bad performance, and missing features!

#### Usage:

Drop TTJson.cs into your project.
Read any string containing json data using:

```C#
TTJson.Value document = new TTJson.Parser(text).Parse();
```

Confidently access the content by accessing the right attributes:

```C#
Debug.Log(document.listValue[0].objectValue["a"].stringValue);
```

Or less confidently do some sanity checking first:

```C#
if(document.valueType != TTJson.Value.EType.List) { Debug.LogError("Unexpected document format, expected root element to be a list."); }
if(document.listValue.Count == 0) { Debug.LogError("Unexpected document format, expected at least one element in the list."); }
if(document.listValue[0].valueType != TTJson.Value.EType.Object) { Debug.LogError("Unexpected document format, expected root element to be a list of objects."); }
if(!document.listValue[0].objectValue.ContainsKey("a")) { Debug.LogError("Unexpected document format, missing attribute 'a'."); }
if(!document.listValue[0].objectValue["a"].valueType != TTJson.Value.EType.String) { Debug.LogError("Unexpected document format, expected attribute 'a' to be a string."); }
Debug.Log(document.listValue[0].objectValue["a"].stringValue);
```

#### TODO:
1. Serialization
2. Make escaped and unicode character parsing less horrific
