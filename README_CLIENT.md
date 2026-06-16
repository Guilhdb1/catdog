# CatDog - Client/Server Run Instructions

## Backend (ASP.NET Core)

From repo root run:

```powershell
cd src\CatDog.Api
dotnet build
dotnet run
```

The API will run on the configured port (use `appsettings.json` or environment overrides). Ensure `JwtSettings:Secret` is set in production.

## Frontend (Angular - development)

This repo contains a minimal Angular scaffolding. To run the client during development:

```bash
# from repo root
npm install
# then
npm start
```

This uses the `proxy.conf.json` to forward `/auth` requests to the backend at `https://localhost:5001`. Ensure the backend is running and its HTTPS endpoint is reachable.

Notes:
- The Angular project created here is a minimal manifest. You may prefer to generate a full Angular app with `ng new` and then copy the `src/app/auth` folder into it.
- Email service is currently a placeholder; configure a transactional provider before using email flows.
- SonarQube analysis could not be executed in this environment due to missing scanner/Docker; run Sonar locally or via CI with available tokens.
## Running SonarQube analysis (CI or locally)

This repository includes a GitHub Actions workflow `.github/workflows/ci-sonar.yml` that runs Sonar analysis using `dotnet-sonarscanner`. To use it configure the following repository secrets:

- `SONAR_TOKEN` — your Sonar authentication token
- `SONAR_HOST_URL` — the SonarQube or SonarCloud host URL (e.g. https://sonarcloud.io)

Alternatively, to run locally:

1. Install `dotnet-sonarscanner`:

```powershell
dotnet tool install --global dotnet-sonarscanner
```

2. Run the scanner around a build:

```powershell
dotnet sonarscanner begin /k:"catdog-api" /d:sonar.host.url:"<SONAR_HOST_URL>" /d:sonar.login:"<SONAR_TOKEN>"
dotnet build src/CatDog.Api -c Release
dotnet sonarscanner end /d:sonar.login:"<SONAR_TOKEN>"
```

If you cannot run Sonar locally (missing Docker or scanner), push a branch and open a pull request to trigger the CI workflow.
