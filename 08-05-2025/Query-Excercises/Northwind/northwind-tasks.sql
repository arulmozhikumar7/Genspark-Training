--1. List all orders with the customer name and the employee who handled the order.

SELECT 
    o.OrderID,
    c.CompanyName AS CustomerName,
    CONCAT(e.FirstName, ' ', e.LastName) AS EmployeeName
FROM Orders o
JOIN Customers c ON o.CustomerID = c.CustomerID
JOIN Employees e ON o.EmployeeID = e.EmployeeID
ORDER BY o.OrderDate;

--2. Get a list of products along with their category and supplier name.

SELECT 
    p.ProductID,
    p.ProductName,
    c.CategoryName,
    s.CompanyName AS SupplierName
FROM Products p
JOIN Categories c ON p.CategoryID = c.CategoryID
JOIN Suppliers s ON p.SupplierID = s.SupplierID
ORDER BY p.ProductName;

--3. Show all orders and the products included in each order with quantity and unit price.

SELECT 
    o.OrderID,
    p.ProductName,
    od.Quantity,
    od.UnitPrice
FROM Orders o
JOIN [Order Details] od ON o.OrderID = od.OrderID
JOIN Products p ON od.ProductID = p.ProductID
ORDER BY o.OrderID, p.ProductName;

--4 .List employees who report to other employees (manager-subordinate relationship).

SELECT 
    e.EmployeeID AS SubordinateID,
    e.FirstName + ' ' + e.LastName AS SubordinateName,
    m.EmployeeID AS ManagerID,
    m.FirstName + ' ' + m.LastName AS ManagerName
FROM Employees e
JOIN Employees m ON e.ReportsTo = m.EmployeeID
ORDER BY ManagerName, SubordinateName;

--5 .Display each customer and their total order count.

SELECT 
    c.CompanyName AS CustomerName,
    COUNT(o.OrderID) AS TotalOrders
FROM Customers c
LEFT JOIN Orders o ON c.CustomerID = o.CustomerID
GROUP BY c.CompanyName
ORDER BY TotalOrders DESC;

--6. Find the average unit price of products per category.

SELECT 
    c.CategoryName,
    AVG(p.UnitPrice) AS AverageUnitPrice
FROM Products p
JOIN Categories c ON p.CategoryID = c.CategoryID
GROUP BY c.CategoryName
ORDER BY AverageUnitPrice DESC;

--7. List customers where the contact title starts with 'Owner'.

SELECT 
    CustomerID,
    CompanyName,
    ContactName,
    ContactTitle
FROM Customers
WHERE ContactTitle LIKE 'Owner%';

--8. Show the top 5 most expensive products.

SELECT TOP 5 
    ProductName,
    UnitPrice
FROM Products
ORDER BY UnitPrice DESC;

--9. Return the total sales amount (quantity × unit price) per order

SELECT 
    o.OrderID,
    SUM(od.Quantity * od.UnitPrice) AS TotalSalesAmount
FROM [Order Details] od
JOIN Orders o ON od.OrderID = o.OrderID
GROUP BY o.OrderID
ORDER BY TotalSalesAmount DESC;

--10. Create a stored procedure that returns all orders for a given customer ID.

CREATE OR ALTER PROCEDURE GetOrdersByCustomerID
    @CustomerID NVARCHAR(5)
AS
BEGIN
    SELECT 
       o.*
    FROM Orders o
    WHERE o.CustomerID = @CustomerID
    ORDER BY o.OrderDate DESC;
END

EXEC GetOrdersByCustomerID @CustomerID = 'ALFKI';

--11. Write a stored procedure that inserts a new product.

CREATE OR ALTER PROCEDURE InsertNewProduct
    @ProductName NVARCHAR(255),
    @SupplierID INT,
    @CategoryID INT,
    @QuantityPerUnit NVARCHAR(50),
    @UnitPrice DECIMAL(18, 2),
    @UnitsInStock INT,
    @UnitsOnOrder INT,
    @ReorderLevel INT,
    @Discontinued BIT
AS
BEGIN
    INSERT INTO Products 
    (
        ProductName, 
        SupplierID, 
        CategoryID, 
        QuantityPerUnit, 
        UnitPrice, 
        UnitsInStock, 
        UnitsOnOrder, 
        ReorderLevel, 
        Discontinued
    )
    VALUES
    (
        @ProductName, 
        @SupplierID, 
        @CategoryID, 
        @QuantityPerUnit, 
        @UnitPrice, 
        @UnitsInStock, 
        @UnitsOnOrder, 
        @ReorderLevel, 
        @Discontinued
    );

END

EXEC InsertNewProduct 
    'New Product',     -- @ProductName
    1,                 -- @SupplierID
    2,                 -- @CategoryID
    '10 boxes',        -- @QuantityPerUnit
    20.99,             -- @UnitPrice
    100,               -- @UnitsInStock
    50,                -- @UnitsOnOrder
    20,                -- @ReorderLevel
    0;                 -- @Discontinued

--12.  Create a stored procedure that returns total sales per employee.

CREATE OR ALTER PROCEDURE GetTotalSalesPerEmployee
AS
BEGIN
    SELECT 
        e.EmployeeID,
        e.FirstName + ' ' + e.LastName AS EmployeeName,
        SUM(od.Quantity * od.UnitPrice) AS TotalSales
    FROM 
        Employees e
    JOIN 
        Orders o ON e.EmployeeID = o.EmployeeID
    JOIN 
       [Order Details] od ON o.OrderID = od.OrderID
    GROUP BY 
        e.EmployeeID, e.FirstName, e.LastName
    ORDER BY 
        TotalSales DESC;
END;

EXEC GetTotalSalesPerEmployee;

--13. Use a CTE to rank products by unit price within each category.

WITH RankedProducts AS
(
    SELECT 
        p.ProductID,
        p.ProductName,
        p.CategoryID,
        p.UnitPrice,
        ROW_NUMBER() OVER (PARTITION BY p.CategoryID ORDER BY p.UnitPrice DESC) AS Rank
    FROM 
        Products p
)
SELECT 
    *
FROM 
    RankedProducts
ORDER BY 
    CategoryID, Rank;

--14 .Create a CTE to calculate total revenue per product and filter products with revenue > 10,000.

WITH ProductRevenue AS
(
    SELECT 
        p.ProductID,
        p.ProductName,
        SUM(od.Quantity * od.UnitPrice) AS TotalRevenue
    FROM 
        [Order Details] od
    JOIN 
        Products p ON od.ProductID = p.ProductID
    GROUP BY 
        p.ProductID, p.ProductName
)
SELECT 
    ProductID,
    ProductName,
    TotalRevenue
FROM 
    ProductRevenue
WHERE 
    TotalRevenue > 10000
ORDER BY 
    TotalRevenue DESC;

--15. Use a CTE with recursion to display employee hierarchy.

WITH EmployeeHierarchy (EmployeeID, FullName, ReportsTo, Level) AS (
    SELECT 
        EmployeeID,
        FirstName + ' ' + LastName AS FullName,
        ReportsTo,
        0 AS Level
    FROM Employees
    WHERE ReportsTo IS NULL

    UNION ALL

    SELECT 
        e.EmployeeID,
        e.FirstName + ' ' + e.LastName,
        e.ReportsTo,
        eh.Level + 1
    FROM Employees e
    INNER JOIN EmployeeHierarchy eh ON e.ReportsTo = eh.EmployeeID
)

SELECT 
    EmployeeID,
    FullName,
    ReportsTo,
    Level
FROM EmployeeHierarchy
ORDER BY Level, ReportsTo;










