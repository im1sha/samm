-- 1 
-- ������� �� ����� ������ ����������� 
-- � ��������� ������������ ������, 
-- �� ������� �� ����������� �������� ��������.

SELECT 
    [emp].[BusinessEntityID]
,   [emp].[JobTitle]
,   MAX([payhist].[Rate]) AS [MaxRate]
FROM [HumanResources].[Employee] [emp]
INNER JOIN [HumanResources].[EmployeePayHistory] [payhist]
    ON [emp].[BusinessEntityID] = [payhist].[BusinessEntityID]
	GROUP BY [emp].[BusinessEntityID], [emp].[JobTitle]



-------------------------------------



-- 2
-- ������� ��� ��������� ������ �� ������ ����� �������, 
-- ����� ���������� ������ ������� � ���� ������. 
-- ������ ����� ������ ���� ������������ �� ����������� ������. 
-- �������� ������� [RankRate].

SELECT
    [emp].[BusinessEntityID]
,   [emp].[JobTitle]
,   [payhist].[Rate]
,   DENSE_RANK() OVER (ORDER BY [payhist].[Rate] ASC) AS [RankRate]
FROM [HumanResources].[Employee] [emp]
JOIN [HumanResources].[EmployeePayHistory] [payhist]
    ON [emp].[BusinessEntityID] = [payhist].[BusinessEntityID]



------------------------------------------


-- 3
-- ������� �� ����� ���������� �� ������� � 
-- ���������� � ��� �����������, ��������������� �� ���� ShiftID 
-- � ������ �Document Control� � �� ���� BusinessEntityID 
-- �� ���� ��������� �������


SELECT
    [dept].[Name] as [DepName]
,   [emp].[BusinessEntityID]
,   [emp].[JobTitle]
,   [edh].[ShiftID]
FROM [HumanResources].[Department] [dept]
JOIN [HumanResources].[EmployeeDepartmentHistory] [edh]
    ON [dept].[DepartmentID] = [edh].[DepartmentID]
JOIN [HumanResources].[Employee] [emp]
    ON [edh].[BusinessEntityID] = [emp].[BusinessEntityID]
ORDER BY [dept].[Name] ASC,
    CASE 
        WHEN [dept].[Name] = N'Document Control' THEN [edh].[ShiftID]
        ELSE [emp].[BusinessEntityID]
    END ASC

