﻿using Lunggo.ApCommon.Activity.Model.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Service
{
    public partial class ActivityService
    {
        public GetTransactionStatementOutput GetTransactionStatement(string operatorId)
        {
            var getTransactionStatementOutput = GetTransactionStatementFromDb(operatorId);
            return getTransactionStatementOutput;    
        }
    }
}
