using System;

namespace TattoistDAL.EntityFramework.Abstract
{
    interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        ICustomerRepository Customer { get; }
        IEmployeeRepository Employee { get; }
        IBrokerRepository Broker { get; }
        IExpenseRepository Expense { get; }
        IAppointmentRepository Appointment { get; }
        IPaymentRepository Payment { get; }

        int Complete();
    }
}
