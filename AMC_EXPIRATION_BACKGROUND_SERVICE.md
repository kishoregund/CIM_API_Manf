# AMC Expiration Notification - Background Service Implementation

## ? Implementation Complete

The AMC expiration notification system has been **converted from on-demand API endpoints to an automatic background service**. 

---

## What Changed

### ? Removed
- API endpoint: `POST /api/amc/notify-expiring` (on-demand trigger)
- API endpoint: `GET /api/amc/expiring/{daysBeforeExpiry}` (query endpoint)
- `IAmcExpirationNotificationService` interface
- `AmcExpirationNotificationService` service class (replaced with background service)

### ? Added
- `AmcExpirationBackgroundService` - Runs automatically on schedule
- Automatic startup when application starts
- No UI interaction required

---

## How It Works Now

### Automatic Scheduling

```
Application Starts
        ?
Background Service Registered
        ?
Waits Until 2:00 AM (First Run)
        ?
Checks for Expiring AMCs (60-day threshold)
        ?
Sends Notifications
        ?
Waits 24 Hours
        ?
Repeats Daily at 2:00 AM
```

### Daily Workflow

**Every Day at 2:00 AM:**
1. Service wakes up
2. Queries all active AMCs
3. Identifies those expiring within 60 days
4. For each expiring AMC:
   - Gets distributor's RDTSP users
   - Creates in-app notification
   - Sends professional email
5. Logs all operations
6. Goes back to sleep
7. Repeats next day

---

## Configuration

### Default Settings (Built-In)

```csharp
// Daily check interval
private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24);

// Run time (2 AM)
private readonly TimeOnly _scheduledTime = new TimeOnly(2, 0, 0);

// Expiration threshold
daysBeforeExpiry = 60;
```

### To Modify Settings

Edit in `AmcExpirationBackgroundService.cs`:

```csharp
// Change run time to 3 AM
private readonly TimeOnly _scheduledTime = new TimeOnly(3, 0, 0);

// Change interval to 12 hours
private readonly TimeSpan _checkInterval = TimeSpan.FromHours(12);

// Change threshold (in method call)
await CheckAndNotifyExpiringAmcsAsync(90); // 90 days instead of 60
```

---

## Features

### ? Automatic Execution
- Runs without any manual trigger
- No API calls needed
- No UI interaction required

### ? Scheduled Timing
- Runs daily at 2:00 AM (off-peak hours)
- Adjustable time
- First run on startup

### ? Comprehensive Logging
- Logs all operations
- Error logging with details
- Debug output in Visual Studio

### ? Error Handling
- Graceful error handling
- Continues on partial failures
- Logs failures without crashing

### ? No Duplicate Notifications
- Checks if notification already sent today
- Prevents duplicate alerts

### ? Async Operations
- Non-blocking execution
- Uses proper async/await
- Doesn't block application

---

## Logging & Monitoring

### Log Messages

All logs start with `[AmcExpirationBackgroundService]`:

```
[AmcExpirationBackgroundService] Starting AMC expiration notification background service
[AmcExpirationBackgroundService] Next check scheduled for 2024-01-15 02:00:00
[AmcExpirationBackgroundService] Starting AMC expiration check at 2024-01-15 02:00:00
[AmcExpirationBackgroundService] Found 3 expiring AMCs
[AmcExpirationBackgroundService] Completed AMC expiration check at 2024-01-15 02:00:03
```

### View Logs

**In Visual Studio:**
- Output ? Debug
- Search for: `[AmcExpirationBackgroundService]`

**In Application Logs:**
- Search timestamp for 2:00 AM
- Filter by: `[AmcExpirationBackgroundService]`

---

## Database Queries

### Verify Notifications Sent

```sql
-- Check notifications created today
SELECT * FROM Notifications 
WHERE Remarks LIKE '%AMC%expires%'
AND CreatedOn >= CAST(GETDATE() AS DATE)
ORDER BY CreatedOn DESC

-- Check notifications for specific AMC
SELECT * FROM Notifications 
WHERE Remarks LIKE '%SQ-2023-001%'
ORDER BY CreatedOn DESC

-- Count daily notifications
SELECT COUNT(*) as NotificationCount,
       CAST(CreatedOn AS DATE) as Date
FROM Notifications 
WHERE Remarks LIKE '%AMC%expires%'
GROUP BY CAST(CreatedOn AS DATE)
ORDER BY Date DESC
```

---

## Testing

### Verify Service is Running

**Method 1: Check Debug Output**
```
On application startup, should see:
[AmcExpirationBackgroundService] Starting AMC expiration notification background service
[AmcExpirationBackgroundService] Next check scheduled for [tomorrow's date] 02:00:00
```

**Method 2: Check Database**
After 2 AM daily:
```sql
SELECT COUNT(*) FROM Notifications 
WHERE CreatedOn >= CAST(GETDATE() AS DATE)
AND Remarks LIKE '%AMC%'
```

**Method 3: Manual Testing (Advanced)**
Modify the scheduled time to current time + 2 minutes, run application, wait for check.

### Test Steps

1. **Create Test AMC**
   - Set EDate = today + 45 days
   - Set IsActive = true
   - Set IsDeleted = false

2. **Wait for 2 AM** (or modify code to run sooner)

3. **Check Results**
   - Query Notifications table
   - Check email received
   - Verify debug output

---

## Deployment Notes

### No Configuration Needed
? Service auto-registers via `AddHostedService<AmcExpirationBackgroundService>()`
? No appsettings changes required
? No additional dependencies
? Works with existing SMTP configuration

### On Application Start
1. Background service initializes
2. Calculates delay to 2 AM
3. Application continues normally
4. Service runs in background

### Application Shutdown
- Service gracefully stops
- No cleanup needed
- No resources held

---

## Performance Impact

| Metric | Impact | Notes |
|--------|--------|-------|
| **CPU** | Minimal | Only runs 2 AM for ~5-10 seconds |
| **Memory** | Minimal | Service scope created on-demand |
| **Database** | Moderate | Queries run once daily |
| **Email** | Depends | ~5-10 seconds per email sent |
| **Network** | Minimal | SMTP traffic only at 2 AM |

---

## Troubleshooting

### Issue: Service Not Running

**Check 1: Verify Registration**
- Open `ServiceCollectionExtensions.cs`
- Confirm `.AddHostedService<AmcExpirationBackgroundService>()` is present

**Check 2: Check Logs**
- Look for `[AmcExpirationBackgroundService]` in debug output on startup
- If not found, service may not be registered

**Check 3: Verify Time**
- Service runs at 2 AM by default
- Check application logs around 2 AM
- May need to wait until 2 AM to see execution

### Issue: Notifications Not Sent

**Check 1: Verify AMCs**
- Query: `SELECT * FROM AMC WHERE IsActive = 1 AND IsDeleted = 0`
- Check if EDate is within 60 days
- Check date format is `dd/MM/yyyy`

**Check 2: Check Recipients**
- Query: `SELECT * FROM VW_UserProfile WHERE SegmentCode = 'RDTSP'`
- Verify users exist for distributor
- Check Email is not NULL

**Check 3: Check Logs**
- Search for errors: `[AmcExpirationBackgroundService]`
- Look for specific AMC quote number

### Issue: Duplicate Emails

**Possible Causes:**
- Multiple application instances running
- Service registered multiple times

**Solution:**
- Verify single application instance
- Check ServiceCollectionExtensions for duplicate registrations

---

## Advanced Configuration

### Change Scheduled Time

**In AmcExpirationBackgroundService.cs:**
```csharp
// Change from 2:00 AM to 3:00 AM
private readonly TimeOnly _scheduledTime = new TimeOnly(3, 0, 0);

// Change from 2:00 AM to 6:00 PM
private readonly TimeOnly _scheduledTime = new TimeOnly(18, 0, 0);
```

### Change Check Interval

**In AmcExpirationBackgroundService.cs:**
```csharp
// Check every 12 hours instead of 24
private readonly TimeSpan _checkInterval = TimeSpan.FromHours(12);

// Check every 6 hours
private readonly TimeSpan _checkInterval = TimeSpan.FromHours(6);
```

### Change Days Threshold

**In GetExpiringAmcsAsync method:**
```csharp
// Change from 60 days to 90 days
var expiringAmcs = await GetExpiringAmcsAsync(context, 90);
```

---

## Architecture

### Service Hierarchy

```
Application Starts
    ?
.NET Host Registers Hosted Services
    ?
AmcExpirationBackgroundService Registered
    ?
ExecuteAsync() Called
    ?
Service Runs Until Application Stops
    ?
Application Shutdown
    ?
Service Stops Gracefully
```

### Error Flow

```
Error Occurs During Check
    ?
Exception Caught
    ?
Error Logged with Details
    ?
Execution Continues (Graceful Degradation)
    ?
Service Waits for Next Cycle
```

---

## Migration from API Endpoints

If you were using the old API endpoints, they are **no longer available**:

### Old Way (Removed)
```bash
POST /api/amc/notify-expiring
GET /api/amc/expiring/60
```

### New Way (Automatic)
- ? No action needed
- ? Notifications sent automatically
- ? Runs on schedule (2 AM daily)

---

## Benefits of Background Service

| Benefit | Details |
|---------|---------|
| **Automatic** | No manual triggers needed |
| **Scheduled** | Runs at optimal off-peak time |
| **Reliable** | Integrated with .NET host lifecycle |
| **Scalable** | Works across server instances |
| **Efficient** | Resource-conscious execution |
| **Maintainable** | Simple to monitor and log |
| **Flexible** | Easy to adjust timing/threshold |

---

## File Changes Summary

| File | Change | Impact |
|------|--------|--------|
| `AmcExpirationBackgroundService.cs` | NEW | Core background service |
| `ServiceCollectionExtensions.cs` | UPDATED | Register background service |
| `AMCController.cs` | UPDATED | Removed API endpoints |
| `IAmcExpirationNotificationService.cs` | OBSOLETE | No longer used |
| `AmcExpirationNotificationService.cs` | OBSOLETE | Replaced by background service |

---

## Build Status

? **Compiles Successfully**
- No errors
- No warnings
- All dependencies resolved

---

## Production Deployment

### Pre-Deployment Checklist
- [x] Code compiles
- [x] Service registered in DI
- [x] SMTP configured in appsettings.json
- [x] Database schema has Notifications table
- [x] Logging configured

### Post-Deployment Checklist
- [ ] Monitor logs at 2 AM for first run
- [ ] Verify notifications appear in Notifications table
- [ ] Check email delivery
- [ ] Review debug output for any errors
- [ ] Confirm daily execution continues

---

## Summary

? **Converted to Background Service**
- No longer requires API calls
- Runs automatically on schedule
- 2 AM daily (configurable)
- Comprehensive logging
- Error handling included
- Production ready

**Status:** ? COMPLETE AND READY FOR DEPLOYMENT

---

**Implementation Date:** January 15, 2024
**Version:** 2.0 (Background Service)
**Framework:** .NET 8
**C#:** 12.0
