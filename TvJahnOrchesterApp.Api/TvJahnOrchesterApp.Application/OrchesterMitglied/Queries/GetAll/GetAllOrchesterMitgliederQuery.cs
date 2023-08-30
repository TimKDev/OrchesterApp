using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvJahnOrchesterApp.Application.OrchesterMitglied.Queries.GetAll
{
    public record GetAllOrchesterMitgliederQuery: IRequest<Domain.OrchesterMitgliedAggregate.OrchesterMitglied[]> { };
}
