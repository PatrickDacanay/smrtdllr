using SmartDollarWorker.Domains.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDollarWorker.Domains.Interfaces
{
    public interface ISmartDollar
    {
        public SmrtDllrCnst GetTransactions();
        void PostSmartDollar(SmrtDllrCnst request);
    }
}
