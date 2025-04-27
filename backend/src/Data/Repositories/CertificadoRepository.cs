using Business.Entities;
using Business.Interfaces;
using Data.Context;

namespace Data.Repositories
{
    public class CertificadoRepository(AppDbContext dbContext) : Repository<Certificado>(dbContext), ICertificadoRepository
    {
    }
}
