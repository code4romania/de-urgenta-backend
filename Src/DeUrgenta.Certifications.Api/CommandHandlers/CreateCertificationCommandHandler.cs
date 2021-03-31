using DeUrgenta.Certifications.Api.Commands;
using DeUrgenta.Domain;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeUrgenta.Certifications.Api.CommandHandlers
{
    public class CreateCertificationCommandHandler : IRequestHandler<CreateCertification, int>
    {
        private readonly DeUrgentaContext _context;

        public CreateCertificationCommandHandler(DeUrgentaContext context)
        {
            _context = context;
        }

        public Task<int> Handle(CreateCertification request, CancellationToken cancellationToken)
        {


            throw new NotImplementedException();
        }
    }
}
