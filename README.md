This project aims to use code-generation to short-circuit the development process from design to actual working code. It is called x10 because of the anticipated (minimum) gain of ten times in overall speed in going from full design to full implementation.

# Sub-Directories

## data

Data files for help with test data generations - things like list of nouns,
adjectives, cities, etc.

## hot_chocolate_small

Will eventually move to platforms. This is a complete working HotChocolate
GraphQL server setup along with generated code for the "small" project.

## platforms

Anything that is platform specific - i.e. wpf, react/flow, react/typescript,
etc, should go in here. This is a mix of hand-coded and generated code,
but it is all specific to one platform or another.

## x10

This is the core of the x10 system - it parses YAML files which contain
data about Data Models and XML files which describe UX layout and then
uses code-generators to auto-generated code for various UX and
server-side platforms.

## x10_csharp

## x10-test

Partner test project for x10.