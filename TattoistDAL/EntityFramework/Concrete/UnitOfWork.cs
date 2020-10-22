using TattoistDAL.EntityFramework.Abstract;
using TattoistDAL.Models;

namespace TattoistDAL.EntityFramework.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IUserRepository User { get; }
        public ICustomerRepository Customer { get; }
        public IEmployeeRepository Employee { get; }
        public IBrokerRepository Broker { get; }
        public IExpenseRepository Expense { get; }
        public IAppointmentRepository Appointment { get; }
        public IPaymentRepository Payment { get; }

        public UnitOfWork(AppDbContext context)
        {
            _db = context;
            User = new UserRepository(_db);
            Customer = new CustomerRepository(_db);
            Employee = new EmployeeRepository(_db);
            Broker = new BrokerRepository(_db);
            Expense = new ExpenseRepository(_db);
            Appointment = new AppointmentRepository(_db);
            Payment = new PaymentRepository(_db);
        }

        public int Complete()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
