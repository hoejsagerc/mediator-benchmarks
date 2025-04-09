using BenchmarkDotNet;
using BenchmarkDotNet.Running;
using Mediator.Benchmarks;

// var summary = BenchmarkRunner.Run<MediatorCommandTypeBenchmarks>();
var summary = BenchmarkRunner.Run<MediatorImplementationBenchmarks>();
Console.WriteLine(summary);