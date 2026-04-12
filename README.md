# Automation.HOS.TimelineNavigator

A .NET library for navigating multi-dimensional Hours of Service (HOS) timelines for commercial vehicle drivers. It provides bi-directional, timestamp-based navigation across up to ten concurrent event streams — duty status, GPS, engine bus, rest periods, shift extensions, and regulatory exceptions — and exposes a unified state snapshot at any point in time.

## Solution Structure

```
Automation.HOS.TimelineNavigator/
├── Automation.HOS.TimelineNavigator        # Core library
└── Automation.HOS.TImelineNavigator.UnitTests  # TUnit test suite
```

## Target Frameworks

Both projects multi-target **.NET 8**, **.NET 9**, and **.NET 10**.

## Core Concepts

### Moment

`Moment` is the abstract base for every point-in-time event. All subclasses are sealed and implement `ICloneable`.

| Type | Key Properties |
|------|----------------|
| `DutyStatusChangeMoment` | `CurrentDutyStatus`, `Comment` |
| `GpsMoment` | `Latitude`, `Longitude`, `DistanceTo(GpsMoment)` |
| `EngineBusMoment` | `Odometer`, `DistanceTo(EngineBusMoment)` |
| `RestMoment` | `Duration`, `IsFullRest`, `IsGlobalReset`, `IsQualified`, `IsPrimary`, `IsPaired` |
| `AnchorMoment` | `Comment` |
| `EventMoment` | `EventCode`, `Comment` |
| `ShiftExtensionMoment` | `IsExtended` |
| `AgriculturalExceptionMoment` | `IsEnabled` |
| `AdverseConditionsMoment` | `IsEnabled` |
| `StartOfDayMoment` | _(timestamp only)_ |

Every moment carries `Timestamp`, `TruckNumber`, and `DriverIdNumber` from the base class.

### Segment

Segments represent time ranges (start/finish). Upserting a segment produces two moments: one at `StartTimestamp` with the feature enabled, one at `FinishTimestamp` with it disabled.

| Type | Key Properties |
|------|----------------|
| `ShiftExtensionSegment` | `IsExtended`, `StartTimestamp`, `FinishTimestamp` |
| `AgriculturalExceptionSegment` | `IsEnabled`, `StartTimestamp`, `FinishTimestamp` |
| `AdverseConditionsSegment` | `IsEnabled`, `StartTimestamp`, `FinishTimestamp` |

### Timeline\<T\>

A sorted, index-navigable list of moments. `Initialize()` must be called once after all `Add` calls to sort by timestamp.

```csharp
public interface ITimeline<T> where T : Moment
{
    void Add(T value);
    void Upsert(T value);      // Add or replace by timestamp
    void Initialize();         // Sort by timestamp — call once after loading
    void Clear();
    void Next();
    void Prior();
    bool IsBeginningOfTime();  // Current index is at the sentinel
    bool IsEndOfTime();        // Current index is at the last moment
    T CurrentMoment { get; }
    void MoveOnOrBefore(DateTime timestamp);  // Greatest moment ≤ timestamp
    void MoveOnOrAfter(DateTime timestamp);   // Least moment ≥ timestamp
}
```

`Timeline<T>` maintains a beginning-of-time sentinel (`DateTime.MinValue`) so navigation never runs off the start of the list.

### TimelineNavigator

`TimelineNavigator` orchestrates ten `Timeline<T>` instances behind a single interface. It tracks a **current window** between `Start` and `Finish` moments and keeps cached current-moment references for each timeline, refreshed on every navigation call.

```
┌──────────────────────────────────────────────────────────────────┐
│  Duty Status │ GPS │ Engine Bus │ Rest │ Shift Ext │ Ag Ex │ ...  │
│  ────────────────────────────────────────────────────────────── │
│  [BOT]──[M1]──[M2]──[M3]──[M4]──[M5]──[M6]──[M7]──[M8]─[EOT]  │
│                      ^Start        ^Finish                       │
└──────────────────────────────────────────────────────────────────┘
```

`Start` and `Finish` are the boundary moments of the current interval. `Next()` shifts the window forward; `Prior()` shifts it back.

## Installation

Reference the library project directly:

```xml
<ProjectReference Include="..\Automation.HOS.TimelineNavigator\Automation.HOS.TimelineNavigator.csproj" />
```

Register with the DI container:

```csharp
services.AddTimelineNavigator();
// Registers ITimelineNavigator as Transient
```

## Usage

```csharp
var navigator = new TimelineNavigator();

// 1. Add moments (any order)
navigator.Add(new DutyStatusChangeMoment(DateTime.Parse("2024-05-15 08:00"), DutyStatus.Driving, "DRV001"));
navigator.Add(new GpsMoment(DateTime.Parse("2024-05-15 08:00"), 40.7128, -74.0060, "DRV001"));
navigator.Add(new RestMoment(DateTime.Parse("2024-05-15 18:00"), duration: TimeSpan.FromHours(10), ...));

// 2. Add a segment (produces two moments internally)
navigator.Upsert(new ShiftExtensionSegment {
    StartTimestamp = DateTime.Parse("2024-05-15 16:00"),
    FinishTimestamp = DateTime.Parse("2024-05-15 18:00"),
    IsExtended = true
});

// 3. Initialize — sorts all timelines
navigator.Initialize();

// 4. Jump to a timestamp
navigator.JumpTo(DateTime.Parse("2024-05-15 09:00"));
Console.WriteLine(navigator.DutyStatus);   // Driving
Console.WriteLine(navigator.Latitude);     // 40.7128

// 5. Step forward/backward
navigator.Next();
navigator.Prior();

// 6. Jump to regulatory events
navigator.JumpToNextRest();
navigator.JumpToPriorShiftExtension(isExtended: true);

// 7. Enumerate all moments
foreach (var moment in navigator)
{
    Console.WriteLine($"{moment.Timestamp}: {navigator.DutyStatus}");
}
```

## API Reference

### Add / Upsert

```csharp
void Add(DutyStatusChangeMoment)
void Add(GpsMoment)
void Add(EngineBusMoment)
void Add(AnchorMoment)
void Add(RestMoment)
void Add(EventMoment)

void Upsert(DutyStatusChangeMoment)
void Upsert(GpsMoment)
void Upsert(EngineBusMoment)
void Upsert(AnchorMoment)
void Upsert(RestMoment)
void Upsert(EventMoment)
void Upsert(ShiftExtensionSegment)
void Upsert(AgriculturalExceptionSegment)
void Upsert(AdverseConditionsSegment)
```

### Navigation

```csharp
void Initialize()                               // Sort all timelines — call once after loading

void JumpTo(DateTime)                           // Move to closest moment on/before timestamp
void Next()                                     // Advance one interval
void Prior()                                    // Step back one interval

void JumpToNextRest(bool? paired = null)        // Find next rest, optionally filter by IsPaired
void JumpToPriorRest(bool? paired = null)
void JumpToNextShiftExtension(bool? isExtended = null)
void JumpToPriorShiftExtension(bool? isExtended = null)

bool IsBeginningOfTime()                        // Start.Timestamp == DateTime.MinValue
bool IsEndOfTime()                              // No later moment exists
```

### State Queries

```csharp
Moment Start                        // Current interval start
Moment Finish                       // Current interval end
DateTime StartTimestamp
DateTime FinishTimestamp
TimeSpan Length                     // FinishTimestamp - StartTimestamp

DutyStatusChangeMoment CurrentDutyStatusChangeMoment
DutyStatus DutyStatus

GpsMoment CurrentGpsMoment
double Latitude
double Longitude

EngineBusMoment CurrentEngineBusMoment
decimal Odometer

RestMoment CurrentRestMoment

string? DriverIdNumber
string? TruckNumber

bool IsShiftExtended
bool IsAgriculturalExceptionEnabled
bool IsAdverseConditionsEnabled

bool IsStartOfDay
DateTime CurrentDay
DateTime StartOfDay(DateTime timestamp)
```

### Rest Explorer Extensions

High-level helpers for rest-period navigation:

```csharp
bool IsRest(this ITimelineNavigator)
void MoveOffRest(this ITimelineNavigator, TimelineDirection, ILogger?)
void MoveToStartOfCurrentRest(this ITimelineNavigator)
void MoveToEndOfCurrentRest(this ITimelineNavigator)

Moment FindRest(
    this ITimelineNavigator navigator,
    TimeSpan minToFind,
    TimelineDirection direction = TimelineDirection.Forward,
    PreferredEndOfRest preferredEndOfRest = PreferredEndOfRest.Ending,
    MoveTo moveTo = MoveTo.NewLocation)
```

`FindRest` accumulates consecutive rest time until `minToFind` is satisfied and positions at the beginning or end of that rest period. Pass `MoveTo.None` to restore the original position after the search.

## DutyStatus

```csharp
public enum DutyStatus
{
    Unknown = 0,
    OffDuty = 1,
    Sleeper = 2,
    Driving = 3,
    OnDuty = 4,
    OffDutyWaitingAtWellSite = 5
}
```

`DutyStatuses` provides static groupings: `RestDutyStatuses`, `WorkingDutyStatuses`, `DrivingDutyStatus`, `AllNormalDutyStatuses`, etc.

## Geotab Mapping

```csharp
DutyStatus status = GeotabDutyStatusToDutyStatusMapper.Map("D"); // → Driving
// "OFF" → OffDuty | "SB" → Sleeper | "D" → Driving | "ON" → OnDuty | "WT" → OffDutyWaitingAtWellSite
```

## Logging

```csharp
ILogger logger = new InMemoryLogger();
logger.Initialize(new List<string> { LoggerCategories.RestBuilding });
navigator.DumpSplitRestTimeline(logger);
string output = logger.GetResults();
await logger.SaveToFileAsync("timeline_dump.txt");
```

Use `NullLogger` in production. `LoggerCategories` constants: `General`, `RestBuilding`, `Pairing`, `Accumulators`, `SleeperSplitLoop`, `DailyLoop`, `Rule`, `Violations`, `ShiftExtAudit`, and more.

## Serialization

`TimelineNavigator` is marked `[Serializable]`. All moments and timelines are serializable. Runtime-cached current-moment references are `[NonSerialized]` and are rehydrated on the next navigation call.

## Building and Testing

```bash
dotnet build
dotnet test
```

Tests use [TUnit](https://github.com/thomhurst/TUnit) with the Microsoft Testing Platform (MTP).
