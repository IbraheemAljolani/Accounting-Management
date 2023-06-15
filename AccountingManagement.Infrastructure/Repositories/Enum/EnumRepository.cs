using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Infrastructure.Repositories
{
    #region User Gender
    public enum UserGender
    {
        male,
        female,
        other
    }
    #endregion

    #region User Status
    public enum UserStatus
    {
        active,
        delete
    }
    #endregion

    #region Currency
    public enum Currencies
    {
        jd,
        usd,
        eur
    }
    #endregion

    #region Credit Type
    public enum CreditType
    {
        credit,
        debit
    }
    #endregion

    #region Transaction Status
    public enum TransactionStatus
    {
        completed,
        reversed,
    }
    #endregion
}
