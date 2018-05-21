using System;
using Dapper;
using Lunggo.ApCommon.Payment.Database;
using Lunggo.Framework.TestHelpers;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Lunggo.Framework.TestHelpers.TestHelper;

namespace Lunggo.PaymentTest.DbServiceTests
{
    [TestClass]
    public class GetUserBankAccountsTests
    {
        [TestMethod]
        // Should return list of bank accounts when data exists under supplied userId
        public void Should_return_list_of_bank_accounts_when_data_exists_under_supplied_userId()
        {
            UseDb(conn =>
            {
                var userId = RandomString();
                var expected1 = new UserBankAccountTableRecord
                {
                    UserId = userId,
                    AccountNumber = RandomString(),
                    BankName = RandomString(),
                    Branch = RandomString(),
                    OwnerName = RandomString()
                };
                var expected2 = new UserBankAccountTableRecord
                {
                    UserId = userId,
                    AccountNumber = RandomString(),
                    BankName = RandomString(),
                    Branch = RandomString(),
                    OwnerName = RandomString()
                };
                UserBankAccountTableRepo.GetInstance().Delete(conn, new UserBankAccountTableRecord { UserId = userId });
                UserBankAccountTableRepo.GetInstance().Insert(conn, expected1);
                UserBankAccountTableRepo.GetInstance().Insert(conn, expected2);

                var actual = new PaymentDbService().GetUserBankAccounts(userId);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.Count == 2);
                Assert.IsTrue(actual.Exists(a =>
                    a.AccountNumber == expected1.AccountNumber &&
                    a.BankName == expected1.BankName &&
                    a.Branch == expected1.Branch &&
                    a.OwnerName == expected1.OwnerName));
                Assert.IsTrue(actual.Exists(a =>
                    a.AccountNumber == expected2.AccountNumber &&
                    a.BankName == expected2.BankName &&
                    a.Branch == expected2.Branch &&
                    a.OwnerName == expected2.OwnerName));

                UserBankAccountTableRepo.GetInstance().Delete(conn, new UserBankAccountTableRecord { UserId = userId });
            });
        }

        [TestMethod]
        // Should return empty list when there is no data existing under supplier userId
        public void Should_return_empty_list_when_there_is_no_data_existing_under_supplier_userId()
        {
            UseDb(conn =>
            {
                var userId = RandomString();
                UserBankAccountTableRepo.GetInstance().Delete(conn, new UserBankAccountTableRecord { UserId = userId });

                var actual = new PaymentDbService().GetUserBankAccounts(userId);

                Assert.IsNotNull(actual);
                Assert.IsTrue(actual.Count == 0);
            });
        }

    }
}
