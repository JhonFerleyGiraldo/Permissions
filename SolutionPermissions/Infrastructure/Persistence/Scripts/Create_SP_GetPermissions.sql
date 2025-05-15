USE [Permisos]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetPermissions]    Script Date: 14/05/2025 22:59:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_GetPermissions]
AS
BEGIN
    SELECT  Id,
            EmployeeForename,
            EmployeeSurname,
            PermissionType AS PermissionTypeId,
            PermissionDate
    FROM [Permissions]
END
GO