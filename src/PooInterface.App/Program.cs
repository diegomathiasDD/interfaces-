using PooInterface.Core.Formatters;
using PooInterface.Core.Models;
using PooInterface.Core.Repositories;

Console.WriteLine("PooInterface demo - Fase 3: OO sem Interface\n");

// Demo formatter usage (Fase 3: OO com polimorfismo, sem if/switch)
var raw = "hello world from poointerface";

// Usando factory para seleção dinâmica de formatadores (sem switch)
var modos = new[] { "upper", "lower", "title", "reverse", "invalid" };
Console.WriteLine("OO formatters (polimorfismo):");
foreach (var modo in modos)
{
    var formatter = FormatterOOFactory.GetFormatter(modo);
    var result = formatter.Format(raw);
    Console.WriteLine($"  Mode '{modo}': {result}");
}

// Demo repository (in-memory)
var repo = new InMemoryRepository();
var t1 = new ToDo("Write demo");
repo.Add(t1);
Console.WriteLine($"Added todo: {t1.Id} - {t1.Title}");

foreach (var t in repo.List())
    Console.WriteLine($"Stored: {t.Id} {t.Title} (done:{t.Done})");

Console.WriteLine("\nDemo finished.");