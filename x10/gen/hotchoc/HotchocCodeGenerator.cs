using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FileInfo = x10.parsing.FileInfo;
using x10.compiler;
using x10.model;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.composition;
using x10.utils;
using x10.ui.platform;
using x10.parsing;
using x10.formula;
using x10.gen.wpf;

namespace x10.gen.hotchoc {
  public class HotchocCodeGenerator : CodeGenerator {
    public override void Generate(ClassDefX10 classDef) { }

    #region Generate Common
    public override void GenerateCoomon() {
      GenerateRepositoryInterface();
      GenerateRepository();
      GenerateQueries();
      GenerateMutations();
    }

    #region Generate Repository - Interface
    private void GenerateRepositoryInterface() {
      Begin("Repositories/IRepository.cs");

      GenerateRepositoryHeader();
      WriteLine(1, "public interface IRepository {");

      GenerateRepositoryInterfaceQueries();
      GenerateRepositoryInterfaceMutations();

      WriteLine(1, "}");
      WriteLine(0, "}");

      End();
    }

    private void GenerateRepositoryHeader() {
      WriteLine(0, "using System.Collections.Generic;");
      WriteLine(0, "using System.Linq;");
      WriteLine();
      WriteLine(0, "using x10.hotchoc.Entities;");
      WriteLine();
      WriteLine(0, "namespace x10.hotchoc.Repositories {");
    }

    private IEnumerable<Entity> ConcreteEntities() {
      return AllEntities.All.Where(x => !x.IsAbstract && !x.IsContext);
    }

    private void GenerateRepositoryInterfaceQueries() {
      WriteLine(2, "// Queries");

      foreach (Entity entity in ConcreteEntities())
        WriteLine(2, "{0} Get{0}(int id);", entity.Name);
      WriteLine();

      foreach (Entity entity in ConcreteEntities())
        WriteLine(2, "IQueryable<{0}> Get{1}();", entity.Name, NameUtils.Pluralize(entity.Name));
      WriteLine();
    }

    private void GenerateRepositoryInterfaceMutations() {
      WriteLine(2, "// Mutations");

      foreach (Entity entity in ConcreteEntities()) {
        string varName = NameUtils.UncapitalizeFirstLetter(entity.Name);
        WriteLine(2, "int AddOrUpdate{0}(int? dbid, {0} {1});", entity.Name, varName);
      }
    }
    #endregion

    private void GenerateRepository() {
      Begin("Repositories/Repository.cs");

      GenerateRepositoryHeader();
      WriteLine(1, "public class Repository : IRepository {");

      GenerateRepositoryDictionaries();
      GenerateRepositoryConstructor();
      GenerateRepositoryImplementations();

      WriteLine(1, "}");
      WriteLine(0, "}");

      End();
    }

    private void GenerateRepositoryDictionaries() {
      foreach (Entity entity in ConcreteEntities())
        WriteLine(2, "private Dictionary<int, {0}> _{1} = new Dictionary<int, {0}>();", 
          entity.Name,
          NameUtils.UncapitalizeFirstLetter(NameUtils.Pluralize(entity.Name)));

      WriteLine();
    }


    private void GenerateRepositoryConstructor() {
      WriteLine(2, "public Repository() {");

      WriteLine(3, "// Do nothing");
          
      WriteLine(2, "}");
      WriteLine();
    }

    private void GenerateRepositoryImplementations() {
      foreach (Entity entity in ConcreteEntities()) {
        string entityName = entity.Name;
        string varName = NameUtils.UncapitalizeFirstLetter(entityName);
        string pluralUpper = NameUtils.Pluralize(entityName);
        string pluralLower = NameUtils.UncapitalizeFirstLetter(pluralUpper);

        WriteLine(2, "#region {0}", pluralUpper);

        // Get multiple
        WriteLine(2, "public IQueryable<{0}> Get{1}() => _{2}.Values.AsQueryable();",
          entityName, pluralUpper, pluralLower);
        WriteLine();

        // Get single
        WriteLine(2, "public {0} Get{0}(int id) { return _{1}[id]; }", entityName, pluralLower);
        WriteLine();

        // Add or Update
        WriteLine(2, "public int AddBuilding({0} {1}) {", entityName, varName);
        WriteLine(3, "return RepositoryUtils.AddOrUpdate(dbid, {0}, _{1}, EnsureUniqueDbids);",
          varName, pluralLower);
        WriteLine(2, "}");
        WriteLine();

        GenerateEnsureUniqueDbids(entity);

        WriteLine(2, "#endregion");
      }
    }

    private void GenerateEnsureUniqueDbids(Entity entity) {
        string entityName = entity.Name;
        string varName = NameUtils.UncapitalizeFirstLetter(entityName);

        WriteLine(2, "public int EnsureUniqueDbids({0} {1}) {", entityName, varName);



        WriteLine(2, "}");
    }

    

    private void GenerateQueries() {

    }

    private void GenerateMutations() {

    }
    #endregion

    #region Generate Entity
    public override void Generate(Entity entity) {
      if (entity.IsContext)
        return;

      Begin(entity.TreeElement.FileInfo, ".cs");

      WriteLine(0, "using System;");
      WriteLine(0, "using System.Collections.Generic;");
      WriteLine();
      WriteLine(0, "using HotChocolate;");
      WriteLine();
      WriteLine(0, "namespace x10.hotchoc.Entities {");

      GenerateEnums(entity);
      GenerateMainEntity(entity);

      WriteLine(0, "}");
      WriteLine();

      End();
    }

    #region Generate Enums
    private void GenerateEnums(Entity entity) {
      IEnumerable<DataTypeEnum> enums = FindLocalEnums(entity);
      if (enums.Count() == 0)
        return;

      WriteLine(1, "// Enums");

      foreach (DataTypeEnum theEnum in enums)
        GenerateEnum(1, theEnum);

      WriteLine();
    }
    #endregion

    #region Generate Main Entity

    private void GenerateMainEntity(Entity entity) {
      WriteLine(1, "/// <summary>");
      WriteLine(1, "/// {0}", entity.Description);
      WriteLine(1, "/// </summary>");
      WriteLine(1, "public {0}class {1} : {2} {",
        entity.IsAbstract ? "abstract " : "",
        entity.Name,
        entity.InheritsFromName == null ? "PrimordialEntityBase" : entity.InheritsFrom.Name);

      GenerateRegularAttributes(entity);
      GenerateToStringRepresentation(entity);
      GenerateAssociations(entity);

      WriteLine(1, "}");
    }

    private void GenerateRegularAttributes(Entity entity) {
      IEnumerable<X10RegularAttribute> attributes = entity.RegularAttributes
        .Where(x => !x.IsId);
      if (attributes.Count() == 0)
        return;

      WriteLine(2, "// Regular Attributes");

      foreach (X10RegularAttribute attribute in attributes) {
        if (NonNull(attribute))
          WriteLine(2, "[GraphQLNonNullType]");
        WriteLine(2, "public {0} {1} { get; set; }",
          DataType(attribute),
          PropName(attribute));
      }

      WriteLine();
    }

    private void GenerateToStringRepresentation(Entity entity) {
      if (entity.IsAbstract)
        return;

      WriteLine(2, "// To String Representation");
      WriteLine(2, "[GraphQLNonNullType]");

      string formula = entity.StringRepresentation == null ?
        string.Format("\"{0}: \" + Dbid", entity.Name) :
        ExpressionToString(entity.StringRepresentation);

      WriteLine(2, "public string? ToStringRepresentation => {0};", formula);

      WriteLine();
    }

    private void GenerateAssociations(Entity entity) {
      IEnumerable<Association> associations = entity.Associations;
      if (associations.Count() == 0)
        return;

      WriteLine(2, "// Associations");

      foreach (Association association in associations) {
        Entity refedEntity = association.ReferencedEntity;
        string propName = PropName(association);

        if (association.IsMany) {
          WriteLine(2, "[GraphQLNonNullType]");
          WriteLine(2, "public List<{0}>? {1} { get; set; }", refedEntity.Name, propName);
        } else {
          if (association.IsMandatory)
            WriteLine(2, "[GraphQLNonNullType]");
          WriteLine(2, "public {0}? {1} { get; set; }", refedEntity.Name, propName);
        }
      }

      WriteLine();
    }
    #endregion
    #endregion

    #region Generate Enum Files
    public override void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums) {
      Begin(fileInfo, ".cs");

      WriteLine(0, "using wpf_lib.lib.attributes;");
      WriteLine();

      foreach (DataTypeEnum anEnum in enums)
        GenerateEnum(0, anEnum);

      End();
    }
    #endregion

    #region Utils
    private static string PropName(Member member) {
      return NameUtils.CapitalizeFirstLetter(member.Name);
    }

    private static bool NonNull(X10Attribute attribute) {
      DataType dataType = attribute.DataType;
      if (dataType == DataTypes.Singleton.String ||
          dataType == DataTypes.Singleton.Boolean)
        return true;

      return attribute.IsMandatory;
    }

    private static string DataType(X10Attribute attribute) {
      DataType dataType = attribute.DataType;

      if (dataType == DataTypes.Singleton.Boolean) return "bool";
      if (dataType == DataTypes.Singleton.Date) return "DateTime?";
      if (dataType == DataTypes.Singleton.Float) return "double?";
      if (dataType == DataTypes.Singleton.Integer) return "int?";
      if (dataType == DataTypes.Singleton.String) return "string?";
      if (dataType == DataTypes.Singleton.Timestamp) return "DateTime?";
      if (dataType == DataTypes.Singleton.Money) return "double?";
      if (dataType is DataTypeEnum enumType) return EnumToName(enumType) + "?";

      throw new Exception("Unknown data type: " + dataType.Name);
    }

    private static string EnumToName(DataTypeEnum enumType) {
      return enumType.Name + "Enum";
    }

    private static string ExpressionToString(ExpBase expression) {
      if (expression == null)
        return "EXPRESSION MISSING";

      using StringWriter writer = new StringWriter();

      WpfFormulaWriter formulaWriterVisitor = new WpfFormulaWriter(writer, false);
      expression.Accept(formulaWriterVisitor);
      return writer.ToString();
    }

    public void GenerateEnum(int level, DataTypeEnum theEnum) {
      WriteLine(level, "public enum {0} {", WpfGenUtils.EnumToName(theEnum));

      foreach (EnumValue enumValue in theEnum.EnumValues)
        WriteLine(level + 1, "{0},", enumValue.ValueUpperCased);

      WriteLine(level, "}");
      WriteLine();
    }
    #endregion
  }
}