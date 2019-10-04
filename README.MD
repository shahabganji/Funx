
## Funx

This is an experimental functional library for C# developers, all the features are implemented based on what [Enrico](https://github.com/la-yumba) taught in his [Functional Programming in C#](https://www.amazon.com/Functional-Programming-write-better-code/dp/1617293954). I tried to learn them and write one for myself.


### Introduction

Funx provides robust and well-tested core basic structs for functional programming in C#, Option and Either. Option represents a possibility of lack of value.

### Installation

Easily install it via [Nuget](https://www.nuget.org/packages/Funx/).

```
Install-Package Funx

dotnet add package Funx
```


### Usage

### Option

```csharp
using Funx;
// using static Funx.Helpers; // in some scenarios it may provide better syntax && readability

var none = Option<string>.None; // or Option<string> none = None;
var author = Option<string>.Some("Enrico");

var value = author.Match(() => "No Name", v => v);
WriteLine(value); // Enrico

value = none.Match(() => "No Name", v => v);
WriteLine(value); //  No Name
```

I would rather to use it in a different way, since all native types can be implicitly cast to Option<T>

```csharp
using Funx;
using static Funx.Helpers;

Option<string> none = None;
Option<string> author = "Enrico";

var value = author.Match(() => "No Name", v => v);
WriteLine(value); // Enrico

value = none.Match(() => "No Name", v => v);
WriteLine(value); //  No Name
```


Should you require more functionality and methods, import `Funx.Extensions` namespace and you'll get a bunch of handy extension method.

```csharp
using Funx;
using Funx.Extensions;

var student = new Author { Name = "Jane", Age = 17 };

var isTeenager = student.Where( s => s.Age < 20 && s.Age > 13 );

// ***** OR *********

var isTeenager = from s in student
                 where s.Age > 13 && s.Age < 20
                 select s;
```



