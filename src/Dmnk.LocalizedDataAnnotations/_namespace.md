---
uid: Dmnk.LocalizedDataAnnotations
---

![NuGet Version](https://img.shields.io/nuget/v/Dmnk.LocalizedDataAnnotations?style=flat-square&color=blue&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FDmnk.LocalizedDataAnnotations%2F)

Localized `System.ComponentModel.DataAnnotations`.

# Why?

Regrettably, `System.ComponentModel.DataAnnotations` is no longer localized in modern .NET. This
would not be so big of an issue if one could at least provide their own translations. However, the
only official way of doing this is on every single attribute usage. Even more weird/annoying is that
the messages have *almost* not changed for a long time, so MS *could* just ship the old
translations with minimal effort, maybe as a nuget package.

# Overview

This package provides two mechanisms to work around this issue:

- `IValidator` and `LocalizedValidator` behave pretty much exactly like `Validator`, but they
  will be localized by default, using the old translations from .NET Framework **with some slight
  custom adaptations** (see below). You can also provide your own translations by implementing
  `IDefaultValidationMessageProvider`.
- `LocalizedValidatorReflectionHack.Hack()` rewrites the `ResourceManager` on a private field
  of an internal class in `System.ComponentModel.DataAnnotations` to use the old translations.
  This *should* be reasonably reliable, since it has been a long time since the last change to
  that class and the package in general. Use at your own risk though.

All default subclasses of `ValidationAttribute` are supported. Your own custom subclasses should
use their own `FormatMessage` implementation anyway, so this package does not affect them.

Currently, only English, German and French translations are included. Pull requests are welcome.
You may use the method outlined below to acquire the old translations.

# Usage

## `IValidator` and `LocalizedValidator`

Be aware that this approach re-implements the internal logic of validator. There are a good amount
of test-cases. Just be aware of that risk profile.

```csharp
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dmnk.LocalizedDataAnnotations;

IValidator validator = new LocalizedValidator();
var model = new MyModel { Name = "" };
ValidationContext context = new ValidationContext(model);
ICollection<ValidationResult> results = new List<ValidationResult>();
bool isValid = validator.TryValidateObject(model, context, results, validateAllProperties: true);

class MyModel
{
    [Required]
    public string Name { get; set; }
}

```

## `LocalizedValidatorReflectionHack.Hack()`

Be aware that some attributes *cache their error message*. This means that you **cannot reliably
switch culture at runtime** and expect correct localization.

```csharp
using Dmnk.LocalizedDataAnnotations;

// somewhere early in e.g. Program.cs
LocalizedValidatorReflectionHack.Hack();
```

# How where the old translations acquired?

This is more for documentation of what was done here - you don't have to do it.

The old translations can be found by downloading the 
[.NET Framework language packs](https://support.microsoft.com/en-us/topic/microsoft-net-framework-4-8-language-pack-offline-installer-for-windows-4bee217e-4096-6922-eba9-3e3c27342ff6).

Then, the `.exe` has to be extracted with e.g. 7-Zip. Then, extract the
`x64-Windows10.0-KB4073126-x64.cab` or similar. There should then be a
`system.componentmodel.dataannotations.resources.dll` in a directory like
`msil_system.componentmod..notations.resources_31bf3856ad364e35_4.0.15552.17062_de-de_b21fc9c5dc6a4ca2`.

Open that up in e.g. `dotPeek`. You should see the resx file now under `Resources`.

From there, just copy/paste the key/value pairs into whatever resx file you have set up.

Do note that these files contain a lot more translations than just the validation messages,
e.g. for thrown exceptions. Despite not being necessary, the original keys are left unchanged,
mostly to make them more easily identifiable/reproducable. This may also mean though that
you might get some localized exceptions when using the reflection hack.

# How exactly were the translations files in this repo created?

The initial English version was taken from the current `System.ComponentModel.DataAnnotations` in
net10. Then, a German version was extracted using the method above. There were a few keys missing:

**In netfx, but not in net10** (de in this example):

- (internal exception): `ArgumentIsNullOrWhiteSpace`
- (internal exception): `AttributeStore_Type_Must_Be_Public`
- (internal exception): `AttributeStore_Unknown_Method`
- (honestly, no idea): `Common_NullOrEmpty`
- (internal exception): `ValidationContextServiceContainer_ItemAlreadyExists`
- (internal exception): `ValidationContextServiceContainer_Must_Be_Method`

Most of these are for internal exceptions and not user-facing. The only exception is
`Common_NullOrEmpty`, of which I am unsure where it was used.

**In net10, but not in netfx** (en in this example):

- (user-facing): `AllowedValuesAttribute_Invalid`
- (user-facing): `Base64StringAttribute_Invalid`
- (user-facing): `DeniedValuesAttribute_Invalid`
- (internal exception): `LengthAttribute_InvalidMaxLength`
- (internal exception): `LengthAttribute_InvalidMinLength`
- (internal exception): `LengthAttribute_InvalidValueType`
- (user-facing): `LengthAttribute_ValidationError`
- (user-facing): `RangeAttribute_ValidationError_MaxExclusive`
- (user-facing): `RangeAttribute_ValidationError_MinExclusive`
- (user-facing): `RangeAttribute_ValidationError_MinExclusive_MaxExclusive`

As is clear in the list above, a lot of these are very much user-facing keys.  As a native german,
I (dominiksta) decided to **write my own German translations** of these keys and 
**use AI for the missing French keys** (For the time being. This library will likely see some use at
*my workplace at which point the French translations will be revisited).

For the non-user-facing keys I decided to simply use the English versions.
