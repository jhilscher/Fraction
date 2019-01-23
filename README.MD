A .NET Fraction DataType
=============

[![Build Status](https://travis-ci.org/jhilscher/Fraction.svg?branch=master)](https://travis-ci.org/jhilscher/Fraction)


```csharp
var f = new Fraction(1,4);
```



#### Usage like a standard c# datatype:
```csharp
Fraction a = new Fraction(1, 5);
Fraction c = a * 5 + new Fraction(2, 5) - (Fraction)0.2;
// c: 6/5
```

#### Features

* Fully integrated fraction datatype
* Supports all arithmetic operators and comparisons
* Cast to/from other types
* Create Fractions from Doubles
* Parse strings to Fractions
* c#7 Tuple support

#### Docs

* Fraction is a value DataType