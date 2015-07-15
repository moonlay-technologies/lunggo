﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.TableStorage
{
    internal abstract class TableStorageClient
    {
        internal abstract void Init();
        internal abstract void Init(string connString);
        internal abstract CloudTable GetTableByReference(TableStorage reference);
        protected abstract bool CreateIfNotExist(TableStorage reference);
        protected abstract string GetTableReferenceName(TableStorage reference);
        internal abstract void InsertEntityToTableStorage<T>(T objectParam, TableStorage reference) where T : ITableEntity, new();
        internal abstract void InsertOrReplaceEntityToTableStorage<T>(T objectParam, TableStorage reference) where T : ITableEntity, new();
    }
}