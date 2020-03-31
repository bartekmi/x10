using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using Xunit;
using Xunit.Abstractions;

using x10.parsing;
using x10.model.definition;
using x10.model.metadata;
using x10.model;

namespace x10.compiler {
  public class EntitiesCompilerPass2Test {

    private readonly ITestOutputHelper _output;
    private readonly MessageBucket _messages = new MessageBucket();

    public EntitiesCompilerPass2Test(ITestOutputHelper output) {
      _output = output;
    }

    [Fact]
    public void RehydrateAssociation() {
      Entity building = new Entity() {
        Name = "Building",
      };
      Association association = new Association() {
        ReferencedEntityName = "Apartment",
      };
      building.Members.Add(association);
      AddAttribute(association, "dataType", "Apartment");

      Entity apartment = new Entity() {
        Name = "Apartment",
      };

      RunTest(new Entity[] { building, apartment });
      Assert.Same(apartment, building.Associations.Single().ReferencedEntity);
    }

    [Fact]
    public void WrongDefaultValueType() {
      Entity entity = new Entity() {
        Name = "MyEntity",
      };
      X10RegularAttribute attribute = new X10RegularAttribute() {
        Name = "MyBooleanAttribute",
        DataTypeName = "Boolean",
        DefaultValueAsString = "7",
      };
      AddAttribute(attribute, "dataType", "Boolean");
      AddAttribute(attribute, "default", "7");

      entity.Members.Add(attribute);

      RunTest(new Entity[] { entity },
        "Could not parse a(n) Boolean from '7' for attribute 'default'. Examples of valid data of this type: True, False", 10, 20);
    }


    [Fact]
    public void RehydrateInheritanceParent() {
      Entity child = new Entity() {
        Name = "Child",
        InheritsFromName = "Parent",
      };
      AddAttribute(child, "inheritsFrom", "Parent");

      Entity parent = new Entity() {
        Name = "Parent",
      };

      RunTest(new Entity[] { child, parent });
      Assert.Same(parent, child.InheritsFrom);
    }

    [Fact]
    public void ReferenceNotFound() {
      Entity entity = CreateEntityWithInheritsFrom("DoesNotExist");
      RunTest(new Entity[] { entity }, "Entity 'DoesNotExist' not found", 10, 20);
    }

    [Fact]
    public void MultipleReferencesFound() {
      Entity entity = CreateEntityWithInheritsFrom("Duplicate");
      Entity duplicate1 = new Entity() {
        Name = "Duplicate",
      };
      Entity duplicate2 = new Entity() {
        Name = "Duplicate",
      };

      RunTest(new Entity[] { entity, duplicate1, duplicate2 },
        "Multiple entities with the name 'Duplicate' exist", 10, 20);
    }

    [Fact]
    public void UniquenessOfEntityNames() {
      Entity entity1 = CreateEntity("MyEntity");
      Entity entityUnique = CreateEntity("Unique");
      Entity entity2 = CreateEntity("MyEntity");

      RunTest(new Entity[] { entity1, entityUnique, entity2 },
        "The Entity name 'MyEntity' is not unique.", 10, 20);
    }

    [Fact]
    public void UniquenessOfEnumNames() {

      DataTypes.Singleton.AddModelEnum(CreateEnum("Duplicate"));
      DataTypes.Singleton.AddModelEnum(CreateEnum("Unique"));
      DataTypes.Singleton.AddModelEnum(CreateEnum("Duplicate"));

      RunTest(new Entity[] {},
        "The Enum name 'Duplicate' is not unique.", 10, 20);
    }

    #region Utilities

    private DataType CreateEnum(string name) {
      DataType theEnum = new DataType() {
        Name = name,
      };
      AddAttribute(theEnum, "name", name);
      return theEnum;
    }

    private Entity CreateEntity(string name) {
      Entity entity = new Entity() {
        Name = name,
      };

      AddAttribute(entity, "name", name);

      return entity;
    }

    private Entity CreateEntityWithInheritsFrom(string parentEntityName) {
      Entity entity = new Entity() {
        Name = "MyEntity",
        InheritsFromName = parentEntityName,
      };

      AddAttribute(entity, "inheritsFrom", parentEntityName);

      return entity;
    }

    private void AddAttribute(IAcceptsModelAttributeValues entity, string name, string value) {
      TreeElement element = new TreeScalar(null) {
        Start = new PositionMark() {
          LineNumber = 10,
          CharacterPosition = 20,
        }
      };

      AppliesTo appliesTo = AppliesToHelper.GetForObject(entity);
      entity.AttributeValues.Add(new ModelAttributeValue(element) {
        Value = value,
        Definition = ModelAttributeDefinitions.All
          .Single(x => x.Name == name && x.AppliesToType(appliesTo)),
      });
    }

    private void RunTest(IEnumerable<Entity> entities) {
      AllEntities allEntities = new AllEntities(entities, _messages);
      EntityCompilerPass2 compiler = new EntityCompilerPass2(_messages, allEntities);
      compiler.CompileAllEntities();
      TestUtils.DumpMessages(_messages, _output);
    }

    private void RunTest(IEnumerable<Entity> entities, string expectedErrorMessage, int expectedLine, int expectedChar) {
      RunTest(entities);

      CompileMessage message = _messages.Messages.FirstOrDefault(x => x.Message == expectedErrorMessage);
      Assert.NotNull(message);

      Assert.Equal(expectedLine, message.TreeElement.Start.LineNumber);
      Assert.Equal(expectedChar, message.TreeElement.Start.CharacterPosition);
    }
    #endregion
  }
}