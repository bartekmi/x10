using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using x10.utils;
using x10.parsing;
using x10.gen.sql;
using x10.gen.sql.primitives;
using x10.gen.hotchoc;
using x10.model;
using x10.model.metadata;
using x10.model.definition;
using x10.compiler;

using x10.hotchoc.Repositories;

namespace x10.hotchoc {
  public class DataIngest {
    private MessageBucket _messages; // Instantiated in Generate()

    public static void GenerateTestData(string x10ProjectDir, Repository repository) {
      DataIngest ingest = new DataIngest();
      ingest.Generate(x10ProjectDir, repository);
    }

    public void Generate(string x10ProjectDir, Repository repository) {
      _messages = new MessageBucket();

      FakeDataGenerator generator = GenerateData(x10ProjectDir);
      PopulateData(generator, repository);

      _messages.DumpErrors();
    }

    private FakeDataGenerator GenerateData(string x10ProjectDir) {
      EntitiesAndEnumsCompiler compiler = new EntitiesAndEnumsCompiler(_messages, new AllEnums(_messages), new AllFunctions(_messages));
      List<Entity> entities = compiler.Compile(x10ProjectDir);

      FakeDataGenerator generator = new FakeDataGenerator(_messages, entities, new Random(0), "../data") {
        AllowMultipleReverseAssociationsToSameEntity = true,
      };
      generator.GenerateData();
      return generator;
    }

    private void PopulateData(FakeDataGenerator generator, Repository repository) {
      foreach (EntityInfo entityInfo in generator.EntityInfos.Values) {
        foreach (Row row in entityInfo.Rows) {
          PrimordialEntityBase? instance = CreateAndPopulate(entityInfo.Entity, row);
          if (instance != null)
            repository.Add(row.Id, instance);
        }
      }
    }

    private PrimordialEntityBase CreateAndPopulate(Entity entity, Row row) {
      Type type = FindType(entity);

      PrimordialEntityBase? instance = (PrimordialEntityBase?)Activator.CreateInstance(type);
      if (instance == null)
        throw new Exception("Could not instantiate " + type.Name);

      PopulateEntity(instance, row);
      return instance;
    }

    private void PopulateEntity(PrimordialEntityBase instance, Row row) {
      instance.Dbid = row.Id;

      PopulateEmptyLists(instance);
      PopulateEntityAttributes(instance, row);
      PopulateEntityOwnedAssociations(instance, row);
    }

    private void PopulateEmptyLists(PrimordialEntityBase instance) {
      foreach (PropertyInfo prop in instance.GetType().GetProperties()) {
        if (prop.PropertyType.Name == "List`1") {
          Type type = prop.PropertyType.GenericTypeArguments[0];
          prop.SetValue(instance, CreateGenericList(type));
        }
      }
    }

    private void PopulateEntityAttributes(PrimordialEntityBase instance, Row row) {
      foreach (MemberAndValue field in row.Values) {
        Member member = field.Member;
        PropertyInfo prop = PropInfo(member, instance);

        object? value = field.Value;
        if (member is X10RegularAttribute attr) {
          if (attr.IsEnum)
            value = ToEnum(prop, value);
          if (attr.DataType == DataTypes.Singleton.String && value == null)
            value = "";   // We do not allow null strings in x10 because of ambiguity with empty string
        }

        prop.SetValue(instance, value);
      }
    }

    private void PopulateEntityOwnedAssociations(PrimordialEntityBase instance, Row row) {
      if (row.ChildAssociations == null)
        return;

      foreach (var keyAndValue in row.ChildAssociations) {
        Association association = keyAndValue.Key;
        PropertyInfo prop = PropInfo(association, instance);
        List<Row> childRows = keyAndValue.Value;
        List<PrimordialEntityBase> childEntities = childRows.Select(x => CreateAndPopulate(x.Entity, x)).ToList();

        if (association.IsMany) {
          Type childType = FindType(association.ReferencedEntity);
          var typeConvertedList = ConvertToSpecificList(childEntities, childType);
          prop.SetValue(instance, typeConvertedList);
        } else {
          if (childEntities.Count > 1)
            throw new Exception("Multiple child rows for single association: " + association);
          prop.SetValue(instance, childEntities.SingleOrDefault());
        }
      }
    }

    #region Utilities
    private static IList ConvertToSpecificList(List<PrimordialEntityBase> original, Type type) {
      IList typedList = CreateGenericList(type);
      if (typedList == null)
        throw new Exception("Could not create typed list");

      foreach (object item in original)
        typedList.Add(item);

      return typedList;
    }

    private static IList CreateGenericList(Type type) {
      var listType = typeof(List<>);
      var typedListType = listType.MakeGenericType(type);
      IList? list = (IList?)Activator.CreateInstance(typedListType);
      if (list == null)
        throw new Exception("Could not create list");
      return list;
    }

    private Type FindType(Entity entity) {
      Assembly assembly = Assembly.GetExecutingAssembly();
      string className = "x10.hotchoc.Entities." + entity.Name;
      Type? type = assembly.GetType(className);
      if (type == null)
        throw new Exception(string.Format("Could not find type '{0}' in Assembly '{1}'", className, assembly));

      return type;
    }

    private static object? ToEnum(PropertyInfo property, object value) {
      if (value == null)
        return null;

      Type type = property.PropertyType;
      Type? underlyingType = Nullable.GetUnderlyingType(type);
      if (underlyingType != null)
        type = underlyingType;

      if (Enum.TryParse(type, NameUtils.Capitalize(value.ToString()), out object? enumValue))
        return enumValue;

      return null;
    }

    private PropertyInfo PropInfo(Member member, PrimordialEntityBase instance) {
      string propName = HotchocCodeGenerator.PropName(member);
      PropertyInfo? prop = instance.GetType().GetProperty(propName);
      if (prop == null)
        throw new Exception(string.Format("Property does not exist: '{0}'", member));

      return prop;
    }
    #endregion
  }
}