using AutoMapper;
using DartsGame.Data;
using DartsGame.DTO;
using DartsGame.Entities;
using DartsGame.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DartsGame.Services
{
    public class LegService
    {
        private readonly AppDbContext _context;
        public readonly LegRepository _legRepository;
        public readonly IMapper _mapper;

        public LegService(AppDbContext context, LegRepository legRepository, IMapper mapper)
        {
            _context = context;
            _legRepository = legRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LegDTO>> GetAll()
        {
            var legs = await _legRepository.GetAll();
            return _mapper.Map<IEnumerable<LegDTO>>(legs);  
        }

        public async Task<LegDTO> GetById(Guid legId)
        {
            var leg = await _legRepository.GetById(legId);
            if (leg == null)
            {
                throw new KeyNotFoundException($"Leg with ID {legId} not found.");
            }
            return _mapper.Map<LegDTO>(leg);
        }

        public async Task<LegDTO> AddLeg(LegDTO legDTO)
        {
            if (legDTO == null)
            {
                throw new ArgumentNullException("Leg cannot be null.");
            }

            var legEntity = _mapper.Map<Leg>(legDTO);
            var addedLeg = await _legRepository.Create(legEntity);

            if (addedLeg == null)
            {
                throw new InvalidOperationException("Failed to add leg.");
            }

            return _mapper.Map<LegDTO>(addedLeg);
        }

        public async Task<LegDTO> UpdateLeg(Guid legId, LegDTO legDTO)
        {
            if (legDTO == null)
            {
                throw new ArgumentNullException("Leg cannot be null.");
            }

            var legById = await _legRepository.GetById(legId);
            if (legById == null)
            {
                throw new KeyNotFoundException($"Leg with ID {legId} not found.");
            }

            var legEntity = _mapper.Map(legDTO, legById);
            legEntity.LegId = legId;

            var updatedLeg = await _legRepository.Update(legEntity);

            return _mapper.Map<LegDTO>(updatedLeg);
        }

        public async Task DeleteLeg(Guid legId)
        {
            var leg = await _legRepository.GetById(legId);
            if (leg == null)
            {
                throw new KeyNotFoundException($"Leg with ID {legId} not found.");
            }

            await _legRepository.Delete(legId);
        }



        public async Task<Leg> StartLeg(Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match), "Match cannot be null.");
            }

            var activePlayers = await _context.PlayerMatches.Where(pm => pm.MatchId == match.MatchId).Select(pm => pm.PlayerId).ToListAsync();

            var startingPlayer = activePlayers.FirstOrDefault();
            if (startingPlayer == Guid.Empty)
            {
                throw new InvalidOperationException("No active players found to start the leg.");
            }

            var lastSet = _context.Sets.Where(m => m.MatchId == match.MatchId).OrderByDescending(s => s.SetNumber).FirstOrDefault();

            if (lastSet == null)
            {
                throw new InvalidOperationException("No sets found for the match.");
            }

            var legNumbers = await _context.Legs.Where(s => s.SetId == lastSet.SetId).Select(l => l.LegNumber).ToListAsync();

            var currentLegNumber = legNumbers.DefaultIfEmpty(0).Max();


            var legId = Guid.NewGuid();
            var leg = new Leg(
                legId,
                lastSet.SetId,
                currentLegNumber + 1,
                false
                );

            _context.Legs.Add(leg);
            await _context.SaveChangesAsync();

            return leg;
        }

        public async Task ValidateLegCompletion(Leg currentLeg, int totalScore)
        {
            bool IsLegWon = false;
            bool isBusted = false;

            if (IsLegWon)
            {
                currentLeg.IsFinished = true;
                _context.Legs.Update(currentLeg);
            }
            else if (isBusted)
            {
                currentLeg.IsFinished = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
