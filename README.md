# Visual C++ PackageReference Support

This repository contains a NuGet package that enables the use of PackageReference in
Visual C++ projects (`*.vcxproj` files).

## How to use this package

You must add the following MSBuild verbiage in a `Directory.Build.props` file in a
directory above all your C++ projects (the directory containing the solution file
is usually a good spot for this). Referencing this package in any other way (including
as an entry in a `packages.config` file) _will do nothing_.

```xml
<Project>
  <Sdk Name="Sunburst.VisualCpp.PackageReference" Version="1.0.0" />
</Project>
```

Once this is done, any C++ projects will now restore PackageReferences before building.
Just like in C# projects, you specify these with a `<PackageReference />` item in your
project file. Since unloading and reloading project files to edit them gets to be a pain
after doing it over and over, this package automatically imports a file called `PackageReferences.props`
in the same directory as your project file, if it exists. You can put your PackageReference
items there and they will be picked up automatically.

Note that due to a limitation in the C++ build system, if the PackageReference used by
a project change, the build must be restarted after they are restored. An error will be
emitted telling you this when necessary.

Although this package will be imported in all other projects in the directory tree
as well (because SDK references cannot be conditionalized), this package will detect
if it has been included in a project that is not a C++ project, and do nothing in that case.
