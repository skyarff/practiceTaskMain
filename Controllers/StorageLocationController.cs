﻿using Microsoft.AspNetCore.Mvc;
using StockService.Models;
using StockService.Models.dto;
using StockService.Repository.CookieRep;
using StockService.Repository.EmployeeRep;
using StockService.Repository.StorageLocationRep;

namespace StockService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageLocationController : ControllerBase
    {
        private readonly IStorageLocationService _storageLocationService;
        private Response _response;

        public StorageLocationController(IStorageLocationService storageLocationService)
        {
            _storageLocationService = storageLocationService;
            this._response = new Response();
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateStorageLocation([FromForm] StorageLocationDto storageLocationDto)
        {
            try
            {
                _response = await _storageLocationService.CreateStorageLocationAsync(storageLocationDto);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpDelete("dellById/{storageLocationId}")]
        public async Task<IActionResult> DeletesStorageLocationAsync(int storageLocationId)
        {
            try
            {
                _response = await _storageLocationService.DeletesStorageLocationAsync(storageLocationId);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpGet("getById/{storageLocationId}")]
        public async Task<IActionResult> GetStorageLocationById(int storageLocationId)
        {
            try
            {
                _response = await _storageLocationService.GetStorageLocationByIdAsync(storageLocationId);

                if (_response.IsSuccess)
                {
                    return Ok(_response);
                }
                else
                {
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpGet("getByStockId/{stockId}")]
        public async Task<IActionResult> GetStorageLocationsByStockIdAsync(int stockId)
        {
            try
            {
                _response = await _storageLocationService.GetStorageLocationsByStockIdAsync(stockId);

                if (_response.IsSuccess)
                {
                    return Ok(_response);
                }
                else
                {
                    return NotFound(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return BadRequest(_response);
            }
        }

        [HttpPut("update")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateStorageLocationAsync([FromForm] StorageLocationDto storageLocationDto)
        {
            try
            {
                var response = await _storageLocationService.UpdateStorageLocationAsync(storageLocationDto);
                if (response.IsSuccess)
                {
                    return Ok(response);
                }
                else
                {
                    return NotFound(response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errors.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, _response);
            }
        }

    }
}