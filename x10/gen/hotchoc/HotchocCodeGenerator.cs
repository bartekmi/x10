using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FileInfo = x10.parsing.FileInfo;
using x10.utils;
using x10.formula;
using x10.gen.wpf;
using x10.model;
using x10.model.definition;
using x10.model.metadata;
using x10.ui.composition;
using x10.compiler;
using x10.ui;

namespace x10.gen.hotchoc {
  public class HotchocCodeGenerator : CodeGenerator {
    public string PackageName { get; set; }
    public string CustomMutationsClass { get; set; }

    public override void Generate(ClassDefX10 classDef) { }

    #region Generate Common
    public override void GenerateCommon() {
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
      WriteLine(0, "using x10.hotchoc.{0}.Entities;", PackageName);
      WriteLine();
      WriteLine(0, "namespace x10.hotchoc.{0}.Repositories {", PackageName);
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
      WriteLine(1, "public class Repository : RepositoryBase, IRepository {");

      GenerateRepositoryDictionaries();
      GenerateRepositoryTypes();
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

    private void GenerateRepositoryTypes() {
      WriteLine(2, "public override IEnumerable<Type> Types() {");
      WriteLine(3, "return new Type[] {");
      WriteLine(4, "typeof(Queries),");
      WriteLine(4, "typeof({0}),", CustomMutationsClass ?? "Mutations");

      foreach (Entity entity in ConcreteEntities())
        WriteLine(4, "typeof({0}),", entity.Name);

      WriteLine(3, "};");
      WriteLine(2, "}");
      WriteLine();
    }

    private void GenerateRepositoryAddMethod() {
      WriteLine(2, "public override void Add(PrimordialEntityBase instance) {");
      WriteLine(3, "int id = instance.DbidHotChoc;");
      WriteLine();

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

using x10.hotchoc.{0}.Entities;
using x10.hotchoc.{0}.Repositories;

namespace x10.hotchoc.{0} {{
  [ExtendObjectType(Name = ""Query"")]
  public partial class Queries {{

", PackageName);
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
          repository.Get{0}(IdUtils.FromFrontEndIdMandatory(id));

", entity.Name);
    }

    private void GenerateGetAll(Entity entity) {
      WriteRaw(
@"    /// <summary>
    /// Gets all {0}.
    /// </summary>
    /// <param name=""repository""></param>
    /// <returns>All {0}.</returns>
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

    #region Generate Mutations
    private void GenerateMutations() {
      Begin("MutationsSpecificUpdate.cs");

      GenerateMutationsHeader();

      foreach (ClassDefX10 classDef in AllUiDefinitions.All) {
        Entity model = classDef.ComponentDataModel;
        if (UiUtils.IsForm(classDef) && model != null) {
          WriteLine(2, "#region {0}", classDef.Name);
          GenerateMutationInputType(classDef, model);
          WriteLine();
          GenerateMutationMethodPrototype(classDef, model);
          WriteLine(2, "#endregion");
          WriteLine();
        }
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

using x10.hotchoc.{0}.Entities;
using x10.hotchoc.{0}.Repositories;

namespace x10.hotchoc.{0} {{
  [ExtendObjectType(Name = ""Mutation"")]
  public partial class Mutations {{

", PackageName);
    }

    private void GenerateMutationInputType(ClassDefX10 classDef, Entity model) {
      MemberWrapper dataInventory = UiComponentDataCalculator.ExtractData(classDef);

      WriteLine(2, "/// <summary>");
      WriteLine(2, "/// Input Data Type for {0}Update{1} Mutation", classDef.Name, model.Name);
      WriteLine(2, "/// </summary>");
      WriteLine(2, "public class {0}{1} : Base {", classDef.Name, model.Name);

      foreach (MemberWrapper wrapper in dataInventory.Children) {
        Member member = wrapper.Member;
        if (member is X10RegularAttribute regular) {
          if (!regular.IsId)
            GenerateRegularAttribute(3, regular);
        } else if (member is Association association) {
          string propName = PropName(association);
          Entity refedEntity = association.ReferencedEntity;

          if (association.IsMany) {
            if (!association.Owns)
              // A work-around is to strucutre your data model to created an owned child entity
              // which then links to the non-owned entity
              throw new NotImplementedException("Non-owned 'many' association");

            WriteLine(3, "[GraphQLNonNullType]");
            WriteLine(3, "public List<{0}>? {1} { get; set; }", refedEntity.Name, propName);
          } else {
            if (association.IsMandatory)
              WriteLine(3, "[GraphQLNonNullType]");
            if (association.Owns)
              WriteLine(3, "public {0} {1} { get; set; }", refedEntity.Name, propName);
            else
              WriteLine(3, "public IdWrapper? {0} { get; set; }", propName);
          }
        } else
          throw new NotImplementedException("Anything coming back from MemberWrapper should be regular attr or association");
      }

      WriteLine(2, "}");
    }

    private void GenerateMutationMethodPrototype(ClassDefX10 classDef, Entity model) {
      WriteRaw(
@"    /// <summary>
    /// Update mutation for the {0} component
    /// </summary>
    public virtual {1} {0}Update{1}(
      {0}{1} data,
      [Service] IRepository repository) {{
        throw new NotImplementedException(""Manually override this method"");
    }}
",
      classDef.Name,
      model.Name);
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
      WriteLine(0, "using x10.hotchoc.{0}.Repositories;", PackageName);
      WriteLine();
      WriteLine(0, "namespace x10.hotchoc.{0}.Entities {", PackageName);

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

      // Commenting out for the following reasons: 
      // * current HotChocolate implementation was embedding this in mutation input gql,
      //   which is causing issues while trying to execute mutations.
      // * The original goal of this was to support AssociationEditor, which would 
      //   make use of it to show a drop-down list of human-readable choices.
      // * We should have equal success using x10 "derived properties" without introducing
      //   a new concept, even at the expense of slightly more data transmitted over-
      //   the wire.
      // * Keeping this code for now for a potential future optimization.
      // GenerateToStringRepresentation(entity);
      
      GenerateAssociations(entity);
      GenerateEnsureUniqueDbid(entity);
      GenerateSetNonOwnedAssociations(entity);

      WriteLine(1, "}");
    }

    private void GenerateRegularAttributes(Entity entity) {
      IEnumerable<X10RegularAttribute> attributes = entity.RegularAttributes
        .Where(x => !x.IsId);
      if (attributes.Count() == 0)
        return;

      WriteLine(2, "// Regular Attributes");

      foreach (X10RegularAttribute attribute in attributes)
        GenerateRegularAttribute(2, attribute);

      WriteLine();
    }

    private void GenerateRegularAttribute(int level, X10RegularAttribute attribute) {
      WriteLine(level, "public {0} {1} { get; set; }",
        DataType(attribute.DataType, false),
        PropName(attribute));
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
      WriteLine();
    }


    private void GenerateSetNonOwnedAssociations(Entity entity) {
      bool hasParent = entity.InheritsFromName != null;
      WriteLine(2, "internal {0} void SetNonOwnedAssociations(IRepository repository) {",
        hasParent ? "override" : "virtual");
      if (hasParent)
        WriteLine(3, "base.SetNonOwnedAssociations(repository);");

      foreach (Association association in entity.Associations) {
        Entity refedEntity = association.ReferencedEntity;
        string varName = association.Name;
        string propName = PropName(association);

        WriteLine();

        if (association.IsMany) {
          WriteLine(3, "if ({0} != null)", propName);
          WriteLine(4, "foreach ({0} {1} in {2})", refedEntity.Name, varName, propName);
          WriteLine(5, "{0}.SetNonOwnedAssociations(repository);", varName);
        } else {
          if (association.Owns) {
            WriteLine(3, "{0}?.SetNonOwnedAssociations(repository);", propName);
          } else {
            WriteLine(3, "int? {0} = IdUtils.FromFrontEndId({1}?.Id);", varName, propName);
            WriteLine(3, "{0} = {1} == null ? null : repository.Get{2}({1}.Value);",
              propName, varName, refedEntity.Name);
          }
        }
      }

      WriteLine(2, "}");
    }

    #endregion
    #endregion

    #region Generate Enum Files
    public override void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums) {
      Begin(fileInfo, ".cs");

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

      // See note on NullableMarker()
      return false;
      //return attribute.IsMandatory;
    }

    private static string NullableMarker(bool isMandatory) {
      // For now, making all fields optional because we do not distinguish between
      // 'mandatory' for UI purposes vs. in the model. This is TBD.
      return "?";
      //return isMandatory ? "" : "?";
    }

    private static string DataType(DataType dataType, bool isMandatory) {
      // Booleans are never optional
      if (dataType == DataTypes.Singleton.Boolean) return "bool";

      string type = null;
      if (dataType == DataTypes.Singleton.Date) type = "DateTime";
      if (dataType == DataTypes.Singleton.Time) type = "string";
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