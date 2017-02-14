using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Database;

namespace Lunggo.Repository.TableRecord
{
    public class SavedCreditCardTableRecord : Lunggo.Framework.Database.TableRecord
    {
		private static List<ColumnMetadata> _recordMetadata;
        private static List<ColumnMetadata> _primaryKeys;
        private static String _tableName;

		public String CompanyId
		{
		    get { return _CompanyId; }
		    set
		    {
		        _CompanyId = value;
		        IncrementLog("CompanyId");
		    }
		}

        public String MaskedCardNumber
        {
            get { return _MaskedCardNumber; }
            set
            {
                _MaskedCardNumber = value;
                IncrementLog("MaskedCardNumber");
            }
        }

		public String Token
		{
		    get { return _Token; }
		    set
		    {
		        _Token = value;
		        IncrementLog("Token");
		    }
		}

        public Boolean? IsPrimaryCard
        {
            get { return _IsPrimaryCard; }
            set
            {
                _IsPrimaryCard = value;
                IncrementLog("IsPrimaryCard");
            }
        }
		public String CardHolderName
		{
		    get { return _CardHolderName; }
		    set
		    {
		        _CardHolderName = value;
		        IncrementLog("CardHolderName");
		    }
		}
		public DateTime? TokenExpiry
		{
		    get { return _TokenExpiry; }
		    set
		    {
		        _TokenExpiry = value;
		        IncrementLog("TokenExpiry");
		    }
		}

        public DateTime? CardExpiry
        {
            get { return _CardExpiry; }
            set
            {
                _CardExpiry = value;
                IncrementLog("CardExpiry");
            }
        }

		
		private String _CompanyId;
        private String _MaskedCardNumber;
        private Boolean? _IsPrimaryCard;
		private String _Token;
		private String _CardHolderName;
		private DateTime? _TokenExpiry;
        private DateTime? _CardExpiry;



		public static SavedCreditCardTableRecord CreateNewInstance()
        {
            var record = new SavedCreditCardTableRecord();
            var iRecord = record.AsInterface();
            iRecord.ManuallyCreated = true;
            return record;
        }

		public SavedCreditCardTableRecord()
        {
            ;
        }

        static SavedCreditCardTableRecord()
        {
            InitTableName();
            InitRecordMetadata();
            InitPrimaryKeysMetadata();
        }

        private static void InitTableName()
        {
            _tableName = "SavedCreditCard";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("CompanyId", true),
                new ColumnMetadata("MaskedCardNumber",true),
				new ColumnMetadata("IsPrimaryCard", false),
				new ColumnMetadata("Token", false),
				new ColumnMetadata("CardHolderName", false),
				new ColumnMetadata("TokenExpiry", false),
                new ColumnMetadata("CardExpiry", false)
            };
        }

        private static void InitPrimaryKeysMetadata()
        {
            _primaryKeys = _recordMetadata.Where(p => p.IsPrimaryKey).ToList();
        }

		public override List<ColumnMetadata> GetMetadata()
        {
            return _recordMetadata;
        }

        public override string GetTableName()
        {
            return _tableName;
        }

        public override List<ColumnMetadata> GetPrimaryKeys()
        {
            return _primaryKeys;
        }



    }
}
