global using Xunit;
global using FluentAssertions;
global using System.Text.Json;
global using System.Text.Json.Serialization;
using System.Globalization;

// Keep tests isolated from machine culture surprises (e.g., decimal commas)
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]
