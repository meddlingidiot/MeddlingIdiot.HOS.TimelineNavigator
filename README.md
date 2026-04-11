# ProMiles HOS Timeline Navigator

A .NET library for navigating and managing Hours of Service (HOS) timelines for commercial vehicle drivers. This library provides a robust framework for tracking duty status changes, GPS data, engine bus events, and regulatory compliance elements such as rest periods, shift extensions, and agricultural/adverse conditions exceptions.

## Overview

The Timeline Navigator manages multiple concurrent timelines of driver activity data, allowing applications to efficiently navigate through time-series events, query driver status at any point in time, and track regulatory compliance elements required for Hours of Service management.

## Solution Structure

- **ProMiles.HOS.TimelineNavigator** - Core library containing timeline navigation logic and moment/segment definitions
- **ProMiles.HOS.TimelineNavigator.Contracts** - Contract definitions and shared interfaces
- **ProMiles.HOS.UnitTests** - Comprehensive unit tests with ELD file test data

## Key Features

- **Multi-Timeline Management**: Simultaneously manages multiple timelines for different data types (duty status, GPS, engine bus, rest periods, etc.)
- **Bi-directional Navigation**: Move forward and backward through time with `Next()` and `Prior()` methods
- **Smart Jumping**: Jump to specific timestamps or regulatory events (rest periods, shift extensions)
- **Moment Types**: Support for various moment types including:
  - Duty status changes
  - GPS locations
  - Engine bus events
  - Rest periods
  - Shift extensions
  - Agricultural exceptions
  - Adverse conditions
  - Anchor points and events
- **Start-of-Day Tracking**: Configurable start-of-day definitions for compliance calculations
- **Segment Support**: Track extended time periods like shift extensions and regulatory exceptions
- **State Queries**: Query current duty status, odometer, location, and compliance flags at any timeline position

## Getting Started

### Prerequisites

- .NET 10.0 or .NET 8.0 (for main library)
- .NET 6.0 (for contracts)
- .NET 10.0 (for unit tests)

### Installation

Add a reference to the `ProMiles.HOS.TimelineNavigator` project in your solution:

```xml
<ProjectReference Include="..\ProMiles.HOS.TimelineNavigator\ProMiles.HOS.TimelineNavigator.csproj" />
```

### Dependency Injection Setup

The library includes dependency injection extensions for easy integration:

```csharp
services.AddTimelineNavigator(options => {
    // Configure start-of-day options if needed
});
```

### Basic Usage

```csharp
// Create a timeline navigator
var navigator = new TimelineNavigator();

// Add moments to the timeline
navigator.Add(new DutyStatusChangeMoment {
    Timestamp = DateTime.Now,
    DutyStatus = DutyStatus.Driving
});

navigator.Add(new GpsMoment {
    Timestamp = DateTime.Now,
    Latitude = 40.7128,
    Longitude = -74.0060
});

// Initialize after adding all moments
navigator.Initialize();

// Navigate through the timeline
navigator.JumpTo(specificTimestamp);
var currentStatus = navigator.DutyStatus;
var location = (navigator.Latitude, navigator.Longitude);

// Move to next/previous moments
navigator.Next();
navigator.Prior();

// Jump to specific events
navigator.JumpToNextRest();
navigator.JumpToPriorShiftExtension();
```

## API Reference

### Core Interface (ITimelineNavigator)

**Add/Upsert Operations:**
- `Add(DutyStatusChangeMoment)` / `Upsert(DutyStatusChangeMoment)`
- `Add(GpsMoment)` / `Upsert(GpsMoment)`
- `Add(EngineBusMoment)` / `Upsert(EngineBusMoment)`
- `Add(AnchorMoment)` / `Upsert(AnchorMoment)`
- `Add(RestMoment)` / `Upsert(RestMoment)`
- `Add(EventMoment)` / `Upsert(EventMoment)`
- `Upsert(ShiftExtensionSegment)`
- `Upsert(AgriculturalExceptionSegment)`
- `Upsert(AdverseConditionsSegment)`

**Navigation:**
- `Initialize()` - Initialize timelines after adding moments
- `JumpTo(DateTime)` - Jump to specific timestamp
- `JumpToPriorRest(bool? paired)` - Jump to previous rest period
- `JumpToNextRest(bool? paired)` - Jump to next rest period
- `JumpToPriorShiftExtension(bool? isExtended)` - Jump to previous shift extension
- `JumpToNextShiftExtension(bool? isExtended)` - Jump to next shift extension
- `Next()` - Move to next moment
- `Prior()` - Move to previous moment

**State Queries:**
- `CurrentDutyStatusChangeMoment` - Current duty status moment
- `DutyStatus` - Current duty status
- `Odometer` - Current odometer reading
- `Latitude` / `Longitude` - Current GPS coordinates
- `DriverIdNumber` / `TruckNumber` - Current driver and truck identifiers
- `IsShiftExtended` - Whether current shift is extended
- `IsAgriculturalExceptionEnabled` - Agricultural exception status
- `IsAdverseConditionsEnabled` - Adverse conditions status
- `IsStartOfDay` - Whether at start of day
- `StartOfDay(DateTime)` - Get start of day for given timestamp

## Build and Test

### Building the Solution

```bash
dotnet build
```

### Running Tests

The unit tests include test data from ELD files located in `ProMiles.HOS.UnitTests/TestData/`:

```bash
dotnet test
```

Test data includes:
- GEOTAB ELD files (.eld format)
- Driver files (.eldrivers format)
- Various test scenarios from May-July 2024

## Target Frameworks

- **TimelineNavigator**: Multi-targets .NET 10.0 and .NET 8.0
- **Contracts**: Targets .NET 6.0
- **UnitTests**: Targets .NET 10.0

## Dependencies

- Microsoft.Extensions.DependencyInjection.Abstractions 8.0.1
- Ninject 3.3.6 (Contracts project)
- NUnit 3.13.3 (Testing)
- FuelTaxAutomation.ConnectionStrings 1.0.15 (Testing)