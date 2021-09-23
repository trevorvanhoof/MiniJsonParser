# MiniJsonParser

C# - Read json to generic dictionary ( System.Collections.Dictionary<String, TTJson.Value> )

#### Why?

I am in an old Unity version that does not have System.Text.Json available.
I could not easily find a lightweight solution like this, only larger multi-purpose libraries.

This is a single file drop-in solution, I write it in a short amount of time to solve my immediate need, so be warned that there may be bugs, bad performance, and missing features!

#### TODO:
1. Serialization
2. Make escaped and unicode character parsing less horrific
