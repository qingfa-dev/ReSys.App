# ReSys.App

A .NET 10 solution built with Vertical Slices architecture and Aspire.

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later
- [Docker](https://www.docker.com/products/docker-desktop/) (for Testcontainers)

## Quick Start

```bash
# Clone
git clone https://github.com/your-org/ReSys.App.git
cd ReSys.App

# Build
make build

# Run API
dotnet run --project src/Api

# Run with Aspire
dotnet run --project src/AppHost

# Run tests
make test
```

## Architecture

Vertical Slices with clear layer separation:

```
src/
├── Api/              HTTP entry point
├── Application/      Business logic (vertical slices)
├── Platform/         Core technology layer
├── ServiceDefaults/  Aspire defaults
└── AppHost/          Aspire orchestrator
```

**Dependency chain:** `Api → Application → Platform`

See [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) for details.

## Project Structure

```
ReSys.App/
├── src/                    Source projects
│   ├── Api/
│   ├── Application/
│   ├── Platform/
│   ├── ServiceDefaults/
│   └── AppHost/
├── tests/                  Test projects
│   ├── Api.Tests/
│   ├── Application.Tests/
│   ├── Platform.Tests/
│   └── ArchitectureTests/
├── docs/                   Documentation
├── scripts/                Build/test scripts
├── Directory.Packages.props
├── Directory.Build.props
└── Makefile
```

## Commands

| Command | Description |
|---------|-------------|
| `make build` | Build solution |
| `make test` | Run all tests |
| `make test-unit` | Unit tests only |
| `make test-int` | Integration tests |
| `make test-arch` | Architecture tests |
| `make ci` | Full CI pipeline |
| `make clean` | Clean artifacts |

## License

[MIT](LICENSE)
