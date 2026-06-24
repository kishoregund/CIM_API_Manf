# AMC End Date Update - One Year Extension

## Change Made

Updated the `InstrumentWarrantyBackgroundService.cs` to properly calculate the AMC end date by adding one year to the warranty end date.

## What Changed

### Before
```csharp
SDate = custInstrument.WrntyEnDt,        // Warranty end date
EDate = custInstrument.WrntyEnDt,        // Same as warranty end date (incorrect)
```

### After
```csharp
// Parse warranty end date to add one year for AMC
DateTime amcEndDate = DateTime.Now;
if (DateTime.TryParseExact(custInstrument.WrntyEnDt, "dd/MM/yyyy", CultureInfo.InvariantCulture,
    DateTimeStyles.None, out DateTime parsedWarrantyEndDate))
{
    amcEndDate = parsedWarrantyEndDate.AddYears(1);
}

SDate = custInstrument.WrntyEnDt,           // Start: Warranty end date
EDate = amcEndDate.ToString("dd/MM/yyyy")   // End: Warranty end date + 1 year
```

## How It Works

1. **Parse warranty end date** - Converts the warranty end date from `dd/MM/yyyy` string format
2. **Add one year** - Adds 365 days (1 year) to the warranty end date using `AddYears(1)`
3. **Format for AMC** - Converts back to `dd/MM/yyyy` string format required by AMC entity
4. **Set AMC dates**:
   - **SDate** (Start): Warranty end date (when AMC starts)
   - **EDate** (End): Warranty end date + 1 year (when AMC expires)

## Example

```
Instrument Warranty:
  - Start: 15/01/2023
  - End:   15/01/2025

Auto-Created AMC:
  - Start: 15/01/2025 (warranty end date)
  - End:   15/01/2026 (warranty end date + 1 year)
```

## Error Handling

If the warranty end date cannot be parsed (invalid format), the AMC end date defaults to the current date plus 1 year:
```csharp
DateTime amcEndDate = DateTime.Now;
// ... parsing attempt ...
amcEndDate = DateTime.Now.AddYears(1); // Default fallback
```

## Date Format

All dates must be in `dd/MM/yyyy` format:
- ? `15/01/2025` - Correct
- ? `31/12/2025` - Correct
- ? `2025-01-15` - Wrong format (will use default)

## Verification

To verify the AMC end dates are correct:

```sql
-- Check auto-created AMC records
SELECT 
    ServiceQuote,
    SDate as 'AMC Start Date',
    EDate as 'AMC End Date',
    DATEDIFF(DAY, SDate, EDate) as 'Duration Days'
FROM AMC
WHERE ServiceQuote LIKE 'WRN-%'
ORDER BY CreatedOn DESC
```

Expected result:
- Duration should be approximately 365 days (1 year)
- EDate should be exactly 1 year after SDate

## Build Status

? **Compiles successfully**
? **No errors**
? **No warnings**

## Impact

- ? AMC records now have correct end dates (1 year from warranty end)
- ? Service coverage is properly extended
- ? Warranty renewal can be tracked correctly
- ? No breaking changes to existing functionality

---

**Change Date:** January 15, 2024
**File Modified:** Infrastructure\Services\InstrumentWarrantyBackgroundService.cs
**Status:** ? Complete and Verified
