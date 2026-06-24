# AMC Expiration Notification System - Complete Documentation

## Overview

A comprehensive system has been implemented to automatically notify distributors when Annual Maintenance Contracts (AMCs) are expiring within 60 days. The system provides dual-channel notifications: in-app notifications and email alerts.

## Features

? **Automatic Notifications** - Notifies distributors 60 days before AMC end date
? **Dual Channel** - In-app notifications + Email alerts
? **Smart Recipients** - Only RDTSP (Regional Distributor) users are notified
? **Detailed Alerts** - Includes AMC details, days remaining, and action items
? **Color-Coded Alerts** - Visual priority (Warning yellow for 31-60 days, Red for ?30 days)
? **Error Handling** - Graceful degradation with detailed logging
? **Configurable** - Adjustable expiration threshold (default 60 days)

## Architecture

### Components

#### 1. **IAmcExpirationNotificationService** (Application Layer)
Location: `Application\Features\AMCS\IAmcExpirationNotificationService.cs`

Interface defining the contract for AMC expiration notifications:
```csharp
public interface IAmcExpirationNotificationService
{
    Task<bool> NotifyDistributorForExpiringAmcAsync();
    Task<List<AMC>> GetExpiringAmcsAsync(int daysBeforeExpiry = 60);
    Task<List<VW_UserProfile>> GetDistributorContactsAsync(Guid distributorId);
}
```

#### 2. **AmcExpirationNotificationService** (Infrastructure Layer)
Location: `Infrastructure\Services\AmcExpirationNotificationService.cs`

Implementation of the notification service with:
- `NotifyDistributorForExpiringAmcAsync()` - Main trigger method
- `GetExpiringAmcsAsync()` - Fetches AMCs within threshold
- `GetDistributorContactsAsync()` - Gets recipient list
- `NotifyForAmcAsync()` - Private method for individual AMC notification
- `SendAmcExpirationEmailAsync()` - Email dispatch

#### 3. **AMCController** Endpoints
Location: `WebApi\Controllers\AMCController.cs`

Two new API endpoints:
- **POST** `/api/amc/notify-expiring` - Triggers notifications
- **GET** `/api/amc/expiring/{daysBeforeExpiry}` - Lists expiring AMCs

## How It Works

### Flow Diagram

```
???????????????????????????????????????
? Trigger: POST /api/amc/notify-expiring
???????????????????????????????????????
                 ?
    ??????????????????????????????
    ? NotifyDistributorForExpiringAmcAsync()
    ??????????????????????????????
                 ?
    ??????????????????????????????
    ? GetExpiringAmcsAsync(60)
    ? - Get all active AMCs
    ? - Parse EDate (dd/MM/yyyy)
    ? - Check if within 60 days
    ??????????????????????????????
                 ?
    ??????????????????????????????
    ? For each expiring AMC:
    ? - Get Site & Customer details
    ? - Get Distributor
    ??????????????????????????????
                 ?
    ??????????????????????????????
    ? GetDistributorContactsAsync()
    ? - Fetch all RDTSP users
    ? - for the distributor
    ??????????????????????????????
                 ?
    ??????????????????????????????
    ? For each contact:
    ? - Create in-app notification
    ? - Send email notification
    ??????????????????????????????
```

### Date Calculation Logic

```
Today: 2024-01-15
AMC End Date: 2024-03-15 (format: dd/MM/yyyy)

Days Remaining = 59 days

Notification Trigger Condition:
- AMC End Date >= Today (NOT expired)
- AMC End Date <= (Today + 60 days)
- Result: NOTIFY (within 60-day window)
```

## API Endpoints

### 1. Trigger Expiration Notifications

**Endpoint:** `POST /api/amc/notify-expiring`

**Permission:** `CimAction.View` + `CimFeature.AMC`

**Request:** No body required

**Response - Success:**
```json
{
    "success": true,
    "message": "AMC expiration notifications sent successfully"
}
```

**Response - Error:**
```json
{
    "success": false,
    "message": "Failed to send AMC expiration notifications"
}
```

**Example cURL:**
```bash
curl -X POST "https://api.example.com/api/amc/notify-expiring" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -H "Content-Type: application/json"
```

### 2. Get Expiring AMCs

**Endpoint:** `GET /api/amc/expiring/{daysBeforeExpiry}`

**Permission:** `CimAction.View` + `CimFeature.AMC`

**Parameters:**
- `daysBeforeExpiry` (optional, default: 60) - Threshold in days

**Response - Success:**
```json
{
    "success": true,
    "count": 3,
    "amcs": [
        {
            "id": "guid",
            "serviceQuote": "SQ-2023-001",
            "sDate": "15/01/2023",
            "eDate": "15/03/2024",
            "serviceType": "guid",
            "custSite": "guid",
            "billTo": "guid",
            ...
        }
    ]
}
```

**Example cURL:**
```bash
curl -X GET "https://api.example.com/api/amc/expiring/60" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## Notification Templates

### In-App Notification

```
Message: "AMC SQ-2023-001 for ABC Manufacturing at Site-Mumbai-01 expires in 45 days"

Properties:
- Remarks: Notification message
- RoleId: Distributor's role
- UserId: Recipient user ID
- IsActive: true
- CreatedOn: Current timestamp
```

### Email Notification

**Subject:** `AMC Expiration Alert - SQ-2023-001 expires in 45 days`

**Content:**
- AMC Header with warning icon
- Service Quote Number
- Customer Name
- Site Name
- AMC Period (SDate - EDate)
- Days Remaining (color-coded)
- Service Type
- Recommended Actions section
- Dashboard call-to-action
- System-generated footer

**Color Coding:**
- ?30 days: Red (#dc3545)
- 31-60 days: Warning Yellow (#ffc107)

## Database Interactions

### Queries Executed

1. **Fetch Active AMCs**
```sql
SELECT * FROM AMC WHERE IsActive = 1 AND IsDeleted = 0
```

2. **Get Site Details**
```sql
SELECT * FROM Site WHERE Id = @custSiteId
```

3. **Get Customer Details**
```sql
SELECT * FROM Customer WHERE Id = @customerId
```

4. **Get Distributor Contacts**
```sql
SELECT * FROM VW_UserProfile 
WHERE SegmentCode = 'RDTSP' 
AND EntityParentId = @distributorId
```

5. **Create Notification**
```sql
INSERT INTO Notifications 
(Id, UserId, Remarks, RoleId, IsActive, IsDeleted, CreatedBy, CreatedOn, ...)
VALUES (...)
```

## Configuration

### appsettings.json Requirements

```json
{
  "AppSettings": {
    "EmailSettings": {
      "Host": "smtp.office365.com",
      "Port": "587",
      "SSL": true,
      "SMTPUser": "noreply@company.com",
      "SMTPPassword": "app-specific-password",
      "DisplayName": "CIM System"
    },
    "DistEmails": "admin@company.com"
  }
}
```

## Dependency Injection

The service is registered in `Infrastructure\ServiceCollectionExtensions.cs`:

```csharp
.AddScoped<IAmcExpirationNotificationService, AmcExpirationNotificationService>()
```

## Error Handling

### Exception Handling Strategy

| Error Scenario | Handling |
|---|---|
| No expiring AMCs | Returns empty list, no notifications |
| Invalid date format | Skips that AMC, logs error |
| Missing site data | Skips notification for that AMC |
| SMTP connection fails | Notification created, email fails silently |
| Email address missing | Skips email, notification still created |
| Database error | Exception logged, returns false |

### Debug Output

Errors are logged to Visual Studio Debug output:
```
[AmcExpirationNotificationService] Error: Connection timeout
[GetExpiringAmcsAsync] Error: Invalid date format
[SendAmcExpirationEmailAsync] Error: SMTP error: Unauthorized
```

## Scheduling (Recommended)

For production, schedule the notification trigger:

### Option 1: Hangfire (Recommended)
```csharp
RecurringJob.AddOrUpdate(
    "amc-expiration-notifications",
    () => amcService.NotifyDistributorForExpiringAmcAsync(),
    Cron.Daily(2, 0)); // Daily at 2 AM
```

### Option 2: Azure Functions
```csharp
[FunctionName("AmcExpirationNotification")]
public static async Task Run(
    [TimerTrigger("0 2 * * *")] TimerInfo myTimer) // Daily at 2 AM
{
    await amcService.NotifyDistributorForExpiringAmcAsync();
}
```

### Option 3: Windows Task Scheduler
- Create a script that calls the API endpoint
- Schedule via Task Scheduler (Windows Server)

## Testing

### Unit Test Example

```csharp
[Test]
public async Task NotifyDistributorForExpiringAmcAsync_WithExpiringAmc_ShouldNotify()
{
    // Arrange
    var expiringAmc = new AMC 
    { 
        EDate = DateTime.Now.AddDays(30).ToString("dd/MM/yyyy"),
        ServiceQuote = "SQ-TEST-001",
        IsActive = true
    };
    
    // Act
    var result = await _service.NotifyDistributorForExpiringAmcAsync();
    
    // Assert
    Assert.IsTrue(result);
    // Verify notification in database
}
```

### Manual Testing Checklist

- [ ] Create test AMC with EDate 60 days from today
- [ ] Call POST `/api/amc/notify-expiring`
- [ ] Verify in-app notification created in Notifications table
- [ ] Check email received by distributor user
- [ ] Verify email contains correct details
- [ ] Test with EDate 30 days away (red color)
- [ ] Test with already-expired AMC (should not notify)
- [ ] Test with inactive AMC (should not notify)

## Performance Considerations

| Operation | Impact |
|---|---|
| Fetch all active AMCs | Moderate (indexed on IsActive, IsDeleted) |
| Date parsing | Low (done in-memory per AMC) |
| Fetch site/customer data | Moderate (1 query per AMC) |
| Fetch distributor contacts | Low (filtered by EntityParentId) |
| Email sending | High (60+ second timeout per email) |
| Database inserts | Moderate (1 per notification) |

**Optimization Tips:**
- Use batch email sending if >100 recipients
- Cache distributor contacts for 1 hour
- Implement email queue for async sending
- Run notifications during off-peak hours

## Security

? **Authentication:** Requires valid JWT token
? **Authorization:** Checks `CimAction.View` + `CimFeature.AMC`
? **Data Validation:** Parses dates with try-catch
? **SQL Injection:** Uses EF Core parameterized queries
? **Email Validation:** Checks for null/empty email before sending
? **Error Messages:** Minimal info in responses, detailed in logs

## Troubleshooting

| Issue | Solution |
|---|---|
| No notifications sent | Check if any AMCs match criteria (within 60 days, active, not deleted) |
| Email not received | Verify SMTP configuration, check email address in DB |
| Wrong recipients | Verify user profile segment code is "RDTSP" |
| Duplicate notifications | Check if endpoint called multiple times |
| Date parsing errors | Ensure EDate format is "dd/MM/yyyy" |

## Monitoring

### Key Metrics to Track

1. **Notification Success Rate**
   - Total triggered / Total successful

2. **Email Delivery Rate**
   - Emails sent / Emails failed

3. **Response Time**
   - Time from trigger to completion

4. **Database Performance**
   - Query execution time
   - Notification table growth

### Query for Monitoring

```sql
-- Count notifications sent today
SELECT COUNT(*) FROM Notifications 
WHERE CreatedOn >= CAST(GETDATE() AS DATE)
AND Remarks LIKE '%AMC%expires%'

-- Check for failed notifications (NULL emails)
SELECT * FROM VW_UserProfile 
WHERE SegmentCode = 'RDTSP' 
AND Email IS NULL
```

## Files Modified/Created

| File | Type | Purpose |
|---|---|---|
| `Application\Features\AMCS\IAmcExpirationNotificationService.cs` | New | Interface definition |
| `Infrastructure\Services\AmcExpirationNotificationService.cs` | New | Implementation |
| `WebApi\Controllers\AMCController.cs` | Modified | Added 2 endpoints |
| `Infrastructure\ServiceCollectionExtensions.cs` | Modified | Added DI registration |

## Version Information

- **Version:** 1.0
- **Status:** Production Ready
- **Framework:** .NET 8
- **C# Version:** 12.0
- **Release Date:** 2024-01-15

## Maintenance

### Regular Tasks

- [ ] Monitor notification logs daily
- [ ] Check SMTP error rates
- [ ] Verify email deliverability
- [ ] Update threshold if needed
- [ ] Test with new AMC records

### Future Enhancements

- [ ] Add SMS notifications
- [ ] Implement WhatsApp alerts
- [ ] Create dashboard widget
- [ ] Add notification preferences per user
- [ ] Implement notification history/archive
- [ ] Add 30-day and 7-day alerts
- [ ] Create custom threshold profiles

---

**For Support:** Refer to the troubleshooting section or check debug logs for `[AmcExpirationNotificationService]` entries.
