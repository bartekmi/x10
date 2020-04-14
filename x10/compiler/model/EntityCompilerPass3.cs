﻿using System;
using System.Collections.Generic;
using x10.model;
using x10.model.definition;
using x10.ui.metadata;

namespace x10.compiler {
  public class EntityCompilerPass3 {

    private readonly AllUiDefinitions _allUiDefinitions;

    internal EntityCompilerPass3(AllUiDefinitions allUiDefinitions) {
      _allUiDefinitions = allUiDefinitions;
    }

    internal void CompileAllEntities(AllEntities allEntities) {
      foreach (Entity entity in allEntities.All)
        CompileEntity(entity);
    }

    internal void CompileEntity(Entity entity) {
      entity.Ui = FindUiComponentWithError(entity.UiName, entity);

      foreach (Member member in entity.Members)
        member.Ui = FindUiComponentWithError(member.UiName, member);
    }

    private ClassDef FindUiComponentWithError(string uiName, ModelComponent modelComponent) {
      ModelAttributeValue attrUi = AttributeUtils.FindAttribute(modelComponent, "ui");
      if (attrUi == null)
        return null;

      return _allUiDefinitions.FindDefinitionByNameWithError(uiName, attrUi.TreeElement);
    }
  }
}