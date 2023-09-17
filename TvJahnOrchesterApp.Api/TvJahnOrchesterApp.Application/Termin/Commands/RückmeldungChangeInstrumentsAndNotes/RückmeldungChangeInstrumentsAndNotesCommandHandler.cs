using MediatR;
using TvJahnOrchesterApp.Application.Common.Interfaces.Persistence.Repositories;
using TvJahnOrchesterApp.Domain.Common.ValueObjects;

namespace TvJahnOrchesterApp.Application.Termin.Commands.RückmeldungChangeInstrumentsAndNotes
{
    internal class RückmeldungChangeInstrumentsAndNotesCommandHandler : IRequestHandler<RückmeldungChangeInstrumentsAndNotesCommand, Unit>
    {
        private readonly ITerminRepository terminRepository;

        public RückmeldungChangeInstrumentsAndNotesCommandHandler(ITerminRepository terminRepository)
        {
            this.terminRepository = terminRepository;
        }

        public async Task<Unit> Handle(RückmeldungChangeInstrumentsAndNotesCommand request, CancellationToken cancellationToken)
        {
            var termin = await terminRepository.GetById(request.TerminId, cancellationToken);
            var rückmeldung = termin.TerminRückmeldungOrchesterMitglieder.FirstOrDefault(e => e.Id.Value == request.RückmeldungsId);
            if(rückmeldung is null)
            {
                throw new Exception("Füge hier eine Custom Exception ein");
            }
            rückmeldung.ChangeInstruments(request.Instruments.Select(i => Instrument.Create(i.Name, i.ArtInstrument)).ToList());
            rückmeldung.ChangeNotenstimme(request.Notenstimme.ToList());

            return new Unit();

        }
    }
}
