# NetDiff [![NuGet version](https://badge.fury.io/nu/netdiff.svg)](https://badge.fury.io/nu/netdiff) [![Build Status](https://travis-ci.org/etkirsch/NetDiff.svg?branch=master)](https://travis-ci.org/etkirsch/NetDiff)
Used to find differences between two dynamic objects.

## Using
```csharp
var calculator = new DiffCalculator();
var result = calculator.Diff(objectA, objectB);
```
