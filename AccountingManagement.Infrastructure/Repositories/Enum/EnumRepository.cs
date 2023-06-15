using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingManagement.Infrastructure.Repositories
{
    #region Gender
    public enum Gender
    {
        male,
        female,
        other
    }
    #endregion

    #region Status
    public enum Status
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
}
