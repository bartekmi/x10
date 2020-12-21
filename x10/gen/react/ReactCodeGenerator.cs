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
using x10.ui.metadata;

namespace x10.gen.react {
  public class ReactCodeGenerator : CodeGenerator {

    private readonly string[] IGNORE_ATTRIBUTES = new string[] {
      UiAttributeDefinitions.NAME,
      UiAttributeDefinitions.PATH,
    };

    public override void Generate(ClassDefX10 classDef) {
      GenerateReactFunctionalComponent(classDef);
    }

    private void GenerateReactFunctionalComponent(ClassDefX10 classDef) {
      Begin(classDef.XmlElement.FileInfo, ".jsx");

      bool isForm = true; // TODO

      WriteLine(0, "// @flow");
      WriteLine();

      Entity model = classDef.ComponentDataModel;

      GenerateImports(isForm);
      if (model != null)
        GenerateType(classDef.ComponentDataModel);
      GenerateComponent(classDef);
      if (model != null)
        GenerateDefaultEntity(classDef.ComponentDataModel);

      End();
    }

    private void GenerateImports(bool isForm) {
      WriteLine(0, "import * as React from 'react';");
      WriteLine(0, "import { v4 as uuid } from 'uuid';");
      if (isForm)
        WriteLine(0, "import { graphql, commitMutation } from 'react-relay';");

      WriteLine();
      WriteLine(0, "import { DBID_LOCALLY_CREATED } from 'react_lib/constants';");
    }

    private void GenerateType(Entity model) {
      WriteLine(0, "export type {0} = {{|", model.Name);
      foreach (Member member in model.Members)
        WriteLine(1, "+{0}: {1},", member.Name, GetType(member));
      WriteLine(0, "|}};");
    }

    private string GetType(Member member) {
      if (member is Association association) {
        string refedEntityName = association.ReferencedEntity.Name;
        if (association.IsMany)
          return string.Format("$ReadOnlyArray<{0}>", refedEntityName);
        else
          return string.Format("{0}{1}", association.IsMandatory ? "" : "?", refedEntityName);
      } else if (member is X10Attribute attribute) {
        DataType dataType = attribute.DataType;
        return GetAtomicFlowType(dataType);
      }

      return "null,";
    }

    private string GetAtomicFlowType(DataType dataType) {
      if (dataType == DataTypes.Singleton.Boolean) return "bool";
      if (dataType == DataTypes.Singleton.Date) return "string";
      if (dataType == DataTypes.Singleton.Float) return "number";
      if (dataType == DataTypes.Singleton.Integer) return "number";
      if (dataType == DataTypes.Singleton.String) return "string";
      if (dataType == DataTypes.Singleton.Timestamp) return "string";
      if (dataType == DataTypes.Singleton.Money) return "number";
      if (dataType is DataTypeEnum enumType) return dataType.Name;

      throw new Exception("Unknown data type: " + dataType.Name);
    }


    private void GenerateComponent(ClassDefX10 classDef) {
      Entity model = classDef.ComponentDataModel;
      string typeName = model?.Name;
      string variableName = model == null ? null : NameUtils.UncapitalizeFirstLetter(typeName);

      // Props
      WriteLine(0, "type Props = {{|");
      if (model != null) {
        WriteLine(1, "+{0}: {1},", variableName, typeName);
        WriteLine(1, "+onChange: ({0}: {1}) => void,", variableName, typeName);
      }
      WriteLine(0, "|}};");

      // Component Definition
      WriteLine(0, "export default function {0}(props: Props): React.Node {", classDef.Name);
      if (model != null) {
        WriteLine(1, "const { {0}, onChange } = props;", variableName);
        WriteLine(1, "const {");
        // TODO: Generate data destructuring
        WriteLine(1, "} = {0};", variableName);
      }

      WriteLine(1, "return (");
      GenerateComponentRecursively(2, classDef.RootChild);
      WriteLine(1, ");");
      WriteLine(0, "}");
    }

    private void GenerateComponentRecursively(int level, Instance instance) {
      if (instance == null)
        return;

      // Find the Platform Class Definition
      PlatformClassDef platClassDef = FindPlatformClassDef(instance);
      if (platClassDef == null) {
        Messages.AddError(instance.XmlElement, "No platform-specific Class Definition for Logical Class {0}",
          instance.ClassDef.Name);
        return;
      }

      // Open the React tag
      WriteLineMaybe(level, "<{0}", platClassDef.EffectivePlatformName);

      // Write Static Attributes
      foreach (PlatformAttributeStatic staticAttr in platClassDef.StaticPlatformAttributes)
        WriteLine(level + 1, "{0}=\"{1}\"", staticAttr.PlatformName, staticAttr.Value);

      // Write ByFunc Attributes
      foreach (PlatformAttributeByFunc byFuncAttr in platClassDef.ByFuncPlatformAttributes) {
        string value = byFuncAttr.Function(instance);
        if (value != null)
          WriteLine(level + 1, "{0}=\"{1}\"", byFuncAttr.PlatformName, value);
      }

      UiAttributeValue primaryValue = instance.PrimaryValue;
      PlatformAttributeDataBind dataBind = platClassDef.DataBindAttribute;
      bool dataBindAlreadyRendered = false;

      // Write the logical attributes contained in the instance
      foreach (UiAttributeValue attrValue in instance.AttributeValues) {
        string attrName = attrValue.Definition.Name;

        if (attrValue == primaryValue || IGNORE_ATTRIBUTES.Contains(attrValue.Definition.Name))
          continue;

        PlatformAttributeDynamic dynamicAttr = platClassDef.FindDyamicAttribute(attrName);
        if (dynamicAttr == dataBind)
          dataBindAlreadyRendered = true;

        // Write the attribute
        if (attrValue is UiAttributeValueAtomic atomicValue) {
          string name = dynamicAttr == null ? NameUtils.Capitalize(attrName) : dynamicAttr.PlatformName;
          string value;
          if (atomicValue.Expression != null)
            value = GenerateFormula(instance, dynamicAttr, name, atomicValue.Expression);
          else if (atomicValue.Value != null && dynamicAttr != null)
            value = dynamicAttr.GenerateAttributeForValue(atomicValue.Value);
          else
            continue;

          WriteLine(level + 1, "{0}=\"{1}\"", name, value);
        } else {
          // TODO - e.g. Action from submit button
        }
      }

      // Write the primary binding attribute (e.g. Text of TextBox) if not explicitly specified in instance
      Member member = instance.ModelMember;
      if (dataBind != null && member != null && !dataBindAlreadyRendered)
        WriteLine(level + 1, "{0}={ {1} }", dataBind.PlatformName, member.Name);

      // Are there any nested children to be generated?
      List<PlatformClassDef> nestedClassDefs = new List<PlatformClassDef>();
      PlatformClassDef child = platClassDef.NestedClassDef;
      while (child != null) {
        nestedClassDefs.Add(child);
        child = child.NestedClassDef;
      }
      string primaryAttrWrapper = platClassDef.PrimaryAttributeWrapperProperty == null ?
        null : string.Format("{0}.{1}", platClassDef.PlatformName, platClassDef.PrimaryAttributeWrapperProperty);


      // Close the XAML element, possibly recursively writing nested content
      if (primaryValue == null)
        WriteLineClose(level, "/>");
      else {
        WriteLineClose(level, ">");

        foreach (PlatformClassDef nested in nestedClassDefs) // See PlatformClassDef.NestedClassDef
          WriteLine(++level, "<{0}>", nested.PlatformName);

        if (primaryAttrWrapper != null)
          WriteLine(++level, "<{0}>", primaryAttrWrapper);

        bool reducesManyToOne = ((UiAttributeDefinitionComplex)primaryValue.Definition).ReducesManyToOne;
        WriteChildren(level + 1, primaryValue);

        if (primaryAttrWrapper != null)
          WriteLine(level--, "</{0}>", primaryAttrWrapper);

        nestedClassDefs.Reverse();
        foreach (PlatformClassDef nested in nestedClassDefs)
          WriteLine(level--, "</{0}>", nested.PlatformName);

        WriteLine(level, "</{0}>", platClassDef.EffectivePlatformName);
      }
    }

    private void WriteChildren(int level, UiAttributeValue attrValue) {
      if (attrValue is UiAttributeValueComplex complexAttr) {
        foreach (Instance childInstance in complexAttr.Instances)
          GenerateComponentRecursively(level, childInstance);
      } else
        throw new NotImplementedException();
    }

    private string GenerateFormula(Instance context, PlatformAttributeDynamic dynamicAttr, string name, ExpBase expression) {
      if (expression == null)
        return "EXPRESSION MISSING";

      using StringWriter writer = new StringWriter();

      JavascriptFormulaWriter formulaWriterVisitor = new JavascriptFormulaWriter(writer);
      expression.Accept(formulaWriterVisitor);
      return writer.ToString();
    }


    private void GenerateDefaultEntity(Entity model) {
      WriteLine(0, "export function createDefault{0}(): {0} {", model.Name);
      WriteLine(1, "return {");

      WriteLine(2, "id: uuid(),");
      WriteLine(2, "dbid: DBID_LOCALLY_CREATED,");

      foreach (Member member in model.Members)
        WriteLine(2, "{0}: {1},", member.Name, GetDefaultValue(member));

      WriteLine(1, "};");
      WriteLine(0, "}");
    }

    private string GetDefaultValue(Member member) {
      if (member is Association association) {
        if (association.IsMany)
          return "[]";
        else
          if (association.IsMandatory) {
          // TODO: Need to register an import
          return string.Format("createDefault{0}()", association.ReferencedEntity.Name);
        }
      } else if (member is X10RegularAttribute attribute) {
        object defaultValue = attribute.DefaultValue;
        DataType dataType = attribute.DataType;

        if (defaultValue == null) {
          if (dataType == DataTypes.Singleton.Boolean) return "false";
          if (dataType == DataTypes.Singleton.String) return "''";
        } else
          return ReactGenUtils.TypedLiteralToString(defaultValue, dataType as DataTypeEnum);
      }

      return "null";
    }

    public override void Generate(Entity entity) {
    }

    public override void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums) {
      Begin(fileInfo, ".js");

      // foreach (DataTypeEnum anEnum in enums)
      //   GenerateEnum(0, anEnum);

      End();
    }
  }
}
