﻿using TvJahnOrchesterApp.Contracts.Common.Dto;

namespace TvJahnOrchesterApp.Contracts.Termine.Einsatzplan
{
    public record UpdateEinsatzplanRequest(DateTime StartZeit, DateTime EndZeit, AdresseDto TreffPunkt, int[] Noten, int[] Uniform, string? WeitereInformationen);
}
