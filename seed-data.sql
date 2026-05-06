SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRAN;

    IF OBJECT_ID(N'dbo.EmployeeDepartments', N'U') IS NOT NULL
        ALTER TABLE [dbo].[EmployeeDepartments] NOCHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.Employees', N'U') IS NOT NULL
        ALTER TABLE [dbo].[Employees] NOCHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.Departments', N'U') IS NOT NULL
        ALTER TABLE [dbo].[Departments] NOCHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.ProductionLines', N'U') IS NOT NULL
        ALTER TABLE [dbo].[ProductionLines] NOCHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.JopDescriptions', N'U') IS NOT NULL
        ALTER TABLE [dbo].[JopDescriptions] NOCHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.Missions', N'U') IS NOT NULL
        ALTER TABLE [dbo].[Missions] NOCHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.Schedules', N'U') IS NOT NULL
        ALTER TABLE [dbo].[Schedules] NOCHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.Attendances', N'U') IS NOT NULL
        ALTER TABLE [dbo].[Attendances] NOCHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.SwapRequests', N'U') IS NOT NULL
        ALTER TABLE [dbo].[SwapRequests] NOCHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.VacationRequests', N'U') IS NOT NULL
        ALTER TABLE [dbo].[VacationRequests] NOCHECK CONSTRAINT ALL;

    IF OBJECT_ID(N'dbo.Shifts', N'U') IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM [dbo].[Shifts])
    BEGIN
        SET IDENTITY_INSERT [dbo].[Shifts] ON;
        INSERT INTO [dbo].[Shifts] ([ShiftId], [Name], [StartTime], [EndTime])
        VALUES
            (1, N'Morning', '2024-01-01T08:00:00', '2024-01-01T16:00:00'),
            (2, N'Evening', '2024-01-01T16:00:00', '2024-01-02T00:00:00');
        SET IDENTITY_INSERT [dbo].[Shifts] OFF;
    END

    IF OBJECT_ID(N'dbo.Departments', N'U') IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM [dbo].[Departments])
    BEGIN
        SET IDENTITY_INSERT [dbo].[Departments] ON;
        INSERT INTO [dbo].[Departments] ([DepartmentId], [Name], [EmployeeCount], [ManagerId])
        VALUES
            (1, N'Manufacturing', 2, 1001);
        SET IDENTITY_INSERT [dbo].[Departments] OFF;
    END

    IF OBJECT_ID(N'dbo.ProductionLines', N'U') IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM [dbo].[ProductionLines])
    BEGIN
        SET IDENTITY_INSERT [dbo].[ProductionLines] ON;
        INSERT INTO [dbo].[ProductionLines] ([ProductionLineId], [Name], [DepartmentId])
        VALUES
            (1, N'Assembly Line A', 1);
        SET IDENTITY_INSERT [dbo].[ProductionLines] OFF;
    END

    IF OBJECT_ID(N'dbo.Employees', N'U') IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM [dbo].[Employees])
    BEGIN
        INSERT INTO [dbo].[Employees]
            ([Ssn], [FirstName], [LastName], [Salary], [Sex], [BirthDate], [Address], [PhoneNumber], [VacationBalance], [DefaultRole], [ManagerSsn], [ShiftId], [ProductionLineId], [UserId])
        VALUES
            (1001, N'Amina', N'Hassan', 12000.00, N'F', '1990-06-15', N'Cairo', 111222333, 15, N'Manager', NULL, 1, 1, NULL),
            (1002, N'Omar', N'Fathy', 8000.00, N'M', '1995-03-10', N'Giza', 222333444, 12, N'Operator', 1001, 2, 1, NULL);
    END

    IF OBJECT_ID(N'dbo.EmployeeDepartments', N'U') IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM [dbo].[EmployeeDepartments])
    BEGIN
        INSERT INTO [dbo].[EmployeeDepartments] ([EmployeeID], [DepartmentID], [Hours])
        VALUES
            (1001, 1, 40),
            (1002, 1, 40);
    END

    IF OBJECT_ID(N'dbo.JopDescriptions', N'U') IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM [dbo].[JopDescriptions])
    BEGIN
        SET IDENTITY_INSERT [dbo].[JopDescriptions] ON;
        INSERT INTO [dbo].[JopDescriptions] ([JopDescriptionId], [DailyTasks], [RequiredCount], [RoleName], [ProductionId])
        VALUES
            (1, N'Inspect and assemble parts.', N'5', N'Assembler', 1);
        SET IDENTITY_INSERT [dbo].[JopDescriptions] OFF;
    END

    IF OBJECT_ID(N'dbo.Missions', N'U') IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM [dbo].[Missions])
    BEGIN
        SET IDENTITY_INSERT [dbo].[Missions] ON;
        INSERT INTO [dbo].[Missions] ([MissionId], [Status], [Purpose], [Destination], [StartDate], [EndDate], [AuthorizedEmployeeId], [GoesOnEmployeeId])
        VALUES
            (1, 1, N'Equipment inspection', N'Plant A', '2024-02-01', '2024-02-02', 1001, 1002);
        SET IDENTITY_INSERT [dbo].[Missions] OFF;
    END

    IF OBJECT_ID(N'dbo.VacationRequests', N'U') IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM [dbo].[VacationRequests])
    BEGIN
        SET IDENTITY_INSERT [dbo].[VacationRequests] ON;
        INSERT INTO [dbo].[VacationRequests] ([VacationRequestId], [StartDate], [EndDate], [Reason], [Status], [EmployeeId])
        VALUES
            (1, '2024-07-01', '2024-07-07', N'Family trip', 0, 1002);
        SET IDENTITY_INSERT [dbo].[VacationRequests] OFF;
    END

    IF OBJECT_ID(N'dbo.Schedules', N'U') IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM [dbo].[Schedules])
    BEGIN
        SET IDENTITY_INSERT [dbo].[Schedules] ON;
        INSERT INTO [dbo].[Schedules]
            ([ScheduleId], [ScheduleName], [ScheduleDate], [EmployeeId], [MissionId], [ShiftId], [JopDescriptionId], [ProductionLineId], [VacationRequestId])
        VALUES
            (1, N'Morning Shift', '2024-02-01', 1002, 1, 2, 1, 1, 1);
        SET IDENTITY_INSERT [dbo].[Schedules] OFF;
    END

    IF OBJECT_ID(N'dbo.Attendances', N'U') IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM [dbo].[Attendances])
    BEGIN
        SET IDENTITY_INSERT [dbo].[Attendances] ON;
        INSERT INTO [dbo].[Attendances] ([AttendanceId], [TimeIn], [TimeOut], [ScheduleId])
        VALUES
            (1, '2024-02-01T08:05:00', '2024-02-01T16:01:00', 1);
        SET IDENTITY_INSERT [dbo].[Attendances] OFF;
    END

    IF OBJECT_ID(N'dbo.SwapRequests', N'U') IS NOT NULL
        AND NOT EXISTS (SELECT 1 FROM [dbo].[SwapRequests])
    BEGIN
        SET IDENTITY_INSERT [dbo].[SwapRequests] ON;
        INSERT INTO [dbo].[SwapRequests] ([RequestId], [RequestingEmployeeId], [RecipientEmployeeId], [ScheduleId])
        VALUES
            (1, 1002, 1001, 1);
        SET IDENTITY_INSERT [dbo].[SwapRequests] OFF;
    END

    IF OBJECT_ID(N'dbo.EmployeeDepartments', N'U') IS NOT NULL
        ALTER TABLE [dbo].[EmployeeDepartments] WITH CHECK CHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.Employees', N'U') IS NOT NULL
        ALTER TABLE [dbo].[Employees] WITH CHECK CHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.Departments', N'U') IS NOT NULL
        ALTER TABLE [dbo].[Departments] WITH CHECK CHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.ProductionLines', N'U') IS NOT NULL
        ALTER TABLE [dbo].[ProductionLines] WITH CHECK CHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.JopDescriptions', N'U') IS NOT NULL
        ALTER TABLE [dbo].[JopDescriptions] WITH CHECK CHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.Missions', N'U') IS NOT NULL
        ALTER TABLE [dbo].[Missions] WITH CHECK CHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.Schedules', N'U') IS NOT NULL
        ALTER TABLE [dbo].[Schedules] WITH CHECK CHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.Attendances', N'U') IS NOT NULL
        ALTER TABLE [dbo].[Attendances] WITH CHECK CHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.SwapRequests', N'U') IS NOT NULL
        ALTER TABLE [dbo].[SwapRequests] WITH CHECK CHECK CONSTRAINT ALL;
    IF OBJECT_ID(N'dbo.VacationRequests', N'U') IS NOT NULL
        ALTER TABLE [dbo].[VacationRequests] WITH CHECK CHECK CONSTRAINT ALL;

    COMMIT TRAN;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRAN;

    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
    DECLARE @ErrorState INT = ERROR_STATE();

    RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;
