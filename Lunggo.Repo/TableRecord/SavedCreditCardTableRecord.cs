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

		public String Email
		{
		    get { return _Email; }
		    set
		    {
		        _Email = value;
		        IncrementLog("Email");
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


		
		private String _Email;
		private String _MaskedCardNumber;
		private String _Token;
		private String _CardHolderName;
		private DateTime? _TokenExpiry;


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
            _tableName = "Activities";
        }

        private static void InitRecordMetadata()
        {
            _recordMetadata = new List<ColumnMetadata>
            {
				new ColumnMetadata("Email", true),
				new ColumnMetadata("MaskedCardNumber", true),
				new ColumnMetadata("Token", false),
				new ColumnMetadata("CardHolderName", false),
				new ColumnMetadata("TokenExpiry", false),

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
