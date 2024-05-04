## v3.7.0
- Update: add appsettings.json option for batch size operation
- Add: Id PLN and Email duplicate indicator
- Update: Presale Data .xlsx export

## v3.6.0
- Fix: Top level boundary range date
- Fix: Tracking responsive media query
- Fix: PIC verification handling
- Change: In-Progress chart color scheme
- Add: Error boundary component to Guest page
- Add: Refresh button (Workload)
- Add: Refresh button (Dashboard)

## v3.5.0
- Fix: Keterangan verifikasi
- Fix: "Top Level" boundary filtering
- Fix: Sanitize import model strings
- Add: "On Wait" status verifikasi
- Add: PIC columns
- Add: Keterangan verifkasi column
- Add: Keterangan to timeline tracking

## v3.4.1
- Fix: CSV (upload) date time parsing
- Update: CSV import guide

## v3.4.0
- Add: CSV import
- Add: CSV (.xlsx) import template
- Add: In-Progress dashboard tabulation

## v3.3.1
- Fix: Remove duplicate import on single copy-paste operation

## v3.3.0
- Add: .xlsx Status Approval export
- Add: .xlsx Root Cause export
- Add: .xlsx Aging Import export
- Add: .xlsx Aging Verifikasi export
- Add: .xlsx Aging Chat/Call Pick-Up export
- Add: .xlsx Aging Chat/Call Validasi export
- Add: .xlsx Aging Approval export

## v3.2.0
- Fix: Upper boundary filter range
- Fix: Approval status lower boundary pie chart reference
- Add: SLA Achievement Import
- Add: SLA Win Rate Import
- Add: SLA Achievement Chat/Call Pick-Up
- Add: SLA Win Rate Chat/Call Pick-Up
- Add: SLA Achievement Chat/Call Validasi
- Add: SLA Win Rate Chat/Call Validasi
- Add: SLA Achievement Approval
- Add: SLA Win Rate Approval
- Add: Aging Import Exclusion Filter
- Add: Aging Verification Exclusion Filter
- Add: Aging Chat/Call Pick-Up Exclusion Filter
- Add: Aging Chat/Call Validasi Exclusion Filter
- Add: Aging Approval Exclusion Filter

## v3.1.0
- Add: Presale Data .xlsx export

## v3.0.1
- Fix: Dashboard (Root Cause) page refresh on exclusion filtering
- Fix: Font weight bug
- Fix: Page margin
- Fix: Seamless Root Cause setting change detection
- Fix: Setting page tab navigation
- Change: Move "Sign Out" option into sidebar
- Change: Translate "Exclusion" to "Pilah"

## v3.0.0
- Change: Dashboard filter version 3
- Change: Dialog based boundary filtering
- Add: "Permohonan" timeline tracking
- Add: Approval Status tabulation Bar Chart
- Add: Approval Status tabulation Pie Chart
- Add: Approval Status tabulation exclusion filter
- Add: Root Cause tabulation Heatmap Chart
- Add: Root Cause tabulation Pie Chart
- Update: Dashboard page responsive width

## v2.4.0
- Add: Root Cause setting page
- Add: Add new Root Cause
- Add: Toggle enable/disable Root Cause
- Add: Direct Approval setting page
- Add: Add new Direct Approval
- Add: Toggle enable/disable Direct Approval
- Add: Chat Template setting page

## v2.3.0
- Fix: Weekly presale data filter
- Change (minor): Dashboard layout
- Add: Monthly Verikasi Aging tabulation report
- Add: Weekly Verikasi Aging tabulation report
- Add: Daily Verikasi Aging tabulation report
- Add: Monthly Chat/Call Mulai Aging tabulation report
- Add: Weekly Chat/Call Mulai Aging tabulation report
- Add: Daily Chat/Call Mulai Aging tabulation report
- Add: Monthly Chat/Call Respons Aging tabulation report
- Add: Weekly Chat/Call Respons Aging tabulation report
- Add: Daily Chat/Call Respons Aging tabulation report
- Add: Monthly Approval Aging tabulation report
- Add: Weekly Approval ApprovalAging tabulation report
- Add: Daily Approval Aging tabulation report

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