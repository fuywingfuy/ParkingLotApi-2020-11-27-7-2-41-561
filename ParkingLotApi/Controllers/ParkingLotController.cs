﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("parkinglots")]
    public class ParkingLotController : ControllerBase
    {
        private readonly ParkingLotService parkingLotService;

        public ParkingLotController(ParkingLotService parkingLotService)
        {
            this.parkingLotService = parkingLotService;
        }

        [HttpPost]
        public async Task<ActionResult<ParkingLotDto>> Add(ParkingLotDto parkingLotDto)
        {
            if (string.IsNullOrEmpty(parkingLotDto.Name))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Input with Name null or empty." });
            }

            if (string.IsNullOrEmpty(parkingLotDto.Location))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Input with No Location null or empty" });
            }

            if (parkingLotDto.Capacity == null || parkingLotDto.Capacity < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Input with Capacity null or minus." });
            }

            var id = await this.parkingLotService.AddParkingLot(parkingLotDto);
            return CreatedAtAction(nameof(GetById), new { id = id }, parkingLotDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetById(int id)
        {
            var parkingLotDto = await this.parkingLotService.GetById(id);
            return Ok(parkingLotDto);
        }
    }
}
