using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Commands.RückmeldungChangeInstrumentsAndNotes
{
    internal class RückmeldungChangeInstrumentsAndNotesCommandHandler : IRequestHandler<RückmeldungChangeInstrumentsAndNotesCommand, Unit>
    {
        private readonly ITerminRepository terminRepository;
        private readonly IUnitOfWork unitOfWork;

        public RückmeldungChangeInstrumentsAndNotesCommandHandler(ITerminRepository terminRepository, IUnitOfWork unitOfWork)
        {
            this.terminRepository = terminRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RückmeldungChangeInstrumentsAndNotesCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            var rückmeldung = termin.TerminRückmeldungOrchesterMitglieder.FirstOrDefault(e => e.Id.Value == request.RückmeldungsId);
            if(rückmeldung is null)
            {
                throw new Exception("Füge hier eine Custom Exception ein");
            }
            rückmeldung.ChangeInstruments(request.Instruments.Select(InstrumentId.Create).ToList());
            rückmeldung.ChangeNotenstimme(request.Notenstimme.Select(NotenstimmeId.Create).ToList());

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return new Unit();

        }
    }
}
