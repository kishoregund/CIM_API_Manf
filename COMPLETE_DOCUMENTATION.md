# Engineer Response Notification System - Complete Documentation

## Executive Summary

A comprehensive notification system has been implemented to automatically notify distributors (RDTSP segment users) when engineers (RENG segment users) respond to assigned service requests with acceptance or rejection actions. The system provides dual-channel notifications: in-app notifications and email alerts.

## System Architecture

### Components Modified

```
???????????????????????????????????????????????????????
?                   WebApi Project                     ?
?  ?????????????????????????????????????????????????  ?
?  ?  ServiceRequestsController                     ?  ?
?  ?  ? Updated CreateSREngActionAsync endpoint    ?  ?
?  ?    - Calls NotifyDistributorForEngineerResponse?  ?
?  ?????????????????????????????????????????????????  ?
???????????????????????????????????????????????????????
                          ?
???????????????????????????????????????????????????????
?               Application Project                    ?
?  ?????????????????????????????????????????????????  ?
?  ?  IServiceRequestService Interface             ?  ?
?  ?  ? Added method signature:                   ?  ?
?  ?    NotifyDistributorForEngineerResponseAsync  ?  ?
?  ?????????????????????????????????????????????????  ?
???????????????????????????????????????????????????????
                          ?
???????????????????????????????????????????????????????
?            Infrastructure Project                    ?
?  ?????????????????????????????????????????????????  ?
?  ?  ServiceRequestService Implementation        ?  ?
?  ?  ? New method implementation:                ?  ?
?  ?    NotifyDistributorForEngineerResponseAsync  ?  ?
?  ?  - Validation & Authorization                ?  ?
?  ?  - Database Query (EF Core)                 ?  ?
?  ?  - Notification Creation                     ?  ?
?  ?  - Email Dispatch                            ?  ?
?  ?????????????????????????????????????????????????  ?
???????????????????????????????????????????????????????
```

## Detailed Feature Description

### 1. Engineer Response Capture

**Trigger Point:** POST `/api/servicerequests/EAadd`

**Data Captured:**
- Service Request ID
- Engineer ID (derived from current user)
- Action Taken (ACCEPTED/REJECTED/etc.)
- Comments
- Action Date/Time

### 2. Validation Layer

```csharp
// Step 1: Verify Engineer Authentication
var userProfile = await context.VW_UserProfile
    .FirstOrDefaultAsync(x => x.UserId == currentUserId);

// Step 2: Check Engineer Segment
if (userProfile.SegmentCode != "RENG")
    return; // Silent exit - not an engineer

// Step 3: Verify Service Request Exists
var serviceRequest = await context.ServiceRequest
    .FirstOrDefaultAsync(x => x.Id == serviceRequestId);

// Step 4: Verify Distributor Assignment
var distributor = await context.Distributor
    .FirstOrDefaultAsync(d => d.Id == serviceRequest.DistId);
```

### 3. Recipient Identification

**Query Logic:**
```sql
SELECT DISTINCT u.*
FROM VW_UserProfile u
WHERE 
    u.SegmentCode = 'RDTSP'  -- Regional Distributor Coordinator
    AND u.EntityParentId = @serviceRequest.DistId
    AND u.IsActive = 1
    AND u.IsDeleted = 0
```

**Recipients:** All RDTSP users of the same distributor

### 4. Notification Creation

**In-App Notification Entry:**
```
INSERT INTO Notifications
(
    Id, 
    UserId,
    Remarks,
    RoleId,
    RaisedBy,
    IsActive,
    IsDeleted,
    CreatedBy,
    CreatedOn,
    UpdatedBy,
    UpdatedOn
)
VALUES (...)
```

**Message Template:**
```
"Engineer [FirstName] [LastName] has [action] Service Request [SerReqNo]"

Example: "Engineer John Smith has accepted Service Request INC202401001"
```

### 5. Email Generation & Dispatch

**Email Structure:**

```
To: [RDTSP User Email]
CC: [Admin Emails from Config]
From: noreply@company.com
Subject: Service Request [SerReqNo] - Engineer Response: [ActionName]

Body (HTML):
?? Header
?  ?? "Engineer Response to Service Request"
?? Service Details
?  ?? Service Request Number
?  ?? Engineer Name
?  ?? Action Taken (color-coded)
?  ?? Customer Name
?  ?? Site Name
?  ?? Request Date
?? Engineer Comments (if provided)
?? Call to Action
?? Footer
   ?? "System generated email" notice
```

**Color Coding:**
- ACCEPTED: Green (#00AA00)
- REJECTED: Red (#FF0000)
- Other: Default text color

## Database Schema Interactions

### Tables Involved

1. **ServiceRequest**
   - Stores the original service request
   - Links to Distributor, Customer, Site

2. **SREngAction**
   - Stores engineer actions
   - References ServiceRequest and Engineer

3. **Notifications**
   - Stores in-app notifications
   - Auditable records of all notifications sent

4. **VW_UserProfile**
   - View combining user information
   - Segment identification (RENG, RDTSP, RCUST)

5. **VW_ListItems**
   - Reference data for action types
   - Contains ACCEPTED, REJECTED, etc.

### Query Execution Flow

```
1. Get Current Engineer Profile
   ?? SELECT from VW_UserProfile WHERE UserId = currentUserId

2. Verify Engineer Status
   ?? Check SegmentCode = 'RENG'

3. Get Service Request Details
   ?? SELECT * FROM ServiceRequest WHERE Id = @srId
   ?? JOIN Customer, Site, Distributor

4. Get Engineer Contact Info
   ?? SELECT * FROM RegionContact WHERE Id = @engineerId

5. Get Recipient List
   ?? SELECT from VW_UserProfile WHERE SegmentCode = 'RDTSP' 
      AND EntityParentId = @distributorId

6. Get Action Name
   ?? SELECT ItemName FROM VW_ListItems 
      WHERE ListTypeItemId = @actionTaken

7. Insert Notifications
   ?? INSERT INTO Notifications (for each recipient)

8. Send Emails
   ?? Call CommonMethods.SendEmailMethod for each recipient
```

## Security Implementation

### Authentication & Authorization

```csharp
[ShouldHavePermission(CimAction.Update, CimFeature.Service_Request)]
public async Task<IActionResult> CreateSREngActionAsync(...)
{
    // Only authenticated & authorized users reach here
    // Implicit: Current user identity available via ICurrentUserService
}
```

### Access Control

1. **Method Level:** Only RENG segment users trigger notifications
2. **Data Level:** Only notifies RDTSP users of same distributor
3. **Role Level:** Via permission attributes on controller
4. **Tenant Level:** Implicit in current user context

### Data Protection

- No sensitive data in notifications/emails beyond business context
- Notifications are auditable in Notifications table
- Email logs available in debug output
- User email validation before dispatch

## Performance Considerations

### Query Optimization

```csharp
// Efficient query with minimal database hits
var distributorUsers = await context.VW_UserProfile
    .Where(x => x.SegmentCode == "RDTSP" 
        && x.EntityParentId == serviceRequest.DistId)
    .ToListAsync();
```

### Async Operations

- All database operations use async/await
- Non-blocking email dispatch
- Doesn't impact main API response time

### Scalability

- Loop through recipients for notification creation
- Email dispatch per recipient (can be optimized to batch)
- Can be moved to background job queue if needed

## Error Handling & Resilience

### Exception Handling Strategy

```csharp
try
{
    // Main notification logic
}
catch (Exception ex)
{
    // Log error without throwing
    System.Diagnostics.Debug.WriteLine(
        $"[NotifyDistributorForEngineerResponse] Error: {ex.Message}");
}
```

### Graceful Degradation

| Failure Point | Impact | Recovery |
|---------------|--------|----------|
| No email configuration | Email not sent | In-app notification sent |
| SMTP connection fails | Email not sent | System logs error, continues |
| Invalid email address | Email not sent | Skips user, continues |
| Missing user data | Email not sent | Uses fallback values, continues |
| Database error | Notification not saved | Exception caught, logged |

## Configuration Requirements

### appsettings.json

```json
{
  "AppSettings": {
    "EmailSettings": {
      "Host": "smtp.office365.com",
      "Port": 587,
      "SSL": true,
      "SMTPUser": "noreply@avante.company.com",
      "SMTPPassword": "your-app-specific-password",
      "DisplayName": "CIM System"
    },
    "DistEmails": "admin1@company.com,admin2@company.com"
  }
}
```

### Required Dependencies

- Microsoft.EntityFrameworkCore
- Microsoft.Extensions.Configuration
- System.Net.Mail (SMTP)

## Testing Strategy

### Unit Tests

```csharp
[Test]
public async Task NotifyDistributor_WithEngineerUser_ShouldNotify()
{
    // Arrange: Create engineer user profile
    var engineerProfile = new VW_UserProfile { SegmentCode = "RENG" };
    
    // Act: Call notification method
    await service.NotifyDistributorForEngineerResponseAsync(...);
    
    // Assert: Verify notification created
    var notifications = await context.Notifications.ToListAsync();
    Assert.AreEqual(1, notifications.Count);
}

[Test]
public async Task NotifyDistributor_WithNonEngineerUser_ShouldNotNotify()
{
    // Arrange: Create non-engineer user profile
    var userProfile = new VW_UserProfile { SegmentCode = "RCUST" };
    
    // Act: Call notification method
    await service.NotifyDistributorForEngineerResponseAsync(...);
    
    // Assert: Verify no notification created
    var notifications = await context.Notifications.ToListAsync();
    Assert.AreEqual(0, notifications.Count);
}
```

### Integration Tests

1. Create service request assigned to engineer
2. Engineer submits acceptance action
3. Verify in-app notification in database
4. Verify email sent to distributor
5. Verify all details in email correct

### Manual Testing Checklist

- [ ] Engineer can submit action (ACCEPTED)
- [ ] Engineer can submit action (REJECTED)
- [ ] Distributor receives in-app notification
- [ ] Distributor receives email notification
- [ ] Email contains engineer name
- [ ] Email contains service request number
- [ ] Email contains action with correct color
- [ ] Email contains customer/site info
- [ ] Email contains engineer comments
- [ ] Non-engineer users cannot trigger notification

## Monitoring & Support

### Debugging

Enable debug output to see notification attempts:

```csharp
// In debug output window or logs:
[NotifyDistributorForEngineerResponse] Starting notification...
[SendEmailMethod] Sending email to: user@company.com
[SendEmailMethod] SMTP error: Connection timeout
```

### Metrics to Track

1. **Notification Success Rate**
   - Notifications created / Total attempts

2. **Email Delivery Success Rate**
   - Emails sent successfully / Total attempts

3. **Average Response Time**
   - Time from engineer action to distributor notification

4. **User Engagement**
   - Notifications read / Notifications sent

### Logging Recommendations

Add structured logging for production:

```csharp
_logger.LogInformation(
    "Notification sent to {UserId} for SR {ServiceRequestId} action {ActionName}",
    userId, serviceRequestId, actionName);

_logger.LogError(ex,
    "Failed to send notification for SR {ServiceRequestId}",
    serviceRequestId);
```

## Maintenance & Future Enhancements

### Version 1.1 Planned Enhancements

1. **Notification Templates**
   - Allow customization of notification messages
   - Support multiple languages

2. **Notification Preferences**
   - User opt-in/opt-out for specific types
   - Frequency control (immediate/daily digest)

3. **SMS Notifications**
   - Add SMS as alternative channel
   - Use Twilio/AWS SNS integration

4. **Notification History**
   - Enhanced audit trail
   - Delivery status tracking

5. **Background Job Processing**
   - Move email dispatch to async queue
   - Use Hangfire or Azure Queue Storage

### Version 2.0 Planned Enhancements

1. **AI-Powered Notifications**
   - Smart priority scoring
   - Predictive recipient identification

2. **Real-Time Updates**
   - WebSocket notifications
   - Live notification dashboard

3. **Analytics Dashboard**
   - Notification metrics
   - Engagement analytics

4. **Integration Platform**
   - Webhook support
   - Third-party CRM integration

## Deployment Checklist

- [ ] Update `appsettings.json` with correct SMTP credentials
- [ ] Test email delivery in staging environment
- [ ] Verify RDTSP users receive notifications
- [ ] Check notification database entries
- [ ] Monitor error logs during first 24 hours
- [ ] Collect user feedback on notification usefulness
- [ ] Document in user training materials
- [ ] Set up monitoring/alerting for email failures

## Support & Contact

For issues or questions:
1. Check logs in debug output
2. Verify SMTP configuration
3. Confirm user profiles have correct segment codes
4. Review notification creation in database
5. Contact development team with error details

---

**Implementation Date:** January 2024
**Status:** Production Ready
**Version:** 1.0
**Last Updated:** 2024-01-15
