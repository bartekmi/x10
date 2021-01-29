using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using x10.utils;
using x10.parsing;
using x10.gen.sql;
using x10.gen.sql.primitives;
using x10.gen.hotchoc;
using x10.model;
using x10.model.definition;
using x10.compiler;

using x10.hotchoc.Repositories;

namespace x10.hotchoc {
  public static class DataIngest {
    public static void GenerateTestData(string x10ProjectDir, Repository repository) {
      MessageBucket messages = new MessageBucket();
      FakeDataGenerator generator = GenerateData(messages, x10ProjectDir);
      PopulateData(messages, generator, repository);

      messages.DumpErrors();
    }

    private static FakeDataGenerator GenerateData(MessageBucket messages, string x10ProjectDir) {
      EntitiesAndEnumsCompiler compiler = new EntitiesAndEnumsCompiler(messages, new AllEnums(messages), new AllFunctions(messages));
      List<Entity> entities = compiler.Compile(x10ProjectDir);

      FakeDataGenerator generator = new FakeDataGenerator(messages, entities, new Random(0), "../data") {
        AllowMultipleReverseAssociationsToSameEntity = true,
      };
      generator.GenerateData();
      return generator;
    }

    private static void PopulateData(MessageBucket messages, FakeDataGenerator generator, Repository repository) {
      foreach (EntityInfo entityInfo in generator.EntityInfos.Values) {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string className = "x10.hotchoc.Entities." + entityInfo.Entity.Name;
        Type? type = assembly.GetType(className);
        if (type == null) {
          messages.AddError(null, string.Format("Could not find type '{0}' in Assembly '{1}'", className, assembly));
          continue;
        }

        foreach (Row row in entityInfo.Rows) {
          PrimordialEntityBase? instance = (PrimordialEntityBase?)Activator.CreateInstance(type);
          if (instance == null)
            throw new Exception("Could not instantiate " + type.Name);

          PopulateEntity(instance, row);
          repository.Add(row.Id, instance);
        }
      }
    }
    
    private static void PopulateEntity(PrimordialEntityBase instance, Row row) {
      instance.Dbid = row.Id;

      foreach (MemberAndValue field in row.Values) {
        Member member = field.Member;

        string propName = HotchocCodeGenerator.PropName(member);
        PropertyInfo? prop = instance.GetType().GetProperty(propName);
        if (prop == null)
          throw new Exception(string.Format("Property does not exist: '{0}'", member));

        object? value = field.Value;
        if (member is X10RegularAttribute attr && attr.IsEnum)
          value = ToEnum(prop, value);

        prop.SetValue(instance, value);
      }
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
  }
}