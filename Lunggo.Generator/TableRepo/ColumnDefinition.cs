using System;

namespace Lunggo.Generator.TableRepo
{
    class ColumnDefinition
    {
        public String ColumnType { get; set; }
        public InformationSchemaColumnDefinition OriginDefinition { get; set; }
    }
}
