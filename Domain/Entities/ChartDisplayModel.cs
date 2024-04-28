﻿namespace Domain.Entities;

public class ChartDisplayModel
{
    public ChartDisplayModel() {
        Series = new List<Serie> { new Serie() };
    }

    public string Name { get;set;}

    public List<Serie> Series { get; set;}
}

public class Serie
{
    public string Name { get;set;}

    public double Value { get;set;}
}

