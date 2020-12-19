using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FileInfo = x10.parsing.FileInfo;
using x10.compiler;
using x10.formula;
using x10.model;
using x10.model.definition;
using x10.model.libraries;
using x10.model.metadata;
using x10.parsing;
using x10.ui.composition;
using static x10.ui.metadata.ClassDefNative;
using x10.utils;
using x10.ui.platform;
using x10.ui;
using x10.gen.wpf.codelet;
using x10.ui.metadata;

namespace x10.gen.wpf {
  public class ReactCodeGenerator : CodeGenerator {

    private readonly string _defaultNamespace;

    public ReactCodeGenerator(
      MessageBucket messages,
      string rootGenerateDir,
      string defaultNamespace,
      AllEntities allEntities,
      AllEnums allEnums,
      AllUiDefinitions allUiDefinitions,
      IEnumerable<PlatformLibrary> platformLibraries
      ) : base(messages, rootGenerateDir, allEntities, allEnums, allUiDefinitions, platformLibraries) {
      _defaultNamespace = defaultNamespace;
    }

    #region Generate XAML, XAML.cs, VM (View Model), Custom VM

    public override void Generate(ClassDefX10 classDef) {
    }

    // Some trivial components might not have a data model - perhaps just text, etc
    private bool GenerateVM(ClassDefX10 classDef) {
      return classDef.ComponentDataModel != null;
    }

    private void GenerateViewModelCustomFile(ClassDefX10 classDef) {
      // TODO
    }
    #endregion

    #region Generate Models
    public override void Generate(Entity entity) {
    }

    #region Create Default Entity
    private void GenerateCreateFunction(Entity entity) {
      WriteLine();
      WriteLine(2, "public static {0} Create(EntityBase owner) {", entity.Name);
      WriteLine(3, "{0} newEntity = new {0} {", entity.Name);
      WriteLine(4, "Owner = owner,");

      foreach (X10RegularAttribute attribute in entity.RegularAttributes) {
        ModelAttributeValue defaultValue = attribute.FindAttribute(BaseLibrary.DEFAULT);
        if (defaultValue != null) {
          WriteLine(4, "{0} = {1},",
            WpfGenUtils.MemberToName(attribute),
            // AttributeValueToString(defaultValue));
        }
      }
      WriteLine(3, "};");
      WriteLine();

      IEnumerable<Association> associations = entity.Associations.Where(x => x.Owns);
      if (associations.Count() > 0) {
        foreach (Association association in associations) {
          string associationTarget = association.ReferencedEntity.Name;
          string initializer;
          if (association.IsMany)
            initializer = string.Format("new List<{0}>()", associationTarget);
          else
            initializer = string.Format("{0}.Create(newEntity)", associationTarget);

          WriteLine(3, "newEntity.{0} = {1};", WpfGenUtils.MemberToName(association), initializer);
        }
        WriteLine();
      }

      WriteLine(3, "return newEntity;");
      WriteLine(2, "}");
    }
    #endregion

    #endregion

    #region Enums

    public override void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums) {
      Begin(fileInfo, ".cs");

      WriteLine(0, "using wpf_lib.lib.attributes;");
      WriteLine();

      foreach (DataTypeEnum anEnum in enums)
        GenerateEnum(0, anEnum);

      End();
    }

    private void GenerateEnum(int level, DataTypeEnum theEnum) {
      WriteLine(level, "public enum {0} {", WpfGenUtils.EnumToName(theEnum));

      foreach (EnumValue enumValue in theEnum.EnumValues) {
        if (enumValue.Label != null)
          WriteLine(level + 1, "[Label(\"{0}\")]", enumValue.Label);
        if (enumValue.IconName != null)
          WriteLine(level + 1, "[Icon(\"{0}\")]", enumValue.IconName);
        WriteLine(level + 1, "{0},", enumValue.ValueUpperCased);
      }

      WriteLine(level, "}");
      WriteLine();
    }
    #endregion
  }
}
