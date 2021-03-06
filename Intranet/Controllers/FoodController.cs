﻿using AutoMapper;
using Intranet.Contract;
using Intranet.DataObject;
using Intranet.Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Intranet.Controllers
{
    [Route("/api/[controller]/[action]")]
    public class FoodController : BaseController
    {
        public IMapper _mapper;
        public IFoodRepository _foodRepository;
        public FoodController(IMapper mapper, IFoodRepository foodRepository)
        {
            _mapper = mapper;
            _foodRepository = foodRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var foods = await _foodRepository.FindAll().ToListAsync(cancellationToken);
            return Ok(_mapper.Map<IEnumerable<FoodDTO>>(foods));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var food = await _foodRepository.FindByIdAsync(id, cancellationToken);
            if (food is null) return NotFound();
            return Ok(_mapper.Map<FoodDTO>(food));
        }

        [HttpPost]
        public async Task<IActionResult> Create(FoodDTO dto, CancellationToken cancellationToken = default)
        {
            var food = _mapper.Map<Food>(dto);
            _foodRepository.Create(food);
            await _foodRepository.SaveChangesAsync(cancellationToken);
            return CreatedAtAction(nameof(Get), new { food.Id }, _mapper.Map<FoodDTO>(food));
        }
        [HttpPut]
        public async Task<IActionResult> Update(FoodDTO dto, CancellationToken cancellationToken = default)
        {
            var food = await _foodRepository.FindByIdAsync(dto.Id, cancellationToken);
            if (food is null) return NotFound();
            _mapper.Map(dto, food);
            await _foodRepository.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var food = await _foodRepository.FindByIdAsync(id, cancellationToken);
            if (food is null) return NotFound();
            _foodRepository.Delete(food);
            await _foodRepository.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteAll(CancellationToken cancellationToken = default)
        {
            await _foodRepository.DeleteAll(cancellationToken);
            await _foodRepository.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}
