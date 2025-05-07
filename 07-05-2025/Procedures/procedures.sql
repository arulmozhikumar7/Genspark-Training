
-- Create the Products table 
CREATE TABLE Products
(
    id INT IDENTITY(1,1) CONSTRAINT pk_productId PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    details NVARCHAR(MAX)
);
GO

-- Create the proc_InsertProduct stored procedure
CREATE PROC proc_InsertProduct(@pname NVARCHAR(100), @pdetails NVARCHAR(MAX))
AS
BEGIN
    INSERT INTO Products(name, details)
    VALUES (@pname, @pdetails)
END;
GO

-- Execute the procedure with values
EXEC proc_InsertProduct 'Laptop', '{"brand":"Dell","spec":{"ram":"16GB","cpu":"i5"}}';

-- ADHOC QUERY TO GET PRODUCT SPECIFICATION ALONE
SELECT JSON_QUERY(details,'$.spec') AS Product_Specification from Products

-- PROCEDURE TO UPDATE RAM OF THE PRODUCT

CREATE PROC proc_UpdateProductRam (@pid int,@newvalue varchar(20))
AS
BEGIN 
	UPDATE Products set details = JSON_MODIFY(details,'$.spec.ram',@newvalue) where id = @pid
END;
GO

EXEC proc_UpdateProductRam 1,'64GB'


-- PROCEDURE TO UPDATE CPU OF THE PRODUCT

CREATE PROC proc_UpdateProductCpu (@pid INT, @newvalue VARCHAR(50))
AS
BEGIN 
    UPDATE Products 
    SET details = JSON_MODIFY(details, '$.spec.cpu', @newvalue) 
    WHERE id = @pid
END;
GO

EXEC proc_UpdateProductCpu 1, 'Intel i9';

-- PROCEDURE TO UPDATE BOTH SPEC (FLEXIBLE)

CREATE PROC proc_UpdateRamAndCpu
    @pid INT,
    @newRam NVARCHAR(50) = NULL,
    @newCpu NVARCHAR(50) = NULL
AS
BEGIN
    DECLARE @updatedDetails NVARCHAR(MAX);

    -- Get current details
    SELECT @updatedDetails = details FROM Products WHERE id = @pid;

    -- Update RAM if provided
    IF @newRam IS NOT NULL
        SET @updatedDetails = JSON_MODIFY(@updatedDetails, '$.spec.ram', @newRam);

    -- Update CPU if provided
    IF @newCpu IS NOT NULL
        SET @updatedDetails = JSON_MODIFY(@updatedDetails, '$.spec.cpu', @newCpu);

    -- Apply changes
    UPDATE Products SET details = @updatedDetails WHERE id = @pid;
END;
GO

-- Update both
EXEC proc_UpdateRamAndCpu 1, '64GB', 'Intel i7';

-- Update only RAM
EXEC proc_UpdateRamAndCpu 1, '128GB', NULL;

-- Update only CPU
EXEC proc_UpdateRamAndCpu 1, NULL, 'Ryzen 7';

-- SELECT ALL FROM PRODUCTS

SELECT * FROM PRODUCTS

-- CREATING A POSTS TABLE
 create table Posts
  (id int primary key,
  title nvarchar(100),
  user_id int,
  body nvarchar(max))
Go

-- CREATING PROCEDURE TO BULK INSERT POSTS

create proc proc_BulKInsertPosts(@jsondata nvarchar(max))
  as
  begin
	insert into Posts(user_id,id,title,body)
	select userId,id,title,body from openjson(@jsondata)
	with (userId int,id int, title varchar(100), body varchar(max))
  end

-- BULK INSERT USING STORED PROCEDURE
proc_BulKInsertPosts '
[
  {
    "userId": 1,
    "id": 1,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  },
  {
    "userId": 1,
    "id": 2,
    "title": "qui est esse",
    "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
  }]'

  -- FETCH ALL FROM POSTS
  SELECT * FROM Posts

  -- Retrieves products where the RAM value in the details JSON is '128GB'.
  SELECT * 
FROM products 
WHERE TRY_CAST(JSON_VALUE(details, '$.spec.ram') AS NVARCHAR(20)) = '128GB';

  -- Create the procedure to get posts by user_id
CREATE PROCEDURE proc_GetPostsByUserID
    @user_id INT
AS
BEGIN
    SELECT id, title, body
    FROM Posts
    WHERE user_id = @user_id;
END;
GO

EXEC proc_GetPostsByUserID 1;
