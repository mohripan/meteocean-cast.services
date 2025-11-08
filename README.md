# Meteocean.Services

A cloud-agnostic contracts library for metocean microservices:
- Shared **DTOs** (sites, thresholds, time series, events, subscriptions)
- **Provider** & **rule engine** interfaces
- **Message topic** constants
- Minimal **System.Text.Json** converters for compact time-series arrays

Targets **.NET 8.0** and **.NET 6.0**.

## Install (after publishing to NuGet)
```powershell
dotnet add package Meteocean.Services