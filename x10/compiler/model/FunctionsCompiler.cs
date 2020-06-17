using System;
using System.Collections.Generic;
using System.Text;

using x10.parsing;
using x10.model.metadata;
using x10.model;
using x10.model.definition;

namespace x10.compiler {
  public class FunctionsCompiler {
    private readonly MessageBucket _messages;
    private readonly AllEntities _allEntities;
    private readonly AllEnums _allEnums;
    private readonly AllFunctions _allFunctions;
    private readonly AttributeReader _attrReader;

    internal FunctionsCompiler(MessageBucket messages, AllEntities allEntities, AllEnums allEnums, AllFunctions allFunctions, AttributeReader attrReader) {
      _messages = messages;
      _allEntities = allEntities;
      _allEnums = allEnums;
      _allFunctions = allFunctions;
      _attrReader = attrReader;
    }

    internal void CompileFunctionsFile(TreeNode rootNodeUntyped) {
      TreeSequence rootNode = rootNodeUntyped as TreeSequence;
      if (rootNode == null) {
        _messages.AddError(rootNodeUntyped, "The root node of a Functions file must be an Array, but was: " + rootNodeUntyped.GetType().Name);
        return;
      }

      foreach (TreeNode funcRootNode in rootNode.Children)
        CompileFunction(funcRootNode);
    }

    internal void CompileFunctionFromString(string yaml) {
      ParserYaml parser = new ParserYaml(_messages, null);
      TreeNode rootNode = parser.ParseFromString(yaml);
      CompileFunction(rootNode);
    }

    internal void CompileFunction(TreeNode funcRootNode) {
      TreeHash funcHash = AttributeReader.EnsureObjectWithAttributresIsHash(funcRootNode, _messages);
      if (funcHash == null)
        return;

      Function function = new Function() {
        TreeElement = funcRootNode,
      };

      // Extract the arguments
      TreeNode arguments = TreeUtils.GetOptionalAttribute(funcHash, "arguments");
      if (arguments != null) {
        if (arguments is TreeSequence sequence) {
          foreach (TreeNode argumentNode in sequence.Children) {
            Argument argument = new Argument();
            bool isHash = _attrReader.ReadAttributes(argumentNode, AppliesTo.FunctionArgument, argument);
            if (isHash)
              function.Arguments.Add(argument);
          }

          // Check uniqueness of argument names
          UniquenessChecker.Check("name",
            function.Arguments,
            _messages,
            "The name '{0}' is not unique among all the arguments of this Function.");
        } else 
          _messages.AddError(arguments, "The list of arguments must be an Array, but was: " + arguments.GetType().Name);
      }

      _attrReader.ReadAttributes(funcRootNode, AppliesTo.Function, function, "arguments");
      _allFunctions.Add(function);

      InvokePass2_Actions(function);
    }

    private void InvokePass2_Actions(Function function) {
      foreach (ModelAttributeValue value in function.AttributeValues)
        value.Definition.Pass2Action?.Invoke(_messages, _allEntities, _allEnums, function, value);

      foreach (Argument argument in function.Arguments)
        foreach (ModelAttributeValue value in argument.AttributeValues)
          value.Definition.Pass2Action?.Invoke(_messages, _allEntities, _allEnums, argument, value);
    }
  }
}

