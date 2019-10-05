
-- a) 
-- выполните код, созданный во втором задании 
-- второй лабораторной работы. Добавьте в таблицу 
-- dbo.Person поля SalesYTD MONEY, SalesLastYear MONEY и 
-- OrdersNum INT. Также создайте в таблице 
-- вычисляемое поле SalesDiff, 
-- считающее разницу значений в полях SalesYTD и SalesLastYear.

-- b) 
-- создайте временную таблицу #Person, 
-- с первичным ключом по полю BusinessEntityID. 
-- Временная таблица должна включать все поля 
-- таблицы dbo.Person за исключением поля SalesDiff.

-- c)
-- заполните временную таблицу данными из dbo.Person. 
-- Поля SalesYTD и SalesLastYear заполните значениями 
-- из таблицы Sales.SalesPerson. Посчитайте количество заказов,
-- оформленных каждым продавцом (SalesPersonID) в таблице 
-- Sales.SalesOrderHeader и заполните этими значениями поле
-- OrdersNum. Подсчет количества заказов осуществите 
-- в Common Table Expression (CTE).

-- d) 
-- удалите из таблицы dbo.Person одну строку
-- (где BusinessEntityID = 290)

-- e)
-- напишите Merge выражение, использующее 
-- dbo.Person как target, а временную таблицу 
-- как source. Для связи target и source используйте 
-- BusinessEntityID. Обновите поля SalesYTD, 
-- SalesLastYear и OrdersNum таблицы dbo.Person, 
-- если запись присутствует и в source и в target. 
-- Если строка присутствует во временной таблице, 
-- но не существует в target, добавьте строку в dbo.Person. 
-- Если в dbo.Person присутствует такая строка, 
-- которой не существует во временной таблице, 
-- удалите строку из dbo.Person.



