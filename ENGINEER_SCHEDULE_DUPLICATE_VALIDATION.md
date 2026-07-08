# Engineer Schedule Duplicate Validation Implementation

## Overview
Added comprehensive validation checks in `CreateEngSchedulerAsync` method to prevent duplicate and overlapping engineer schedules.

## Changes Made

### File Modified
- **Location:** `Infrastructure\Services\EngSchedulerService.cs`
- **Method:** `CreateEngSchedulerAsync`

### Validation Checks Added

#### 1. **Required Field Validations**
- ? Validates `EngId` is not empty
- ? Validates `StartTime` is not null or empty
- ? Validates `EndTime` is not null or empty

**Exception Type:** `ArgumentException`
**Error Message:** Clear message indicating which field is missing

#### 2. **Exact Duplicate Check**
Prevents creating a schedule when:
- Same engineer (`EngId`)
- **AND** Same start time
- **AND** Same end time
- **AND** Record is not deleted

**Query:**
```csharp
WHERE !IsDeleted 
  AND EngId == newEngId 
  AND StartTime == newStartTime 
  AND EndTime == newEndTime
```

**Exception Type:** `InvalidOperationException`
**Error Message:** `"A schedule already exists for engineer {EngId} with the same start time ({StartTime}) and end time ({EndTime})."`

#### 3. **Overlapping Schedule Check**
Prevents creating a schedule when:
- Same engineer (`EngId`)
- **AND** Schedules have overlapping time slots
- **AND** Record is not deleted

**Overlap Logic:**
```
Existing:  [----existing.Start----existing.End----]
New:                    [----new.Start----new.End----]
                        ? Overlaps!

Comparison:
- existing.StartTime < new.EndTime (existing starts before new ends)
- existing.EndTime > new.StartTime (existing ends after new starts)
```

**Exception Type:** `InvalidOperationException`
**Error Message:** `"An overlapping schedule exists for engineer {EngId}. Existing schedule: {StartTime} to {EndTime}. New schedule: {NewStartTime} to {NewEndTime}."`

## Technical Details

### Validation Order
1. Required field validation (fails fast if data is invalid)
2. Exact duplicate check (prevents exact duplicates)
3. Overlapping schedule check (prevents time conflicts)
4. If all validations pass ? Schedule is created

### Database Query Performance
- **IsDeleted Filter:** Only active (non-deleted) schedules are checked
- **String Comparison:** Uses `CompareTo()` for time string comparisons
  - Assumes time format is consistent and comparable (e.g., "HH:mm", "HH:mm:ss", ISO 8601)

### Exception Handling
The method throws exceptions for validation failures:
- `ArgumentException` - For missing required fields
- `InvalidOperationException` - For duplicate/overlapping conflicts

**Caller Responsibility:** API controller or service layer should catch these exceptions and return appropriate HTTP status codes (400 Bad Request, 409 Conflict, etc.)

## Usage Example

### Valid Schedule Creation
```csharp
var newSchedule = new EngScheduler
{
    EngId = engineerId,
    StartTime = "2024-01-15 09:00",
    EndTime = "2024-01-15 12:00",
    Subject = "Service Visit",
    SerReqId = serviceRequestId
};

var scheduleId = await engSchedulerService.CreateEngSchedulerAsync(newSchedule);
// ? Success - Schedule created
```

### Validation Failures

**Missing Engineer ID:**
```csharp
var newSchedule = new EngScheduler
{
    EngId = Guid.Empty,  // ? Invalid
    StartTime = "2024-01-15 09:00",
    EndTime = "2024-01-15 12:00"
};

// ? Throws: ArgumentException("Engineer ID (EngId) is required.")
```

**Duplicate Schedule:**
```csharp
// First schedule (exists in DB)
var existing = new EngScheduler
{
    EngId = engineerId,
    StartTime = "2024-01-15 09:00",
    EndTime = "2024-01-15 12:00"
};

// Second schedule (attempt to create identical)
var duplicate = new EngScheduler
{
    EngId = engineerId,  // Same engineer
    StartTime = "2024-01-15 09:00",  // Same start time
    EndTime = "2024-01-15 12:00"  // Same end time
};

// ? Throws: InvalidOperationException("A schedule already exists...")
```

**Overlapping Schedule:**
```csharp
// Existing schedule
Existing: 09:00 - 12:00

// New overlapping schedule (attempt to create)
New: 10:00 - 13:00  // ? Overlaps with existing (10:00-12:00)

// ? Throws: InvalidOperationException("An overlapping schedule exists...")
```

## Testing Recommendations

### Unit Tests
- [ ] Test with null/empty EngId
- [ ] Test with null/empty StartTime
- [ ] Test with null/empty EndTime
- [ ] Test exact duplicate prevention
- [ ] Test overlapping schedule prevention (various overlap scenarios)
- [ ] Test valid schedule creation
- [ ] Test soft-deleted schedules are ignored

### Integration Tests
- [ ] Create schedule for engineer
- [ ] Attempt duplicate ? Should fail
- [ ] Attempt overlap ? Should fail
- [ ] Create non-overlapping schedule ? Should succeed
- [ ] Multiple engineers with same time ? Should succeed

## Build Status
? **Build Successful** - No compilation errors

## Files Modified
- `Infrastructure\Services\EngSchedulerService.cs` (1 method)

## Breaking Changes
?? **Exception Handling Required**
- Calling code must now handle `ArgumentException` and `InvalidOperationException`
- API controllers should catch these and return appropriate HTTP status codes

## Recommendations for API Controller

```csharp
[HttpPost("create")]
public async Task<IActionResult> CreateSchedule([FromBody] EngSchedulerRequest request)
{
    try
    {
        var scheduler = request.Adapt<EngScheduler>();
        var scheduleId = await engSchedulerService.CreateEngSchedulerAsync(scheduler);
        return Ok(new { Id = scheduleId });
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { Error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return Conflict(new { Error = ex.Message });
    }
}
```

## Next Steps
1. Update API controllers to handle new exceptions
2. Add unit tests for validation scenarios
3. Update API documentation with new error responses
4. Test with real scheduling workflows

---

**Status:** ? Implementation Complete
**Date:** $(date)
**Version:** 1.0
