## v2.2.1
- Fix: (Dashboard) reload presale operators on new user creation.

## v2.2.0
- Change: Page header margin styling
- Change: middle and lower boundary filter layout position
- Add: Stacktrace exception console logging
- Add: Monthly Root Cause tabulation report
- Add: Weekly Root Cause tabulation report
- Add: Daily Root Cause tabulation report
- Add: Root Cause tabulation exclusion filter
- Add: Monthly Import Aging tabulation report
- Add: Weekly Import Aging tabulation report
- Add: Daily Import Aging tabulation report

## v2.1.0
- Fix: Chat history link max-width element
- Update: Reload `PresaleData` on outbound `DataTime` filter range
- Add: Secondary Dashboard page (WIP for Dashboard version 2)
- Add: "Upper Boundary" Dashboard filter (monthly)
- Add: "Middle Boundary" Dashboard filter (weekly)
- Add: "Lower Boundary" Dashboard filter (daily)
- Add: Monthly, Weekly, Daily tabulation report options
- Add: Monthly Approval Status tabulation report
- Add: Weekly Approval Status tabulation report
- Add: Daily Approval Status tabulation report
- Add: "Middle Boundary" filter options to change days range report
- Add: "Lower Boundary" filter options to change day report

## v2.0.0
- Fix: Redis caching for filtered items. Improved page loading time.
- Fix: Redis caching with Sorted Set for archived Presale Data. Prevent future memory leak if Presale data reaches 100MB per 1 active filtering.
- Fix: Limit fetch filtering to ~8MB. Prevent memory leak.
- Added: Unix time as Redis Sorted Set score. Improve date filtering.

## v1.9.2
- Fix: Parallel batch operation (Redis). Improved page loading time.

## v1.9.1
- Update: aging approval includes direct approval count

## v1.9.0
- Fix: Root cause display glitch
- Change: Tracker margin (spacer) to auto
- Change: "Status Tracker" tab name to "Tracking"
- Remove: Tracking interval time display information
- Update: Match tracking detail report badge element with approval status
- Add: Direct approval feature
- Add: Database options for Direct Approval "ODS," "SARMA," "GREBEK," and "EVENT"

## v1.8.0
- Fix: Filter performance overhead. Improved page loading time.
- Fix: Page loading indicator
- Fix: Validation page aging display
- Fix: `WaktuRespons` time constraint
- Fix: Missing Session alias "Sales" and "Management"
- Fix: Accent color scheme
- Change: Progress tracking layout
- Change: Home as default Dashboard
- Update: Timeline (progress tracker)
- Add: Status Tracker page

## v1.7.1
- Fix: Approval aging calculation on user not responding
- Fix: greyed out disabled label (Root Cause) on  Approval page
- Fix: register Dashboard to tab navigation system
- Change: font to `Helvetica` FOSS alternative `Inter`
- Change: Dashboard as public page
- Change: Move "Scroll Mode" to next to item count selection
- Update: color scheme (PLN corporate color)
- Update: Add DataGrid loading indicators
- Add: Reopen Presale data
- Add: Reopen indicator icon on Verification page
- Add: User role `Sales`
- Add: User role `Management`

## v1.6.1
- Add: Dashboard system
- Add: Aging calculation system (with Frozen Interval)
- Add: Metric Status Approval per Kantor Perwakilan (Monthly, Weekly, Daily)
- Add: Metric Root Cause
- Add: Metric Aging Import (Monthly)
- Add: Metric Aging Import Verification (Monthly)
- Add: Metric Aging Chat/Call Mulai (Monthly)
- Add: Metric Aging Chat/Call Validasi (Monthly)
- Add: Metric Aging Approval (Monthly)