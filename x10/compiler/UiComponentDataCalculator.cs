using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using x10.utils;
using x10.formula;
using x10.model.definition;
using x10.ui.composition;
using x10.ui;
using x10.model.libraries;

// The UiComponentDataCalculator class can be used to generate a tree structure of all 
// data referenced in a Class Def. Perfect for generating GraphQL Fragments, for example.
// It includes both data referenced via the "path" attribute and also data referenced
// by formulas, including recursively looking at Derived Attributes.
namespace x10.compiler {

  #region Helper Classes
  // MemberWrapper builds a tree of members for which data need to be pulled in by the UI.
  // A tree of MemberWrappers will be a partial subset of the graph formed by Member.
  public class MemberWrapper {
    public Entity RootEntity { get; private set; }
    public Member Member { get; private set; }
    public List<MemberWrapper> Children { get; private set; }

    // List of references to other components. In the relay world,
    // this is how we'd know to include fragments
    public List<ClassDefX10> ComponentReferences { get; private set; }

    internal MemberWrapper() {
      Children = new List<MemberWrapper>();
      ComponentReferences = new List<ClassDefX10>();
    }

    internal MemberWrapper(Entity rootEntity) : this() {
      RootEntity = rootEntity;
    }

    internal MemberWrapper(Member member) : this() {
      Member = member;
    }

    internal bool RecursivelyContainsMember(Member member) {
      if (Member == member)
        return true;

      return Children.Any(x => x.RecursivelyContainsMember(member));
    }

    // Useful for testing
    public void Print(TextWriter writer, int indent) {
      if (RootEntity != null)
        PrintGraphQL_Children(writer, indent);
      else if (Member is Association association) {
        PrintUtils.WriteLineIndented(writer, indent, Member.Name + " {");
        PrintGraphQL_Children(writer, indent + 1);
        PrintUtils.WriteLineIndented(writer, indent, "}");
      } else
        PrintUtils.WriteLineIndented(writer, indent, Member.Name);
    }

    private void PrintGraphQL_Children(TextWriter writer, int indent) {
      foreach (MemberWrapper child in Children.OrderBy(x => x.Member.Name))
        child.Print(writer, indent);
    }

    public string PrintGraphQL(int indent) {
      using (TextWriter writer = new StringWriter()) {
        Print(writer, indent);
        return writer.ToString();
      }
    }

    // Given a MemberWrapper that corresponds to an Entity, extend the tree by adding
    // the specified member, if it does not already exist.
    // Returns:
    //   - The "this" if only leaves were added (plural in the case of derived attribute)
    //   - The added or found node if an association was passed in
    internal MemberWrapper FindOrCreate(Member member) {
      if (member.Owner.IsNonFetchable ||  // DataType Entities
          member.Owner.IsContext)         // Context is not part of fetchable data
        return this;

      // Treat derived attributes by recursing - they need to be decomposed
      // into their source regular attributes
      if (member is X10DerivedAttribute derived) {
        foreach (X10RegularAttribute regular in derived.ExtractSourceAttributes())
          FindOrCreate(regular);
        return this;
      }

      MemberWrapper child = Children.SingleOrDefault(x => x.Member == member);

      if (child == null) {
        child = new MemberWrapper(member);
        Children.Add(child);
      }

      return member is Association ? child : this;
    }

    public override string ToString() {
      return RootEntity == null ?
        Member.ToString() :
        RootEntity.Name;
    }
  }
  #endregion

  public static class UiComponentDataCalculator {

    public static MemberWrapper ExtractData(ClassDefX10 classDef) {
      if (classDef.ComponentDataModel == null)
        throw new Exception("Must have a component data model");
      return ExtractData(classDef.RootChild, classDef.ComponentDataModel);
    }

    public static MemberWrapper ExtractData(Instance root, Entity rootEntity) {
      MemberWrapper rootWrapper = new MemberWrapper(rootEntity);
      ExtractDataRecursive(root, rootWrapper);
      return rootWrapper;
    }

    private static void ExtractDataRecursive(Instance instance, MemberWrapper wrapper) {
      // Process all formulas from the instance
      foreach (UiAttributeValueAtomic atomicValue in instance.AtomicAttributeValues())
        if (atomicValue.Expression != null) {
          foreach (IEnumerable<Member> path in FormulaUtils.ExtractMemberPaths(atomicValue.Expression))
            BuildWrapper(path, wrapper);
        }

      // If the instance descends into nested entities, do so
      if (instance.PathComponents != null)
        wrapper = BuildWrapper(instance.PathComponents, wrapper);

      // If the instance references an X10 component, record this in the wrapper
      // (Used for including fragments)
      if (instance.RenderAs is ClassDefX10 classDef)
        wrapper.ComponentReferences.Add(classDef);

      // This is a hack, but a nicker solution would take more serious thought
      // Ideally, at the BaseLibrary level, a component definition should
      // be able to define a default function or derived attribute. The default
      // would be toStringRepresentation.
      if (instance.RenderAs.Name == x10.ui.libraries.BaseLibrary.ASSOCIATION_DISPLAY) {
        X10DerivedAttribute toString = instance.DataModelEntity.GetToStringRepresentationAttr();
        foreach (IEnumerable<Member> path in FormulaUtils.ExtractMemberPaths(toString.Expression))
          BuildWrapper(path, wrapper);
      }

      foreach (Instance child in instance.ChildInstances)
        ExtractDataRecursive(child, wrapper);
    }

    private static MemberWrapper BuildWrapper(IEnumerable<Member> pathComponents, MemberWrapper wrapper) {
      foreach (Member member in pathComponents)
        wrapper = wrapper.FindOrCreate(member);

      return wrapper;
    }
  }
}