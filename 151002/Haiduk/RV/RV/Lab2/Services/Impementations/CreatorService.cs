﻿using AutoMapper;
using FluentValidation;
using Lab2.DTO.RequestDTO;
using Lab2.DTO.ResponseDTO;
using Lab2.Exceptions;
using Lab2.Infrastructure.Validators;
using Lab2.Models;
using Lab2.Repositories.Interfaces;
using Lab2.Services.Interfaces;

namespace Lab2.Services.Impementations
{
    public class CreatorService(ICreatorRepository creatorRepository,
        IMapper mapper, CreatorRequestDtoValidator validator) : ICreatorService
    {
        private readonly ICreatorRepository _creatorRepository = creatorRepository;
        private readonly IMapper _mapper = mapper;
        private readonly CreatorRequestDtoValidator _validator = validator;

        public async Task<IEnumerable<CreatorResponseDto>> GetCreatorsAsync()
        {
            var creators = await _creatorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CreatorResponseDto>>(creators);
        }

        public async Task<CreatorResponseDto> GetCreatorByIdAsync(long id)
        {
            var creator = await _creatorRepository.GetByIdAsync(id)
                ?? throw new NotFoundException(ErrorMessages.CreatorNotFoundMessage(id));
            return _mapper.Map<CreatorResponseDto>(creator);
        }

        public async Task<CreatorResponseDto> CreateCreatorAsync(CreatorRequestDto creator)
        {
            _validator.ValidateAndThrow(creator);
            var creatorToCreate = _mapper.Map<Creator>(creator);
            var createdCreator = await _creatorRepository.CreateAsync(creatorToCreate);
            return _mapper.Map<CreatorResponseDto>(createdCreator);
        }

        public async Task<CreatorResponseDto> UpdateCreatorAsync(CreatorRequestDto creator)
        {
            _validator.ValidateAndThrow(creator);
            var creatorToUpdate = _mapper.Map<Creator>(creator);
            var updatedCreator = await _creatorRepository.UpdateAsync(creatorToUpdate)
                ?? throw new NotFoundException(ErrorMessages.CreatorNotFoundMessage(creator.Id));
            return _mapper.Map<CreatorResponseDto>(updatedCreator);
        }

        public async Task DeleteCreatorAsync(long id)
        {
            if (!await _creatorRepository.DeleteAsync(id))
            {
                throw new NotFoundException(ErrorMessages.CreatorNotFoundMessage(id));
            }
        }
    }
}