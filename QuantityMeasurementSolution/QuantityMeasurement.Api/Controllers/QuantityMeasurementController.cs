using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using ModelLayer.Entities;
using QuantityMeasurement.Api.Contracts;

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
            bool result = _quantityMeasurementService.Compare(
                MapToDto(request.FirstQuantity),
                MapToDto(request.SecondQuantity)
            );

            return Ok(
                new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Comparison completed successfully.",
                    Data = result,
                }
            );
        }

        [HttpPost("convert")]
        public ActionResult<ApiResponse<QuantityDto>> Convert([FromBody] ConvertRequest request)
        {
            var result = _quantityMeasurementService.Convert(
                MapToDto(request.SourceQuantity),
                request.TargetUnit
            );

            return Ok(
                new ApiResponse<QuantityDto>
                {
                    Success = true,
                    Message = "Conversion completed successfully.",
                    Data = result,
                }
            );
        }

        [HttpPost("add")]
        public ActionResult<ApiResponse<QuantityDto>> Add([FromBody] ArithmeticRequest request)
        {
            var result = _quantityMeasurementService.Add(
                MapToDto(request.FirstQuantity),
                MapToDto(request.SecondQuantity),
                request.TargetUnit
            );

            return Ok(
                new ApiResponse<QuantityDto>
                {
                    Success = true,
                    Message = "Addition completed successfully.",
                    Data = result,
                }
            );
        }

        [HttpPost("subtract")]
        public ActionResult<ApiResponse<QuantityDto>> Subtract([FromBody] ArithmeticRequest request)
        {
            var result = _quantityMeasurementService.Subtract(
                MapToDto(request.FirstQuantity),
                MapToDto(request.SecondQuantity),
                request.TargetUnit
            );

            return Ok(
                new ApiResponse<QuantityDto>
                {
                    Success = true,
                    Message = "Subtraction completed successfully.",
                    Data = result,
                }
            );
        }

        [HttpPost("divide")]
        public ActionResult<ApiResponse<double>> Divide([FromBody] ArithmeticRequest request)
        {
            double result = _quantityMeasurementService.Divide(
                MapToDto(request.FirstQuantity),
                MapToDto(request.SecondQuantity)
            );

            return Ok(
                new ApiResponse<double>
                {
                    Success = true,
                    Message = "Division completed successfully.",
                    Data = result,
                }
            );
        }

        [HttpGet("history/operation/{operation}")]
        public ActionResult<
            ApiResponse<IReadOnlyList<QuantityMeasurementEntity>>
        > GetHistoryByOperation(string operation)
        {
            var items = _quantityMeasurementService.GetHistoryByOperation(operation);

            return Ok(
                new ApiResponse<IReadOnlyList<QuantityMeasurementEntity>>
                {
                    Success = true,
                    Message = "Operation history retrieved successfully.",
                    Data = items,
                }
            );
        }

        [HttpGet("history/type/{measurementType}")]
        public ActionResult<
            ApiResponse<IReadOnlyList<QuantityMeasurementEntity>>
        > GetHistoryByMeasurementType(string measurementType)
        {
            var items = _quantityMeasurementService.GetHistoryByMeasurementType(measurementType);

            return Ok(
                new ApiResponse<IReadOnlyList<QuantityMeasurementEntity>>
                {
                    Success = true,
                    Message = "Measurement type history retrieved successfully.",
                    Data = items,
                }
            );
        }

        [HttpGet("history/errored")]
        public ActionResult<
            ApiResponse<IReadOnlyList<QuantityMeasurementEntity>>
        > GetErroredHistory()
        {
            var items = _quantityMeasurementService.GetErroredHistory();

            return Ok(
                new ApiResponse<IReadOnlyList<QuantityMeasurementEntity>>
                {
                    Success = true,
                    Message = "Errored history retrieved successfully.",
                    Data = items,
                }
            );
        }

        [HttpGet("count/{operation}")]
        public ActionResult<ApiResponse<int>> GetOperationCount(string operation)
        {
            int count = _quantityMeasurementService.GetOperationCount(operation);

            return Ok(
                new ApiResponse<int>
                {
                    Success = true,
                    Message = "Operation count retrieved successfully.",
                    Data = count,
                }
            );
        }

        private static QuantityDto MapToDto(QuantityRequest request)
        {
            return new QuantityDto(request.Value, request.Unit, request.MeasurementType);
        }
    }
}
