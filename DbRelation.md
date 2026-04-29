# Database Relations

This document summarizes entity relationships, keys, cardinality, and example table rows based on the current model classes in `DEPI.DAL/Models`.

## Primary Keys

- `Employee` -> `Ssn`
- `Department` -> `DepartmentId`
- `EmployeeDepartment` -> composite (`EmployeeID`, `DepartmentID`)
- `Shift` -> `ShiftId`
- `Schedule` -> `ScheduleId`
- `Attendance` -> `AttendanceId`
- `ProductionLine` -> `ProductionLineId`
- `JopDescription` -> `JopDescriptionId`
- `Mission` -> `MissionId`
- `VacationRequest` -> `VacationRequestId`
- `SwapRequest` -> `RequestId`

## Relations

### Department -> Employee (Manager)
- **FK**: `Department.EmployeeId` -> `Employee.Ssn`
- **Cardinality**: Employee (1) ↔ Department (0..1) as manager

**Sample tables**

`Employee`
| Ssn | FirstName | LastName |
|---|---|---|
| 1001 | Sara | Ali |

`Department`
| DepartmentId | Name | EmployeeId |
|---|---|---|
| 10 | Assembly | 1001 |

### Employee (Manager) -> Employee (Subordinates)
- **FK**: `Employee.ManagerSsn` -> `Employee.Ssn`
- **Cardinality**: Manager (1) → Subordinates (0..many)

**Sample table**

`Employee`
| Ssn | FirstName | ManagerSsn |
|---|---|---|
| 1001 | Sara | NULL |
| 1002 | Omar | 1001 |

### Shift -> Employee
- **FK**: `Employee.ShiftId` -> `Shift.ShiftId`
- **Cardinality**: Shift (1) → Employees (0..many)

**Sample tables**

`Shift`
| ShiftId | Name |
|---|---|
| 1 | Morning |

`Employee`
| Ssn | FirstName | ShiftId |
|---|---|---|
| 1002 | Omar | 1 |

### Employee -> Schedule
- **FK**: `Schedule.EmployeeId` -> `Employee.Ssn`
- **Cardinality**: Employee (1) → Schedules (0..many)

**Sample tables**

`Employee`
| Ssn | FirstName |
|---|---|
| 1002 | Omar |

`Schedule`
| ScheduleId | ScheduleName | EmployeeId |
|---|---|---|
| 500 | WeekA | 1002 |

### Mission -> Employee (Authorized)
- **FK**: `Mission.AuthorizedEmployeeId` -> `Employee.Ssn`
- **Cardinality**: Employee (1) → Missions Authorized (0..many)

### Mission -> Employee (Goes On)
- **FK**: `Mission.GoesOnEmployeeId` -> `Employee.Ssn`
- **Cardinality**: Employee (1) → Missions Goes On (0..many)

**Sample tables**

`Employee`
| Ssn | FirstName |
|---|---|
| 1001 | Sara |
| 1002 | Omar |

`Mission`
| MissionId | Purpose | AuthorizedEmployeeId | GoesOnEmployeeId |
|---|---|---|---|
| 200 | Audit | 1001 | 1002 |

### Mission -> Schedule
- **FK**: `Schedule.MissionId` -> `Mission.MissionId`
- **Cardinality**: Mission (1) → Schedules (0..many)

**Sample tables**

`Mission`
| MissionId | Purpose |
|---|---|
| 200 | Audit |

`Schedule`
| ScheduleId | MissionId |
|---|---|
| 500 | 200 |

### ProductionLine -> Employee
- **FK**: `Employee.ProductionLineId` -> `ProductionLine.ProductionLineId`
- **Cardinality**: ProductionLine (1) → Employees (0..many)

**Sample tables**

`ProductionLine`
| ProductionLineId | Name |
|---|---|
| 300 | Line A |

`Employee`
| Ssn | FirstName | ProductionLineId |
|---|---|---|
| 1002 | Omar | 300 |

### Department -> ProductionLine
- **FK**: `ProductionLine.DepartmentId` -> `Department.DepartmentId`
- **Cardinality**: Department (1) → ProductionLines (0..many)

**Sample tables**

`Department`
| DepartmentId | Name |
|---|---|
| 10 | Assembly |

`ProductionLine`
| ProductionLineId | Name | DepartmentId |
|---|---|---|
| 300 | Line A | 10 |

### ProductionLine -> JopDescription
- **FK**: `JopDescription.ProductionId` -> `ProductionLine.ProductionLineId`
- **Cardinality**: ProductionLine (1) → JopDescriptions (0..many)

**Sample tables**

`ProductionLine`
| ProductionLineId | Name |
|---|---|
| 300 | Line A |

`JopDescription`
| JopDescriptionId | RoleName | ProductionId |
|---|---|---|
| 400 | Operator | 300 |

### ProductionLine -> Schedule
- **FK**: `Schedule.ProductionLineId` -> `ProductionLine.ProductionLineId`
- **Cardinality**: ProductionLine (1) → Schedules (0..many)

**Sample tables**

`ProductionLine`
| ProductionLineId | Name |
|---|---|
| 300 | Line A |

`Schedule`
| ScheduleId | ProductionLineId |
|---|---|
| 500 | 300 |

### Schedule -> Shift
- **FK**: `Schedule.ShiftId` -> `Shift.ShiftId`
- **Cardinality**: Shift (1) → Schedules (0..many)

**Sample tables**

`Shift`
| ShiftId | Name |
|---|---|
| 1 | Morning |

`Schedule`
| ScheduleId | ShiftId |
|---|---|
| 500 | 1 |

### Schedule -> JopDescription
- **FK**: `Schedule.JopDescriptionId` -> `JopDescription.JopDescriptionId`
- **Cardinality**: JopDescription (1) → Schedules (0..many)

**Sample tables**

`JopDescription`
| JopDescriptionId | RoleName |
|---|---|
| 400 | Operator |

`Schedule`
| ScheduleId | JopDescriptionId |
|---|---|
| 500 | 400 |

### VacationRequest -> Employee
- **FK**: `VacationRequest.EmployeeId` -> `Employee.Ssn`
- **Cardinality**: Employee (1) → VacationRequests (0..many)

**Sample tables**

`Employee`
| Ssn | FirstName |
|---|---|
| 1002 | Omar |

`VacationRequest`
| VacationRequestId | EmployeeId |
|---|---|
| 600 | 1002 |

### VacationRequest -> Schedule
- **FK**: `Schedule.VacationRequestId` -> `VacationRequest.VacationRequestId` (nullable)
- **Cardinality**: VacationRequest (1) → Schedules (0..many)

**Sample tables**

`VacationRequest`
| VacationRequestId |
|---|
| 600 |

`Schedule`
| ScheduleId | VacationRequestId |
|---|---|
| 500 | 600 |

### Schedule -> Attendance
- **FK**: `Attendance.ScheduleId` -> `Schedule.ScheduleId`
- **Cardinality**: Schedule (1) ↔ Attendance (0..1)

**Sample tables**

`Schedule`
| ScheduleId |
|---|
| 500 |

`Attendance`
| AttendanceId | ScheduleId | TimeIn | TimeOut |
|---|---|---|---|
| 700 | 500 | 2025-01-10 08:00 | 2025-01-10 16:00 |

### SwapRequest -> Employee (Requesting)
- **FK**: `SwapRequest.RequestingEmployeeId` -> `Employee.Ssn`
- **Cardinality**: Employee (1) → SwapRequests Sent (0..many)

### SwapRequest -> Employee (Recipient)
- **FK**: `SwapRequest.RecipientEmployeeId` -> `Employee.Ssn`
- **Cardinality**: Employee (1) → SwapRequests Received (0..many)

### SwapRequest -> Schedule (Requesting Employee Schedule)
- **FK**: `SwapRequest.RequestingEmployeeScheduleId` -> `Schedule.ScheduleId`
- **Cardinality**: Schedule (1) ↔ SwapRequest (0..1)

**Sample tables**

`Employee`
| Ssn | FirstName |
|---|---|
| 1002 | Omar |
| 1003 | Mona |

`Schedule`
| ScheduleId | EmployeeId |
|---|---|
| 500 | 1002 |

`SwapRequest`
| RequestId | RequestingEmployeeId | RecipientEmployeeId | RequestingEmployeeScheduleId |
|---|---|---|---|
| 800 | 1002 | 1003 | 500 |

### EmployeeDepartment (Join Table)
- **FK**: `EmployeeDepartment.EmployeeID` -> `Employee.Ssn`
- **FK**: `EmployeeDepartment.DepartmentID` -> `Department.DepartmentId`
- **Cardinality**: Employee (1) ↔ Department (0..many) with join table

**Sample tables**

`Employee`
| Ssn | FirstName |
|---|---|
| 1002 | Omar |

`Department`
| DepartmentId | Name |
|---|---|
| 10 | Assembly |

`EmployeeDepartment`
| EmployeeID | DepartmentID | Hours |
|---|---|---|
| 1002 | 10 | 40 |
