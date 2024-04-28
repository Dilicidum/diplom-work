﻿using Domain.Interfaces;
using Services.Abstractions.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Domain.Entities;
using Domain.Interfaces;
using Services.Abstractions.DTO;
using System.Net.Http.Json;
using Services.Abstractions.Interfaces;
using System;
namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AssesmentsController : ControllerBase
    {
        private readonly IVikorService _vikorService;
        private readonly ICandidatesService _candidatesService;
        //private readonly ITopsisService _topsisService;
        public AssesmentsController(IVikorService vikorService, ICandidatesService candidatesService)
        {
            _vikorService = vikorService;
            _candidatesService = candidatesService;
            //_topsisService = topsisService;
        }

        [HttpPost("{vacancyId}")]
        public async Task<IActionResult> GetAssesmentsFromFile([FromForm]FileUploadModel model, int vacancyId) {
            
            var criteriasArray = JsonConvert.DeserializeObject<double[]>(model.Criterias);

            var array = await _candidatesService.GetCriteriasForCandidatesForVacancy(vacancyId);
            //model.File = null;
            if(model.File != null)
            {
                array = await ParseFile(model.File);
            }

            var result = new ModelResponse();

            switch (model.Method) {
                case Method.Vikor:
                    result = _vikorService.GetBestAlternatives(array,criteriasArray.ToList());
                    break;
                case Method.Topsis:
                    //result = _topsisService.GetBestAlternatives(array);
                    break;
            }

            return Ok(result);
        } 

        private async Task<double[,]> ParseFile(IFormFile file)
        {
            var Matrix = new List<List<double>>();
            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream))
            {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                
                var row = new List<double>();
                var values = line.Split(' ');
                foreach (var value in values)
                {
                    if (int.TryParse(value, out int number))
                    {
                        row.Add(number);
                    }
                }
                Matrix.Add(row);
            }
            }
            
            int rows = Matrix.Count;
            int cols = Matrix.Max(subList => subList.Count);
            double[,] array = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < Matrix[i].Count; j++) // Using list[i].Count to handle lists of different sizes
                {
                    array[i, j] = Matrix[i][j];
                }
            }

            return array;
        }
    }
}