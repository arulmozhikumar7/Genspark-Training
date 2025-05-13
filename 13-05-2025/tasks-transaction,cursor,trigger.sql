-- Cursors 
--1. Write a cursor to list all customers and how many rentals each made. Insert these into a summary table.

CREATE TABLE rental_summary (
    customer_id INT PRIMARY KEY,
    rental_count INT
);

DO $$
DECLARE
    cur_customer RECORD;
    rental_count INT;
BEGIN
    DELETE FROM rental_summary;
    FOR cur_customer IN
        SELECT customer_id FROM customer
    LOOP
        SELECT COUNT(*) INTO rental_count
        FROM rental
        WHERE rental.customer_id = cur_customer.customer_id;
        INSERT INTO rental_summary (customer_id, rental_count)
        VALUES (cur_customer.customer_id, rental_count);
    END LOOP;
END;
$$;

SELECT * FROM rental_summary;

--2. Using a cursor, print the titles of films in the 'Comedy' category rented more than 10 times.

DO $$
DECLARE
    film_record RECORD;
    film_cursor CURSOR FOR
        SELECT f.title, COUNT(r.rental_id) AS rental_count
        FROM film f
        JOIN film_category fc ON f.film_id = fc.film_id
        JOIN category c ON fc.category_id = c.category_id
        JOIN inventory i ON f.film_id = i.film_id
        JOIN rental r ON i.inventory_id = r.inventory_id
        WHERE c.name = 'Comedy'
        GROUP BY f.film_id, f.title
        HAVING COUNT(r.rental_id) > 10;
BEGIN
    OPEN film_cursor;

    LOOP
        FETCH film_cursor INTO film_record;
        EXIT WHEN NOT FOUND;

        RAISE NOTICE 'Title: %, Rentals: %', film_record.title, film_record.rental_count;
    END LOOP;

    CLOSE film_cursor;
END $$;

--3. Create a cursor to go through each store and count the number of distinct films available, and insert results into a report table.

CREATE TABLE store_report (
    store_id INT PRIMARY KEY,
    distinct_film_count INT
);

DO $$
DECLARE
    store_record RECORD;
    film_count INT;
    store_cursor CURSOR FOR
        SELECT store_id
        FROM store;
BEGIN
    OPEN store_cursor;
    LOOP
        FETCH store_cursor INTO store_record;
        EXIT WHEN NOT FOUND;
        SELECT COUNT(DISTINCT f.film_id) INTO film_count
        FROM inventory i
        JOIN film f ON i.film_id = f.film_id
        WHERE i.store_id = store_record.store_id;
        INSERT INTO store_report (store_id, distinct_film_count)
        VALUES (store_record.store_id, film_count);
    END LOOP;
    CLOSE store_cursor;
END $$;

SELECT * FROM store_report ;

--4 . Loop through all customers who haven't rented in the last 6 months and insert their details into an inactive_customers table.

CREATE TABLE inactive_customers (
    customer_id INT PRIMARY KEY,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    email VARCHAR(100),
    last_rental_date DATE
);

DO $$
DECLARE
    customer_record RECORD;
    last_rental DATE;
    inactivity_threshold DATE := CURRENT_DATE - INTERVAL '6 months';
BEGIN
    DELETE FROM inactive_customers;
    FOR customer_record IN
        SELECT c.customer_id, c.first_name, c.last_name, c.email,
               MAX(r.rental_date) AS last_rental_date
        FROM customer c
        LEFT JOIN rental r ON c.customer_id = r.customer_id
        GROUP BY c.customer_id, c.first_name, c.last_name, c.email
        HAVING MAX(r.rental_date) < inactivity_threshold OR MAX(r.rental_date) IS NULL
    LOOP
        INSERT INTO inactive_customers (customer_id, first_name, last_name, email, last_rental_date)
        VALUES (customer_record.customer_id, customer_record.first_name, customer_record.last_name, 
                customer_record.email, customer_record.last_rental_date);
    END LOOP;
END $$;

SELECT * FROM inactive_customers ;

-- Transactions 

--1. Write a transaction that inserts a new customer, adds their rental, and logs the payment – all atomically.

BEGIN;

-- Insert a new customer
WITH new_customer AS (
    INSERT INTO customer (
        store_id, first_name, last_name, email, address_id, active, create_date
    )
    VALUES (
        1, 'Arulmozhikumar', 'K', 'arulmozhikumar7@gmail.com', 5, 1, NOW()
    )
    RETURNING customer_id
),

-- Insert a rental for the new customer
new_rental AS (
    INSERT INTO rental (
        rental_date, inventory_id, customer_id, staff_id
    )
    SELECT NOW(), 1000, customer_id, 1
    FROM new_customer
    RETURNING rental_id, customer_id
)

-- Insert payment for the rental
INSERT INTO payment (
    customer_id, staff_id, rental_id, amount, payment_date
)
SELECT
    customer_id, 1, rental_id, 3.99, NOW()
FROM new_rental;

COMMIT;

--2. Simulate a transaction where one update fails (e.g., invalid rental ID), and ensure the entire transaction rolls back.

BEGIN;

INSERT INTO payment (
    customer_id, staff_id, rental_id, amount, payment_date
)
VALUES (
    1, 1, 100, 4.99, NOW()
);

-- Invalid rental_id
INSERT INTO payment (
    customer_id, staff_id, rental_id, amount, payment_date
)
VALUES (
    1, 1, 0, '3.99', NOW()
);

COMMIT;

ROLLBACK;

--3. Use SAVEPOINT to update multiple payment amounts. Roll back only one payment update using ROLLBACK TO SAVEPOINT.
BEGIN;

-- Update the first payment record (payment_id = 17503)
UPDATE payment
SET amount = 10.99
WHERE payment_id = 17504;

-- Create a SAVEPOINT after the first update
SAVEPOINT before_second_update;

-- Update the second payment record (payment_id = 17504)
UPDATE payment
SET amount = 5.99
WHERE payment_id = 17506;

-- Rollback to the SAVEPOINT before the second update
ROLLBACK TO SAVEPOINT before_second_update;

-- Update the third payment record (payment_id = 17505)
UPDATE payment
SET amount = 9.99
WHERE payment_id = 17507;

COMMIT;

SELECT payment_id, amount
FROM payment
WHERE payment_id IN (17504, 17506, 17507);

--4. Perform a transaction that transfers inventory from one store to another (delete + insert) safely.

BEGIN;

-- Find an inventory item for film 101 at store 1 that was never rented
DELETE FROM inventory
WHERE inventory_id = (
  SELECT i.inventory_id
  FROM inventory i
  LEFT JOIN rental r ON i.inventory_id = r.inventory_id
  WHERE i.film_id = 101 AND i.store_id = 1 AND r.rental_id IS NULL
  LIMIT 1
);

-- Insert it at store 2
INSERT INTO inventory (film_id, store_id, last_update)
VALUES (101, 2, NOW());

COMMIT;

--5. Create a transaction that deletes a customer and all associated records (rental, payment), ensuring referential integrity.

BEGIN;
-- Example: customer_id = 341

-- Step 1: Delete payments made by the customer
DELETE FROM payment
WHERE customer_id = 341;

-- Step 2: Delete rentals made by the customer
DELETE FROM rental
WHERE customer_id = 341;

-- Step 3: Delete the customer record
DELETE FROM customer
WHERE customer_id = 341;

COMMIT;


-- Triggers

--1. Create a trigger to prevent inserting payments of zero or negative amount.

CREATE OR REPLACE FUNCTION prevent_invalid_payment_amount()
RETURNS TRIGGER AS $$
BEGIN
  IF NEW.amount <= 0 THEN
    RAISE EXCEPTION 'Payment amount must be greater than zero. Attempted value: %', NEW.amount;
  END IF;
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_prevent_zero_or_negative_payment
BEFORE INSERT ON payment
FOR EACH ROW
EXECUTE FUNCTION prevent_invalid_payment_amount();

-- This will FAIL with the custom error
INSERT INTO payment (customer_id, staff_id, rental_id, amount, payment_date)
VALUES (1, 1, 1, 0, NOW());

--2. Set up a trigger that automatically updates last_update on the film table when the title or rental rate is changed.

CREATE OR REPLACE FUNCTION update_last_update_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.last_update := CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER set_last_update_before_update
BEFORE UPDATE ON film
FOR EACH ROW
EXECUTE FUNCTION update_last_update_column();

UPDATE film
SET title = 'New Film Title'
WHERE film_id = 1;

SELECT title, last_update
FROM film
WHERE film_id = 1;

--3. Write a trigger that inserts a log into rental_log whenever a film is rented more than 3 times in a week.

CREATE TABLE rental_log (
  log_id SERIAL PRIMARY KEY,
  film_id INT NOT NULL,
  rental_count INT NOT NULL,
  log_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION log_frequent_rentals()
RETURNS TRIGGER AS $$
DECLARE
  v_film_id INT;
  v_rental_count INT;
BEGIN

  SELECT film_id INTO v_film_id
  FROM inventory
  WHERE inventory_id = NEW.inventory_id;
  
  SELECT COUNT(*) INTO v_rental_count
  FROM rental r
  JOIN inventory i ON r.inventory_id = i.inventory_id
  WHERE i.film_id = v_film_id
    AND r.rental_date >= (CURRENT_DATE - INTERVAL '7 days');

  -- If more than 3 times, insert into rental_log
  IF v_rental_count > 3 THEN
    INSERT INTO rental_log (film_id, rental_count)
    VALUES (v_film_id, v_rental_count);
  END IF;

  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_log_frequent_rentals
AFTER INSERT ON rental
FOR EACH ROW
EXECUTE FUNCTION log_frequent_rentals();

-- Testing the trigger
-- Get an inventory_id and its film_id
SELECT i.inventory_id, i.film_id
FROM inventory i
LIMIT 1;

-- 1st Rental
INSERT INTO rental (rental_date, inventory_id, customer_id, return_date, staff_id)
VALUES (CURRENT_DATE, 2, 1, CURRENT_DATE + INTERVAL '1 day', 1);

-- 2nd Rental
INSERT INTO rental (rental_date, inventory_id, customer_id, return_date, staff_id)
VALUES (CURRENT_DATE, 2, 2, CURRENT_DATE + INTERVAL '1 day', 1);

-- 3rd Rental
INSERT INTO rental (rental_date, inventory_id, customer_id, return_date, staff_id)
VALUES (CURRENT_DATE, 2, 3, CURRENT_DATE + INTERVAL '1 day', 1);

-- 4th Rental — should trigger the log
INSERT INTO rental (rental_date, inventory_id, customer_id, return_date, staff_id)
VALUES (CURRENT_DATE, 2, 4, CURRENT_DATE + INTERVAL '1 day', 1);

SELECT * FROM rental_log WHERE film_id = 1 ORDER BY log_time DESC;



-- Others
-- Disable trigger temporarily
ALTER TABLE rental DISABLE TRIGGER ALL;

-- Re-enable after insert
ALTER TABLE rental ENABLE TRIGGER ALL;

-- Lists All triggers 
SELECT tgname 
FROM pg_trigger
WHERE tgrelid = 'rental'::regclass;
