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
      WriteLine(0, "using System;");
      WriteLine(0, "using System.Collections.Generic;");
      WriteLine(0, "using System.Linq;");
      WriteLine();
      WriteLine(0, "using x10.hotchoc.Entities;");
      WriteLine();
      WriteLine(0, "namespace x10.hotchoc.Repositories {");
    }

    public static string GetterName(Entity entity) {
      return "Get" + entity.Name;
    }

    private void GenerateRepositoryInterfaceQueries() {
      WriteLine(2, "// Queries");

      foreach (Entity entity in ConcreteEntities())
        WriteLine(2, "{0} {1}(int id);", entity.Name, GetterName(entity));
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

    #region Generate Repository - Implementation
    private void GenerateRepository() {
      Begin("Repositories/Repository.cs");

      GenerateRepositoryHeader();
      WriteLine(1, "public class Repository : IRepository {");

      GenerateRepositoryDictionaries();
      GenerateRepositoryAddMethod();
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

    private void GenerateRepositoryAddMethod() {
      WriteLine(2, "internal void Add(int id, PrimordialEntityBase instance) {");

      foreach (Entity entity in ConcreteEntities()) {
        string entityName = entity.Name;
        string varName = NameUtils.UncapitalizeFirstLetter(entityName);
        string pluralUpper = NameUtils.Pluralize(entityName);
        string pluralLower = NameUtils.UncapitalizeFirstLetter(pluralUpper);

        WriteLine(3, "if (instance is {0} {1}) _{2}[id] = {1};",
          entityName, varName, pluralLower);
      }

      WriteLine(2, "}");
      WriteLine();
    }

    private void GenerateRepositoryImplementations() {
      foreach (Entity entity in ConcreteEntities()) {
        string entityName = entity.Name;
        string varName = NameUtils.UncapitalizeFirstLetter(entityName);
        string pluralUpper = NameUtils.Pluralize(entityName);
        string pluralLower = NameUtils.UncapitalizeFirstLetter(pluralUpper);
        string getterName = GetterName(entity);

        WriteLine(2, "#region {0}", pluralUpper);

        // Get multiple
        WriteLine(2, "public IQueryable<{0}> Get{1}() => _{2}.Values.AsQueryable();",
          entityName, pluralUpper, pluralLower);

        // Get single
        WriteLine(2, "public {0} {1}(int id) { return _{2}[id]; }", entityName, getterName, pluralLower);

        // Add or Update
        WriteLine(2, "public int AddOrUpdate{0}(int? dbid, {0} {1}) {", entityName, varName);
        WriteLine(3, "return RepositoryUtils.AddOrUpdate(dbid, {0}, _{1});", varName, pluralLower);
        WriteLine(2, "}");

        WriteLine(2, "#endregion");
        WriteLine();
      }
    }
    #endregion

    #region Generate Queries
    private void GenerateQueries() {
      Begin("Queries.cs");

      GenerateQueriesHeader();

      foreach (Entity entity in ConcreteEntities()) {
        WriteLine(2, "#region {0}", entity.Name);
        GenerateGetOne(entity);
        GenerateGetAll(entity);
        WriteLine(2, "#endregion");
        WriteLine();
      }

      WriteLine(1, "}");
      WriteLine(0, "}");

      End();
    }

    private void GenerateQueriesHeader() {
      WriteRaw(
@"using System.Collections.Generic;

using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

using x10.hotchoc.Entities;
using x10.hotchoc.Repositories;

namespace x10.hotchoc {{
  [ExtendObjectType(Name = ""Query"")]
  public class Queries {{

");
    }

    private void GenerateGetOne(Entity entity) {
      WriteRaw(
@"    /// <summary>
    /// Retrieve a {0} by id
    /// </summary>
    /// <param name=""id"">The id of the {0}.</param>
    /// <param name=""repository""></param>
    /// <returns>The {0}.</returns>
    public {0} Get{0}(
        string id,
        [Service] IRepository repository) =>
          repository.Get{0}(IdUtils.FromRelayIdMandatory(id));

", entity.Name);
    }

    private void GenerateGetAll(Entity entity) {
      WriteRaw(
@"    /// <summary>
    /// Gets all {0}.
    /// </summary>
    /// <param name=""repository""></param>
    /// <returns>All {0}.</returns>
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IEnumerable<{1}> Get{0}(
        [Service] IRepository repository) =>
          repository.Get{0}();
",
      NameUtils.Pluralize(entity.Name),
      entity.Name);
    }
    #endregion

    #region Generation Mutations
    private void GenerateMutations() {
      Begin("Mutations.cs");

      GenerateMutationsHeader();

      foreach (Entity entity in ConcreteEntities()) {
        WriteLine(2, "#region {0}", entity.Name);
        GenerateCreateOrUpdate(entity);
        WriteLine(2, "#endregion");
        WriteLine();
      }

      WriteLine(1, "}");
      WriteLine(0, "}");

      End();
    }

    private void GenerateMutationsHeader() {
      WriteRaw(
@"using System;
using System.Collections.Generic;
using System.Linq;

using HotChocolate;
using HotChocolate.Types;

using x10.hotchoc.Entities;
using x10.hotchoc.Repositories;

namespace x10.hotchoc {{
  [ExtendObjectType(Name = ""Mutation"")]
  public class Mutations {{

");
    }

    private void GenerateCreateOrUpdate(Entity entity) {
      string entityName = entity.Name;
      string varName = NameUtils.UncapitalizeFirstLetter(entityName);
      IEnumerable<Member> writableMembers = entity.Members.Where(x => !x.IsReadOnly);

      WriteRaw(
@"    /// <summary>
    /// Creates a new {0} or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdate{0}(
", entityName);

      // Method parameters
      WriteLine(4, "string id,");
      foreach (Member member in writableMembers) 
        WriteLine(4, "{0} {1},", GetDataType(member), member.Name);

      WriteLine(4, "[Service] IRepository repository) {");
      WriteLine();

      // Instantiate entity
      WriteLine(3, "{0} {1} = new {0}() {", entityName, varName);

      foreach (Member member in writableMembers) 
        WriteLine(4, "{0} = {1}{2},", 
          PropName(member), 
          member.Name,
          member is Association assoc && assoc.IsMany ? ".ToList()" : "");

      WriteLine(3, "};");
      WriteLine();

      // Method body and return
      WriteLine(3, "int dbid = repository.AddOrUpdate{0}(IdUtils.FromRelayId(id), {1});",
        entityName, varName);
      WriteLine(3, "return IdUtils.ToRelayId<{0}>(dbid);", entityName);
      WriteLine(2, "}");
    }
    #endregion
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
      GenerateEnsureUniqueDbid(entity);

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
          DataType(attribute.DataType, false),
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

      WriteLine(2, "public string? ToStringRepresentation {", formula);
      WriteLine(3, "get { return {0}; }", formula);
      WriteLine(3, "set { /* Needed to make Hot Chocolate happy */ }");
      WriteLine(2, "}");

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

    private void GenerateEnsureUniqueDbid(Entity entity) {
      string entityName = entity.Name;
      string varName = NameUtils.UncapitalizeFirstLetter(entityName);

      WriteLine(2, "public override void EnsureUniqueDbid() {");
      WriteLine(3, "base.EnsureUniqueDbid();");

      foreach (Association association in entity.Associations.Where(x => x.Owns)) {
        string propName = PropName(association);

        if (association.IsMany)
          WriteLine(3, "{0}?.ForEach(x => x.EnsureUniqueDbid());", propName);
        else
          WriteLine(3, "{0}?.EnsureUniqueDbid();", propName);
      }

      WriteLine(2, "}");
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

    private IEnumerable<Entity> ConcreteEntities() {
      return AllEntities.All.Where(x => !x.IsAbstract && !x.IsContext);
    }

    public static string PropName(Member member) {
      return NameUtils.CapitalizeFirstLetter(member.Name);
    }

    private static bool NonNull(X10Attribute attribute) {
      DataType dataType = attribute.DataType;
      if (dataType == DataTypes.Singleton.String ||
          dataType == DataTypes.Singleton.Boolean)
        return true;

      return attribute.IsMandatory;
    }

    private static string GetDataType(Member member) {
      if (member is X10Attribute attribute)
        return DataType(attribute.DataType, attribute.IsMandatory);
      else if (member is Association association) {
        Entity refedEntity = association.ReferencedEntity;
        if (association.IsMany)
          return string.Format("IEnumerable<{0}>", refedEntity.Name);
        else
          return refedEntity.Name + NullableMarker(association.IsMandatory);
      } else
        throw new NotImplementedException("Neither attribute nor association");
    }

    private static string NullableMarker(bool isMandatory) {
      return isMandatory ? "" : "?";
    }

    private static string DataType(DataType dataType, bool isMandatory) {
      // Booleans are never optional
      if (dataType == DataTypes.Singleton.Boolean) return "bool";

      string type = null;
      if (dataType == DataTypes.Singleton.Date) type = "DateTime";
      if (dataType == DataTypes.Singleton.Float) type = "double";
      if (dataType == DataTypes.Singleton.Integer) type = "int";
      if (dataType == DataTypes.Singleton.String) type = "string";
      if (dataType == DataTypes.Singleton.Timestamp) type = "DateTime";
      if (dataType == DataTypes.Singleton.Money) type = "double";
      if (dataType is DataTypeEnum enumType) type = EnumToName(enumType);

      if (type == null)
        throw new Exception("Unknown data type: " + dataType.Name);

      return type + NullableMarker(isMandatory);
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