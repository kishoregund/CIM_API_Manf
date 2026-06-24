# Engineer Response Notification - API Usage Guide

## Overview
This guide demonstrates how to use the new engineer response notification system in the CIM API.

## API Endpoint

### Create Engineer Action (with Notification)
**Endpoint:** `POST /api/servicerequests/EAadd`

**Permission:** 
- `CimAction.Update`
- `CimFeature.Service_Request`

**Request Body:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "companyId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "isActive": true,
  "isDeleted": false,
  "createdBy": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "createdOn": "2024-01-15T10:30:00Z",
  "updatedBy": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "updatedOn": "2024-01-15T10:30:00Z",
  "engineerId": "5fab1234-5717-4562-b3fc-2c963f66afa6",
  "actiontaken": "9f1c6d2a-4567-8901-b234-567890abcdef",
  "comments": "Service request received. Will visit site on 15th January at 2 PM.",
  "teamviewRecording": "teamview_session_id_123",
  "actionDate": "2024-01-15T10:30:00Z",
  "serviceRequestId": "7ab2cdef-1234-5678-9abc-def012345678"
}
```

**Response (Success):**
```json
{
  "isSuccessful": true,
  "statusCode": 200,
  "message": "SREngAction added successfully.",
  "data": "7ab2cdef-1234-5678-9abc-def012345678"
}
```

**Response (Failure):**
```json
{
  "isSuccessful": false,
  "statusCode": 400,
  "message": "Failed to add SREngAction.",
  "data": null
}
```

## Request Parameters Explained

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| engineerId | Guid | Yes | ID of the engineer taking the action |
| actiontaken | String (Guid) | Yes | ID of the action taken (from VW_ListItems, e.g., "ACCEPTED" or "REJECTED") |
| comments | String | No | Comments from the engineer about their decision |
| serviceRequestId | Guid | Yes | ID of the service request being responded to |
| actionDate | DateTime | No | Date and time when action was taken |
| teamviewRecording | String | No | TeamViewer session ID if applicable |

## Action Type Reference

Common action types (from VW_ListItems):
- **ACCEPTED** - Engineer accepts the service request
- **REJECTED** - Engineer rejects the service request
- **PENDING_INFO** - Engineer needs more information
- **RESCHEDULED** - Engineer reschedules the service request

## Notification Flow

### What Gets Sent

**In-App Notification (Database):**
```
Message: "Engineer [FirstName] [LastName] has [action] Service Request [SerReqNo]"
Target Users: All RDTSP users of the engineer's distributor
Status: Active and undeleted
```

**Email Notification:**
Recipient: All RDTSP users of the engineer's distributor

Email Content:
```
Subject: Service Request [SerReqNo] - Engineer Response: [ActionName]

Body:
- Service Request Number: INC202401001
- Engineer Name: John Smith
- Action Taken: ACCEPTED (in green)
- Customer: ABC Manufacturing Ltd
- Site: Site-Mumbai-01
- Request Date: 15/01/2024
- Comments: Service request received. Will visit site on 15th January at 2 PM.
- Action Required: Please review the engineer's response in the CIM dashboard.
```

## Example cURL Request

```bash
curl -X POST "https://api.example.com/api/servicerequests/EAadd" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '{
    "engineerId": "5fab1234-5717-4562-b3fc-2c963f66afa6",
    "actiontaken": "9f1c6d2a-4567-8901-b234-567890abcdef",
    "comments": "Service request received. Will visit site on 15th January at 2 PM.",
    "serviceRequestId": "7ab2cdef-1234-5678-9abc-def012345678",
    "actionDate": "2024-01-15T10:30:00Z"
  }'
```

## Example JavaScript/TypeScript Request

```typescript
// Using Fetch API
async function createEngineerAction() {
  const payload = {
    engineerId: "5fab1234-5717-4562-b3fc-2c963f66afa6",
    actiontaken: "9f1c6d2a-4567-8901-b234-567890abcdef", // ACCEPTED action ID
    comments: "Service request received. Will visit site on 15th January at 2 PM.",
    serviceRequestId: "7ab2cdef-1234-5678-9abc-def012345678",
    actionDate: new Date().toISOString()
  };

  try {
    const response = await fetch('/api/servicerequests/EAadd', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${jwtToken}`
      },
      body: JSON.stringify(payload)
    });

    const result = await response.json();
    
    if (result.isSuccessful) {
      console.log('Action created successfully:', result.data);
      console.log('Distributor has been notified via email and in-app notification');
    } else {
      console.error('Failed to create action:', result.message);
    }
  } catch (error) {
    console.error('Request failed:', error);
  }
}
```

## Business Logic Details

### Who Gets Notified?

**Notification Recipients:**
1. All users with RDTSP segment code (Distributor Regional Coordinators)
2. Who are associated with the same distributor as the service request
3. In the same company/tenant context

### When Does Notification Trigger?

1. **Automatically** when engineer action is created via API
2. **Status:** Only triggers if SREngAction creation is successful
3. **Async:** Notification is sent asynchronously after SR action record is created

### What if Notification Fails?

- API still returns success (200)
- Error logged in debug output
- Service request action is still recorded in database
- Failed email doesn't block other notifications

## Email Configuration Requirements

The system uses SMTP settings configured in `appsettings.json`:

```json
{
  "AppSettings": {
    "EmailSettings": {
      "Host": "smtp.office365.com",
      "Port": "587",
      "SSL": true,
      "SMTPUser": "noreply@company.com",
      "SMTPPassword": "your-app-password",
      "DisplayName": "CIM - Avante"
    },
    "DistEmails": "supervisor1@company.com,supervisor2@company.com"
  }
}
```

## Error Scenarios

### Scenario 1: Non-Engineer User Tries to Create Action

**What Happens:**
- API returns success (200)
- Action is recorded in database
- Notification is NOT sent (engineer validation fails silently)
- Debug message logged: "User is not an engineer (RENG)"

### Scenario 2: Invalid Service Request ID

**What Happens:**
- API returns success (200)
- Action is recorded in database
- Notification is NOT sent (service request not found)
- Debug message logged: "Service request not found"

### Scenario 3: No Distributor Users to Notify

**What Happens:**
- API returns success (200)
- Action is recorded in database
- Notifications created for each user (if any)
- No emails sent (if no users found)
- System completes without error

### Scenario 4: SMTP Server Unreachable

**What Happens:**
- API returns success (200)
- Action is recorded in database
- In-app notifications created
- Email fails silently
- Debug message logged: "SMTP error: [details]"

## Monitoring & Debugging

### Check Notification Creation

Query the database:
```sql
SELECT * FROM Notifications 
WHERE Remarks LIKE '%Engineer%' 
AND CreatedOn >= CAST(GETDATE() AS DATE)
ORDER BY CreatedOn DESC;
```

### View Email Logs

Application debug output logs email attempts:
```
[NotifyDistributorForEngineerResponse] Sending email to: user@company.com
[SendEmailMethod] SMTP error: Connection timeout | Inner: Failed to connect to server
```

## Best Practices

1. **Always Include Comments:** Provide context for the engineer's decision
2. **Set Correct Action Type:** Use appropriate action IDs from VW_ListItems
3. **Include Action Date:** Set accurate action date for audit trail
4. **Test Before Production:** Verify email configuration before going live
5. **Monitor Email Delivery:** Check that distributors are receiving emails
6. **Handle Timezone:** Ensure all DateTime values are in UTC or properly offset

## Troubleshooting

| Issue | Cause | Solution |
|-------|-------|----------|
| Notifications not sent | Engineer segment check fails | Verify user profile has "RENG" segment code |
| No email received | SMTP not configured | Update appsettings.json with correct email settings |
| Wrong recipients notified | Multiple distributors data | Verify ServiceRequest.DistId is correct |
| Notification sent multiple times | Retry logic in queue | Check if request was retried by client |
| Email formatting broken | HTML rendering issue | Test email in different clients |

## Future API Enhancements

1. Add endpoint to resend notification manually
2. Add ability to customize notification templates
3. Add query endpoint to get notification status
4. Add bulk action creation endpoint
5. Add notification preference settings per user
