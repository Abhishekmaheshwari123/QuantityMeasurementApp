using Microsoft.AspNetCore.Mvc;
using QuantityMeasurement.Api.Contracts;
using ModelLayer.DTOs;
using BusinessLayer.Interfaces;
using QuantityMeasurement.Domain.Exceptions;

namespace QuantityMeasurement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class QuantityMeasurementController : ControllerBase
    {
        private readonly IQuantityMeasurementService _quantityMeasurementService;

        public QuantityMeasurementController(IQuantityMeasurementService quantityMeasurementService)
        {
            _quantityMeasurementService = quantityMeasurementService;
        }

        [HttpPost("compare")]
        public ActionResult<ApiResponse<bool>> Compare([FromBody] CompareRequest request)
        {
            try
            {
                bool result = _quantityMeasurementService.Compare(
                    MapToDto(request.FirstQuantity),
                    MapToDto(request.SecondQuantity));

                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Comparison completed successfully.",
                    Data = result
                });
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(new ApiResponse<bool>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = false
                });
            }
        }

        [HttpPost("convert")]
        public ActionResult<ApiResponse<QuantityDto>> Convert([FromBody] ConvertRequest request)
        {
            try
            {
                var result = _quantityMeasurementService.Convert(
                    MapToDto(request.SourceQuantity),
                    request.TargetUnit);

                return Ok(new ApiResponse<QuantityDto>
                {
                    Success = true,
                    Message = "Conversion completed successfully.",
                    Data = result
                });
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(new ApiResponse<QuantityDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("add")]
        public ActionResult<ApiResponse<QuantityDto>> Add([FromBody] ArithmeticRequest request)
        {
            try
            {
                var result = _quantityMeasurementService.Add(
                    MapToDto(request.FirstQuantity),
                    MapToDto(request.SecondQuantity),
                    request.TargetUnit);

                return Ok(new ApiResponse<QuantityDto>
                {
                    Success = true,
                    Message = "Addition completed successfully.",
                    Data = result
                });
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(new ApiResponse<QuantityDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("subtract")]
        public ActionResult<ApiResponse<QuantityDto>> Subtract([FromBody] ArithmeticRequest request)
        {
            try
            {
                var result = _quantityMeasurementService.Subtract(
                    MapToDto(request.FirstQuantity),
                    MapToDto(request.SecondQuantity),
                    request.TargetUnit);

                return Ok(new ApiResponse<QuantityDto>
                {
                    Success = true,
                    Message = "Subtraction completed successfully.",
                    Data = result
                });
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(new ApiResponse<QuantityDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("divide")]
        public ActionResult<ApiResponse<double>> Divide([FromBody] ArithmeticRequest request)
        {
            try
            {
                double result = _quantityMeasurementService.Divide(
                    MapToDto(request.FirstQuantity),
                    MapToDto(request.SecondQuantity));

                return Ok(new ApiResponse<double>
                {
                    Success = true,
                    Message = "Division completed successfully.",
                    Data = result
                });
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(new ApiResponse<double>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = 0
                });
            }
        }

        private static QuantityDto MapToDto(QuantityRequest request)
        {
            return new QuantityDto(
                request.Value,
                request.Unit,
                request.MeasurementType);
        }
    }
}