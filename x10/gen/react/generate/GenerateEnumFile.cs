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
using x10.utils;
using x10.ui.platform;
using x10.ui;
using x10.ui.metadata;

namespace x10.gen.react.generate {
  public partial class ReactCodeGenerator : CodeGenerator {

    public override void GenerateEnumFile(FileInfo fileInfo, IEnumerable<DataTypeEnum> enums) {
      Begin(fileInfo, ".js");

      // foreach (DataTypeEnum anEnum in enums)
      //   GenerateEnum(0, anEnum);

      End();
    }
  }
}
