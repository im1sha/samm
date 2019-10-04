-- a) 
-- создайте таблицу dbo.Person с такой же структурой 
-- как Person.Person, кроме полей xml, uniqueidentifier, 
-- не включа¤ индексы, ограничени¤ и триггеры;

IF OBJECT_ID('[dbo].[Person]', 'U') IS NOT NULL 
DROP TABLE [dbo].[Person];

CREATE TABLE [dbo].[Person]
(
    [BusinessEntityID] INT NOT NULL
,   [PersonType] NVARCHAR (2) NOT NULL
,	[NameStyle] NameStyle NOT NULL
,	[Title] NVARCHAR (8) NULL
,	[FirstName] Name NOT NULL
,	[MiddleName] Name NULL
,	[LastName] Name NOT NULL
,	[Suffix] NVARCHAR (10) NULL
,	[EmailPromotion] INT NOT NULL
,	[ModifiedDate] DATETIME NOT NULL
);


------------------------------------------------


-- b) 
-- использу¤ инструкцию ALTER TABLE, 
-- добавьте в таблицу dbo.Person новое поле ID, 
-- которое ¤вл¤етс¤ первичным ключом типа bigint и 
-- имеет свойство identity.
-- Ќачальное значение дл¤ пол¤ identity задайте 10 и 
-- приращение задайте 10;

ALTER TABLE [dbo].[Person]
ADD [ID] BIGINT IDENTITY(10, 10) PRIMARY KEY;


-------------------------------------------------

-- c) использу¤ инструкцию ALTER TABLE, 
-- создайте дл¤ таблицы dbo.Person ограничение дл¤ пол¤ Title, 
-- чтобы заполнить его можно было только значени¤ми СMr.Т или СMs.Т;


ALTER TABLE [dbo].[Person]
ADD CONSTRAINT [CK_Title] 
CHECK ([Title] IN ('Mr.', 'Ms.'));


-------------------------------------------------

-- d) 
-- использу¤ инструкцию ALTER TABLE, 
-- создайте дл¤ таблицы dbo.Person ограничение DEFAULT 
-- дл¤ пол¤ Suffix, задайте значение по умолчанию СN/AТ;


ALTER TABLE [dbo].[Person]
ADD CONSTRAINT [DF_Suffix]  
DEFAULT N'N/A' FOR [Suffix];


-------------------------------------------------

-- e) заполните новую таблицу данными из Person.Person 
-- только дл¤ тех сотрудников, которые существуют 
-- в таблице HumanResources.Employee, 
-- исключив сотрудников из отдела СExecutiveТ;


--INSERT INTO [dbo].[Person]
--(
--    [BusinessEntityID] 
--,   [PersonType] 
--,	[NameStyle]
--,	[Title] 
--,	[FirstName] 
--,	[MiddleName] 
--,	[LastName] 
--,	[Suffix] 
--,	[EmailPromotion]
--,	[ModifiedDate] 
--)
--SELECT
--	[person].[BusinessEntityID] 
--,   [person].[PersonType] 
--,	[person].[NameStyle]
--,	[person].[Title] 
--,	[person].[FirstName] 
--,	[person].[MiddleName] 
--,	[person].[LastName] 
--,	[person].[Suffix] 
--,	[person].[EmailPromotion]
--,	[person].[ModifiedDate] 
--FROM [HumanResources].[Employee] [emp]
--JOIN [Person].[Person] [person]
--    ON emp.[BusinessEntityID] = [person].[BusinessEntityID]
	
	
--	;

-------------------------------------------------

-- f) 
-- измените размерность пол¤ Suffix, 
-- уменьшите размер пол¤ до 5-ти символов.