using System;

namespace Lunggo.Generator.TableRepo
{
    class InformationSchemaColumnDefinition
    {
        public String COLUMN_NAME { get; set; }
        public String DATA_TYPE { get; set; }
        public int? CHARACTER_MAXIMUM_LENGTH { get; set; }
        public int? NUMERIC_PRECISION { get; set; }
        public int? NUMERIC_SCALE { get; set; }
        public Boolean IsPrimaryKey { get; set; }

    }
}
