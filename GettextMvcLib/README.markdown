Gettext MVC support
========================

Usage in MVC code, controllers or libraries.

```csharp
S._("Hello world from Controller code!");
S._("{fileCount} file", "{fileCount} files", i).FormatWith(new {fileCount = i})
```

Usage in Razor views.

```csharp
@Html._("Hello world from Razor View!")
@Html._("{fileCount} file", "{fileCount} files", 1).FormatWith(new {fileCount = 1})
```

Usage in JavaScript files (.js)

```javascript
Gettext._("Hello world from Javascript!")
Gettext._("{fileCount} file", "{fileCount} files", i).formatWith({ "fileCount": i })
```
