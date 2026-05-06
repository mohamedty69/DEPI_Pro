# DEPI Project – Knowledge Brief

This document is a **quick reminder** of all the major discussions, decisions, and **error fixes** we went through while building the 3‑tier ASP.NET Core MVC application.

---

## 1. Architecture & Pattern

- **3‑Tier Layers**: `DAL` (Models, DbContext), `BLL` (Services, DTOs), `PL` (Controllers, Views, ViewModels)
- **Identity Integration**: `ApplicationUser : IdentityUser` placed in `DAL/Models`, `AppDbContext : IdentityDbContext<ApplicationUser>` in `DAL/DbContext`
- **Employee ↔ ApplicationUser**: Separate one‑to‑one via `Employee.UserId` (string, nullable). **Do not inherit** `Employee` from `IdentityUser`.

---

## 2. Domain Model (Entities & Relationships)

| Relationship | Mapping | FK on | Notes |
|--------------|---------|-------|-------|
| Manage (Employee ↔ Department) | 1 : 0..1 | `Department.EmployeeId` (nullable) | Employee is principal |
| Employee self‑reference (Manager) | 1 : N | `Employee.ManagerSsn` (nullable) | `DeleteBehavior.Restrict` |
| Works_on (Employee ↔ Department) | M : N (with Hours) | Join table `EmployeeDepartment` | Composite PK, both FKs `Restrict` |
| Shift → Employee | 1 : N | `Employee.ShiftId` (nullable – see §4) | |
| ProductionLine → Employee | 1 : N | `Employee.ProductionLineId` (nullable) | |
| Employee → Schedule | 1 : N | `Schedule.EmployeeId` | |
| Mission → Employee (Authorised) | 1 : N | `Mission.AuthorizedEmployeeId` | `Restrict` |
| Mission → Employee (Goes On) | 1 : N | `Mission.GoesOnEmployeeId` | `Restrict` |
| Schedule ↔ Attendance | 1 : 0..1 | `Attendance.ScheduleId` | FK on Attendance |
| Schedule ↔ VacationRequest | 1 : N | `Schedule.VacationRequestId` (nullable) | |
| Schedule → Shift / ProductionLine / JopDescription | 1 : N | Respective IDs on Schedule | |
| VacationRequest → Employee | 1 : N | `VacationRequest.EmployeeId` | |
| SwapRequest → Employee (Request/Recipient) | 1 : N on each | `RequestingEmployeeId`, `RecipientEmployeeId` | `Restrict` on both |
| SwapRequest → Schedule | 1 : 0..1 | `SwapRequest.ScheduleId` | |

- **Composite Keys**: `EmployeeDepartment` uses `(EmployeeID, DepartmentID)`.

---

## 3. Identity & Roles

- **Roles**: `Admin`, `Manager`, `Employee` stored in `AspNetRoles`.
- **Seeding roles**: Use `RoleManager<IdentityRole>` in a static `DbInitializer.SeedRolesAsync()` called from `Program.cs` (after `app.Build()`).
- **Admin UI**: Recommended to create an **Area** called `Admin` with separate controllers for each resource, secured with `[Authorize(Roles = "Admin")]`.

---

## 4. Nullable vs. Non‑Nullable Foreign Keys

Based on real‑world workflow, the following are **nullable**:

- `Employee.ShiftId` → `int?`
- `Employee.ProductionLineId` → `int?`
- `Employee.UserId` → `string?`
- `Employee.ManagerSsn` → `int?`
- `Schedule.MissionId` → `int?`
- `Schedule.VacationRequestId` → `int?`

All others remain non‑nullable (required).

---

## 5. Data Type Decisions

- **Salary**: `decimal` (use `HasPrecision(18,4)` if needed)
- **Ssn**: `int` **without** identity (`ValueGeneratedNever()`)
- **Enums**: `MissionStatus` (`Pending, InProgress, Completed, Cancelled`), `VacationRequestStatus` (`Pending, Approved, Rejected, Cancelled`). Stored as `string` via `.HasConversion<string>()`.

---

## 6. Errors & Solutions (Quick Reference)

### ❌ Error: *“Unable to create DbContext … ‘IdentityUserLogin<string>’ requires a primary key”*

**Cause**: Forgot to call `base.OnModelCreating(modelBuilder)` in your `AppDbContext`.  
**Fix**: Add `base.OnModelCreating(modelBuilder);` as the **first line** inside your `OnModelCreating` override.

---

### ❌ Error: *Type mismatch between `UserId` (int) and `IdentityUser.Id` (string)*

**Cause**: `Employee.UserId` was `int`, but `ApplicationUser.Id` is `string` (GUID).  
**Fix**: Change `Employee.UserId` to `string?` and update the relationship configuration accordingly.

---

### ❌ Error: *“Invalid value for key 'Integrated Security'”* (connection string)

**Cause**: Incorrect connection string syntax – commas used instead of semicolons.  
**Fix**: `Server=.;Database=DEPI;Integrated Security=True;TrustServerCertificate=True`

---

### ❌ Error: *“The certificate chain was issued by an authority that is not trusted”* (SSL)

**Cause**: SQL Server’s self‑signed certificate not trusted.  
**Fix**: Add `TrustServerCertificate=True` in the connection string (development only).

---

### ❌ Error: *“To change the IDENTITY property of a column, the column needs to be dropped and recreated.”*

**Cause**: You tried to remove `IDENTITY` from `Ssn` via a migration that just altered the column.  
**Fix**: Since no data existed, drop the database, delete existing migrations, add `.ValueGeneratedNever()` to the `Ssn` configuration, then create a fresh `InitialCreate` migration.

---

### ❌ Error: *“INSERT statement conflicted with FOREIGN KEY constraint ‘FK_Employees_ProductionLines_ProductionLineId’”* (and similar for `ShiftId`)

**Cause**: Inserting an employee with a `ProductionLineId` or `ShiftId` that doesn’t exist (often `0` by default).  
**Fix**:
- **Immediate**: Either seed the parent tables first or change the FK to nullable (`int?`).
- **Design**: Decide those relationships are optional (see §4) and make them nullable.

---

### ❌ Error: *“Cycles or multiple cascade paths”* (on `EmployeeDepartment`, `Schedule-JopDescription`)

**Cause**: EF Core defaults to `Cascade` for all FKs, creating multiple delete paths to the same table.  
**Fix**: Add `.OnDelete(DeleteBehavior.Restrict)` to the offending relationships. For join tables (`EmployeeDepartment`), use `Restrict` on both FKs. For `Schedule → JopDescription`, use `Restrict` to break the two‑path cycle.

---

### ❌ Delete behavior summary

| Behaviour | Effect on delete of parent |
|-----------|----------------------------|
| `Cascade` | Child rows deleted automatically |
| `Restrict` | Delete blocked if child rows exist |
| `SetNull` | Child FK set to NULL (requires nullable FK) |

**Preferred for join tables and to avoid cycles**: `Restrict`.

---

## 7. LINQ & Data Retrieval

- Use `.Include(e => e.IdentityUser)` to eagerly load the linked `ApplicationUser` – this generates a `LEFT JOIN` because the FK is nullable.
- Result: **All** employees are returned; email shows `NULL` if no login exists.
- Manual `join` only needed for ad‑hoc conditions not covered by navigation properties.

---

## 8. Seeding Sample Data

A complete SQL seed script was provided (inserts one department, production line, shift, three employees, join record, schedules, attendance, mission, vacation request, swap request).  
Run it only after migrations are applied. If you get FK errors during seeding, check the order of inserts (parents first).

---

## 9. Migration Ssn Identity Removal

The final migration for `Ssn` MUST be generated after `.ValueGeneratedNever()` is set. Recreate the initial migration if necessary.

---

## 10. Important Configuration Snippets

### `OnModelCreating` checklist

```csharp
base.OnModelCreating(modelBuilder);   // first!

// Employee
entity.Property(e => e.Ssn).ValueGeneratedNever();
entity.HasOne(e => e.Manager).WithMany(e => e.Subordinates)
      .HasForeignKey(e => e.ManagerSsn).OnDelete(DeleteBehavior.Restrict);
entity.HasOne(e => e.IdentityUser).WithOne()
      .HasForeignKey<Employee>(e => e.UserId).IsRequired(false);

// EmployeeDepartment
entity.HasKey(ed => new { ed.EmployeeID, ed.DepartmentID });
entity.HasOne(...).WithMany(...).HasForeignKey(ed => ed.EmployeeID)
      .OnDelete(DeleteBehavior.Restrict);
entity.HasOne(...).WithMany(...).HasForeignKey(ed => ed.DepartmentID)
      .OnDelete(DeleteBehavior.Restrict);

// Mission
entity.HasOne(m => m.AuthorizedEmployee).WithMany(e => e.AuthorizedMissions)
      .HasForeignKey(m => m.AuthorizedEmployeeId).OnDelete(DeleteBehavior.Restrict);
entity.HasOne(m => m.GoesOnEmployee).WithMany(e => e.GoesOnMissions)
      .HasForeignKey(m => m.GoesOnEmployeeId).OnDelete(DeleteBehavior.Restrict);

// SwapRequest
entity.HasOne(sr => sr.RequestEmployee).WithMany(e => e.SentSwapRequests)
      .HasForeignKey(sr => sr.RequestingEmployeeId).OnDelete(DeleteBehavior.Restrict);
entity.HasOne(sr => sr.RecipientEmployee).WithMany(e => e.ReceivedSwapRequests)
      .HasForeignKey(sr => sr.RecipientEmployeeId).OnDelete(DeleteBehavior.Restrict);

// Schedule → JopDescription (to avoid cycle)
entity.HasOne(s => s.JopDescription).WithMany(j => j.Schedules)
      .HasForeignKey(s => s.JopDescriptionId).OnDelete(DeleteBehavior.Restrict);