using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using App.Entity.Models;
using App.Web.Context;

namespace App.Web.Helper
{
    public static class CrmDb
    {
        //public static bool UpdateBalance(CrmDbContext db, BankAccount account, double amount, BalanceMode balanceMode = BalanceMode.Increment)
        //{
        //    try
        //    {
        //        switch (balanceMode)
        //        {
        //            case BalanceMode.Increment:
        //                account.Balance += amount;
        //                break;
        //            case BalanceMode.Decrement:
        //                account.Balance -= amount;
        //                break;
        //        }
        //        db.Entry(account).State = EntityState.Modified;
        //        return db.SaveChanges() > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public static bool UpdateBalance(this CrmDbContext db, BankAccount account, double amount, BalanceMode balanceMode = BalanceMode.Increment)
        {
            try
            {
                switch (balanceMode)
                {
                    case BalanceMode.Increment:
                        account.Balance += amount;
                        break;
                    case BalanceMode.Decrement:
                        account.Balance -= amount;
                        break;
                }
                db.Entry(account).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}