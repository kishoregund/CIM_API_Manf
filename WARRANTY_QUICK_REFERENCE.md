# Warranty Expiration & AMC Auto-Creation - Quick Reference

## ? What's New

A new **automated warranty monitoring system** that:
- Runs daily at **3:00 AM**
- Notifies distributors **60 days** before warranty expires
- **Auto-creates AMC records** for instruments with expiring warranties
- Sends **professional email alerts**

---

## ?? How It Works (Simple)

```
Every Day at 3 AM:
  1. System checks all instrument warranties
  2. Finds those expiring in next 60 days
  3. For each expiring warranty:
     ? Creates new AMC record automatically
     ? Notifies distributor via email & in-app
     ? Links instrument to new AMC
  4. Logs all actions
```

---

## ?? What Gets Created Automatically

### AMC Record
- Service Quote: `WRN-{InstrumentID}`
- Dates: From instrument warranty dates
- Status: Active & ready to use
- Linked to: Instrument & Site

### Notifications
- In-app: Notification table
- Email: HTML formatted alert
- Recipients: RDTSP users (Distributors)
- Frequency: Once per day per warranty

### Email Includes
- Warranty dates
- Customer & site info
- Instrument details
- Days remaining
- Auto-creation confirmation
- Next actions

---

## ?? Configuration

### Default Timing
| Setting | Default | Change Location |
|---------|---------|-----------------|
| Check Time | 3:00 AM | `_scheduledTime` |
| Frequency | Daily (24h) | `_checkInterval` |
| Threshold | 60 days | `_daysBeforeExpiration` |

### Example: Change to 4:00 AM
```csharp
// InstrumentWarrantyBackgroundService.cs, line 43
private readonly TimeOnly _scheduledTime = new TimeOnly(4, 0, 0);
```

---

## ?? Database Tables Affected

| Table | Action | Purpose |
|-------|--------|---------|
| CustomerInstrument | READ | Get warranty data |
| AMC | CREATE | Auto-create AMC records |
| AMCInstrument | CREATE | Link instrument to AMC |
| Notifications | CREATE | In-app notifications |

---

## ?? Verify It's Working

### Check Logs at 3 AM
```
Debug Output ? Search: [InstrumentWarrantyBackgroundService]
```

### Query for Auto-Created AMCs
```sql
SELECT * FROM AMC 
WHERE ServiceQuote LIKE 'WRN-%' 
AND CreatedOn >= CAST(GETDATE() AS DATE)
```

### Check Notifications
```sql
SELECT * FROM Notifications 
WHERE Remarks LIKE '%warranty%'
AND CreatedOn >= CAST(GETDATE() AS DATE)
```

---

## ?? Warranty Date Format

? **CORRECT:** `dd/MM/yyyy` (e.g., `31/12/2025`)
? **WRONG:** `yyyy-MM-dd` or others

If dates are wrong format, warranty check will fail silently.

---

## ?? Quick Test

1. Create CustomerInstrument:
   - Warranty: **true**
   - WrntyEnDt: **Today + 45 days** (format: dd/MM/yyyy)
   - IsActive: **true**
   - IsDeleted: **false**

2. Wait until 3:00 AM (or modify code to test sooner)

3. Verify:
   - Check AMC table for `WRN-{ID}` records
   - Check Notifications table for new entries
   - Check inbox for emails

---

## ?? Production Deployment

1. Pull latest code
2. Build: `dotnet build`
3. Deploy normally
4. Restart application
5. Service auto-registers and runs daily at 3 AM

**No configuration needed!**

---

## ?? Troubleshooting

| Problem | Solution |
|---------|----------|
| No AMCs created | Check warranty dates are dd/MM/yyyy format |
| No notifications | Verify RDTSP users exist for distributor |
| No emails sent | Check SMTP configuration |
| Duplicate AMCs | Check site doesn't have existing AMC |
| Duplicate emails | Notifications checked for today's date |

---

## ?? Support Files

- **Full Documentation:** `INSTRUMENT_WARRANTY_EXPIRATION_SYSTEM.md`
- **Code File:** `Infrastructure\Services\InstrumentWarrantyBackgroundService.cs`
- **Logs:** Check debug output at 3 AM

---

## ? Summary

New feature runs automatically:
- ? Checks warranty dates daily
- ? Creates AMC 60 days before expiration
- ? Notifies distributors automatically
- ? Sends professional emails
- ? Creates in-app notifications
- ? Zero manual work

**Status: Production Ready ?**

---

**Version:** 1.0
**Build:** ? Successful
**Deployment:** Ready
