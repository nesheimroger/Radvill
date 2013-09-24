using Radvill.DataFactory.Internal.Services;
using Radvill.Services.DataFactory;
using Radvill.Services.DataFactory.Repositories;

namespace Radvill.DataFactory.Public
{
    public class DataFactory : IDataFactory
    {
        private readonly IRadvillContext _context;

        public DataFactory(IRadvillContext context, IUserRepository userRepository)
        {
            _context = context;
            UserRepository = userRepository;
        }
        
        public IUserRepository UserRepository { get; set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}