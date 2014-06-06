using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Generator.TableRepo
{
    class ColumnDefinition
    {
        public String ColumnType { get; set; }
        public InformationSchemaColumnDefinition OriginDefinition { get; set; }
    }
}
