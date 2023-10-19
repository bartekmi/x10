# Sub-Directories

## Compiler - (C#)

* Compiler for Models
* Compiler for UX
* Uber-compiler for all of the above

## Doc (GENERATED - Markdown)

x10 libraries have an auto-documentation feature where the components
themselves each contain general and property-specific descriptions.
This folder holds an instance of generating the docs for all libraries.

## Examples (YAML / XML)

Hand-coded examples of x10 meta-files - ie. models and UX

The most important is the one called "small" - this is meant to be a
minimalist hypothetical working examples of a property management
solftware solution that nevertheless excercises all features of x10.

## Formula (C#)

Libraries for dealing with formula parsing

## Gen (C#)

Code for all code generation. Each directory under here has all the code
to generate a particular technology - wpf / react&flow / sql / hot chocolate.
The root folder holds a few general/utility files.

## Gql (C#)

This looks like a foray into the world of GraphQL parsing in C#. Apparently
not used. Keeping this around in case it may be useful in the future.

## Model (C#)

All things to do with parsing Data Model YAML files.

## Parsing (C#)

Utilities for parsing YAML and XML in a way where each token can be
traced to its original file, line and character position.

## UI (C#)

All things to do with parsing UX XML files.

## Utils (C#)

Miscellenaous code for things like converting name formats, dealing
with enums, graphs, yaml, etc.