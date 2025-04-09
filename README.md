# mediator-benchmarks
small testing repo for benchmarking custom and well-known mediator libraries


## Add new test implementation and run tests

*for testing benchmarks*
1. create new project where name == Mediator.Impl.<what you want> - should be classlib .NET 8

2. Add your handler in the Mediator.Shared/TestHandlers - just do the same as the Simple mediator but with new command and handler names

3. Add your implementation in the benchmarks. Mediator.Benchmarks/MediatorImplementationBenchmarks.cs


build the benchmarks
```csharp
cd src/Mediator.Benchmarks/
dotnet build -c Release
```

run the benchmarks
```csharp
dotnet bin/Release/net8.0/Mediator.Benchmarks.dll
```

*for testing api*

4. Add your api request in the Mediator.Performance/program.cs

5. build the docker container

```powershell
docker build -t mediatr -f src/Mediator.Performance/Dockerfile .
```

6. run the docker container

```powershell
docker run -d -p 8080:8080 --cpus="2.0" --memory="2g" mediatr
```

7. change the url in the performance_test.js to your new api endpoint

8. run the k6 test
```powershell
k6 run performance_test.json
```


## Run Benchmarks



## Testing Simple Mediator vs the Official in api request/sec

### Official Mediator library request/sec
![offical-mediator](docs/assets/official-mediator.png)

### Simple Mediator library requests/sec
![simple-mediator](docs/assets/simple-mediator.png)


</br>

## Testing types as requests

![type benchmarks](docs/assets/mediator-types-benchmark.png)


</br>

## Testing implementations vs Official

![implementation benchmarks](docs/assets/mediator-implementations.png)
