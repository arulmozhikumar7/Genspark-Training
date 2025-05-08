-------------------------------------------08-05-2025-------------------------------------------------------

CREATE PROC proc_FilterProducts(@pcpu VARCHAR(20),@pcount INT OUT)
AS
BEGIN 
	SET @pcount = (SELECT count(*) FROM Products 
	WHERE TRY_CAST(JSON_VALUE(details, '$.spec.cpu') AS NVARCHAR(20)) = @pcpu)
END

BEGIN
 DECLARE @cnt INT
 EXEC proc_FilterProducts 'i7', @cnt OUT
 PRINT concat('The number of computers is ',@cnt)
END

create table people
(id int primary key,
name nvarchar(20),
age int)

create or alter proc proc_BulkInsert(@filepath nvarchar(500))
as
begin
   declare @insertQuery nvarchar(max)

   set @insertQuery = 'BULK INSERT people from '''+ @filepath +'''
   with(
   FIRSTROW =2,
   FIELDTERMINATOR='','',
   ROWTERMINATOR = ''\n'')'
   exec sp_executesql @insertQuery
end

EXEC proc_BulkInsert 
  @filepath = 'C:\\Users\\arulmozhikumark\\Desktop\\GenSpark\\08-05-2025\\Procedures\\data.csv';

SELECT * FROM people

create table BulkInsertLog
(LogId int identity(1,1) primary key,
FilePath nvarchar(1000),
status nvarchar(50) constraint chk_status Check(status in('Success','Failed')),
Message nvarchar(1000),
InsertedOn DateTime default GetDate())


create or alter proc proc_BulkInsert(@filepath nvarchar(500))
as
begin
  Begin try
	   declare @insertQuery nvarchar(max)

	   set @insertQuery = 'BULK INSERT people from '''+ @filepath +'''
	   with(
	   FIRSTROW =2,
	   FIELDTERMINATOR='','',
	   ROWTERMINATOR = ''\n'')'

	   exec sp_executesql @insertQuery

	   insert into BulkInsertLog(filepath,status,message)
	   values(@filepath,'Success','Bulk insert completed')
  end try
  begin catch
		 insert into BulkInsertLog(filepath,status,message)
		 values(@filepath,'Failed',Error_Message())
  END Catch
end

proc_BulkInsert 'C:\\Users\\arulmozhikumark\\Desktop\\GenSpark\\08-05-2025\\Procedures\\data.csv'

select * from BulkInsertLog


with cteAuthors
as
(select au_id, concat(au_fname,' ',au_lname) author_name from authors)

SELECT * FROM cteAuthors


DECLARE @page INT = 2, @pageSize INT = 10;

WITH PaginatedBooks AS (
    SELECT 
        title_id, 
        title, 
        price, 
        ROW_NUMBER() OVER (ORDER BY price DESC) AS RowNum
    FROM titles
)
SELECT * 
FROM PaginatedBooks 
WHERE RowNum BETWEEN ((@page - 1) * @pageSize + 1) AND (@page * @pageSize);


CREATE OR ALTER PROCEDURE proc_GetBooksPaged
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StartRow INT = (@PageNumber - 1) * @PageSize;

    SELECT title_id, title, type, price, pubdate
    FROM titles
    ORDER BY title_id
    OFFSET @StartRow ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END

proc_GetBooksPaged 2,10

CREATE FUNCTION dbo.CalculateTaxedPrice (@Price MONEY)
RETURNS MONEY
AS
BEGIN
    DECLARE @TaxRate DECIMAL(5,2) = 0.10;
    RETURN @Price + (@Price * @TaxRate);
END

SELECT 
    title_id,
    title,
    price,
    dbo.CalculateTaxedPrice(price) AS PriceWithTax
FROM titles;

CREATE FUNCTION GetBooksWithTax
(
    @TaxRate DECIMAL(5, 2)
)
RETURNS @BookTaxTable TABLE
(
    title_id NVARCHAR(255),
    title NVARCHAR(255),
    price MONEY,
    tax MONEY,
    price_with_tax MONEY
)
AS
BEGIN
    INSERT INTO @BookTaxTable
    SELECT 
        title_id,
        title,
        price,
        price * @TaxRate AS tax,
        price + (price * @TaxRate) AS price_with_tax
    FROM titles;
    RETURN;
END

SELECT * FROM dbo.GetBooksWithTax(0.10);




