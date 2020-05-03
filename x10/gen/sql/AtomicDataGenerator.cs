using System;
using System.Collections.Generic;
using System.Text;

using x10.gen.sql.primitives;
using x10.model.definition;
using x10.model.metadata;

namespace x10.gen.sql {
  internal static class AtomicDataGenerator {
    private const int DEFAULT_INT_MIN = 1;
    private const int DEFAULT_INT_MAX = 10;
    private const double DEFAULT_FLOAT_MIN = 1.0;
    private const double DEFAULT_FLOAT_MAX = 10.0;

    private const int DEFAULT_DATE_OFFSET_DAYS_MIN = -20;
    private const int DEFAULT_DATE_OFFSET_DAYS_MAX = +5;
    private const double DEFAULT_TIMESTAMP_OFFSET_DAYS_MIN = -20.0;
    private const double DEFAULT_TIMESTAMP_OFFSET_DAYS_MAX = +5.0;

    internal static MemberAndValue Generate(Random random, DataGenerationContext context,  X10Attribute x10Attr) {
      object objMin = x10Attr.FindValue(DataGenLibrary.MIN);
      object objMax = x10Attr.FindValue(DataGenLibrary.MAX);

      object value = null;
      if (x10Attr.DataType == DataTypes.Singleton.Integer) {
        int min = objMin is int _min ? _min : DEFAULT_INT_MIN;
        int max = objMax is int _max ? _max : DEFAULT_INT_MAX;

        value = random.Next(min, max + 1);
      } else if (x10Attr.DataType == DataTypes.Singleton.Float) {
        double min = objMin is double _min ? _min : DEFAULT_FLOAT_MIN;
        double max = objMax is double _max ? _max : DEFAULT_FLOAT_MAX;

        value = random.NextDouble() * (max - min) + min;
      } else if (x10Attr.DataType == DataTypes.Singleton.Date) {
        // For date, min/max is offset from today's date
        int min = objMin is int _min ? _min : DEFAULT_DATE_OFFSET_DAYS_MIN;
        int max = objMax is int _max ? _max : DEFAULT_DATE_OFFSET_DAYS_MAX;

        int offsetDays = random.Next(min, max + 1);
        value = DateTime.Today.AddDays(offsetDays);
      } else if (x10Attr.DataType == DataTypes.Singleton.Timestamp) {
        // For date, min/max is offset from today's date
        double min = objMin is double _min ? _min : DEFAULT_TIMESTAMP_OFFSET_DAYS_MIN;
        double max = objMax is double _max ? _max : DEFAULT_TIMESTAMP_OFFSET_DAYS_MAX;

        double offsetDays = random.NextDouble() * (max - min) + min;
        value = DateTime.Now.AddDays(offsetDays);
      } else if (x10Attr.DataType == DataTypes.Singleton.String) {
        object fromSource = x10Attr.FindValue(DataGenLibrary.FROM_SOURCE);
        if (fromSource != null) {
          Dictionary<string, string> rules = GenSqlUtils.ToDictionary(fromSource.ToString());
        }

      }

      return new MemberAndValue() {
        Member = x10Attr,
        Value = value,
      };
    }
  }
}
