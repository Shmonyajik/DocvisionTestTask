using LettersRegistration.WebAPI.Domain;

namespace LettersRegistration.WebAPI.DAL
{
    public class LetterRepository : IBaseRepository<Letter>
    {
        private readonly LettersDbContext _context;
        public LetterRepository(LettersDbContext context)
        {
            _context = context;
        }
        public async Task Create(Letter model)
        {
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
        }

        public async Task CreateMultiple(IEnumerable<Letter> model)
        {
            await _context.AddRangeAsync(model);
            await _context.SaveChangesAsync();
        }


        public IQueryable<Letter> GetBy()
        {
            return _context.Letters;
        }

        public async Task<IEnumerable<Letter>> GetAll()
        {
            return await _context.Letters.ToListAsync();
        }

        public async Task<Letter> GetById(int id)
        {
            return await _context.Letters.FindAsync(new object[] {id});
            
        }
        public async Task<IEnumerable<Letter>> GetBySender(string sender)
        {
            return await _context.Letters.Where(l=>l.Sender==sender).ToListAsync();
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if(disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        
    }
}
